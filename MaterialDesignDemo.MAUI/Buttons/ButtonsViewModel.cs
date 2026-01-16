namespace MaterialDesignDemo.Maui.Buttons;

public sealed partial class ButtonsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private bool _controlsEnabled = true;

    [ReactiveCommand]
    private void RunDemoAction()
    {
    }
}
