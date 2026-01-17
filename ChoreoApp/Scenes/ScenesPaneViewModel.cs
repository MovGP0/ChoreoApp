using System.Collections.ObjectModel;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.Models;
using ChoreoApp.Dancers;
using ChoreoApp.Settings;
using MessagePipe;

namespace ChoreoApp.Scenes;

public sealed partial class ScenesPaneViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    private readonly GlobalStateModel _globalState;
    private readonly IPublisher<Main.Messages.ShowDialogCommand> _showDialogPublisher;
    private readonly IPublisher<Main.Messages.CloseDialogCommand> _closeDialogPublisher;
    private readonly IHapticFeedback _hapticFeedback;

    public ScenesPaneViewModel(
        GlobalStateModel globalState,
        IEnumerable<IBehavior<ScenesPaneViewModel>> behaviors,
        IHapticFeedback hapticFeedback,
        IPublisher<Main.Messages.ShowDialogCommand> showDialogPublisher,
        IPublisher<Main.Messages.CloseDialogCommand> closeDialogPublisher)
    {
        _globalState = globalState;
        _hapticFeedback = hapticFeedback;
        _showDialogPublisher = showDialogPublisher;
        _closeDialogPublisher = closeDialogPublisher;

        this.WhenActivated(disposables =>
        {
            _globalState
                .WhenAnyValue(gs => gs.SelectedScene)
                .Subscribe(scene =>
                {
                    CanDeleteScene = scene is not null;
                    this.RaisePropertyChanged(nameof(SelectedScene));
                })
                .DisposeWith(disposables);

            _globalState
                .WhenAnyValue(gs => gs.Choreography)
                .Subscribe(_ => UpdateCanSave())
                .DisposeWith(disposables);

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
    private bool _canSaveChoreo;

    [Reactive]
    private bool _canDeleteScene;

    [Reactive]
    private bool _showTimestamps;

    public SceneViewModel? SelectedScene
    {
        get => _globalState.SelectedScene;
        set => _globalState.SelectedScene = value;
    }

    [ReactiveCommand]
    private void AddSceneBefore()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
    }

    [ReactiveCommand]
    private void AddSceneAfter()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
    }

    internal void RefreshScenes()
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

    [Reactive]
    private bool _canNavigateToDancerSettings = true;

    [ReactiveCommand(CanExecute = nameof(CanNavigateToSettings))]
    private async Task NavigateToSettingsAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);

        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync(nameof(SettingsPage));
        }
    }

    [ReactiveCommand(CanExecute = nameof(CanNavigateToDancerSettings))]
    private async Task NavigateToDancerSettingsAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);

        if (Shell.Current is { } shell)
        {
            await shell.GoToAsync(nameof(DancerSettingsPage));
        }
    }

    [ReactiveCommand]
    private Task OpenChoreoAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
        return Task.CompletedTask;
    }

    [ReactiveCommand(CanExecute = nameof(CanSaveChoreo))]
    private Task SaveChoreoAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
        return Task.CompletedTask;
    }

    [ReactiveCommand(CanExecute = nameof(CanDeleteScene))]
    private void DeleteScene()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);

        if (SelectedScene is null)
        {
            return;
        }

        var dialogViewModel = new DeleteSceneDialogViewModel(
            _globalState,
            _closeDialogPublisher,
            _hapticFeedback,
            SelectedScene);
        var dialogView = new DeleteSceneDialogView { ViewModel = dialogViewModel };
        _showDialogPublisher.Publish(new Main.Messages.ShowDialogCommand(dialogView));
    }

    private void UpdateCanSave()
    {
        var path = Preferences.Default.Get(SettingsPreferenceKeys.LastOpenedChoreoFile, string.Empty);
        CanSaveChoreo = _globalState.Choreography is not null
            && !string.IsNullOrWhiteSpace(path)
            && File.Exists(path);
    }
}
