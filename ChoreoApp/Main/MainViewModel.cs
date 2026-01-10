using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.AudioPlayer;
using ChoreoApp.Global;
using ChoreoApp.i18n;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;

namespace ChoreoApp.Main;

public sealed partial class MainViewModel : ReactiveObject, IActivatableViewModel
{
    public MainViewModel(
        IEnumerable<IBehavior<MainViewModel>> behaviors,
        GlobalStateModel globalState,
        ApplicationStateMachine stateMachine,
        AudioPlayerViewModel audioPlayerViewModel)
    {
        _globalState = globalState;
        _stateMachine = stateMachine;
        AudioPlayerViewModel = audioPlayerViewModel;

        ModeOptions = BuildModeOptions();
        SelectedModeOption = ModeOptions.FirstOrDefault(option => option.Mode == _globalState.InteractionMode)
            ?? ModeOptions[0];

        this.WhenActivated(disposables =>
        {
            audioPlayerViewModel.Activator.Activate().DisposeWith(disposables);
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }

            var placeModeSubscription = new SerialDisposable().DisposeWith(disposables);

            _globalState.WhenAnyValue(state => state.SelectedScene)
                .Subscribe(scene =>
                {
                    UpdatePlaceMode(scene);
                    placeModeSubscription.Disposable = scene is null
                        ? Disposable.Empty
                        : Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                                handler => scene.Positions.CollectionChanged += handler,
                                handler => scene.Positions.CollectionChanged -= handler)
                            .Subscribe(_ => UpdatePlaceMode(scene));
                })
                .DisposeWith(disposables);

            _globalState.WhenAnyValue(state => state.Choreography)
                .Subscribe(_ => UpdatePlaceMode(_globalState.SelectedScene))
                .DisposeWith(disposables);

            _globalState.WhenAnyValue(state => state.IsPlaceMode)
                .Subscribe(isPlaceMode =>
                {
                    IsModeSelectionEnabled = !isPlaceMode;
                })
                .DisposeWith(disposables);

            _globalState.WhenAnyValue(state => state.InteractionMode)
                .Subscribe(mode => SelectedModeOption = ModeOptions.FirstOrDefault(option => option.Mode == mode) ?? ModeOptions[0])
                .DisposeWith(disposables);

            this.WhenAnyValue(viewModel => viewModel.SelectedModeOption)
                .Where(option => option is not null)
                .Subscribe(option => _globalState.InteractionMode = option!.Mode)
                .DisposeWith(disposables);
        });
    }

    private const double DefaultNavWidth = 280d;
    private readonly GlobalStateModel _globalState;
    private readonly ApplicationStateMachine _stateMachine;

    public ViewModelActivator Activator { get; } = new();
    public AudioPlayerViewModel AudioPlayerViewModel { get; }
    public IReadOnlyList<InteractionModeOption> ModeOptions { get; }

    [Reactive]
    private InteractionModeOption? _selectedModeOption;

    [Reactive]
    private bool _isModeSelectionEnabled = true;

    [Reactive]
    private GridLength _navColumnWidth = new(DefaultNavWidth);

    [Reactive]
    private bool _isNavOpen = true;

    [Reactive]
    private bool _isAudioPlayerOpen;

    [Reactive]
    private bool _isChoreographySettingsOpen;

    [Reactive]
    private bool _isDialogOpen;

    [Reactive]
    private View? _dialogContentView;

    [ReactiveCommand]
    private Task OpenAudioAsync()
    {
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private Task OpenImageAsync()
    {
        return Task.CompletedTask;
    }

    [ReactiveCommand]
    private void OpenChoreographySettings()
    {
        IsChoreographySettingsOpen = true;
    }

    private void UpdatePlaceMode(SceneViewModel? scene)
    {
        if (scene is null)
        {
            _globalState.IsPlaceMode = false;
            return;
        }

        int dancerCount = _globalState.Choreography.Dancers.Count;
        int positionCount = scene.Positions.Count;
        _globalState.IsPlaceMode = dancerCount > 0 && positionCount < dancerCount;
    }

    private static List<InteractionModeOption> BuildModeOptions() => new()
    {
        new(InteractionMode.View, Translations.ModeView),
        new(InteractionMode.Move, Translations.ModeMove),
        new(InteractionMode.RotateAroundCenter, Translations.ModeRotateAroundCenter),
        new(InteractionMode.RotateAroundDancer, Translations.ModeRotateAroundDancer),
        new(InteractionMode.Scale, Translations.ModeScale),
        new(InteractionMode.LineOfSight, Translations.ModeLineOfSight)
    };

    public void ToggleNavigation()
    {
        IsNavOpen = !IsNavOpen;
        NavColumnWidth = IsNavOpen ? new GridLength(DefaultNavWidth) : new GridLength(0);
    }
}
