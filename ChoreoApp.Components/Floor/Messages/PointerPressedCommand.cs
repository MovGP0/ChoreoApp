using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerPressedCommand(ISKCanvasView CanvasView, PointerEventArgs EventArgs);
