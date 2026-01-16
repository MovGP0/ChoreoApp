using System.Windows.Input;

namespace MaterialDesignThemes.Maui;

[ContentProperty(nameof(RadioContent))]
public sealed class ContentRadioButton : TemplatedView
{
    public const string RadioButtonPartName = "PART_Radio";
    public const string ContentPresenterPartName = "PART_Content";

    private RadioButton? _radioButton;

    public ContentRadioButton()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);
    }

    public event EventHandler<CheckedChangedEventArgs>? CheckedChanged;

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(ContentRadioButton),
        false,
        BindingMode.TwoWay);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(
        nameof(GroupName),
        typeof(string),
        typeof(ContentRadioButton));

    public string? GroupName
    {
        get => (string?)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(object),
        typeof(ContentRadioButton));

    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty RadioButtonStyleProperty = BindableProperty.Create(
        nameof(RadioButtonStyle),
        typeof(Style),
        typeof(ContentRadioButton));

    public Style? RadioButtonStyle
    {
        get => (Style?)GetValue(RadioButtonStyleProperty);
        set => SetValue(RadioButtonStyleProperty, value);
    }

    public static readonly BindableProperty RadioContentProperty = BindableProperty.Create(
        nameof(RadioContent),
        typeof(View),
        typeof(ContentRadioButton));

    public View? RadioContent
    {
        get => (View?)GetValue(RadioContentProperty);
        set => SetValue(RadioContentProperty, value);
    }

    public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
        nameof(Spacing),
        typeof(double),
        typeof(ContentRadioButton),
        8d);

    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(ContentRadioButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(ContentRadioButton));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        if (_radioButton is not null)
        {
            _radioButton.CheckedChanged -= OnCheckedChanged;
        }

        _radioButton = GetTemplateChild(RadioButtonPartName) as RadioButton;

        if (_radioButton is not null)
        {
            _radioButton.CheckedChanged += OnCheckedChanged;
        }

        base.OnApplyTemplate();
    }

    private void OnTapped(object? sender, EventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        IsChecked = true;
        Command?.Execute(CommandParameter);
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        CheckedChanged?.Invoke(this, e);
    }
}
