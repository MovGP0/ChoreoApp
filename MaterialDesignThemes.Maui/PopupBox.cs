using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

public enum PopupBoxPopupMode
{
    Click,
    MouseOver,
    MouseOverEager
}

public enum PopupAnimation
{
    None,
    Fade
}

[ContentProperty(nameof(PopupContent))]
public partial class PopupBox : TemplatedView
{
    public const string PopupPartName = "PART_Popup";
    public const string TogglePartName = "PART_Toggle";
    public const string PopupContentControlPartName = "PART_PopupContentControl";
    public const string PopupIsOpenStateName = "IsOpen";
    public const string PopupIsClosedStateName = "IsClosed";

    private PopupEx? _popup;
    private View? _popupContentControl;
    private View? _toggleButton;

    public PopupBox()
    {
        ClosePopupCommand = new Command(() => IsPopupOpen = false);
    }

    public ICommand ClosePopupCommand { get; }

    public event EventHandler? ToggleCheckedContentClick;
    public event EventHandler? Opened;
    public event EventHandler? Closed;

    public static readonly BindableProperty ToggleContentProperty = BindableProperty.Create(
        nameof(ToggleContent),
        typeof(object),
        typeof(PopupBox),
        propertyChanged: OnToggleContentChanged);

    public object? ToggleContent
    {
        get => GetValue(ToggleContentProperty);
        set => SetValue(ToggleContentProperty, value);
    }

    public static readonly BindableProperty ToggleContentTemplateProperty = BindableProperty.Create(
        nameof(ToggleContentTemplate),
        typeof(DataTemplate),
        typeof(PopupBox),
        propertyChanged: OnToggleContentChanged);

    public DataTemplate? ToggleContentTemplate
    {
        get => (DataTemplate?)GetValue(ToggleContentTemplateProperty);
        set => SetValue(ToggleContentTemplateProperty, value);
    }

    public static readonly BindableProperty ToggleContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(ToggleContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(PopupBox),
        propertyChanged: OnToggleContentChanged);

    public DataTemplateSelector? ToggleContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(ToggleContentTemplateSelectorProperty);
        set => SetValue(ToggleContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty ToggleCheckedContentProperty = BindableProperty.Create(
        nameof(ToggleCheckedContent),
        typeof(object),
        typeof(PopupBox),
        propertyChanged: OnToggleContentChanged);

    public object? ToggleCheckedContent
    {
        get => GetValue(ToggleCheckedContentProperty);
        set => SetValue(ToggleCheckedContentProperty, value);
    }

    public static readonly BindableProperty ToggleCheckedContentTemplateProperty = BindableProperty.Create(
        nameof(ToggleCheckedContentTemplate),
        typeof(DataTemplate),
        typeof(PopupBox),
        propertyChanged: OnToggleContentChanged);

    public DataTemplate? ToggleCheckedContentTemplate
    {
        get => (DataTemplate?)GetValue(ToggleCheckedContentTemplateProperty);
        set => SetValue(ToggleCheckedContentTemplateProperty, value);
    }

    public static readonly BindableProperty ToggleCheckedContentCommandProperty = BindableProperty.Create(
        nameof(ToggleCheckedContentCommand),
        typeof(ICommand),
        typeof(PopupBox));

    public ICommand? ToggleCheckedContentCommand
    {
        get => (ICommand?)GetValue(ToggleCheckedContentCommandProperty);
        set => SetValue(ToggleCheckedContentCommandProperty, value);
    }

    public static readonly BindableProperty ToggleCheckedContentCommandParameterProperty = BindableProperty.Create(
        nameof(ToggleCheckedContentCommandParameter),
        typeof(object),
        typeof(PopupBox));

    public object? ToggleCheckedContentCommandParameter
    {
        get => GetValue(ToggleCheckedContentCommandParameterProperty);
        set => SetValue(ToggleCheckedContentCommandParameterProperty, value);
    }

    public static readonly BindableProperty PopupContentProperty = BindableProperty.Create(
        nameof(PopupContent),
        typeof(object),
        typeof(PopupBox),
        propertyChanged: OnPopupContentChanged);

    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public static readonly BindableProperty PopupContentTemplateProperty = BindableProperty.Create(
        nameof(PopupContentTemplate),
        typeof(DataTemplate),
        typeof(PopupBox),
        propertyChanged: OnPopupContentChanged);

    public DataTemplate? PopupContentTemplate
    {
        get => (DataTemplate?)GetValue(PopupContentTemplateProperty);
        set => SetValue(PopupContentTemplateProperty, value);
    }

    public static readonly BindableProperty IsPopupOpenProperty = BindableProperty.Create(
        nameof(IsPopupOpen),
        typeof(bool),
        typeof(PopupBox),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsPopupOpenChanged);

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public static readonly BindableProperty StaysOpenProperty = BindableProperty.Create(
        nameof(StaysOpen),
        typeof(bool),
        typeof(PopupBox),
        false);

    public bool StaysOpen
    {
        get => (bool)GetValue(StaysOpenProperty);
        set => SetValue(StaysOpenProperty, value);
    }

    public static readonly BindableProperty PlacementModeProperty = BindableProperty.Create(
        nameof(PlacementMode),
        typeof(PopupBoxPlacementMode),
        typeof(PopupBox),
        default(PopupBoxPlacementMode));

    public PopupBoxPlacementMode PlacementMode
    {
        get => (PopupBoxPlacementMode)GetValue(PlacementModeProperty);
        set => SetValue(PlacementModeProperty, value);
    }

    public static readonly BindableProperty PopupModeProperty = BindableProperty.Create(
        nameof(PopupMode),
        typeof(PopupBoxPopupMode),
        typeof(PopupBox),
        PopupBoxPopupMode.Click);

    public PopupBoxPopupMode PopupMode
    {
        get => (PopupBoxPopupMode)GetValue(PopupModeProperty);
        set => SetValue(PopupModeProperty, value);
    }

    public static readonly BindableProperty UnfurlOrientationProperty = BindableProperty.Create(
        nameof(UnfurlOrientation),
        typeof(Orientation),
        typeof(PopupBox),
        Orientation.Vertical);

    public Orientation UnfurlOrientation
    {
        get => (Orientation)GetValue(UnfurlOrientationProperty);
        set => SetValue(UnfurlOrientationProperty, value);
    }

    public static readonly BindableProperty PopupHorizontalOffsetProperty = BindableProperty.Create(
        nameof(PopupHorizontalOffset),
        typeof(double),
        typeof(PopupBox),
        0d);

    public double PopupHorizontalOffset
    {
        get => (double)GetValue(PopupHorizontalOffsetProperty);
        set => SetValue(PopupHorizontalOffsetProperty, value);
    }

    public static readonly BindableProperty PopupVerticalOffsetProperty = BindableProperty.Create(
        nameof(PopupVerticalOffset),
        typeof(double),
        typeof(PopupBox),
        0d);

    public double PopupVerticalOffset
    {
        get => (double)GetValue(PopupVerticalOffsetProperty);
        set => SetValue(PopupVerticalOffsetProperty, value);
    }

    public static readonly BindableProperty PopupUniformCornerRadiusProperty = BindableProperty.Create(
        nameof(PopupUniformCornerRadius),
        typeof(double),
        typeof(PopupBox),
        0d,
        propertyChanged: OnPopupUniformCornerRadiusChanged);

    public double PopupUniformCornerRadius
    {
        get => (double)GetValue(PopupUniformCornerRadiusProperty);
        set => SetValue(PopupUniformCornerRadiusProperty, value);
    }

    public static readonly BindableProperty PopupElevationProperty = BindableProperty.Create(
        nameof(PopupElevation),
        typeof(Elevation),
        typeof(PopupBox),
        Elevation.Dp0);

    public Elevation PopupElevation
    {
        get => (Elevation)GetValue(PopupElevationProperty);
        set => SetValue(PopupElevationProperty, value);
    }

    public static readonly BindableProperty PopupAnimationProperty = BindableProperty.Create(
        nameof(PopupAnimation),
        typeof(PopupAnimation),
        typeof(PopupBox),
        PopupAnimation.Fade);

    public PopupAnimation PopupAnimation
    {
        get => (PopupAnimation)GetValue(PopupAnimationProperty);
        set => SetValue(PopupAnimationProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        UnhookToggle();

        base.OnApplyTemplate();

        _popup = GetTemplateChild(PopupPartName) as PopupEx;
        _popupContentControl = GetTemplateChild(PopupContentControlPartName) as View;
        _toggleButton = GetTemplateChild(TogglePartName) as View;

        HookToggle();
        UpdatePopupContent();
        UpdateToggleContent();
        UpdatePopupCornerRadius();
        UpdatePopupVisibility();
    }

    private static void OnToggleContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PopupBox popupBox)
        {
            popupBox.UpdateToggleContent();
        }
    }

    private static void OnPopupContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PopupBox popupBox)
        {
            popupBox.UpdatePopupContent();
        }
    }

    private static void OnIsPopupOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PopupBox popupBox)
        {
            popupBox.UpdatePopupVisibility();
            popupBox.UpdateToggleContent();
        }
    }

    private static void OnPopupUniformCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PopupBox popupBox)
        {
            popupBox.UpdatePopupCornerRadius();
        }
    }

    private void HookToggle()
    {
        switch (_toggleButton)
        {
            case ToogleButton toggle:
                toggle.Checked += OnToggleChecked;
                toggle.Unchecked += OnToggleUnchecked;
                toggle.IsChecked = IsPopupOpen;
                break;
            case ContentButton button:
                button.Clicked += OnToggleClicked;
                break;
            case Button button:
                button.Clicked += OnToggleClicked;
                break;
        }
    }

    private void UnhookToggle()
    {
        switch (_toggleButton)
        {
            case ToogleButton toggle:
                toggle.Checked -= OnToggleChecked;
                toggle.Unchecked -= OnToggleUnchecked;
                break;
            case ContentButton button:
                button.Clicked -= OnToggleClicked;
                break;
            case Button button:
                button.Clicked -= OnToggleClicked;
                break;
        }
    }

    private void UpdatePopupContent()
    {
        if (_popupContentControl is null)
        {
            return;
        }

        var content = CreateContentView(
            PopupContent,
            PopupContentTemplate,
            null,
            null);

        switch (_popupContentControl)
        {
            case ContentView contentView:
                contentView.Content = content;
                break;
            case Border border:
                border.Content = content;
                break;
        }
    }

    private void UpdateToggleContent()
    {
        if (_toggleButton is null)
        {
            return;
        }

        var content = IsPopupOpen && ToggleCheckedContent is not null
            ? ToggleCheckedContent
            : ToggleContent;

        var template = IsPopupOpen && ToggleCheckedContent is not null
            ? ToggleCheckedContentTemplate
            : ToggleContentTemplate;

        var view = CreateContentView(content, template, ToggleContentTemplateSelector, null);

        switch (_toggleButton)
        {
            case ToogleButton toggle:
                toggle.Content = view;
                toggle.IsChecked = IsPopupOpen;
                break;
            case ContentButton button:
                button.ButtonContent = view;
                break;
            case Button button:
                if (content is string text)
                {
                    button.Text = text;
                }
                break;
        }
    }

    private void UpdatePopupCornerRadius()
    {
        if (_popupContentControl is Border border)
        {
            border.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(PopupUniformCornerRadius)
            };
            return;
        }

        if (_popupContentControl is ContentView contentView && contentView.Content is Border contentBorder)
        {
            contentBorder.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(PopupUniformCornerRadius)
            };
        }
    }

    private void UpdatePopupVisibility()
    {
        if (_popup is not null)
        {
            _popup.IsOpen = IsPopupOpen;
        }

        if (_popupContentControl is not null)
        {
            _popupContentControl.IsVisible = IsPopupOpen;
        }

        VisualStateManager.GoToState(this, IsPopupOpen ? PopupIsOpenStateName : PopupIsClosedStateName);

        if (IsPopupOpen)
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnToggleChecked(object? sender, EventArgs e)
    {
        IsPopupOpen = true;
    }

    private void OnToggleUnchecked(object? sender, EventArgs e)
    {
        if (!StaysOpen)
        {
            IsPopupOpen = false;
        }
    }

    private void OnToggleClicked(object? sender, EventArgs e)
    {
        if (PopupMode != PopupBoxPopupMode.Click)
        {
            IsPopupOpen = true;
            return;
        }

        if (IsPopupOpen && ToggleCheckedContent is not null)
        {
            ToggleCheckedContentClick?.Invoke(this, EventArgs.Empty);

            if (ToggleCheckedContentCommand?.CanExecute(ToggleCheckedContentCommandParameter) == true)
            {
                ToggleCheckedContentCommand.Execute(ToggleCheckedContentCommandParameter);
            }
        }

        IsPopupOpen = !IsPopupOpen;
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
            return new Label { Text = string.Format(stringFormat, content) };
        }

        return new Label { Text = Convert.ToString(content) ?? string.Empty };
    }
}
