namespace MaterialDesignThemes.Maui;

public static class CustomPopupPlacementCallbackHelper
{
    public static readonly Func<Size, Size, Point, Point[]> LargePopupCallback =
        (size, targetSize, offset) => [new Point()];
}
