using SkiaSharp;

namespace ChoreoApp.Global;

public sealed class SvgDocument(SKPicture picture, SKRect bounds) : IDisposable
{
    public SKPicture Picture { get; } = picture;
    public SKRect Bounds { get; } = bounds;

    public void Dispose() => Picture.Dispose();
}
