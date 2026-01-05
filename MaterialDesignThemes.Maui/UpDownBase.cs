using System.Globalization;
using System.Numerics;

namespace MaterialDesignThemes.Maui;

public class UpDownBase<T> : ContentView
    where T : INumber<T>, IMinMaxValue<T>
{
    private readonly Entry _entry = new();
    private readonly ContentButton _increaseButton = new();
    private readonly ContentButton _decreaseButton = new();
    private readonly Grid _layout = [];
    private readonly Grid _buttonLayout = [];
    private bool _isUpdatingText;
    private bool _isUpdatingFromText;

    public UpDownBase()
    {
        _entry.HorizontalOptions = LayoutOptions.Fill;
        _entry.VerticalOptions = LayoutOptions.Center;
        _entry.Keyboard = Keyboard.Numeric;
        _entry.TextChanged += OnEntryTextChanged;
        _entry.Unfocused += OnEntryUnfocused;

        _increaseButton.Clicked += OnIncreaseClicked;
        _decreaseButton.Clicked += OnDecreaseClicked;

        UpdateIncreaseContent();
        UpdateDecreaseContent();
        ApplyEntryStyle();
        ApplyButtonStyle();

        _buttonLayout.RowDefinitions =
        [
            new RowDefinition { Height = GridLength.Star },
            new RowDefinition { Height = GridLength.Star }
        ];

        _buttonLayout.Children.Add(_increaseButton);
        Grid.SetRow(_decreaseButton, 1);
        _buttonLayout.Children.Add(_decreaseButton);

        _layout.ColumnDefinitions =
        [
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Auto }
        ];

        _layout.RowDefinitions =
        [
            new RowDefinition { Height = GridLength.Auto }
        ];

        _layout.Children.Add(_entry);
        Grid.SetColumn(_buttonLayout, 1);
        _layout.Children.Add(_buttonLayout);

        Content = _layout;

        UpdateEntryText(Value);
        UpdateButtonStates();
    }

    public event EventHandler<ValueChangedEventArgs<T>>? ValueChanged;

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
        nameof(Minimum),
        typeof(T),
        typeof(UpDownBase<T>),
        T.MinValue,
        propertyChanged: OnMinimumChanged);

    public T Minimum
    {
        get => (T)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
        nameof(Maximum),
        typeof(T),
        typeof(UpDownBase<T>),
        T.MaxValue,
        coerceValue: CoerceMaximum,
        propertyChanged: OnMaximumChanged);

    public T Maximum
    {
        get => (T)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(T),
        typeof(UpDownBase<T>),
        default(T),
        BindingMode.TwoWay,
        coerceValue: CoerceValue,
        propertyChanged: OnValueChanged);

    public T Value
    {
        get => (T)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty ValueStepProperty = BindableProperty.Create(
        nameof(ValueStep),
        typeof(T),
        typeof(UpDownBase<T>),
        T.One);

    public T ValueStep
    {
        get => (T)GetValue(ValueStepProperty);
        set => SetValue(ValueStepProperty, value);
    }

    public static readonly BindableProperty AllowChangeOnScrollProperty = BindableProperty.Create(
        nameof(AllowChangeOnScroll),
        typeof(bool),
        typeof(UpDownBase<T>),
        false);

    public bool AllowChangeOnScroll
    {
        get => (bool)GetValue(AllowChangeOnScrollProperty);
        set => SetValue(AllowChangeOnScrollProperty, value);
    }

    public static readonly BindableProperty EntryStyleProperty = BindableProperty.Create(
        nameof(EntryStyle),
        typeof(Style),
        typeof(UpDownBase<T>),
        propertyChanged: OnEntryStyleChanged);

    public Style? EntryStyle
    {
        get => (Style?)GetValue(EntryStyleProperty);
        set => SetValue(EntryStyleProperty, value);
    }

    public static readonly BindableProperty ButtonStyleProperty = BindableProperty.Create(
        nameof(ButtonStyle),
        typeof(Style),
        typeof(UpDownBase<T>),
        propertyChanged: OnButtonStyleChanged);

    public Style? ButtonStyle
    {
        get => (Style?)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }

    public static readonly BindableProperty IconForegroundProperty = BindableProperty.Create(
        nameof(IconForeground),
        typeof(Color),
        typeof(UpDownBase<T>),
        Colors.Transparent,
        propertyChanged: OnIconForegroundChanged);

    public Color IconForeground
    {
        get => (Color)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public static readonly BindableProperty IncreaseContentProperty = BindableProperty.Create(
        nameof(IncreaseContent),
        typeof(View),
        typeof(UpDownBase<T>),
        propertyChanged: OnIncreaseContentChanged);

    public View? IncreaseContent
    {
        get => (View?)GetValue(IncreaseContentProperty);
        set => SetValue(IncreaseContentProperty, value);
    }

    public static readonly BindableProperty DecreaseContentProperty = BindableProperty.Create(
        nameof(DecreaseContent),
        typeof(View),
        typeof(UpDownBase<T>),
        propertyChanged: OnDecreaseContentChanged);

    public View? DecreaseContent
    {
        get => (View?)GetValue(DecreaseContentProperty);
        set => SetValue(DecreaseContentProperty, value);
    }

    public void SelectAll()
    {
        if (string.IsNullOrEmpty(_entry.Text))
        {
            return;
        }

        _entry.CursorPosition = 0;
        _entry.SelectionLength = _entry.Text.Length;
    }

    protected virtual string FormatValue(T value)
        => Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty;

    protected virtual bool TryParseText(string? text, out T value)
        => T.TryParse(text, CultureInfo.CurrentCulture, out value);

    private static void OnMinimumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not UpDownBase<T> upDown)
        {
            return;
        }

        upDown.CoerceRange();
    }

    private static void OnMaximumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not UpDownBase<T> upDown)
        {
            return;
        }

        upDown.CoerceRange();
    }

    private static object? CoerceMaximum(BindableObject bindable, object? value)
    {
        if (bindable is not UpDownBase<T> upDown || value is not T maximum)
        {
            return value;
        }

        return T.Max(upDown.Minimum, maximum);
    }

    private static object? CoerceValue(BindableObject bindable, object? value)
    {
        if (bindable is not UpDownBase<T> upDown || value is not T numericValue)
        {
            return value;
        }

        return upDown.ClampValue(numericValue);
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not UpDownBase<T> upDown || oldValue is not T oldNumeric || newValue is not T newNumeric)
        {
            return;
        }

        if (!upDown._isUpdatingFromText)
        {
            upDown.UpdateEntryText(newNumeric);
        }

        upDown.UpdateButtonStates();
        upDown.ValueChanged?.Invoke(upDown, new ValueChangedEventArgs<T>(oldNumeric, newNumeric));
    }

    private static void OnIncreaseContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.UpdateIncreaseContent();
        }
    }

    private static void OnDecreaseContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.UpdateDecreaseContent();
        }
    }

    private static void OnEntryStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.ApplyEntryStyle();
        }
    }

    private static void OnButtonStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.ApplyButtonStyle();
        }
    }

    private static void OnIconForegroundChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.UpdateIncreaseContent();
            upDown.UpdateDecreaseContent();
        }
    }

    private void UpdateIncreaseContent()
    {
        _increaseButton.ButtonContent = IncreaseContent ?? CreateDefaultIncreaseContent();
    }

    private void UpdateDecreaseContent()
    {
        _decreaseButton.ButtonContent = DecreaseContent ?? CreateDefaultDecreaseContent();
    }

    private View CreateDefaultIncreaseContent()
        => new PackIcon
        {
            Kind = PackIconKind.ArrowUp,
            ForegroundColor = IconForeground
        };

    private View CreateDefaultDecreaseContent()
        => new PackIcon
        {
            Kind = PackIconKind.ArrowDown,
            ForegroundColor = IconForeground
        };

    private void ApplyEntryStyle()
    {
        if (EntryStyle is not null)
        {
            _entry.Style = EntryStyle;
        }
    }

    private void ApplyButtonStyle()
    {
        if (ButtonStyle is not null)
        {
            _increaseButton.Style = ButtonStyle;
            _decreaseButton.Style = ButtonStyle;
        }
    }

    private void CoerceRange()
    {
        if (Maximum.CompareTo(Minimum) < 0)
        {
            Maximum = Minimum;
        }

        if (Value.CompareTo(Minimum) < 0)
        {
            Value = Minimum;
        }
        else if (Value.CompareTo(Maximum) > 0)
        {
            Value = Maximum;
        }

        UpdateButtonStates();
    }

    private void UpdateEntryText(T value)
    {
        _isUpdatingText = true;
        _entry.Text = FormatValue(value);
        _isUpdatingText = false;
    }

    private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdatingText)
        {
            return;
        }

        if (!TryParseText(e.NewTextValue, out var parsedValue))
        {
            return;
        }

        _isUpdatingFromText = true;
        Value = ClampValue(parsedValue);
        _isUpdatingFromText = false;
    }

    private void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        UpdateEntryText(Value);
    }

    private void OnIncreaseClicked(object? sender, EventArgs e)
    {
        Value = ClampValue(Value + ValueStep);
    }

    private void OnDecreaseClicked(object? sender, EventArgs e)
    {
        Value = ClampValue(Value - ValueStep);
    }

    private T ClampValue(T value)
        => T.Clamp(value, Minimum, Maximum);

    private void UpdateButtonStates()
    {
        _increaseButton.IsEnabled = Value.CompareTo(Maximum) < 0;
        _decreaseButton.IsEnabled = Value.CompareTo(Minimum) > 0;
    }
}

public sealed class ValueChangedEventArgs<TValue>(TValue oldValue, TValue newValue) : EventArgs
{
    public TValue OldValue { get; } = oldValue;
    public TValue NewValue { get; } = newValue;
}
