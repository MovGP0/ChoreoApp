namespace MaterialDesignThemes.Maui;

public sealed class ClockItemButton : ContentButton
{
    public static readonly BindableProperty CentreXProperty = BindableProperty.Create(
        nameof(CentreX),
        typeof(double),
        typeof(ClockItemButton),
        0d,
        propertyChanged: OnLayoutPropertyChanged);

    public double CentreX
    {
        get => (double)GetValue(CentreXProperty);
        set => SetValue(CentreXProperty, value);
    }

    public static readonly BindableProperty CentreYProperty = BindableProperty.Create(
        nameof(CentreY),
        typeof(double),
        typeof(ClockItemButton),
        0d,
        propertyChanged: OnLayoutPropertyChanged);

    public double CentreY
    {
        get => (double)GetValue(CentreYProperty);
        set => SetValue(CentreYProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(ClockItemButton),
        0);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
        nameof(IsChecked),
        typeof(bool),
        typeof(ClockItemButton),
        false);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public ClockItemButton()
    {
        SizeChanged += (_, _) => UpdateLayoutBounds();
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ClockItemButton button)
        {
            button.UpdateLayoutBounds();
        }
    }

    private void UpdateLayoutBounds()
    {
        if (Parent is not AbsoluteLayout)
        {
            return;
        }

        var width = Width > 0 ? Width : WidthRequest;
        var height = Height > 0 ? Height : HeightRequest;
        if (width <= 0)
        {
            width = 32;
        }

        if (height <= 0)
        {
            height = 32;
        }

        AbsoluteLayout.SetLayoutBounds(this, new Rect(CentreX - width / 2, CentreY - height / 2, width, height));
    }
}
