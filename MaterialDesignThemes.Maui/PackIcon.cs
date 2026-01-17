using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Lightweight MAUI port of the MaterialDesignThemes PackIcon control.
/// </summary>
public class PackIcon : ContentView
{
    private static readonly Brush DefaultBrush = new SolidColorBrush(Colors.Black);
    private static readonly Brush DefaultBackgroundBrush = new SolidColorBrush(Colors.Transparent);
    private const double DefaultViewBoxSize = 24d;

    private readonly Path _path;
    private Geometry? _baseGeometry;

    public PackIcon()
    {
        Background = DefaultBackgroundBrush;

        _path = new Path
        {
            Fill = DefaultBrush,
            Stroke = DefaultBackgroundBrush,
            StrokeThickness = 0,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        Content = _path;

        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Center;
        WidthRequest = 24;
        HeightRequest = 24;

        UpdateIcon();
        UpdateForeground();
    }

    public static readonly BindableProperty KindProperty =
        BindableProperty.Create(
            nameof(Kind),
            typeof(PackIconKind),
            typeof(PackIcon),
            default(PackIconKind),
            propertyChanged: OnKindChanged);

    public PackIconKind Kind
    {
        get => (PackIconKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(
            nameof(Foreground),
            typeof(Brush),
            typeof(PackIcon),
            DefaultBrush,
            propertyChanged: OnForegroundChanged);

    public Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static readonly BindableProperty ForegroundColorProperty =
        BindableProperty.Create(
            nameof(ForegroundColor),
            typeof(Color),
            typeof(PackIcon),
            Colors.Black,
            propertyChanged: OnForegroundColorChanged);

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    private static void OnKindChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control)
        {
            control.UpdateIcon();
        }
    }

    private static void OnForegroundChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control)
        {
            control.UpdateForeground();
        }
    }

    private static void OnForegroundColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PackIcon control && newValue is Color color)
        {
            control.Foreground = new SolidColorBrush(color);
        }
    }

    private void UpdateIcon()
    {
        _baseGeometry = PackIconGeometryParser.Parse(Kind);
        UpdateScale();
    }

    private void UpdateForeground()
    {
        _path?.Fill = Foreground ?? DefaultBrush;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        UpdateScale();
    }

    private void UpdateScale()
    {
        if (_baseGeometry is null)
        {
            return;
        }

        var availableWidth = Width > 0 ? Width : WidthRequest;
        var availableHeight = Height > 0 ? Height : HeightRequest;
        if (availableWidth <= 0 || availableHeight <= 0)
        {
            return;
        }

        var bounds = TryGetGeometryBounds(_baseGeometry, out var geometryBounds)
            ? geometryBounds
            : new Rect(0, 0, DefaultViewBoxSize, DefaultViewBoxSize);

        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return;
        }

        var scale = Math.Min(availableWidth / bounds.Width, availableHeight / bounds.Height);
        var scaledWidth = bounds.Width * scale;
        var scaledHeight = bounds.Height * scale;
        var offsetX = (availableWidth - scaledWidth) / 2 - (bounds.X * scale);
        var offsetY = (availableHeight - scaledHeight) / 2 - (bounds.Y * scale);

        _path.ScaleX = 1;
        _path.ScaleY = 1;
        _path.TranslationX = 0;
        _path.TranslationY = 0;
        _path.Data = ApplyScale(_baseGeometry, scale, offsetX, offsetY);
    }

    private static bool TryGetGeometryBounds(Geometry geometry, out Rect bounds)
    {
        if (geometry is not PathGeometry pathGeometry)
        {
            bounds = default;
            return false;
        }

        var hasPoint = false;
        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;

        void IncludePoint(Point point)
        {
            hasPoint = true;
            minX = Math.Min(minX, point.X);
            minY = Math.Min(minY, point.Y);
            maxX = Math.Max(maxX, point.X);
            maxY = Math.Max(maxY, point.Y);
        }

        foreach (PathFigure figure in pathGeometry.Figures)
        {
            IncludePoint(figure.StartPoint);
            Point lastPoint = figure.StartPoint;

            foreach (PathSegment segment in figure.Segments)
            {
                switch (segment)
                {
                    case LineSegment line:
                        IncludePoint(line.Point);
                        lastPoint = line.Point;
                        break;
                    case PolyLineSegment polyLine:
                        foreach (var point in polyLine.Points)
                        {
                            IncludePoint(point);
                        }
                        if (polyLine.Points.Count > 0)
                        {
                            lastPoint = polyLine.Points[^1];
                        }
                        break;
                    case BezierSegment bezier:
                        IncludePoint(bezier.Point1);
                        IncludePoint(bezier.Point2);
                        IncludePoint(bezier.Point3);
                        lastPoint = bezier.Point3;
                        break;
                    case PolyBezierSegment polyBezier:
                        foreach (var point in polyBezier.Points)
                        {
                            IncludePoint(point);
                        }
                        if (polyBezier.Points.Count > 0)
                        {
                            lastPoint = polyBezier.Points[^1];
                        }
                        break;
                    case QuadraticBezierSegment quadratic:
                        IncludePoint(quadratic.Point1);
                        IncludePoint(quadratic.Point2);
                        lastPoint = quadratic.Point2;
                        break;
                    case PolyQuadraticBezierSegment polyQuadratic:
                        foreach (var point in polyQuadratic.Points)
                        {
                            IncludePoint(point);
                        }
                        if (polyQuadratic.Points.Count > 0)
                        {
                            lastPoint = polyQuadratic.Points[^1];
                        }
                        break;
                    case ArcSegment arc:
                        IncludePoint(arc.Point);
                        IncludePoint(lastPoint);
                        lastPoint = arc.Point;
                        break;
                }
            }
        }

        if (!hasPoint)
        {
            bounds = default;
            return false;
        }

        bounds = new Rect(minX, minY, maxX - minX, maxY - minY);
        return true;
    }

    private static Geometry ApplyScale(Geometry geometry, double scale, double offsetX, double offsetY)
    {
        if (geometry is not PathGeometry pathGeometry)
        {
            return geometry;
        }

        var figures = new PathFigureCollection();

        foreach (PathFigure figure in pathGeometry.Figures)
        {
            var scaledFigure = new PathFigure
            {
                StartPoint = ScalePoint(figure.StartPoint, scale, offsetX, offsetY),
                IsClosed = figure.IsClosed,
                IsFilled = figure.IsFilled
            };

            var segments = new PathSegmentCollection();
            foreach (PathSegment segment in figure.Segments)
            {
                segments.Add(ScaleSegment(segment, scale, offsetX, offsetY));
            }

            scaledFigure.Segments = segments;
            figures.Add(scaledFigure);
        }

        return new PathGeometry { Figures = figures };
    }

    private static PathSegment ScaleSegment(PathSegment segment, double scale, double offsetX, double offsetY)
    {
        switch (segment)
        {
            case LineSegment line:
                return new LineSegment
                {
                    Point = ScalePoint(line.Point, scale, offsetX, offsetY)
                };
            case PolyLineSegment polyLine:
                return new PolyLineSegment
                {
                    Points = ScalePoints(polyLine.Points, scale, offsetX, offsetY)
                };
            case BezierSegment bezier:
                return new BezierSegment
                {
                    Point1 = ScalePoint(bezier.Point1, scale, offsetX, offsetY),
                    Point2 = ScalePoint(bezier.Point2, scale, offsetX, offsetY),
                    Point3 = ScalePoint(bezier.Point3, scale, offsetX, offsetY)
                };
            case PolyBezierSegment polyBezier:
                return new PolyBezierSegment
                {
                    Points = ScalePoints(polyBezier.Points, scale, offsetX, offsetY)
                };
            case QuadraticBezierSegment quadratic:
                return new QuadraticBezierSegment
                {
                    Point1 = ScalePoint(quadratic.Point1, scale, offsetX, offsetY),
                    Point2 = ScalePoint(quadratic.Point2, scale, offsetX, offsetY)
                };
            case PolyQuadraticBezierSegment polyQuadratic:
                return new PolyQuadraticBezierSegment
                {
                    Points = ScalePoints(polyQuadratic.Points, scale, offsetX, offsetY)
                };
            case ArcSegment arc:
                return new ArcSegment
                {
                    Point = ScalePoint(arc.Point, scale, offsetX, offsetY),
                    Size = new Size(arc.Size.Width * scale, arc.Size.Height * scale),
                    RotationAngle = arc.RotationAngle,
                    IsLargeArc = arc.IsLargeArc,
                    SweepDirection = arc.SweepDirection
                };
            default:
                return segment;
        }
    }

    private static PointCollection ScalePoints(PointCollection points, double scale, double offsetX, double offsetY)
    {
        var scaled = new PointCollection();
        foreach (var point in points)
        {
            scaled.Add(ScalePoint(point, scale, offsetX, offsetY));
        }

        return scaled;
    }

    private static Point ScalePoint(Point point, double scale, double offsetX, double offsetY)
    {
        return new Point(point.X * scale + offsetX, point.Y * scale + offsetY);
    }
}
