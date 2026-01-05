using System.Windows.Input;

namespace MaterialDesignThemes.Maui;

[ContentProperty(nameof(Content))]
public class SnackbarMessage : TemplatedView
{
    public const string ActionButtonPartName = "PART_ActionButton";
    public const string MessageContentHostPartName = "PART_MessageContentHost";

    private ContentButton? _actionButton;
    private ContentView? _messageHost;

    public event EventHandler? ActionClick;

    public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(
        nameof(ActionCommand),
        typeof(ICommand),
        typeof(SnackbarMessage),
        propertyChanged: OnActionCommandChanged);

    public ICommand? ActionCommand
    {
        get => (ICommand?)GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
    }

    public static readonly BindableProperty ActionCommandParameterProperty = BindableProperty.Create(
        nameof(ActionCommandParameter),
        typeof(object),
        typeof(SnackbarMessage),
        propertyChanged: OnActionCommandChanged);

    public object? ActionCommandParameter
    {
        get => GetValue(ActionCommandParameterProperty);
        set => SetValue(ActionCommandParameterProperty, value);
    }

    public static readonly BindableProperty ActionContentProperty = BindableProperty.Create(
        nameof(ActionContent),
        typeof(object),
        typeof(SnackbarMessage),
        propertyChanged: OnActionContentChanged);

    public object? ActionContent
    {
        get => GetValue(ActionContentProperty);
        set => SetValue(ActionContentProperty, value);
    }

    public static readonly BindableProperty ActionContentTemplateProperty = BindableProperty.Create(
        nameof(ActionContentTemplate),
        typeof(DataTemplate),
        typeof(SnackbarMessage),
        propertyChanged: OnActionContentChanged);

    public DataTemplate? ActionContentTemplate
    {
        get => (DataTemplate?)GetValue(ActionContentTemplateProperty);
        set => SetValue(ActionContentTemplateProperty, value);
    }

    public static readonly BindableProperty ActionContentStringFormatProperty = BindableProperty.Create(
        nameof(ActionContentStringFormat),
        typeof(string),
        typeof(SnackbarMessage),
        propertyChanged: OnActionContentChanged);

    public string? ActionContentStringFormat
    {
        get => (string?)GetValue(ActionContentStringFormatProperty);
        set => SetValue(ActionContentStringFormatProperty, value);
    }

    public static readonly BindableProperty ActionContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(ActionContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(SnackbarMessage),
        propertyChanged: OnActionContentChanged);

    public DataTemplateSelector? ActionContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(ActionContentTemplateSelectorProperty);
        set => SetValue(ActionContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(object),
        typeof(SnackbarMessage),
        propertyChanged: OnContentChanged);

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(
        nameof(ContentTemplate),
        typeof(DataTemplate),
        typeof(SnackbarMessage),
        propertyChanged: OnContentChanged);

    public DataTemplate? ContentTemplate
    {
        get => (DataTemplate?)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public static readonly BindableProperty ContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(ContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(SnackbarMessage),
        propertyChanged: OnContentChanged);

    public DataTemplateSelector? ContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(ContentTemplateSelectorProperty);
        set => SetValue(ContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty ContentStringFormatProperty = BindableProperty.Create(
        nameof(ContentStringFormat),
        typeof(string),
        typeof(SnackbarMessage),
        propertyChanged: OnContentChanged);

    public string? ContentStringFormat
    {
        get => (string?)GetValue(ContentStringFormatProperty);
        set => SetValue(ContentStringFormatProperty, value);
    }

    public static readonly BindableProperty InlineActionButtonMaxHeightProperty = BindableProperty.CreateAttached(
        "InlineActionButtonMaxHeight",
        typeof(double),
        typeof(SnackbarMessage),
        55d);

    public static void SetInlineActionButtonMaxHeight(BindableObject element, double value) =>
        element.SetValue(InlineActionButtonMaxHeightProperty, value);
    public static double GetInlineActionButtonMaxHeight(BindableObject element) =>
        (double)element.GetValue(InlineActionButtonMaxHeightProperty);

    public static readonly BindableProperty ContentMaxHeightProperty = BindableProperty.CreateAttached(
        "ContentMaxHeight",
        typeof(double),
        typeof(SnackbarMessage),
        36d);

    public static void SetContentMaxHeight(BindableObject element, double value) =>
        element.SetValue(ContentMaxHeightProperty, value);
    public static double GetContentMaxHeight(BindableObject element) =>
        (double)element.GetValue(ContentMaxHeightProperty);

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_actionButton is not null)
        {
            _actionButton.Clicked -= OnActionButtonClicked;
        }

        _actionButton = GetTemplateChild(ActionButtonPartName) as ContentButton;
        _messageHost = GetTemplateChild(MessageContentHostPartName) as ContentView;

        if (_actionButton is not null)
        {
            _actionButton.Clicked += OnActionButtonClicked;
            UpdateActionCommand();
            UpdateActionContent();
        }

        if (_messageHost is not null)
        {
            UpdateContent();
        }
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SnackbarMessage message)
        {
            message.UpdateContent();
        }
    }

    private static void OnActionCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SnackbarMessage message)
        {
            message.UpdateActionCommand();
        }
    }

    private static void OnActionContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SnackbarMessage message)
        {
            message.UpdateActionContent();
        }
    }

    private void UpdateContent()
    {
        if (_messageHost is null)
        {
            return;
        }

        _messageHost.Content = CreateContentView(Content, ContentTemplate, ContentTemplateSelector, ContentStringFormat);
    }

    private void UpdateActionCommand()
    {
        if (_actionButton is null)
        {
            return;
        }

        _actionButton.Command = ActionCommand;
        _actionButton.CommandParameter = ActionCommandParameter;
    }

    private void UpdateActionContent()
    {
        if (_actionButton is null)
        {
            return;
        }

        _actionButton.ButtonContent = CreateContentView(
            ActionContent,
            ActionContentTemplate,
            ActionContentTemplateSelector,
            ActionContentStringFormat);

        _actionButton.IsVisible = ActionContent is not null;
    }

    private void OnActionButtonClicked(object? sender, EventArgs e)
    {
        ActionClick?.Invoke(this, EventArgs.Empty);
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
                Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, stringFormat, content),
                LineBreakMode = LineBreakMode.WordWrap
            };
        }

        return new Label
        {
            Text = Convert.ToString(content, System.Globalization.CultureInfo.CurrentCulture) ?? string.Empty,
            LineBreakMode = LineBreakMode.WordWrap
        };
    }
}
