using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record TouchCommand(SKCanvasView CanvasView, SKTouchEventArgs EventArgs);