#if !(ANDROID || IOS || MACCATALYST || WINDOWS)
namespace MaterialDesignThemes.Maui;

public static class CursorExtensions
{
    public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? mauiContext)
    {
    }
}
#endif
