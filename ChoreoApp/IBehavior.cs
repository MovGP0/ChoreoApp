using System.Reactive.Disposables;

// ReSharper disable once CheckNamespace
namespace ReactiveUI;

public interface IBehavior<in T>
{
    void Activate(T viewModel, CompositeDisposable disposables);
}
