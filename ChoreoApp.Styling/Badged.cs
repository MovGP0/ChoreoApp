using Microsoft.Maui.Graphics;

namespace ChoreoApp.Styling;

public enum BadgePlacementMode
{
    TopLeft,
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left
}

public sealed class BadgeChangedEventArgs(object? oldValue, object? newValue) : EventArgs
{
    public object? OldValue { get; } = oldValue;
    public object? NewValue { get; } = newValue;
}

[ContentProperty(nameof(Content))]
public sealed class Badged : ContentView
{
    public const string BadgeContainerPartName = "PART_BadgeContainer";

    private const double DefaultBadgeAnimationScale = 1.4;
    private static readonly TimeSpan DefaultBadgeAnimationDuration = TimeSpan.FromMilliseconds(300);
    private static readonly CornerRadius DefaultBadgeCornerRadius = new(9);

    private View? _badgeContainer;

    public static readonly BindableProperty BadgeProperty =
        BindableProperty.Create(
            nameof(Badge),
            typeof(object),
            typeof(Badged),
            null,
            propertyChanged: OnBadgeChanged);

    public object? Badge
    {
        get => GetValue(BadgeProperty);
        set => SetValue(BadgeProperty, value);
    }

    public static readonly BindableProperty BadgeBackgroundProperty =
        BindableProperty.Create(
            nameof(BadgeBackground),
            typeof(Color),
            typeof(Badged),
            null);

    public Color? BadgeBackground
    {
        get => (Color?)GetValue(BadgeBackgroundProperty);
        set => SetValue(BadgeBackgroundProperty, value);
    }

    public static readonly BindableProperty BadgeForegroundProperty =
        BindableProperty.Create(
            nameof(BadgeForeground),
            typeof(Color),
            typeof(Badged),
            null);

    public Color? BadgeForeground
    {
        get => (Color?)GetValue(BadgeForegroundProperty);
        set => SetValue(BadgeForegroundProperty, value);
    }

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(Badged),
            null);

    public Brush? Stroke
    {
        get => (Brush?)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(
            nameof(StrokeThickness),
            typeof(double),
            typeof(Badged),
            0d);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly BindableProperty BadgePlacementModeProperty =
        BindableProperty.Create(
            nameof(BadgePlacementMode),
            typeof(BadgePlacementMode),
            typeof(Badged),
            BadgePlacementMode.TopRight);

    public BadgePlacementMode BadgePlacementMode
    {
        get => (BadgePlacementMode)GetValue(BadgePlacementModeProperty);
        set => SetValue(BadgePlacementModeProperty, value);
    }

    private static readonly BindablePropertyKey IsBadgeSetPropertyKey =
        BindableProperty.CreateReadOnly(
            nameof(IsBadgeSet),
            typeof(bool),
            typeof(Badged),
            false);

    public static readonly BindableProperty IsBadgeSetProperty =
        IsBadgeSetPropertyKey.BindableProperty;

    public bool IsBadgeSet
    {
        get => (bool)GetValue(IsBadgeSetProperty);
        private set => SetValue(IsBadgeSetPropertyKey, value);
    }

    public event EventHandler<BadgeChangedEventArgs>? BadgeChanged;

    public static readonly BindableProperty BadgeChangedAnimationDurationProperty =
        BindableProperty.Create(
            nameof(BadgeChangedAnimationDuration),
            typeof(TimeSpan),
            typeof(Badged),
            DefaultBadgeAnimationDuration);

    public TimeSpan BadgeChangedAnimationDuration
    {
        get => (TimeSpan)GetValue(BadgeChangedAnimationDurationProperty);
        set => SetValue(BadgeChangedAnimationDurationProperty, value);
    }

    public static readonly BindableProperty BadgeChangedAnimationScaleProperty =
        BindableProperty.Create(
            nameof(BadgeChangedAnimationScale),
            typeof(double),
            typeof(Badged),
            DefaultBadgeAnimationScale);

    public double BadgeChangedAnimationScale
    {
        get => (double)GetValue(BadgeChangedAnimationScaleProperty);
        set => SetValue(BadgeChangedAnimationScaleProperty, value);
    }

    public static readonly BindableProperty BadgeColorZoneModeProperty =
        BindableProperty.Create(
            nameof(BadgeColorZoneMode),
            typeof(ColorZoneMode),
            typeof(Badged),
            ColorZoneMode.PrimaryLight);

    public ColorZoneMode BadgeColorZoneMode
    {
        get => (ColorZoneMode)GetValue(BadgeColorZoneModeProperty);
        set => SetValue(BadgeColorZoneModeProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(Badged),
            DefaultBadgeCornerRadius);

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_badgeContainer != null)
        {
            _badgeContainer.SizeChanged -= OnBadgeContainerSizeChanged;
        }

        _badgeContainer = GetTemplateChild(BadgeContainerPartName) as View;

        if (_badgeContainer != null)
        {
            _badgeContainer.SizeChanged += OnBadgeContainerSizeChanged;
            UpdateBadgeContainerMargin();
        }
    }

    private static void OnBadgeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Badged badged)
        {
            return;
        }

        badged.IsBadgeSet = newValue is string text
            ? !string.IsNullOrWhiteSpace(text)
            : newValue is not null;

        badged.BadgeChanged?.Invoke(badged, new BadgeChangedEventArgs(oldValue, newValue));
        _ = badged.RunBadgeChangedAnimationAsync();
    }

    private void OnBadgeContainerSizeChanged(object? sender, EventArgs e)
    {
        UpdateBadgeContainerMargin();
    }

    private void UpdateBadgeContainerMargin()
    {
        if (_badgeContainer is null)
        {
            return;
        }

        var width = _badgeContainer.Width;
        var height = _badgeContainer.Height;

        if (width <= 0 || height <= 0)
        {
            return;
        }

        var horizontal = -width / 2;
        var vertical = -height / 2;
        _badgeContainer.Margin = new Thickness(horizontal, vertical, horizontal, vertical);
    }

    private async Task RunBadgeChangedAnimationAsync()
    {
        if (_badgeContainer is null || !IsBadgeSet)
        {
            return;
        }

        var targetScale = BadgeChangedAnimationScale;
        var duration = BadgeChangedAnimationDuration;
        if (duration.TotalMilliseconds <= 0)
        {
            return;
        }

        _badgeContainer.AbortAnimation("BadgeChangedAnimation");
        _badgeContainer.Scale = 1;

        var halfDuration = (uint)Math.Max(1, duration.TotalMilliseconds / 2);
        await _badgeContainer.ScaleTo(targetScale, halfDuration, Easing.SinOut);
        await _badgeContainer.ScaleTo(1, halfDuration, Easing.SinOut);
    }
}
