namespace MaterialDesignDemo.Maui.FieldsLineUp;

public sealed partial class FieldsLineUpViewModel : ReactiveObject, IActivatableViewModel
{
    public FieldsLineUpViewModel()
    {
    }

    public ViewModelActivator Activator { get; } = new();
}
