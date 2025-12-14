using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Scenes;
using MessagePipe;
using Microsoft.Maui.ApplicationModel;

namespace ChoreoApp.Floor.Behaviors;

public sealed class RedrawFloorBehavior(
    GlobalStateModel globalState,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber) : IBehavior<FloorCanvasViewModel>
{
    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        globalState
            .WhenAnyValue(gs => gs.Choreography)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(_ => InvalidateCanvas(viewModel))
            .DisposeWith(disposables);
    }

    private static void InvalidateCanvas(FloorCanvasViewModel viewModel)
    {
        if (viewModel.CanvasView is null)
        {
            return;
        }

        if (MainThread.IsMainThread)
        {
            viewModel.CanvasView.InvalidateSurface();
            return;
        }

        MainThread.BeginInvokeOnMainThread(viewModel.CanvasView.InvalidateSurface);
    }
}
