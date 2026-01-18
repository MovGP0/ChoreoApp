using System.Reactive.Concurrency;
using System.Reactive.Linq;
using ChoreoApp.Components.Tests.Floor;
using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Settings;
using ChoreoApp.StateMachine;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class GestureHandlingBehaviorTestContext : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDisposable _activation;

    private GestureHandlingBehaviorTestContext(
        ServiceProvider serviceProvider,
        FloorCanvasViewModel viewModel,
        ISKCanvasView canvasView)
    {
        _serviceProvider = serviceProvider;
        ViewModel = viewModel;
        CanvasView = canvasView;
        ViewModel.CanvasView = canvasView;
        _activation = ViewModel.Activator.Activate();
    }

    public FloorCanvasViewModel ViewModel { get; }
    public ISKCanvasView CanvasView { get; }

    public static GestureHandlingBehaviorTestContext Create()
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

        services.AddTransient<IBehavior<FloorCanvasViewModel>, GestureHandlingBehavior>();
        services.AddSingleton<FloorCanvasViewModel>();

        var canvasView = Substitute.For<ISKCanvasView>();
        canvasView.Width.Returns(100d);
        canvasView.Height.Returns(100d);
        canvasView.CanvasSize.Returns(new SKSize(100, 100));

        var provider = services.BuildServiceProvider();
        var viewModel = provider.GetRequiredService<FloorCanvasViewModel>();
        return new GestureHandlingBehaviorTestContext(provider, viewModel, canvasView);
    }

    public void SendPointerPressed(Point viewPoint)
    {
        var args = new TestPointerEventArgs(viewPoint);
        ViewModel.PointerPressedCommand
            .Execute(new PointerPressedCommand(CanvasView, args))
            .FirstAsync()
            .Wait();
    }

    public void SendPointerMoved(Point viewPoint)
    {
        var args = new TestPointerEventArgs(viewPoint);
        ViewModel.PointerMovedCommand
            .Execute(new PointerMovedCommand(CanvasView, args))
            .FirstAsync()
            .Wait();
    }

    public void Dispose()
    {
        ViewModel.CanvasView = null;
        _activation.Dispose();
        _serviceProvider.Dispose();
    }
}
