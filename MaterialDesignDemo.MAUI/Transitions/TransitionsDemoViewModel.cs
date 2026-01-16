namespace MaterialDesignDemo.MAUI.Transitions;

public sealed partial class TransitionsDemoViewModel : ReactiveObject, IActivatableViewModel
{
    private const int SlideCount = 7;

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private int _selectedIndex;

    [Reactive]
    private string _name = string.Empty;

    [Reactive]
    private bool _isOriginsSecond;

    [ReactiveCommand]
    private void MoveNext()
    {
        if (SelectedIndex < SlideCount - 1)
        {
            SelectedIndex++;
        }
    }

    [ReactiveCommand]
    private void MovePrevious()
    {
        if (SelectedIndex > 0)
        {
            SelectedIndex--;
        }
    }

    [ReactiveCommand]
    private void ShowFirstOrigin()
    {
        IsOriginsSecond = false;
    }

    [ReactiveCommand]
    private void ShowSecondOrigin()
    {
        IsOriginsSecond = true;
    }
}
