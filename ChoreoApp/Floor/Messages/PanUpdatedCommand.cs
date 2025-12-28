using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Messages;

public sealed record PanUpdatedCommand(SKCanvasView CanvasView, PanUpdatedEventArgs EventArgs);
