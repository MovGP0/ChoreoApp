namespace MaterialDesignDemo.Maui.Cards;

public sealed partial class CardsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();
}
