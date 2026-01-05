using System.Collections;
using System.Collections.Specialized;

namespace MaterialDesignThemes.Maui;

public class TreeListViewItem : ContentView
{
    public TreeListViewItem()
    {
        var doubleTap = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 2
        };
        doubleTap.Tapped += OnDoubleTapped;
        GestureRecognizers.Add(doubleTap);
    }

    public IEnumerable<object?> GetChildren() => Children?.OfType<object?>() ?? [];

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(TreeListViewItem),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsExpandedChanged);

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TreeListViewItem item)
        {
            item.ExpandedChanged?.Invoke(item, EventArgs.Empty);
        }
    }

    public static readonly BindableProperty HasItemsProperty = BindableProperty.Create(
        nameof(HasItems),
        typeof(bool),
        typeof(TreeListViewItem),
        false);

    public bool HasItems
    {
        get => (bool)GetValue(HasItemsProperty);
        set => SetValue(HasItemsProperty, value);
    }

    public static readonly BindableProperty LevelProperty = BindableProperty.Create(
        nameof(Level),
        typeof(int),
        typeof(TreeListViewItem),
        0);

    public int Level
    {
        get => (int)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public static readonly BindableProperty DisableExpandOnDoubleClickProperty = BindableProperty.Create(
        nameof(DisableExpandOnDoubleClick),
        typeof(bool),
        typeof(TreeListViewItem),
        false);

    public bool DisableExpandOnDoubleClick
    {
        get => (bool)GetValue(DisableExpandOnDoubleClickProperty);
        set => SetValue(DisableExpandOnDoubleClickProperty, value);
    }

    internal static readonly BindableProperty ChildrenProperty = BindableProperty.Create(
        nameof(Children),
        typeof(IEnumerable),
        typeof(TreeListViewItem),
        propertyChanged: OnChildrenChanged);

    internal IEnumerable? Children
    {
        get => (IEnumerable?)GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    internal event EventHandler? ExpandedChanged;
    internal event EventHandler<NotifyCollectionChangedEventArgs>? ChildrenChanged;

    private static void OnChildrenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TreeListViewItem item)
        {
            item.OnChildrenChanged(oldValue as IEnumerable, newValue as IEnumerable);
        }
    }

    private void OnChildrenChanged(IEnumerable? oldValue, IEnumerable? newValue)
    {
        if (oldValue is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= OnChildrenCollectionChanged;
        }

        if (newValue is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += OnChildrenCollectionChanged;
        }

        UpdateHasChildren();
        ChildrenChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateHasChildren();
        ChildrenChanged?.Invoke(this, e);
    }

    private void UpdateHasChildren()
    {
        HasItems = GetChildren().Any();
    }

    private void OnDoubleTapped(object? sender, EventArgs e)
    {
        if (!DisableExpandOnDoubleClick)
        {
            IsExpanded = !IsExpanded;
        }
    }
}
