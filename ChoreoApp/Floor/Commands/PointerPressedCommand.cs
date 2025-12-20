using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record PointerPressedCommand(SKCanvasView CanvasView, PointerEventArgs EventArgs);