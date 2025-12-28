using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record PinchUpdatedCommand(SKCanvasView CanvasView, PinchGestureUpdatedEventArgs EventArgs);