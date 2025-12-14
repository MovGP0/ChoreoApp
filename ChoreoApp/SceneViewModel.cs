namespace ChoreoApp;

public sealed partial class SceneViewModel(
    string name,
    Color color):
    ReactiveObject
{
    [Reactive] private string _name = name;
    [Reactive] private Color _color = color;
}
