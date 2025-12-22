#if ANDROID
using Android.Views;
using Microsoft.Maui.Platform;
using Application = Android.App.Application;

namespace ChoreoApp.Styling;

public static class CursorExtensions
{
    public static void SetCustomCursor(this VisualElement visualElement, CursorIcon cursor, IMauiContext? mauiContext)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(24))
        {
            return;
        }

        if (mauiContext is null)
        {
            return;
        }

        var view = visualElement.ToPlatform(mauiContext);
        view.PointerIcon = PointerIcon.GetSystemIcon(Application.Context, GetCursor(cursor));
    }

    private static PointerIconType GetCursor(CursorIcon cursor)
    {
        return cursor switch
        {
            CursorIcon.Hand => PointerIconType.Hand,
            CursorIcon.IBeam => PointerIconType.AllScroll,
            CursorIcon.Cross => PointerIconType.Crosshair,
            CursorIcon.Arrow => PointerIconType.Arrow,
            CursorIcon.SizeAll => PointerIconType.TopRightDiagonalDoubleArrow,
            CursorIcon.Wait => PointerIconType.Wait,
            _ => PointerIconType.Default
        };
    }
}
#endif
