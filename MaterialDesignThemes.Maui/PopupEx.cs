namespace MaterialDesignThemes.Maui;

public class PopupEx : ContentView
{
    public static readonly BindableProperty CloseOnMouseLeftButtonDownProperty =
        BindableProperty.Create(
            nameof(CloseOnMouseLeftButtonDown),
            typeof(bool),
            typeof(PopupEx),
            false);

    public bool CloseOnMouseLeftButtonDown
    {
        get => (bool)GetValue(CloseOnMouseLeftButtonDownProperty);
        set => SetValue(CloseOnMouseLeftButtonDownProperty, value);
    }

    public static readonly BindableProperty AllowTopMostProperty =
        BindableProperty.Create(
            nameof(AllowTopMost),
            typeof(bool),
            typeof(PopupEx),
            true);

    public bool AllowTopMost
    {
        get => (bool)GetValue(AllowTopMostProperty);
        set => SetValue(AllowTopMostProperty, value);
    }

    public static readonly BindableProperty IsOpenProperty =
        BindableProperty.Create(
            nameof(IsOpen),
            typeof(bool),
            typeof(PopupEx),
            false,
            BindingMode.TwoWay,
            propertyChanged: OnIsOpenChanged);

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public void RefreshPosition()
    {
    }

    private static void OnIsOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PopupEx popup)
        {
            popup.IsVisible = (bool)newValue;
        }
    }
}
