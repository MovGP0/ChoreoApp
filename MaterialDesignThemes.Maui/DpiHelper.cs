using Microsoft.Maui.Devices;

namespace MaterialDesignThemes.Maui;

internal static class DpiHelper
{
    private static double Density => DeviceDisplay.MainDisplayInfo.Density;

    public static double TransformToDeviceY(VisualElement visual, double y) =>
        TransformToDeviceY(y);

    public static double TransformFromDeviceY(VisualElement visual, double y) =>
        TransformFromDeviceY(y);

    public static double TransformToDeviceX(VisualElement visual, double x) =>
        TransformToDeviceX(x);

    public static double TransformFromDeviceX(VisualElement visual, double x) =>
        TransformFromDeviceX(x);

    public static double TransformToDeviceY(double y) =>
        y * Density;

    public static double TransformFromDeviceY(double y) =>
        y / Density;

    public static double TransformToDeviceX(double x) =>
        x * Density;

    public static double TransformFromDeviceX(double x) =>
        x / Density;
}
