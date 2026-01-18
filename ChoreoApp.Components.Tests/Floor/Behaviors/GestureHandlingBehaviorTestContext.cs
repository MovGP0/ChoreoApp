using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Transitions;
using MessagePipe;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class GestureHandlingBehaviorTestContext : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    private GestureHandlingBehaviorTestContext(
        ServiceProvider serviceProvider,
        FloorCanvasViewModel viewModel,
        ISKCanvasView canvasView)
    {
        _serviceProvider = serviceProvider;
        ViewModel = viewModel;
        CanvasView = canvasView;
        ViewModel.CanvasView = canvasView;
        ViewModel.Activator.Activate();
    }

    public FloorCanvasViewModel ViewModel { get; }
    public ISKCanvasView CanvasView { get; }

    public static GestureHandlingBehaviorTestContext Create()
    {
        var services = new ServiceCollection();
        services.AddMessagePipe();

        var globalState = Substitute.For<IGlobalStateModel>();
        services.AddSingleton(globalState);
        services.AddSingleton(new ApplicationStateMachine(globalState, Array.Empty<StateTransition>()));
        services.AddTransient<IBehavior<FloorCanvasViewModel>, GestureHandlingBehavior>();

        services.AddTransient(sp => new FloorCanvasViewModel(
            sp.GetRequiredService<IPublisher<DrawFloorCommand>>(),
            sp.GetRequiredService<IEnumerable<IBehavior<FloorCanvasViewModel>>>()));

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
        SendPointer(viewPoint, vm => vm.PointerPressedCommand, args => new PointerPressedCommand(CanvasView, args));
    }

    public void SendPointerMoved(Point viewPoint)
    {
        SendPointer(viewPoint, vm => vm.PointerMovedCommand, args => new PointerMovedCommand(CanvasView, args));
    }

    public void Dispose()
    {
        ViewModel.CanvasView = null;
        _serviceProvider.Dispose();
    }

    private void SendPointer<TCommand>(
        Point viewPoint,
        Func<FloorCanvasViewModel, IReactiveCommand<TCommand, TCommand>> commandSelector,
        Func<PointerEventArgs, TCommand> commandFactory)
    {
        var args = new TestPointerEventArgs(viewPoint);
        commandSelector(ViewModel)
            .Execute(commandFactory(args))
            .Subscribe();
    }
}
