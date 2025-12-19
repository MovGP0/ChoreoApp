using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Scenes;
using ChoreoApp.Styling;
using ChoreoMasterMobile.Json;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Behaviors;

public sealed class DrawFloorBehavior(
    GlobalStateModel globalState,
    ISubscriber<DrawFloorCommand> drawFloorCommandSubscriber,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber) : IBehavior<FloorCanvasViewModel>
{
    private readonly Dictionary<int, SKColor> _roleBorderColors = new();
    private FloorCanvasViewModel? _viewModel;
    private Scenes.SceneViewModel? _selectedScene;

    private static SKColor GetColor(string resourceKey)
    {
        var resources = Application.Current?.Resources;
        if (resources is null
            || !resources.TryGetValue(resourceKey, out var resource)
            || resource is not Color color)
        {
            return SKColors.Transparent;
        }

        return color.ToSKColor();
    }

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        _viewModel = viewModel;
        Disposable
            .Create(() => _viewModel = null)
            .DisposeWith(disposables);

        drawFloorCommandSubscriber
            .Subscribe(command => DrawFloor(command.SurfaceEventArgs))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(evt => _selectedScene = evt.SelectedScene)
            .DisposeWith(disposables);
    }

    private void DrawFloor(SKPaintSurfaceEventArgs args)
    {
        var canvas = args.Surface.Canvas;
        SKColor surfaceColor = GetColor(MaterialDesignColorKey.Surface);
        canvas.Clear(surfaceColor);

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        var floor = choreography.Floor;
        var settings = choreography.Settings;

        float sizeFromCenterToFront = floor.SizeFront;
        float sizeFromCenterToBack = floor.SizeBack;
        float sizeFromCenterToLeft = floor.SizeLeft;
        float sizeFromCenterToRight = floor.SizeRight;

        float canvasWidth = args.Info.Width;
        float canvasHeight = args.Info.Height;
        var scale = CalculateScaleFactor();

        float centerX = canvasWidth / 2f;
        float centerY = canvasHeight / 2f;
        float left = centerX - sizeFromCenterToLeft * scale;
        float right = centerX + sizeFromCenterToRight * scale;
        float top = centerY - sizeFromCenterToFront * scale;
        float bottom = centerY + sizeFromCenterToBack * scale;

        var floorRect = new SKRect(left, top, right, bottom);
        _viewModel?.UpdateFloorBounds(floorRect, new SKSize(canvasWidth, canvasHeight));

        var transformationMatrix = _viewModel?.TransformationMatrix ?? SKMatrix.CreateIdentity();
        canvas.Save();
        canvas.SetMatrix(in transformationMatrix);

        DrawFloorRectangle();
        if (settings.GridLines)
        {
            DrawGridLines();
        }
        var scenePositions = GetScenePositions();

        DrawCenter();
        DrawFloorBorder();
        if (scenePositions is not null)
        {
            DrawAxisLabels(scenePositions);
        }
        DrawPositions(scenePositions);

        canvas.Restore();

        void DrawGridLines()
        {
            canvas.Save();
            canvas.ClipRect(floorRect, SKClipOperation.Intersect, true);

            using var gridPaint = new SKPaint();

            SKColor secondaryColor = GetColor(MaterialDesignColorKey.Secondary);
            gridPaint.Color = secondaryColor;
            gridPaint.Style = SKPaintStyle.Stroke;
            gridPaint.IsAntialias = true;
            gridPaint.StrokeWidth = 1f;

            int maxHorizontalMeters = Math.Max(floor.SizeLeft, floor.SizeRight);
            for (int meter = 1; meter <= maxHorizontalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= sizeFromCenterToLeft * scale)
                {
                    float x = centerX - offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }

                if (offset <= sizeFromCenterToRight * scale)
                {
                    float x = centerX + offset;
                    canvas.DrawLine(x, top, x, bottom, gridPaint);
                }
            }

            int maxVerticalMeters = Math.Max(floor.SizeFront, floor.SizeBack);
            for (int meter = 1; meter <= maxVerticalMeters; meter++)
            {
                float offset = meter * scale;

                if (offset <= sizeFromCenterToFront * scale)
                {
                    float y = centerY - offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }

                if (offset <= sizeFromCenterToBack * scale)
                {
                    float y = centerY + offset;
                    canvas.DrawLine(left, y, right, y, gridPaint);
                }
            }

            canvas.Restore();
        }

        float CalculateScaleFactor()
        {
            float padding = 46f; // pixels
            float scaleX = (canvasWidth - 2 * padding) /
                           (sizeFromCenterToLeft + sizeFromCenterToRight);

            float scaleY = (canvasHeight - 2 * padding) /
                           (sizeFromCenterToFront + sizeFromCenterToBack);

            return Math.Min(scaleX, scaleY);
        }

        void DrawCenter()
        {
            using var centerPaint = new SKPaint();
            centerPaint.Color = GetColor(MaterialDesignColorKey.Primary);
            centerPaint.Style = SKPaintStyle.StrokeAndFill;
            centerPaint.IsAntialias = true;
            centerPaint.StrokeWidth = 2f;

            canvas.DrawLine(left, centerY, right, centerY, centerPaint);
            canvas.DrawLine(centerX, top, centerX, bottom, centerPaint);
            canvas.DrawCircle(centerX, centerY, 4f, centerPaint);
        }

        void DrawFloorBorder()
        {
            using var borderPaint = new SKPaint();
            borderPaint.Color = GetColor(MaterialDesignColorKey.Primary);
            borderPaint.Style = SKPaintStyle.Stroke;
            borderPaint.IsAntialias = true;
            borderPaint.StrokeWidth = 2f;
            canvas.DrawRect(floorRect, borderPaint);
        }

        void DrawAxisLabels(IReadOnlyList<Position> positions)
        {
            SKColor labelColor = GetColor(MaterialDesignColorKey.SurfaceBright);
            using var labelPaint = new SKPaint();
            labelPaint.Color = labelColor;
            labelPaint.IsAntialias = true;

            using var font = new SKFont();
            font.Size = 16f;
            font.Edging = SKFontEdging.Antialias;

            const float labelOffset = 12f;
            float topLabelY = top - labelOffset;
            float bottomLabelY = bottom + labelOffset;
            float leftLabelX = left - labelOffset;
            float rightLabelX = right + labelOffset;

            var xValues = new HashSet<double>();
            var yValues = new HashSet<double>();

            foreach (var position in positions)
            {
                xValues.Add(position.X);
                yValues.Add(position.Y);
            }

            foreach (var x in xValues.OrderBy(value => value))
            {
                float labelX = centerX + (float)x * scale;
                var text = x.ToString("0.##", CultureInfo.CurrentUICulture);
                DrawLabel(text, labelX, topLabelY, SKTextAlign.Center);
                DrawLabel(text, labelX, bottomLabelY, SKTextAlign.Center);
            }

            foreach (var y in yValues.OrderBy(value => value))
            {
                float labelY = centerY - (float)y * scale;
                var text = y.ToString("0.##", CultureInfo.CurrentUICulture);
                DrawLabel(text, leftLabelX, labelY, SKTextAlign.Right);
                DrawLabel(text, rightLabelX, labelY, SKTextAlign.Left);
            }

            void DrawLabel(string text, float x, float y, SKTextAlign align)
            {
                float textY = y + font.Metrics.CapHeight / 2f;
                canvas.DrawText(text, x, textY, align, font, labelPaint);
            }
        }

        void DrawFloorRectangle()
        {
            SKColor floorColor = settings.FloorColor.ToSKColor();
            using var floorPaint = new SKPaint();
            floorPaint.Color = floorColor;
            floorPaint.Style = SKPaintStyle.Fill;
            floorPaint.IsAntialias = true;
            canvas.DrawRect(floorRect, floorPaint);
        }

        void DrawPositions(IReadOnlyList<Position>? positions)
        {
            if (positions is null)
            {
                return;
            }

            var diameter = (float)settings.DancerSize;
            float radius = diameter / 2f * scale;

            using var fillPaint = new SKPaint();
            fillPaint.Style = SKPaintStyle.Fill;
            fillPaint.IsAntialias = true;

            using var borderPaint = new SKPaint();
            borderPaint.Style = SKPaintStyle.Stroke;
            borderPaint.IsAntialias = true;
            borderPaint.StrokeWidth = 2f;

            using var textPaint = new SKPaint();
            textPaint.Color = SKColors.White;
            textPaint.IsAntialias = true;

            using var font = new SKFont();
            font.Size = 24f;
            font.Edging = SKFontEdging.Antialias;

            foreach (var position in positions)
            {
                DrawPosition(position);
            }

            void DrawPosition(Position position)
            {
                var x = centerX + (float)position.X * scale;
                var y = centerY - (float)position.Y * scale;

                fillPaint.Color = position.Dancer.Color.ToSKColor();
                textPaint.Color = PickBlackOrWhite(fillPaint.Color) switch
                {
                    BlackOrWhite.White => GetColor(MaterialDesignColorKey.White),
                    BlackOrWhite.Black => GetColor(MaterialDesignColorKey.Black),
                    _ => throw new ArgumentOutOfRangeException()
                };
                borderPaint.Color = GetRoleBorderColor(position.Dancer.Role);

                canvas.DrawCircle(x, y, radius, fillPaint);
                canvas.DrawCircle(x, y, radius, borderPaint);

                var shortcut = position.Dancer.Shortcut;
                if (string.IsNullOrWhiteSpace(shortcut))
                {
                    return;
                }

                var textY = y + font.Metrics.CapHeight / 2f;
                canvas.DrawText(shortcut, x, textY, SKTextAlign.Center, font, textPaint);
            }
        }

        IReadOnlyList<Position>? GetScenePositions()
        {
            if (_selectedScene is null
                || choreography.Scenes is not { } scenes)
            {
                return null;
            }

            var scene = scenes.FirstOrDefault(s => string.Equals(s.Name, _selectedScene.Name, StringComparison.Ordinal));
            if (scene?.Positions is null)
            {
                return [];
            }

            return scene.Positions.ToList();
        }

        SKColor GetRoleBorderColor(Role role)
        {
            if (_roleBorderColors.TryGetValue(role.ZIndex, out var cached))
            {
                return cached;
            }

            var color = role.Color.ToSKColor();
            _roleBorderColors[role.ZIndex] = color;
            return color;
        }
    }

    private enum BlackOrWhite
    {
        Black,
        White
    }

    private static BlackOrWhite PickBlackOrWhite(SKColor color)
    {
        float luminance = RelativeLuminance(color.Red, color.Green, color.Blue);

        // explicit contrast check
        float contrastBlack = (luminance + 0.05f) / 0.05f;
        float contrastWhite = 1.05f / (luminance + 0.05f);
        return contrastWhite > contrastBlack
            ? BlackOrWhite.White
            : BlackOrWhite.Black;

        // Equivalent shortcut threshold:
        // return luminance < 0.179
        //     ? BlackOrWhite.White
        //     : BlackOrWhite.Black;
    }

    private static float RelativeLuminance(byte r8, byte g8, byte b8)
    {
        float r = LinearizeChannel(r8 / 255.0f);
        float g = LinearizeChannel(g8 / 255.0f);
        float b = LinearizeChannel(b8 / 255.0f);

        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }

    private static float LinearizeChannel(float srgb)
        => srgb <= 0.04045f
            ? srgb / 12.92f
            : MathF.Pow((srgb + 0.055f) / 1.055f, 2.4f);
}
