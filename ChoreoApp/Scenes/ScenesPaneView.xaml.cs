using System.Reactive.Disposables.Fluent;

namespace ChoreoApp.Scenes;

public partial class ScenesPaneView
{
    public ScenesPaneView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            ViewModel?.Activator.Activate().DisposeWith(disposables);
        });
    }

    private void OnNavItemDragStarting(object sender, DragStartingEventArgs e)
    {
        if (sender is BindableObject { BindingContext: SceneViewModel item })
        {
            e.Data.Properties["NavItem"] = item;
        }
    }

    private void OnNavItemDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
    }

    private void OnNavItemDrop(object sender, DropEventArgs e)
    {
        if (BindingContext is not ScenesPaneViewModel viewModel)
        {
            return;
        }

        if (e.Data.Properties.TryGetValue("NavItem", out var dragged)
            && dragged is SceneViewModel draggedItem
            && sender is BindableObject { BindingContext: SceneViewModel targetItem })
        {
            viewModel.MoveScenes(draggedItem, targetItem);
        }
    }
}
