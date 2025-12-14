using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor;

public sealed record DrawFloorCommand(
    SKPaintSurfaceEventArgs SurfaceEventArgs);
