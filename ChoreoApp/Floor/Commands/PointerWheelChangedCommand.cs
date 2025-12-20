using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record PointerWheelChangedCommand(SKCanvasView CanvasView, double Delta, Point? Position);