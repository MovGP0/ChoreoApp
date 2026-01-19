using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record TouchCommand(ISKCanvasView CanvasView, SKTouchEventArgs EventArgs);
