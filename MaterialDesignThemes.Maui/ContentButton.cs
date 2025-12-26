using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// A simple MAUI button-like control that supports arbitrary content (e.g., PackIcon + text).
/// Implements Command/CommandParameter and Clicked event. Uses a <see cref="Border"/> host.
/// </summary>
[ContentProperty(nameof(ButtonContent))]
public sealed partial class ContentButton : ContentView
{
    private const string ForegroundResourceKey = "ContentButton.Foreground";
    private const string ForegroundBrushResourceKey = "ContentButton.ForegroundBrush";
    private const string FontSizeResourceKey = "ContentButton.FontSize";
    private const string FontAttributesResourceKey = "ContentButton.FontAttributes";
    private const string FontFamilyResourceKey = "ContentButton.FontFamily";

    private readonly Border _border;
    private readonly TapGestureRecognizer _tap;
    private readonly PointerGestureRecognizer _pointer;
    private readonly Grid _contentHost;
    private readonly ResourceDictionary _instanceResources = new();
    private readonly ContentPresenter _contentPresenter;

    public ContentButton()
    {
        _border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
            Padding = new Thickness(12, 10),
            BackgroundColor = Colors.Transparent,
            StrokeThickness = 0
        };

        _contentPresenter = new ContentPresenter();
        _contentPresenter.SetBinding(HorizontalOptionsProperty, new Binding(nameof(HorizontalContentAlignment), source: this));
        _contentPresenter.SetBinding(VerticalOptionsProperty, new Binding(nameof(VerticalContentAlignment), source: this));

        _contentHost = new Grid();
        ApplyContentResources(null);
        _contentHost.Children.Add(_contentPresenter);

        _border.Content = _contentHost;
        _border.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background), source: this));
        _border.SetBinding(Border.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this));
        _border.SetBinding(Border.PaddingProperty, new Binding(nameof(Padding), source: this));
        _border.SetBinding(Border.StrokeProperty, new Binding(nameof(Stroke), source: this));
        _border.SetBinding(Border.StrokeThicknessProperty, new Binding(nameof(StrokeThickness), source: this));

        Content = _border;

        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        _pointer = new PointerGestureRecognizer();
        _pointer.PointerEntered += OnPointerEntered;
        _pointer.PointerExited += OnPointerExited;
        GestureRecognizers.Add(_pointer);

        UpdateCornerRadius();
        UpdateEnabledState();
        UpdateForegroundBrush();
    }

    public event EventHandler? Clicked;

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(ContentButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(ContentButton));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty ButtonContentProperty =
        BindableProperty.Create(
            nameof(ButtonContent),
            typeof(View),
            typeof(ContentButton),
            propertyChanged: OnButtonContentChanged);

    public View? ButtonContent
    {
        get => (View?)GetValue(ButtonContentProperty);
        set => SetValue(ButtonContentProperty, value);
    }

    public static readonly BindableProperty HorizontalContentAlignmentProperty =
        BindableProperty.Create(
            nameof(HorizontalContentAlignment),
            typeof(LayoutOptions),
            typeof(ContentButton),
            LayoutOptions.Center);

    public LayoutOptions HorizontalContentAlignment
    {
        get => (LayoutOptions)GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }

    public static readonly BindableProperty VerticalContentAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalContentAlignment),
            typeof(LayoutOptions),
            typeof(ContentButton),
            LayoutOptions.Center);

    public static readonly BindableProperty ResourcesProperty =
        BindableProperty.Create(
            nameof(Resources),
            typeof(ResourceDictionary),
            typeof(ContentButton),
            propertyChanged: OnResourcesChanged);

    public new ResourceDictionary? Resources
    {
        get => (ResourceDictionary?)GetValue(ResourcesProperty);
        set => SetValue(ResourcesProperty, value);
    }

    public LayoutOptions VerticalContentAlignment
    {
        get => (LayoutOptions)GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(
            nameof(Foreground),
            typeof(Color),
            typeof(ContentButton));

    public Color? Foreground
    {
        get => (Color?)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static readonly BindableProperty ForegroundBrushProperty =
        BindableProperty.Create(
            nameof(ForegroundBrush),
            typeof(Brush),
            typeof(ContentButton));

    public Brush? ForegroundBrush
    {
        get => (Brush?)GetValue(ForegroundBrushProperty);
        set => SetValue(ForegroundBrushProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(ContentButton),
            14d);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(
            nameof(FontAttributes),
            typeof(FontAttributes),
            typeof(ContentButton),
            FontAttributes.None);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(ContentButton));

    public string? FontFamily
    {
        get => (string?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(ContentButton));

    public Brush? Stroke
    {
        get => (Brush?)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(
            nameof(StrokeThickness),
            typeof(double),
            typeof(ContentButton),
            0d);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly BindableProperty PressedOpacityProperty =
        BindableProperty.Create(
            nameof(PressedOpacity),
            typeof(double),
            typeof(ContentButton),
            0.8);

    public double PressedOpacity
    {
        get => (double)GetValue(PressedOpacityProperty);
        set => SetValue(PressedOpacityProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(ContentButton),
            8f,
            propertyChanged: OnCornerRadiusChanged);

    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            UpdateEnabledState();
        }

        if (propertyName == ForegroundProperty.PropertyName)
        {
            UpdateForegroundBrush();
        }

        if (propertyName == ForegroundBrushProperty.PropertyName
            || propertyName == FontSizeProperty.PropertyName
            || propertyName == FontAttributesProperty.PropertyName
            || propertyName == FontFamilyProperty.PropertyName)
        {
            UpdateContentResourceValues();
        }
    }

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ContentButton button && newValue is float radius)
        {
            button.UpdateCornerRadius();
        }
    }

    private static void OnButtonContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ContentButton button)
        {
            button.UpdateContent(newValue as View);
        }
    }

    private void UpdateCornerRadius()
    {
        _border.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(CornerRadius)
        };
    }

    private void UpdateContent(View? content)
    {
        if (content is null)
        {
            _contentPresenter.Content = null;
            return;
        }

        if (content.Parent is Layout layout)
        {
            layout.Children.Remove(content);
        }
        else if (content.Parent is ContentView contentView)
        {
            contentView.Content = null;
        }
        else if (content.Parent is Border border)
        {
            border.Content = null;
        }
        else if (content.Parent is ContentPresenter presenter)
        {
            presenter.Content = null;
        }

        _contentPresenter.Content = content;
    }

    private void UpdateEnabledState()
    {
        VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled");
    }

    private void UpdateForegroundBrush()
    {
        ForegroundBrush = Foreground is null ? null : new SolidColorBrush(Foreground);
        UpdateContentResourceValues();
    }

    private static void OnResourcesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ContentButton button)
        {
            button.ApplyContentResources(newValue as ResourceDictionary);
        }
    }

    private void ApplyContentResources(ResourceDictionary? resources)
    {
        var resolvedResources = new ResourceDictionary();

        if (resources is not null)
        {
            resolvedResources.MergedDictionaries.Add(resources);
        }

        resolvedResources.MergedDictionaries.Add(_instanceResources);
        UpdateContentResourceValues();

        _contentHost.Resources = resolvedResources;
        _border.Resources = resolvedResources;
        base.Resources = resolvedResources;
    }

    private void UpdateContentResourceValues()
    {
        _instanceResources[ForegroundResourceKey] = Foreground ?? Colors.Transparent;
        _instanceResources[ForegroundBrushResourceKey] = ForegroundBrush ?? new SolidColorBrush(Colors.Transparent);
        _instanceResources[FontSizeResourceKey] = FontSize;
        _instanceResources[FontAttributesResourceKey] = FontAttributes;
        _instanceResources[FontFamilyResourceKey] = FontFamily;
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "PointerOver");
        }
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "Normal");
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        _ = OnTappedAsync();
    }

    private async Task OnTappedAsync()
    {
        if (!IsEnabled)
        {
            return;
        }

        var cmd = Command;
        var param = CommandParameter;

        VisualStateManager.GoToState(this, "Pressed");

        // brief visual feedback
        var originalOpacity = Opacity;
        Opacity = PressedOpacity;
        await this.FadeToAsync(originalOpacity, 90, Easing.CubicOut);

        if (cmd?.CanExecute(param) == true)
        {
            cmd.Execute(param);
        }

        Clicked?.Invoke(this, EventArgs.Empty);

        VisualStateManager.GoToState(this, "Normal");
    }
}
