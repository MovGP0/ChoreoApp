using Microsoft.Maui.Devices;

namespace MaterialDesignThemes.Maui;

internal sealed class Screen
{
    private static Screen? _primary;

    private Screen()
    {
        var display = DeviceDisplay.MainDisplayInfo;
        var width = display.Width / display.Density;
        var height = display.Height / display.Density;

        Bounds = new Rect(0, 0, width, height);
        WorkingArea = Bounds;
        Primary = true;
        DeviceName = "DISPLAY";
    }

    public static Screen[] AllScreens => [_primary ??= new Screen()];

    public Rect Bounds { get; }

    public string DeviceName { get; }

    public bool Primary { get; }

    public Rect WorkingArea { get; }

    public static Screen PrimaryScreen => _primary ??= new Screen();

    public static Screen FromPoint(Point point) => PrimaryScreen;

    public static Screen FromRect(Rect rect) => PrimaryScreen;

    public static Rect GetWorkingArea(Point pt) => PrimaryScreen.WorkingArea;

    public static Rect GetWorkingArea(Rect rect) => PrimaryScreen.WorkingArea;

    public static Rect GetBounds(Point pt) => PrimaryScreen.Bounds;

    public static Rect GetBounds(Rect rect) => PrimaryScreen.Bounds;

    public override string ToString() =>
        GetType().Name + "[Bounds=" + Bounds + " WorkingArea=" + WorkingArea + " Primary=" + Primary + " DeviceName=" + DeviceName;
}
