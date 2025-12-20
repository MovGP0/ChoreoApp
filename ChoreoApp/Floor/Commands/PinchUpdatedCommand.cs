using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record PinchUpdatedCommand(SKCanvasView CanvasView, PinchGestureUpdatedEventArgs EventArgs);