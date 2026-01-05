using System.Globalization;
using System.Numerics;

namespace MaterialDesignThemes.Maui;

public class UpDownBase<T> : TemplatedView
    where T : INumber<T>, IMinMaxValue<T>
{
    public const string EntryPartName = "PART_Entry";
    public const string IncreaseButtonPartName = "PART_IncreaseButton";
    public const string DecreaseButtonPartName = "PART_DecreaseButton";

    private Entry? _entry;
    private ContentButton? _increaseButton;
    private ContentButton? _decreaseButton;
    private bool _isUpdatingText;
    private bool _isUpdatingFromText;

    public UpDownBase()
    {
        UpdateDefaultContent();
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
        if (_entry?.Text is null)
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

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_entry is not null)
        {
            _entry.TextChanged -= OnEntryTextChanged;
            _entry.Unfocused -= OnEntryUnfocused;
        }

        if (_increaseButton is not null)
        {
            _increaseButton.Clicked -= OnIncreaseClicked;
        }

        if (_decreaseButton is not null)
        {
            _decreaseButton.Clicked -= OnDecreaseClicked;
        }

        _entry = GetTemplateChild(EntryPartName) as Entry;
        _increaseButton = GetTemplateChild(IncreaseButtonPartName) as ContentButton;
        _decreaseButton = GetTemplateChild(DecreaseButtonPartName) as ContentButton;

        if (_entry is not null)
        {
            _entry.Keyboard = Keyboard.Numeric;
            _entry.TextChanged += OnEntryTextChanged;
            _entry.Unfocused += OnEntryUnfocused;
            ApplyEntryStyle();
            UpdateEntryText(Value);
        }

        if (_increaseButton is not null)
        {
            _increaseButton.Clicked += OnIncreaseClicked;
            ApplyButtonStyle();
            UpdateIncreaseButtonContent();
        }

        if (_decreaseButton is not null)
        {
            _decreaseButton.Clicked += OnDecreaseClicked;
            ApplyButtonStyle();
            UpdateDecreaseButtonContent();
        }

        UpdateButtonStates();
    }

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
            upDown.UpdateIncreaseButtonContent();
        }
    }

    private static void OnDecreaseContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is UpDownBase<T> upDown)
        {
            upDown.UpdateDecreaseButtonContent();
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
            upDown.UpdateDefaultIconForeground();
            upDown.UpdateIncreaseButtonContent();
            upDown.UpdateDecreaseButtonContent();
        }
    }

    private void UpdateDefaultContent()
    {
        if (IncreaseContent is null)
        {
            IncreaseContent = CreateDefaultIncreaseContent();
        }

        if (DecreaseContent is null)
        {
            DecreaseContent = CreateDefaultDecreaseContent();
        }
    }

    private void UpdateDefaultIconForeground()
    {
        if (IncreaseContent is PackIcon increaseIcon)
        {
            increaseIcon.ForegroundColor = IconForeground;
        }

        if (DecreaseContent is PackIcon decreaseIcon)
        {
            decreaseIcon.ForegroundColor = IconForeground;
        }
    }

    private void UpdateIncreaseButtonContent()
    {
        UpdateDefaultContent();

        if (_increaseButton is not null)
        {
            _increaseButton.ButtonContent = IncreaseContent;
        }
    }

    private void UpdateDecreaseButtonContent()
    {
        UpdateDefaultContent();

        if (_decreaseButton is not null)
        {
            _decreaseButton.ButtonContent = DecreaseContent;
        }
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
        if (_entry is not null && EntryStyle is not null)
        {
            _entry.Style = EntryStyle;
        }
    }

    private void ApplyButtonStyle()
    {
        if (ButtonStyle is not null)
        {
            if (_increaseButton is not null)
            {
                _increaseButton.Style = ButtonStyle;
            }

            if (_decreaseButton is not null)
            {
                _decreaseButton.Style = ButtonStyle;
            }
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
        if (_entry is null)
        {
            return;
        }

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
        if (_increaseButton is not null)
        {
            _increaseButton.IsEnabled = Value.CompareTo(Maximum) < 0;
        }

        if (_decreaseButton is not null)
        {
            _decreaseButton.IsEnabled = Value.CompareTo(Minimum) > 0;
        }
    }
}

public sealed class ValueChangedEventArgs<TValue> : EventArgs
{
    public ValueChangedEventArgs(TValue oldValue, TValue newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }

    public TValue OldValue { get; }
    public TValue NewValue { get; }
}
