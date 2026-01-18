using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerMovedCommand(ISKCanvasView CanvasView, PointerEventArgs EventArgs);
