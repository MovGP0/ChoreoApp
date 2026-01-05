namespace MaterialDesignThemes.Maui;

public static class TreeViewAssist
{
    public static readonly BindableProperty AdditionalTemplateProperty = BindableProperty.CreateAttached(
        "AdditionalTemplate",
        typeof(DataTemplate),
        typeof(TreeViewAssist),
        null);

    public static void SetAdditionalTemplate(BindableObject element, DataTemplate? value)
    {
        element.SetValue(AdditionalTemplateProperty, value);
    }

    public static DataTemplate? GetAdditionalTemplate(BindableObject element)
    {
        return (DataTemplate?)element.GetValue(AdditionalTemplateProperty);
    }

    public static readonly BindableProperty AdditionalTemplateSelectorProperty = BindableProperty.CreateAttached(
        "AdditionalTemplateSelector",
        typeof(DataTemplateSelector),
        typeof(TreeViewAssist),
        null);

    public static void SetAdditionalTemplateSelector(BindableObject element, DataTemplateSelector? value)
    {
        element.SetValue(AdditionalTemplateSelectorProperty, value);
    }

    public static DataTemplateSelector? GetAdditionalTemplateSelector(BindableObject element)
    {
        return (DataTemplateSelector?)element.GetValue(AdditionalTemplateSelectorProperty);
    }

    private static readonly Lazy<DataTemplate> NoAdditionalTemplateProvider = new(CreateEmptyGridDataTemplate);

    public static DataTemplate SuppressAdditionalTemplate => NoAdditionalTemplateProvider.Value;

    public static DataTemplate CreateEmptyGridDataTemplate()
    {
        return new DataTemplate(() => new Grid());
    }

    public static double GetExpanderSize(BindableObject element)
        => (double)element.GetValue(ExpanderSizeProperty);

    public static void SetExpanderSize(BindableObject element, double value)
        => element.SetValue(ExpanderSizeProperty, value);

    public static readonly BindableProperty ExpanderSizeProperty = BindableProperty.CreateAttached(
        "ExpanderSize",
        typeof(double),
        typeof(TreeViewAssist),
        0d);

    public static bool GetShowSelection(BindableObject element)
        => (bool)element.GetValue(ShowSelectionProperty);

    public static void SetShowSelection(BindableObject element, bool value)
        => element.SetValue(ShowSelectionProperty, value);

    public static readonly BindableProperty ShowSelectionProperty = BindableProperty.CreateAttached(
        "ShowSelection",
        typeof(bool),
        typeof(TreeViewAssist),
        true);

    public static TreeViewExpanderVisibility GetHasNoItemsExpanderVisibility(BindableObject element)
        => (TreeViewExpanderVisibility)element.GetValue(HasNoItemsExpanderVisibilityProperty);

    public static void SetHasNoItemsExpanderVisibility(BindableObject element, TreeViewExpanderVisibility value)
        => element.SetValue(HasNoItemsExpanderVisibilityProperty, value);

    public static readonly BindableProperty HasNoItemsExpanderVisibilityProperty = BindableProperty.CreateAttached(
        "HasNoItemsExpanderVisibility",
        typeof(TreeViewExpanderVisibility),
        typeof(TreeViewAssist),
        TreeViewExpanderVisibility.Hidden);
}

public enum TreeViewExpanderVisibility
{
    Visible,
    Hidden,
    Collapsed
}
