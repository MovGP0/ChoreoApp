using System.Reactive.Concurrency;

using ReactiveUI.Builder;

namespace ChoreoApp.Components.Tests;

internal static class ReactiveUITestInitialization
{
    private static int s_initialized;

    public static void Initialize()
    {
        if (Interlocked.Exchange(ref s_initialized, 1) == 0)
        {
            RxAppBuilder.CreateReactiveUIBuilder()
                .WithCoreServices()
                .BuildApp();
        }

        RxSchedulers.MainThreadScheduler = ImmediateScheduler.Instance;
        RxSchedulers.TaskpoolScheduler = ImmediateScheduler.Instance;
    }
}
