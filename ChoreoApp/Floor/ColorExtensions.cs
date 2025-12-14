using SkiaSharp;

namespace ChoreoApp.Floor;

public static class ColorExtensions
{
    extension (Color color)
    {
        public SKColor ToSKColor()
        {
            return new SKColor(
                (byte)(color.Red * 255),
                (byte)(color.Green * 255),
                (byte)(color.Blue * 255),
                (byte)(color.Alpha * 255));
        }
    }
}
