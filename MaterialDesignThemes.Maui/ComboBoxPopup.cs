namespace MaterialDesignThemes.Maui;

public enum PopupDirection
{
    None,
    Up,
    Down
}

public sealed class ComboBoxPopup : TemplatedView
{
    public static readonly BindableProperty ClassicContentTemplateProperty = BindableProperty.Create(
        nameof(ClassicContentTemplate),
        typeof(ControlTemplate),
        typeof(ComboBoxPopup),
        null);

    public ControlTemplate? ClassicContentTemplate
    {
        get => (ControlTemplate?)GetValue(ClassicContentTemplateProperty);
        set => SetValue(ClassicContentTemplateProperty, value);
    }

    public static readonly BindableProperty UpVerticalOffsetProperty = BindableProperty.Create(
        nameof(UpVerticalOffset),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double UpVerticalOffset
    {
        get => (double)GetValue(UpVerticalOffsetProperty);
        set => SetValue(UpVerticalOffsetProperty, value);
    }

    public static readonly BindableProperty DownVerticalOffsetProperty = BindableProperty.Create(
        nameof(DownVerticalOffset),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double DownVerticalOffset
    {
        get => (double)GetValue(DownVerticalOffsetProperty);
        set => SetValue(DownVerticalOffsetProperty, value);
    }

    public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(
        nameof(Background),
        typeof(Brush),
        typeof(ComboBoxPopup),
        null);

    public Brush? Background
    {
        get => (Brush?)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static readonly BindableProperty DefaultVerticalOffsetProperty = BindableProperty.Create(
        nameof(DefaultVerticalOffset),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double DefaultVerticalOffset
    {
        get => (double)GetValue(DefaultVerticalOffsetProperty);
        set => SetValue(DefaultVerticalOffsetProperty, value);
    }

    public static readonly BindableProperty VisiblePlacementWidthProperty = BindableProperty.Create(
        nameof(VisiblePlacementWidth),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double VisiblePlacementWidth
    {
        get => (double)GetValue(VisiblePlacementWidthProperty);
        set => SetValue(VisiblePlacementWidthProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(ComboBoxPopup),
        new CornerRadius(0));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly BindableProperty ContentMarginProperty = BindableProperty.Create(
        nameof(ContentMargin),
        typeof(Thickness),
        typeof(ComboBoxPopup),
        new Thickness(0));

    public Thickness ContentMargin
    {
        get => (Thickness)GetValue(ContentMarginProperty);
        set => SetValue(ContentMarginProperty, value);
    }

    public static readonly BindableProperty ContentMinWidthProperty = BindableProperty.Create(
        nameof(ContentMinWidth),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double ContentMinWidth
    {
        get => (double)GetValue(ContentMinWidthProperty);
        set => SetValue(ContentMinWidthProperty, value);
    }

    public static readonly BindableProperty RelativeHorizontalOffsetProperty = BindableProperty.Create(
        nameof(RelativeHorizontalOffset),
        typeof(double),
        typeof(ComboBoxPopup),
        0d);

    public double RelativeHorizontalOffset
    {
        get => (double)GetValue(RelativeHorizontalOffsetProperty);
        set => SetValue(RelativeHorizontalOffsetProperty, value);
    }

    public static readonly BindableProperty OpenDirectionProperty = BindableProperty.Create(
        nameof(OpenDirection),
        typeof(PopupDirection),
        typeof(ComboBoxPopup),
        PopupDirection.None);

    public PopupDirection OpenDirection
    {
        get => (PopupDirection)GetValue(OpenDirectionProperty);
        set => SetValue(OpenDirectionProperty, value);
    }
}
