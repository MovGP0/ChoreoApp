namespace ChoreoApp.Styling;

/// <summary>
/// Simple underline control with active/inactive visual states, ported from WPF.
/// Template should define states "Active" and "Inactive" in the "ActivationStates" group.
/// </summary>
public class Underline : ContentView
{
    public const string ActiveStateName = "Active";
    public const string InactiveStateName = "Inactive";

    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
            nameof(IsActive),
            typeof(bool),
            typeof(Underline),
            false,
            propertyChanged: OnIsActiveChanged);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(Underline),
            new CornerRadius(0));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        GotoVisualState(false);
    }

    private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Underline underline)
        {
            underline.GotoVisualState(true);
        }
    }

    private void GotoVisualState(bool useTransitions)
    {
        var state = IsActive ? ActiveStateName : InactiveStateName;
        VisualStateManager.GoToState(this, state);
    }
}
