using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerWheelChangedCommand(ISKCanvasView CanvasView, double Delta, Point? Position);
