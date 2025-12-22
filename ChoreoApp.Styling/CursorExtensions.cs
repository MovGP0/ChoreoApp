#if !(ANDROID || IOS || MACCATALYST || WINDOWS)
namespace ChoreoApp.Styling;

public static class CursorExtensions
{
    public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? mauiContext)
    {
    }
}
#endif
