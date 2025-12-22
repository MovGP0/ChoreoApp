namespace ChoreoApp.Styling;

public static class CursorBehavior
{
    public static readonly BindableProperty CursorProperty = BindableProperty.CreateAttached(
        "Cursor",
        typeof(CursorIcon),
        typeof(CursorBehavior),
        CursorIcon.Arrow,
        propertyChanged: CursorChanged);

    public static CursorIcon GetCursor(BindableObject view) =>
        (CursorIcon)view.GetValue(CursorProperty);

    public static void SetCursor(BindableObject view, CursorIcon value) =>
        view.SetValue(CursorProperty, value);

    private static void CursorChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is VisualElement visualElement && newvalue is CursorIcon cursor)
        {
            visualElement.SetCustomCursor(
                cursor,
                visualElement.Handler?.MauiContext
                    ?? Application.Current?.Windows.LastOrDefault()?.Page?.Handler?.MauiContext);
        }
    }
}
