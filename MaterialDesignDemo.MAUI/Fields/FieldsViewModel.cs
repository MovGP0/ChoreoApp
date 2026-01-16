namespace MaterialDesignDemo.Maui.Fields;

public sealed partial class FieldsViewModel : ReactiveObject, IActivatableViewModel
{
    public FieldsViewModel()
    {
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string? _name;

    [Reactive]
    private string? _email;

    [Reactive]
    private string? _notes;
}
