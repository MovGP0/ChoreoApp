namespace MaterialDesignThemes.Maui;

[ContentProperty(nameof(Child))]
public class Plane3D : ContentView
{
    public static readonly BindableProperty RotationXProperty =
        BindableProperty.Create(
            nameof(RotationX),
            typeof(double),
            typeof(Plane3D),
            0.0);

    public double RotationX
    {
        get => (double)GetValue(RotationXProperty);
        set => SetValue(RotationXProperty, value);
    }

    public static readonly BindableProperty RotationYProperty =
        BindableProperty.Create(
            nameof(RotationY),
            typeof(double),
            typeof(Plane3D),
            0.0);

    public double RotationY
    {
        get => (double)GetValue(RotationYProperty);
        set => SetValue(RotationYProperty, value);
    }

    public static readonly BindableProperty RotationZProperty =
        BindableProperty.Create(
            nameof(RotationZ),
            typeof(double),
            typeof(Plane3D),
            0.0);

    public double RotationZ
    {
        get => (double)GetValue(RotationZProperty);
        set => SetValue(RotationZProperty, value);
    }

    public static readonly BindableProperty FieldOfViewProperty =
        BindableProperty.Create(
            nameof(FieldOfView),
            typeof(double),
            typeof(Plane3D),
            45.0);

    public double FieldOfView
    {
        get => (double)GetValue(FieldOfViewProperty);
        set => SetValue(FieldOfViewProperty, value);
    }

    public static readonly BindableProperty ZFactorProperty =
        BindableProperty.Create(
            nameof(ZFactor),
            typeof(double),
            typeof(Plane3D),
            2.0);

    public double ZFactor
    {
        get => (double)GetValue(ZFactorProperty);
        set => SetValue(ZFactorProperty, value);
    }

    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(View),
            typeof(Plane3D),
            propertyChanged: OnChildChanged);

    public View? Child
    {
        get => (View?)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    private static void OnChildChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Plane3D plane3D)
        {
            plane3D.Content = newValue as View;
        }
    }
}
