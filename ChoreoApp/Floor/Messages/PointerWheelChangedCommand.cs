using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerWheelChangedCommand(SKCanvasView CanvasView, double Delta, Point? Position);