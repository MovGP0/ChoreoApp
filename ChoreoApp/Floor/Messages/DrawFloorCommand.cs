using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Messages;

public sealed record DrawFloorCommand(
    SKPaintSurfaceEventArgs SurfaceEventArgs);
