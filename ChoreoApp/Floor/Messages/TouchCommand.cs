using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record TouchCommand(SKCanvasView CanvasView, SKTouchEventArgs EventArgs);