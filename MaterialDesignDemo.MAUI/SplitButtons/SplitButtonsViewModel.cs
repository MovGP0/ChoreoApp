namespace MaterialDesignDemo.Maui.SplitButtons;

public sealed partial class SplitButtonsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _controlsEnabled = true;
}
