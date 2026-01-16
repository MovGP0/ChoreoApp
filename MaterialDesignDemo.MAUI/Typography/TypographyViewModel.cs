namespace MaterialDesignDemo.Maui.Typography;

public sealed class TypographyViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
