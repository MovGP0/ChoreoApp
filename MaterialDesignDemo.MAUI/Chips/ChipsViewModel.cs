namespace MaterialDesignDemo.Maui.Chips;

public sealed partial class ChipsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
