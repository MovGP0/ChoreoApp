using System.Windows.Input;

namespace ChoreoApp.Styling;

public sealed class Chip : ContentView
{
    public const string DeleteButtonPartName = "PART_DeleteButton";

    private readonly TapGestureRecognizer _tap;
    private View? _deleteButton;
    private bool _suppressTap;

    public Chip()
    {
        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);
    }

    public event EventHandler? Clicked;

    public event EventHandler? DeleteClick;

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(Chip));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(Chip));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(
            nameof(Icon),
            typeof(View),
            typeof(Chip));

    public View? Icon
    {
        get => (View?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly BindableProperty IconBackgroundProperty =
        BindableProperty.Create(
            nameof(IconBackground),
            typeof(Color),
            typeof(Chip));

    public Color? IconBackground
    {
        get => (Color?)GetValue(IconBackgroundProperty);
        set => SetValue(IconBackgroundProperty, value);
    }

    public static readonly BindableProperty IconForegroundProperty =
        BindableProperty.Create(
            nameof(IconForeground),
            typeof(Color),
            typeof(Chip));

    public Color? IconForeground
    {
        get => (Color?)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public static readonly BindableProperty ForegroundProperty =
        BindableProperty.Create(
            nameof(Foreground),
            typeof(Color),
            typeof(Chip));

    public Color? Foreground
    {
        get => (Color?)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(Chip),
            new Thickness(8, 0, 12, 0));

    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(Chip),
            13d);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(
            nameof(Stroke),
            typeof(Brush),
            typeof(Chip));

    public Brush? Stroke
    {
        get => (Brush?)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(
            nameof(StrokeThickness),
            typeof(double),
            typeof(Chip),
            0d);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public static readonly BindableProperty IsDeletableProperty =
        BindableProperty.Create(
            nameof(IsDeletable),
            typeof(bool),
            typeof(Chip),
            false);

    public bool IsDeletable
    {
        get => (bool)GetValue(IsDeletableProperty);
        set => SetValue(IsDeletableProperty, value);
    }

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(Chip));

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public static readonly BindableProperty DeleteCommandParameterProperty =
        BindableProperty.Create(
            nameof(DeleteCommandParameter),
            typeof(object),
            typeof(Chip));

    public object? DeleteCommandParameter
    {
        get => GetValue(DeleteCommandParameterProperty);
        set => SetValue(DeleteCommandParameterProperty, value);
    }

    public static readonly BindableProperty DeleteToolTipProperty =
        BindableProperty.Create(
            nameof(DeleteToolTip),
            typeof(object),
            typeof(Chip));

    public object? DeleteToolTip
    {
        get => GetValue(DeleteToolTipProperty);
        set => SetValue(DeleteToolTipProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_deleteButton is ContentButton oldContentButton)
        {
            oldContentButton.Clicked -= OnDeleteButtonClicked;
        }
        else if (_deleteButton is Button oldButton)
        {
            oldButton.Clicked -= OnDeleteButtonClicked;
        }

        _deleteButton = GetTemplateChild(DeleteButtonPartName) as View;

        if (_deleteButton is ContentButton contentButton)
        {
            contentButton.Clicked += OnDeleteButtonClicked;
        }
        else if (_deleteButton is Button button)
        {
            button.Clicked += OnDeleteButtonClicked;
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (_suppressTap)
        {
            _suppressTap = false;
            return;
        }

        if (!IsEnabled)
        {
            return;
        }

        var cmd = Command;
        var param = CommandParameter;

        if (cmd?.CanExecute(param) == true)
        {
            cmd.Execute(param);
        }

        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnDeleteButtonClicked(object? sender, EventArgs e)
    {
        _suppressTap = true;

        DeleteClick?.Invoke(this, EventArgs.Empty);

        if (DeleteCommand?.CanExecute(DeleteCommandParameter) == true)
        {
            DeleteCommand.Execute(DeleteCommandParameter);
        }
    }
}
