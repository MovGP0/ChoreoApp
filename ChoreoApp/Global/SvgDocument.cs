using SkiaSharp;

namespace ChoreoApp.Global;

public sealed class SvgDocument : IDisposable
{
    public SvgDocument(SKPicture picture, SKRect bounds)
    {
        Picture = picture;
        Bounds = bounds;
    }

    public SKPicture Picture { get; }
    public SKRect Bounds { get; }

    public void Dispose()
    {
        Picture.Dispose();
    }
}
