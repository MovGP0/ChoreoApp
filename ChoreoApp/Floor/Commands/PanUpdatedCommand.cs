using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Floor.Commands;

public sealed record PanUpdatedCommand(SKCanvasView CanvasView, PanUpdatedEventArgs EventArgs);
