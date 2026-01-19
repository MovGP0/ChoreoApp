using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using ChoreoApp.AudioPlayer.Messages;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Scenes;
using MessagePipe;

namespace ChoreoApp.Floor.Behaviors;

public sealed class RedrawFloorBehavior(
    Global.GlobalStateModel globalState,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber,
    ISubscriber<AudioPlayerPositionChangedEvent> audioPositionChangedSubscriber,
    ISubscriber<RedrawFloorCommand> redrawFloorSubscriber)
    : IBehavior<FloorCanvasViewModel>
{
    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);

        globalState
            .WhenAnyValue(gs => gs.SvgDocument)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);

        audioPositionChangedSubscriber
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);

        redrawFloorSubscriber
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);
    }

    private static void InvalidateCanvas(FloorCanvasViewModel viewModel)
    {
        if (viewModel.CanvasView is null)
        {
            return;
        }

        var isMainThread = false;
        try
        {
            isMainThread = MainThread.IsMainThread;
        }
        catch (COMException)
        {
            isMainThread = true;
        }
        catch (TypeInitializationException)
        {
            isMainThread = true;
        }

        if (isMainThread)
        {
            viewModel.CanvasView.InvalidateSurface();
            return;
        }

        try
        {
            MainThread.BeginInvokeOnMainThread(viewModel.CanvasView.InvalidateSurface);
        }
        catch (COMException)
        {
            viewModel.CanvasView.InvalidateSurface();
        }
        catch (TypeInitializationException)
        {
            viewModel.CanvasView.InvalidateSurface();
        }
        catch (InvalidOperationException)
        {
            viewModel.CanvasView.InvalidateSurface();
        }
    }
}
