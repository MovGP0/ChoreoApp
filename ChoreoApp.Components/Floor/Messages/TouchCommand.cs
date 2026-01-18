using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor.Messages;

public sealed record TouchCommand(ISKCanvasView CanvasView, SKTouchEventArgs EventArgs);
