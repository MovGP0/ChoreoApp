using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record PointerPressedCommand(SKCanvasView CanvasView, PointerEventArgs EventArgs);