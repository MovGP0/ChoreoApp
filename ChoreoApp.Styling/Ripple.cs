using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace ChoreoApp.Styling;

[ContentProperty(nameof(RippleContent))]
public sealed class Ripple : ContentView
{
    public const string TemplateStateNormal = "Normal";
    public const string TemplateStateMousePressed = "MousePressed";
    public const string TemplateStateMouseOut = "MouseOut";

    private readonly AbsoluteLayout _layout;
    private readonly ContentPresenter _contentPresenter;
    private readonly Border _ripple;
    private readonly TapGestureRecognizer _tap;
    private readonly PointerGestureRecognizer _pointer;

    private Point _lastPointerPosition;
    private bool _isPointerInside;
    private bool _isAnimating;

    private static readonly BindablePropertyKey RippleSizePropertyKey = BindableProperty.CreateReadOnly(
        nameof(RippleSize),
        typeof(double),
        typeof(Ripple),
        0d);

    public static readonly BindableProperty RippleSizeProperty = RippleSizePropertyKey.BindableProperty;

    private static readonly BindablePropertyKey RippleXPropertyKey = BindableProperty.CreateReadOnly(
        nameof(RippleX),
        typeof(double),
        typeof(Ripple),
        0d);

    public static readonly BindableProperty RippleXProperty = RippleXPropertyKey.BindableProperty;

    private static readonly BindablePropertyKey RippleYPropertyKey = BindableProperty.CreateReadOnly(
        nameof(RippleY),
        typeof(double),
        typeof(Ripple),
        0d);

    public static readonly BindableProperty RippleYProperty = RippleYPropertyKey.BindableProperty;

    public static readonly BindableProperty FeedbackProperty = BindableProperty.Create(
        nameof(Feedback),
        typeof(Color),
        typeof(Ripple),
        null);

    public static readonly BindableProperty RecognizesAccessKeyProperty = BindableProperty.Create(
        nameof(RecognizesAccessKey),
        typeof(bool),
        typeof(Ripple),
        false);

    public static readonly BindableProperty RippleContentProperty = BindableProperty.Create(
        nameof(RippleContent),
        typeof(View),
        typeof(Ripple),
        null);

    public Ripple()
    {
        _layout = new AbsoluteLayout
        {
            IsClippedToBounds = RippleAssist.GetClipToBounds(this)
        };

        _contentPresenter = new ContentPresenter();
        _contentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding(nameof(RippleContent), source: this));

        _ripple = new Border
        {
            Opacity = 0,
            BackgroundColor = Colors.Transparent,
            StrokeThickness = 0
        };

        AbsoluteLayout.SetLayoutFlags(_contentPresenter, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(_contentPresenter, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(_ripple, AbsoluteLayoutFlags.None);

        _layout.Children.Add(_contentPresenter);
        _layout.Children.Add(_ripple);

        Content = _layout;

        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        _pointer = new PointerGestureRecognizer();
        _pointer.PointerMoved += OnPointerMoved;
        _pointer.PointerEntered += OnPointerEntered;
        _pointer.PointerExited += OnPointerExited;
        GestureRecognizers.Add(_pointer);

        SizeChanged += OnSizeChanged;
    }

    public double RippleSize
    {
        get => (double)GetValue(RippleSizeProperty);
        private set => SetValue(RippleSizePropertyKey, value);
    }

    public double RippleX
    {
        get => (double)GetValue(RippleXProperty);
        private set => SetValue(RippleXPropertyKey, value);
    }

    public double RippleY
    {
        get => (double)GetValue(RippleYProperty);
        private set => SetValue(RippleYPropertyKey, value);
    }

    public Color? Feedback
    {
        get => (Color?)GetValue(FeedbackProperty);
        set => SetValue(FeedbackProperty, value);
    }

    public bool RecognizesAccessKey
    {
        get => (bool)GetValue(RecognizesAccessKeyProperty);
        set => SetValue(RecognizesAccessKeyProperty, value);
    }

    public View? RippleContent
    {
        get => (View?)GetValue(RippleContentProperty);
        set => SetValue(RippleContentProperty, value);
    }

    protected override void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == RippleAssist.ClipToBoundsProperty.PropertyName)
        {
            _layout.IsClippedToBounds = RippleAssist.GetClipToBounds(this);
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        try
        {
            _lastPointerPosition = e.GetPosition(this) ?? new Point(Width / 2, Height / 2);
            _isPointerInside = IsInsideBounds(_lastPointerPosition);
        }
        catch
        {
            _lastPointerPosition = new Point(Width / 2, Height / 2);
            _isPointerInside = true;
        }

        if (_isAnimating && !_isPointerInside)
        {
            VisualStateManager.GoToState(this, TemplateStateMouseOut);
        }
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        _isPointerInside = true;
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        _isPointerInside = false;

        if (_isAnimating)
        {
            VisualStateManager.GoToState(this, TemplateStateMouseOut);
        }
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        UpdateRippleSize();
    }

    private void UpdateRippleSize()
    {
        var width = Width;
        var height = Height;

        if (width <= 0 || height <= 0)
        {
            return;
        }

        var radius = Math.Sqrt((width * width) + (height * height));
        RippleSize = 2 * radius * RippleAssist.GetRippleSizeMultiplier(this);
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (RippleAssist.GetIsDisabled(this))
        {
            return;
        }

        var isCentered = RippleAssist.GetIsCentered(this);
        var startPoint = isCentered ? new Point(Width / 2, Height / 2) : _lastPointerPosition;

        if (double.IsNaN(startPoint.X) || double.IsNaN(startPoint.Y))
        {
            startPoint = new Point(Width / 2, Height / 2);
        }

        RippleX = startPoint.X - RippleSize / 2;
        RippleY = startPoint.Y - RippleSize / 2;

        AbsoluteLayout.SetLayoutBounds(_ripple, new Rect(RippleX, RippleY, RippleSize, RippleSize));
        _ripple.BackgroundColor = Feedback ?? RippleAssist.GetFeedback(this) ?? Colors.White;
        _ripple.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(RippleSize / 2) };

        _ripple.Opacity = 0.3;
        _ripple.Scale = 0.1;

        var rippleOnTop = RippleAssist.GetRippleOnTop(this);
        if (rippleOnTop && _layout.Children.LastOrDefault() != _ripple)
        {
            _layout.Children.Remove(_ripple);
            _layout.Children.Add(_ripple);
        }

        VisualStateManager.GoToState(this, TemplateStateMousePressed);
        _isAnimating = true;

        var scaleTask = _ripple.ScaleTo(1, 250, Easing.CubicOut);
        var fadeTask = _ripple.FadeTo(0, 300, Easing.CubicOut);
        await Task.WhenAll(scaleTask, fadeTask);

        _ripple.Opacity = 0;
        VisualStateManager.GoToState(this, TemplateStateNormal);
        _isAnimating = false;
    }

    private bool IsInsideBounds(Point point)
    {
        return point.X >= 0 && point.Y >= 0 && point.X <= Width && point.Y <= Height;
    }
}
