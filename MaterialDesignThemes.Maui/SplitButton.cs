using System.Globalization;
using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Defines how the popup content aligns to the split button.
/// </summary>
public enum PopupBoxPlacementMode
{
    BottomAndAlignLeftEdges,
    BottomAndAlignRightEdges,
    BottomAndAlignCentres,
    TopAndAlignLeftEdges,
    TopAndAlignRightEdges,
    TopAndAlignCentres,
    LeftAndAlignTopEdges,
    LeftAndAlignBottomEdges,
    LeftAndAlignMiddles,
    RightAndAlignTopEdges,
    RightAndAlignBottomEdges,
    RightAndAlignMiddles
}

[ContentProperty(nameof(Content))]
public class SplitButton : TemplatedView
{
    public const string PrimaryButtonPartName = "PART_PrimaryButton";
    public const string RightButtonPartName = "PART_RightButton";
    public const string PopupContentHostPartName = "PART_PopupContentHost";

    private ContentButton? _primaryButton;
    private ContentButton? _rightButton;
    private Border? _popupContentHost;

    public event EventHandler? Clicked;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(SplitButton),
        propertyChanged: OnCommandChanged);

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(SplitButton),
        propertyChanged: OnCommandParameterChanged);

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty PopupPlacementModeProperty = BindableProperty.Create(
        nameof(PopupPlacementMode),
        typeof(PopupBoxPlacementMode),
        typeof(SplitButton),
        default(PopupBoxPlacementMode));

    public PopupBoxPlacementMode PopupPlacementMode
    {
        get => (PopupBoxPlacementMode)GetValue(PopupPlacementModeProperty);
        set => SetValue(PopupPlacementModeProperty, value);
    }

    public static readonly BindableProperty PopupElevationProperty = BindableProperty.Create(
        nameof(PopupElevation),
        typeof(Elevation),
        typeof(SplitButton),
        default(Elevation));

    public Elevation PopupElevation
    {
        get => (Elevation)GetValue(PopupElevationProperty);
        set => SetValue(PopupElevationProperty, value);
    }

    public static readonly BindableProperty PopupUniformCornerRadiusProperty = BindableProperty.Create(
        nameof(PopupUniformCornerRadius),
        typeof(double),
        typeof(SplitButton),
        0d,
        propertyChanged: OnPopupUniformCornerRadiusChanged);

    public double PopupUniformCornerRadius
    {
        get => (double)GetValue(PopupUniformCornerRadiusProperty);
        set => SetValue(PopupUniformCornerRadiusProperty, value);
    }

    public static readonly BindableProperty PopupContentProperty = BindableProperty.Create(
        nameof(PopupContent),
        typeof(object),
        typeof(SplitButton),
        propertyChanged: OnPopupContentChanged);

    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public static readonly BindableProperty PopupContentStringFormatProperty = BindableProperty.Create(
        nameof(PopupContentStringFormat),
        typeof(string),
        typeof(SplitButton),
        propertyChanged: OnPopupContentChanged);

    public string? PopupContentStringFormat
    {
        get => (string?)GetValue(PopupContentStringFormatProperty);
        set => SetValue(PopupContentStringFormatProperty, value);
    }

    public static readonly BindableProperty PopupContentTemplateProperty = BindableProperty.Create(
        nameof(PopupContentTemplate),
        typeof(DataTemplate),
        typeof(SplitButton),
        propertyChanged: OnPopupContentChanged);

    public DataTemplate? PopupContentTemplate
    {
        get => (DataTemplate?)GetValue(PopupContentTemplateProperty);
        set => SetValue(PopupContentTemplateProperty, value);
    }

    public static readonly BindableProperty PopupContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(PopupContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(SplitButton),
        propertyChanged: OnPopupContentChanged);

    public DataTemplateSelector? PopupContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(PopupContentTemplateSelectorProperty);
        set => SetValue(PopupContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty SplitContentProperty = BindableProperty.Create(
        nameof(SplitContent),
        typeof(object),
        typeof(SplitButton),
        propertyChanged: OnSplitContentChanged);

    public object? SplitContent
    {
        get => GetValue(SplitContentProperty);
        set => SetValue(SplitContentProperty, value);
    }

    public static readonly BindableProperty SplitContentStringFormatProperty = BindableProperty.Create(
        nameof(SplitContentStringFormat),
        typeof(string),
        typeof(SplitButton),
        propertyChanged: OnSplitContentChanged);

    public string? SplitContentStringFormat
    {
        get => (string?)GetValue(SplitContentStringFormatProperty);
        set => SetValue(SplitContentStringFormatProperty, value);
    }

    public static readonly BindableProperty SplitContentTemplateProperty = BindableProperty.Create(
        nameof(SplitContentTemplate),
        typeof(DataTemplate),
        typeof(SplitButton),
        propertyChanged: OnSplitContentChanged);

    public DataTemplate? SplitContentTemplate
    {
        get => (DataTemplate?)GetValue(SplitContentTemplateProperty);
        set => SetValue(SplitContentTemplateProperty, value);
    }

    public static readonly BindableProperty SplitContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(SplitContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(SplitButton),
        propertyChanged: OnSplitContentChanged);

    public DataTemplateSelector? SplitContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(SplitContentTemplateSelectorProperty);
        set => SetValue(SplitContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty ButtonStyleProperty = BindableProperty.Create(
        nameof(ButtonStyle),
        typeof(Style),
        typeof(SplitButton),
        propertyChanged: OnButtonStyleChanged);

    public Style? ButtonStyle
    {
        get => (Style?)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }

    public static readonly BindableProperty IsPopupOpenProperty = BindableProperty.Create(
        nameof(IsPopupOpen),
        typeof(bool),
        typeof(SplitButton),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsPopupOpenChanged);

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(object),
        typeof(SplitButton),
        propertyChanged: OnContentChanged);

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty ContentStringFormatProperty = BindableProperty.Create(
        nameof(ContentStringFormat),
        typeof(string),
        typeof(SplitButton),
        propertyChanged: OnContentChanged);

    public string? ContentStringFormat
    {
        get => (string?)GetValue(ContentStringFormatProperty);
        set => SetValue(ContentStringFormatProperty, value);
    }

    public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(
        nameof(ContentTemplate),
        typeof(DataTemplate),
        typeof(SplitButton),
        propertyChanged: OnContentChanged);

    public DataTemplate? ContentTemplate
    {
        get => (DataTemplate?)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public static readonly BindableProperty ContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(ContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(SplitButton),
        propertyChanged: OnContentChanged);

    public DataTemplateSelector? ContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(ContentTemplateSelectorProperty);
        set => SetValue(ContentTemplateSelectorProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_primaryButton is not null)
        {
            _primaryButton.Clicked -= OnPrimaryClicked;
        }

        if (_rightButton is not null)
        {
            _rightButton.Clicked -= OnRightButtonClicked;
        }

        _primaryButton = GetTemplateChild(PrimaryButtonPartName) as ContentButton;
        _rightButton = GetTemplateChild(RightButtonPartName) as ContentButton;
        _popupContentHost = GetTemplateChild(PopupContentHostPartName) as Border;

        if (_primaryButton is not null)
        {
            _primaryButton.Clicked += OnPrimaryClicked;
            ApplyButtonStyle();
            UpdateCommand();
            UpdatePrimaryContent();
            UpdateSplitContent();
            _primaryButton.IsEnabled = IsEnabled;
        }

        if (_rightButton is not null)
        {
            _rightButton.Clicked += OnRightButtonClicked;
            ApplyButtonStyle();
            _rightButton.IsEnabled = IsEnabled;
        }

        UpdatePopupShape();
        UpdatePopupContent();
        UpdatePopupVisibility();
    }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == IsEnabledProperty.PropertyName)
        {
            UpdateEnabledState();
        }
    }

    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdateCommand();
        }
    }

    private static void OnCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdateCommand();
        }
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdatePrimaryContent();
        }
    }

    private static void OnSplitContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdateSplitContent();
        }
    }

    private static void OnPopupContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdatePopupContent();
        }
    }

    private static void OnPopupUniformCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdatePopupShape();
        }
    }

    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.ApplyButtonStyle();
        }
    }

    private static void OnIsPopupOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SplitButton splitButton)
        {
            splitButton.UpdatePopupVisibility();
        }
    }

    private void UpdateEnabledState()
    {
        if (_primaryButton is not null)
        {
            _primaryButton.IsEnabled = IsEnabled;
        }

        if (_rightButton is not null)
        {
            _rightButton.IsEnabled = IsEnabled;
        }
    }

    private void UpdateCommand()
    {
        if (_primaryButton is null)
        {
            return;
        }

        _primaryButton.Command = Command;
        _primaryButton.CommandParameter = CommandParameter;
    }

    private void ApplyButtonStyle()
    {
        if (ButtonStyle is null)
        {
            return;
        }

        if (_primaryButton is not null)
        {
            _primaryButton.Style = ButtonStyle;
        }

        if (_rightButton is not null)
        {
            _rightButton.Style = ButtonStyle;
        }
    }

    private void UpdateSplitContent()
    {
        if (_rightButton is null)
        {
            return;
        }

        UpdateDefaultSplitContent();

        _rightButton.ButtonContent = CreateContentView(
            SplitContent,
            SplitContentTemplate,
            SplitContentTemplateSelector,
            SplitContentStringFormat);
    }

    private void UpdatePopupContent()
    {
        if (_popupContentHost is null)
        {
            return;
        }

        _popupContentHost.Content = CreateContentView(
            PopupContent,
            PopupContentTemplate,
            PopupContentTemplateSelector,
            PopupContentStringFormat);
    }

    private void UpdatePopupShape()
    {
        if (_popupContentHost is null)
        {
            return;
        }

        _popupContentHost.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(PopupUniformCornerRadius)
        };
    }

    private void UpdatePopupVisibility()
    {
        if (_popupContentHost is not null)
        {
            _popupContentHost.IsVisible = IsPopupOpen;
        }
    }

    private void UpdatePrimaryContent()
    {
        if (_primaryButton is null)
        {
            return;
        }

        _primaryButton.ButtonContent = CreateContentView(
            Content,
            ContentTemplate,
            ContentTemplateSelector,
            ContentStringFormat);
    }

    private void UpdateDefaultSplitContent()
    {
        if (SplitContent is null)
        {
            SplitContent = new PackIcon { Kind = PackIconKind.ChevronDown };
        }
    }

    private static View? CreateContentView(
        object? content,
        DataTemplate? template,
        DataTemplateSelector? templateSelector,
        string? stringFormat)
    {
        if (content is null)
        {
            return null;
        }

        var resolvedTemplate = templateSelector?.SelectTemplate(content, null) ?? template;
        if (resolvedTemplate is not null)
        {
            var created = resolvedTemplate.CreateContent();
            if (created is View view)
            {
                view.BindingContext = content;
                return view;
            }

            if (created is ViewCell cell)
            {
                cell.BindingContext = content;
                return cell.View;
            }
        }

        if (content is View contentView)
        {
            return contentView;
        }

        if (!string.IsNullOrWhiteSpace(stringFormat))
        {
            return new Label
            {
                Text = string.Format(CultureInfo.CurrentCulture, stringFormat, content)
            };
        }

        return new Label { Text = Convert.ToString(content, CultureInfo.CurrentCulture) ?? string.Empty };
    }

    private void OnPrimaryClicked(object? sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnRightButtonClicked(object? sender, EventArgs e)
    {
        IsPopupOpen = true;
    }
}
