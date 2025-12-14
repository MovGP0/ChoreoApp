using System.Collections.ObjectModel;
using ChoreoApp.Settings;
using ChoreoApp.AudioPlayer;

namespace ChoreoApp.Scenes;

public sealed partial class ScenesPaneViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public ScenesPaneViewModel(IEnumerable<IBehavior<ScenesPaneViewModel>> behaviors)
    {
        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    [Reactive]
    private string _searchText = string.Empty;

    [ReactiveCollection]
    private ObservableCollection<SceneViewModel> _scenes = [];

    [Reactive]
    private SceneViewModel? _selectedScene;

    [ReactiveCommand]
    private void SelectScene(SceneViewModel scene)
    {
        SelectedScene = scene;
    }

    public void MoveScenes(SceneViewModel? item, SceneViewModel? target)
    {
        if (item is null || target is null || item == target)
        {
            return;
        }

        var oldIndex = Scenes.IndexOf(item);
        var newIndex = Scenes.IndexOf(target);

        if (oldIndex < 0 || newIndex < 0 || oldIndex == newIndex)
        {
            return;
        }

        Scenes.RemoveAt(oldIndex);
        Scenes.Insert(newIndex, item);
    }

    [Reactive]
    private bool _canNavigateToSettings = true;

    [ReactiveCommand(CanExecute = nameof(CanNavigateToSettings))]
    private async Task NavigateToSettingsAsync()
    {
        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync(nameof(SettingsPage));
        }
    }

    [Reactive]
    private bool _canNavigateToAudioPlayer = true;

    [ReactiveCommand(CanExecute = nameof(CanNavigateToAudioPlayer))]
    private async Task NavigateToAudioPlayerAsync()
    {
        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync(nameof(AudioPlayerPage));
        }
    }

    [ReactiveCommand]
    private Task OpenChoreoAsync() => Task.CompletedTask;
}


