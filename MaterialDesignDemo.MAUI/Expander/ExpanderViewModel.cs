namespace MaterialDesignDemo.Maui.Expander;

public sealed partial class ExpanderViewModel : ReactiveObject, IActivatableViewModel
{
    public ExpanderViewModel()
    {
    }

    public ViewModelActivator Activator { get; } = new();
}
