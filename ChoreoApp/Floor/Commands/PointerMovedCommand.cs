using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record PointerMovedCommand(SKCanvasView CanvasView, PointerEventArgs EventArgs);