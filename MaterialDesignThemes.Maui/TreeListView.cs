using System.Collections.Specialized;

namespace MaterialDesignThemes.Maui;

public class TreeListView : CollectionView
{
    public static readonly BindableProperty LevelIndentSizeProperty = BindableProperty.Create(
        nameof(LevelIndentSize),
        typeof(double),
        typeof(TreeListView),
        16d);

    public double LevelIndentSize
    {
        get => (double)GetValue(LevelIndentSizeProperty);
        set => SetValue(LevelIndentSizeProperty, value);
    }

    public TreeListView()
    {
    }

    internal void ItemExpandedChanged(TreeListViewItem item)
    {
    }

    internal void ItemsChildrenChanged(TreeListViewItem item, NotifyCollectionChangedEventArgs e)
    {
    }

    public object? GetParent(object? item)
    {
        return null;
    }

    internal void MoveSelectionToParent(TreeListViewItem item)
    {
    }
}
