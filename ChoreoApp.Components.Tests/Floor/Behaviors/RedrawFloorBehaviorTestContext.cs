using System.Reactive.Concurrency;

using ChoreoApp.AudioPlayer.Messages;
using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Scenes;
using ChoreoApp.Settings;
using ChoreoApp.StateMachine;
using MessagePipe;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class RedrawFloorBehaviorTestContext : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDisposable _activation;

    private RedrawFloorBehaviorTestContext(
        ServiceProvider serviceProvider,
        GlobalStateModel globalState,
        FloorCanvasViewModel viewModel,
        ISKCanvasView canvasView,
        IPublisher<SelectedSceneChangedEvent> selectedSceneChangedPublisher,
        IPublisher<AudioPlayerPositionChangedEvent> audioPositionChangedPublisher,
        IPublisher<RedrawFloorCommand> redrawPublisher)
    {
        _serviceProvider = serviceProvider;
        GlobalState = globalState;
        ViewModel = viewModel;
        CanvasView = canvasView;
        SelectedSceneChangedPublisher = selectedSceneChangedPublisher;
        AudioPositionChangedPublisher = audioPositionChangedPublisher;
        RedrawPublisher = redrawPublisher;
        ViewModel.CanvasView = canvasView;
        _activation = ViewModel.Activator.Activate();
    }

    public GlobalStateModel GlobalState { get; }
    public FloorCanvasViewModel ViewModel { get; }
    public ISKCanvasView CanvasView { get; }
    public IPublisher<SelectedSceneChangedEvent> SelectedSceneChangedPublisher { get; }
    public IPublisher<AudioPlayerPositionChangedEvent> AudioPositionChangedPublisher { get; }
    public IPublisher<RedrawFloorCommand> RedrawPublisher { get; }

    public static RedrawFloorBehaviorTestContext Create()
    {
        RxApp.MainThreadScheduler = ImmediateScheduler.Instance;
        RxApp.TaskpoolScheduler = ImmediateScheduler.Instance;

        var services = new ServiceCollection();
        services.AddMessagePipe();

        var preferences = Substitute.For<IPreferences>();
        preferences.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

        var haptic = Substitute.For<IHapticFeedback>();
        haptic.IsSupported.Returns(false);

        var vibration = Substitute.For<IVibration>();
        vibration.IsSupported.Returns(false);

        services.AddSingleton(preferences);
        services.AddSingleton(haptic);
        services.AddSingleton(vibration);
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<GlobalStateModel>();
        services.AddSingleton<IGlobalStateModel>(sp => sp.GetRequiredService<GlobalStateModel>());
        services.AddApplicationStateMachine();
        services.AddSingleton<SceneViewModel>();

        services.AddSingleton<RedrawFloorBehavior>();
        services.AddSingleton<IBehavior<FloorCanvasViewModel>>(sp => sp.GetRequiredService<RedrawFloorBehavior>());
        services.AddSingleton<FloorCanvasViewModel>();

        var canvasView = Substitute.For<ISKCanvasView>();
        canvasView.Width.Returns(100d);
        canvasView.Height.Returns(100d);
        canvasView.CanvasSize.Returns(new SKSize(100, 100));

        var provider = services.BuildServiceProvider();
        var globalState = provider.GetRequiredService<GlobalStateModel>();
        var viewModel = provider.GetRequiredService<FloorCanvasViewModel>();
        var selectedSceneChangedPublisher = provider.GetRequiredService<IPublisher<SelectedSceneChangedEvent>>();
        var audioPositionChangedPublisher = provider.GetRequiredService<IPublisher<AudioPlayerPositionChangedEvent>>();
        var redrawPublisher = provider.GetRequiredService<IPublisher<RedrawFloorCommand>>();

        return new RedrawFloorBehaviorTestContext(
            provider,
            globalState,
            viewModel,
            canvasView,
            selectedSceneChangedPublisher,
            audioPositionChangedPublisher,
            redrawPublisher);
    }

    public void Dispose()
    {
        ViewModel.CanvasView = null;
        _activation.Dispose();
        _serviceProvider.Dispose();
    }
}
