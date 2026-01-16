using System.Collections.ObjectModel;

namespace MaterialDesignDemo.Maui.Trees;

public sealed partial class TreesViewModel : ReactiveObject, IActivatableViewModel
{
    private int _dynamicNodeIndex = 1;

    public TreesViewModel()
    {
        BasicNodes =
        [
            new("Fruit", 0),
            new("Apple", 1),
            new("Banana", 1),
            new("Grape", 1),
            new("Peach", 1),
            new("Pear", 1),
            new("Strawberry", 1),
            new("OS", 0),
            new("Android", 1),
            new("iOS", 1),
            new("Linux", 1),
            new("Windows", 1),
            new("Empty", 0)
        ];

        MovieNodes =
        [
            new("Drama", 0),
            new("The Shawshank Redemption", 1),
            new("Fight Club", 1),
            new("Sci-Fi", 0),
            new("Interstellar", 1),
            new("The Matrix", 1)
        ];
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<TreeNode> BasicNodes { get; }
    public ObservableCollection<TreeNode> MovieNodes { get; }

    [Reactive]
    private TreeNode? _selectedMovieNode;

    [ReactiveCommand]
    private void AddNode()
    {
        MovieNodes.Add(new TreeNode($"New Item {_dynamicNodeIndex}", 1));
        _dynamicNodeIndex++;
    }

    [ReactiveCommand]
    private void RemoveSelectedNode()
    {
        if (SelectedMovieNode is null)
        {
            return;
        }

        MovieNodes.Remove(SelectedMovieNode);
    }
}
