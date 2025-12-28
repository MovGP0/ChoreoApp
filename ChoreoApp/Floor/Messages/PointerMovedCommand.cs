using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerMovedCommand(SKCanvasView CanvasView, PointerEventArgs EventArgs);