namespace MaterialDesignThemes.Maui;

public static class CollectionViewAssist
{
    private const string SelectionAnimationName = "MaterialDesignCollectionViewSelection";

    public static readonly BindableProperty ItemPaddingProperty =
        BindableProperty.CreateAttached(
            "ItemPadding",
            typeof(Thickness),
            typeof(CollectionViewAssist),
            new Thickness(8, 8, 8, 8));

    public static void SetItemPadding(BindableObject element, Thickness value) =>
        element.SetValue(ItemPaddingProperty, value);

    public static Thickness GetItemPadding(BindableObject element) =>
        (Thickness)element.GetValue(ItemPaddingProperty);

    public static readonly BindableProperty AnimateSelectionProperty =
        BindableProperty.CreateAttached(
            "AnimateSelection",
            typeof(bool),
            typeof(CollectionViewAssist),
            false,
            propertyChanged: OnAnimateSelectionChanged);

    public static bool GetAnimateSelection(BindableObject element) =>
        (bool)element.GetValue(AnimateSelectionProperty);

    public static void SetAnimateSelection(BindableObject element, bool value) =>
        element.SetValue(AnimateSelectionProperty, value);

    public static readonly BindableProperty SelectionAnimationDurationProperty =
        BindableProperty.CreateAttached(
            "SelectionAnimationDuration",
            typeof(uint),
            typeof(CollectionViewAssist),
            (uint)140);

    public static uint GetSelectionAnimationDuration(BindableObject element) =>
        (uint)element.GetValue(SelectionAnimationDurationProperty);

    public static void SetSelectionAnimationDuration(BindableObject element, uint value) =>
        element.SetValue(SelectionAnimationDurationProperty, value);

    public static readonly BindableProperty SelectionScaleProperty =
        BindableProperty.CreateAttached(
            "SelectionScale",
            typeof(double),
            typeof(CollectionViewAssist),
            0.96);

    public static double GetSelectionScale(BindableObject element) =>
        (double)element.GetValue(SelectionScaleProperty);

    public static void SetSelectionScale(BindableObject element, double value) =>
        element.SetValue(SelectionScaleProperty, value);

    private static void OnAnimateSelectionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not CollectionView collectionView)
        {
            return;
        }

        collectionView.SelectionChanged -= OnSelectionChanged;

        if ((bool)newValue)
        {
            collectionView.SelectionChanged += OnSelectionChanged;
        }
    }

    private static void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not CollectionView collectionView)
        {
            return;
        }

        foreach (var item in e.CurrentSelection)
        {
            _ = AnimateSelectionAsync(collectionView, item);
        }
    }

    private static async Task AnimateSelectionAsync(CollectionView collectionView, object item)
    {
        var target = FindVisualElement(collectionView, item);
        if (target is null)
        {
            return;
        }

        var scale = GetSelectionScale(collectionView);
        var duration = GetSelectionAnimationDuration(collectionView);

        target.AbortAnimation(SelectionAnimationName);
        var originalScale = target.Scale;

        await target.ScaleTo(scale, duration / 2, Easing.CubicOut);
        await target.ScaleTo(originalScale, duration / 2, Easing.CubicOut);
    }

    private static VisualElement? FindVisualElement(Element root, object item)
    {
        foreach (var element in root.VisualDepthFirstTraversal())
        {
            if (element is VisualElement visualElement &&
                ReferenceEquals(visualElement.BindingContext, item))
            {
                return visualElement;
            }
        }

        return null;
    }
}
