using System.Collections.ObjectModel;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.Settings;
using DynamicData;

namespace ChoreoApp.Scenes;

public sealed partial class ScenesPaneViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    private readonly GlobalStateModel _globalState;

    public ScenesPaneViewModel(
        GlobalStateModel globalState,
        IEnumerable<IBehavior<ScenesPaneViewModel>> behaviors)
    {
        _globalState = globalState;

        this.WhenActivated(disposables =>
        {
            _globalState
                .WhenAnyValue(gs => gs.SelectedScene)
                .Subscribe(_ => this.RaisePropertyChanged(nameof(SelectedScene)))
                .DisposeWith(disposables);

            _globalState
                .Scenes
                .AsObservableChangeSet()
                .Subscribe(_ => RefreshScenes())
                .DisposeWith(disposables);

            this
                .WhenAnyValue(vm => vm.SearchText)
                .Subscribe(_ => RefreshScenes())
                .DisposeWith(disposables);

            RefreshScenes();

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

    public SceneViewModel? SelectedScene
    {
        get => _globalState.SelectedScene;
        set => _globalState.SelectedScene = value;
    }

    [ReactiveCommand]
    private void AddSceneBefore()
    {
    }

    [ReactiveCommand]
    private void AddSceneAfter()
    {
    }

    public void MoveScenes(SceneViewModel? item, SceneViewModel? target)
    {
        if (item is null || target is null || item == target)
        {
            return;
        }

        var scenes = _globalState.Scenes;
        var oldIndex = scenes.IndexOf(item);
        var newIndex = scenes.IndexOf(target);

        if (oldIndex < 0 || newIndex < 0 || oldIndex == newIndex)
        {
            return;
        }

        scenes.RemoveAt(oldIndex);
        scenes.Insert(newIndex, item);
        RefreshScenes();
    }

    private void RefreshScenes()
    {
        _scenes.Clear();

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            foreach (var scene in _globalState.Scenes)
            {
                _scenes.Add(scene);
            }

            return;
        }

        foreach (var scene in _globalState.Scenes)
        {
            if (scene.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            {
                _scenes.Add(scene);
            }
        }
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

    [ReactiveCommand]
    private Task OpenChoreoAsync() => Task.CompletedTask;
}


