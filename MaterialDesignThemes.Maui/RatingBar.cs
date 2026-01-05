using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace MaterialDesignThemes.Maui;

public sealed class RatingBar : TemplatedView
{
    public const string ItemsHostPartName = "PART_ItemsHost";

    private readonly ObservableCollection<RatingBarButton> _ratingButtonsInternal = [];
    private readonly ReadOnlyObservableCollection<RatingBarButton> _ratingButtons;
    private Layout? _itemsHost;

    public RatingBar()
    {
        _ratingButtons = new ReadOnlyObservableCollection<RatingBarButton>(_ratingButtonsInternal);
    }

    public event EventHandler<ValueChangedEventArgs<double>>? ValueChanged;

    public ReadOnlyObservableCollection<RatingBarButton> RatingButtons => _ratingButtons;

    public static readonly BindableProperty MinProperty = BindableProperty.Create(
        nameof(Min),
        typeof(int),
        typeof(RatingBar),
        1,
        propertyChanged: OnRangePropertyChanged,
        coerceValue: CoerceMin);

    public int Min
    {
        get => (int)GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    public static readonly BindableProperty MaxProperty = BindableProperty.Create(
        nameof(Max),
        typeof(int),
        typeof(RatingBar),
        5,
        propertyChanged: OnRangePropertyChanged,
        coerceValue: CoerceMax);

    public int Max
    {
        get => (int)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(double),
        typeof(RatingBar),
        0d,
        BindingMode.TwoWay,
        propertyChanged: OnValueChanged);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty ValueIncrementsProperty = BindableProperty.Create(
        nameof(ValueIncrements),
        typeof(double),
        typeof(RatingBar),
        1d,
        propertyChanged: OnValueIncrementsChanged,
        coerceValue: CoerceValueIncrements);

    public double ValueIncrements
    {
        get => (double)GetValue(ValueIncrementsProperty);
        set => SetValue(ValueIncrementsProperty, value);
    }

    public static readonly BindableProperty IsPreviewValueEnabledProperty = BindableProperty.Create(
        nameof(IsPreviewValueEnabled),
        typeof(bool),
        typeof(RatingBar),
        false);

    public bool IsPreviewValueEnabled
    {
        get => (bool)GetValue(IsPreviewValueEnabledProperty);
        set => SetValue(IsPreviewValueEnabledProperty, value);
    }

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
        nameof(Orientation),
        typeof(StackOrientation),
        typeof(RatingBar),
        StackOrientation.Horizontal,
        propertyChanged: OnLayoutPropertyChanged);

    public StackOrientation Orientation
    {
        get => (StackOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(RatingBar),
        false);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly BindableProperty InvertDirectionProperty = BindableProperty.Create(
        nameof(InvertDirection),
        typeof(bool),
        typeof(RatingBar),
        false,
        propertyChanged: OnLayoutPropertyChanged);

    public bool InvertDirection
    {
        get => (bool)GetValue(InvertDirectionProperty);
        set => SetValue(InvertDirectionProperty, value);
    }

    public static readonly BindableProperty ValueItemContainerButtonStyleProperty = BindableProperty.Create(
        nameof(ValueItemContainerButtonStyle),
        typeof(Style),
        typeof(RatingBar));

    public Style? ValueItemContainerButtonStyle
    {
        get => (Style?)GetValue(ValueItemContainerButtonStyleProperty);
        set => SetValue(ValueItemContainerButtonStyleProperty, value);
    }

    public static readonly BindableProperty ValueItemTemplateProperty = BindableProperty.Create(
        nameof(ValueItemTemplate),
        typeof(DataTemplate),
        typeof(RatingBar),
        propertyChanged: OnLayoutPropertyChanged);

    public DataTemplate? ValueItemTemplate
    {
        get => (DataTemplate?)GetValue(ValueItemTemplateProperty);
        set => SetValue(ValueItemTemplateProperty, value);
    }

    public static readonly BindableProperty ValueItemTemplateSelectorProperty = BindableProperty.Create(
        nameof(ValueItemTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(RatingBar),
        propertyChanged: OnLayoutPropertyChanged);

    public DataTemplateSelector? ValueItemTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(ValueItemTemplateSelectorProperty);
        set => SetValue(ValueItemTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create(
        nameof(ForegroundColor),
        typeof(Color),
        typeof(RatingBar),
        null);

    public Color? ForegroundColor
    {
        get => (Color?)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public static readonly BindableProperty PaddingProperty = BindableProperty.Create(
        nameof(Padding),
        typeof(Thickness),
        typeof(RatingBar),
        new Thickness(0),
        propertyChanged: OnLayoutPropertyChanged);

    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    private static readonly BindablePropertyKey PreviewValuePropertyKey = BindableProperty.CreateReadOnly(
        nameof(PreviewValue),
        typeof(double?),
        typeof(RatingBar),
        null,
        coerceValue: CoercePreviewValue);

    public static readonly BindableProperty PreviewValueProperty = PreviewValuePropertyKey.BindableProperty;

    public double? PreviewValue
    {
        get => (double?)GetValue(PreviewValueProperty);
        private set => SetValue(PreviewValuePropertyKey, value);
    }

    private bool IsFractionalValueEnabled => Math.Abs(ValueIncrements - 1.0) > 1e-10;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _itemsHost = GetTemplateChild(ItemsHostPartName) as Layout;
        RebuildButtons();
    }

    internal void HandleButtonTapped(RatingBarButton button, Point? point)
    {
        if (IsReadOnly)
        {
            return;
        }

        if (IsFractionalValueEnabled && point is not null)
        {
            Value = GetValueAtPosition(button, point.Value);
            return;
        }

        Value = button.Value;
    }

    internal void HandlePointerMoved(RatingBarButton button, Point? point)
    {
        if (!IsPreviewValueEnabled || point is null)
        {
            return;
        }

        PreviewValue = GetValueAtPosition(button, point.Value);
        UpdateButtonStates();
    }

    internal void ClearPreview()
    {
        if (PreviewValue is null)
        {
            return;
        }

        PreviewValue = null;
        UpdateButtonStates();
    }

    private static void OnRangePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RatingBar ratingBar)
        {
            ratingBar.RebuildButtons();
            ratingBar.UpdateButtonStates();
        }
    }

    private static object CoerceMin(BindableObject bindable, object value)
    {
        var ratingBar = (RatingBar)bindable;
        return Math.Min((int)value, ratingBar.Max);
    }

    private static object CoerceMax(BindableObject bindable, object value)
    {
        var ratingBar = (RatingBar)bindable;
        return Math.Max((int)value, ratingBar.Min);
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RatingBar ratingBar)
        {
            ratingBar.UpdateButtonStates();
            ratingBar.ValueChanged?.Invoke(ratingBar, new ValueChangedEventArgs<double>((double)oldValue, (double)newValue));
        }
    }

    private static void OnValueIncrementsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RatingBar ratingBar)
        {
            ratingBar.RebuildButtons();
            ratingBar.UpdateButtonStates();
        }
    }

    private static object CoerceValueIncrements(BindableObject bindable, object value)
        => Math.Max(double.Epsilon, Math.Min(1.0, (double)value));

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RatingBar ratingBar)
        {
            ratingBar.RebuildButtons();
            ratingBar.UpdateButtonStates();
        }
    }

    private static object? CoercePreviewValue(BindableObject bindable, object? value)
    {
        if (value is null)
        {
            return null;
        }

        var ratingBar = (RatingBar)bindable;
        if (value is double numeric)
        {
            if (!ratingBar.IsFractionalValueEnabled)
            {
                numeric = Math.Ceiling(numeric);
            }

            return ratingBar.CoerceToValidIncrement(numeric);
        }

        return (double)ratingBar.Min;
    }

    private void RebuildButtons()
    {
        if (_itemsHost is null)
        {
            return;
        }

        _itemsHost.Children.Clear();
        _ratingButtonsInternal.Clear();

        if (_itemsHost is StackLayout stackLayout)
        {
            stackLayout.Orientation = Orientation;
            stackLayout.Padding = Padding;
            stackLayout.Spacing = 0;
        }

        var start = IsFractionalValueEnabled ? Min + 1 : Min;
        if (InvertDirection)
        {
            for (var i = Max; i >= start; i--)
            {
                AddButton(i);
            }
        }
        else
        {
            for (var i = start; i <= Max; i++)
            {
                AddButton(i);
            }
        }
    }

    private void AddButton(int value)
    {
        if (_itemsHost is null)
        {
            return;
        }

        var button = new RatingBarButton
        {
            Value = value,
            Style = ValueItemContainerButtonStyle
        };

        button.AttachRatingBar(this);

        var content = CreateButtonContent(value);
        if (content is not null)
        {
            content.BindingContext = value;
            button.ButtonContent = content;
        }

        _ratingButtonsInternal.Add(button);
        _itemsHost.Children.Add(button);
    }

    private View? CreateButtonContent(int value)
    {
        var template = ValueItemTemplateSelector?.SelectTemplate(value, this) ?? ValueItemTemplate;
        if (template is null)
        {
            return CreateDefaultStar();
        }

        if (template.CreateContent() is View view)
        {
            return view;
        }

        return CreateDefaultStar();
    }

    private View CreateDefaultStar()
    {
        var path = new Path
        {
            Data = Geometry.Parse("M12,17.27L18.18,21L16.54,13.97L22,9.24L14.81,8.62L12,2L9.19,8.62L2,9.24L7.45,13.97L5.82,21L12,17.27Z"),
            WidthRequest = 24,
            HeightRequest = 24
        };
        path.SetBinding(Shape.FillProperty, new Binding(nameof(ForegroundColor), source: this));
        return path;
    }

    private double GetValueAtPosition(RatingBarButton ratingBarButton, Point point)
    {
        var percentSelected = 0d;
        if (Orientation == StackOrientation.Horizontal)
        {
            percentSelected = point.X / Math.Max(1, ratingBarButton.Width);
            if (InvertDirection)
            {
                percentSelected = 1 - percentSelected;
            }
        }
        else
        {
            percentSelected = point.Y / Math.Max(1, ratingBarButton.Height);
            if (InvertDirection)
            {
                percentSelected = 1 - percentSelected;
            }
        }

        var value = ratingBarButton.Value - 1 + percentSelected;
        return IsFractionalValueEnabled ? CoerceToValidIncrement(value) : ratingBarButton.Value;
    }

    private double CoerceToValidIncrement(double value)
    {
        var valueInCorrectMultiple = Math.Round(value / ValueIncrements, MidpointRounding.AwayFromZero) * ValueIncrements;
        return Math.Min(Max, Math.Max(Min, valueInCorrectMultiple));
    }

    private void UpdateButtonStates()
    {
        var displayValue = PreviewValue ?? Value;
        var clampedValue = Math.Min(Max, Math.Max(Min, displayValue));

        foreach (var button in _ratingButtonsInternal)
        {
            var ratio = 0d;
            if (clampedValue >= button.Value)
            {
                ratio = 1d;
            }
            else if (IsFractionalValueEnabled && clampedValue > button.Value - 1)
            {
                ratio = clampedValue - (button.Value - 1);
            }

            button.Opacity = ratio > 0 ? 1 : 0.38;
        }
    }
}
