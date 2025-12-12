using Microsoft.Maui.Graphics;

namespace MaterialColorUtilities;

public static class ColorExtensions
{
    extension (Color)
    {
        /// <summary>
        /// Converts a color in 0xAARRGGBB format to a <see cref="Color"/>.
        /// </summary>
        public static Color FromArgb(int argb)
        {
            byte a = (byte)((uint)argb >> 24);
            byte r = (byte)((uint)argb >> 16);
            byte g = (byte)((uint)argb >> 8);
            byte b = (byte)((uint)argb >> 0);

            // MAUI stores channels as floats in [0,1]
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// Converts a <see cref="Color"/> to 0xAARRGGBB format.
        /// </summary>
        public static int ArgbFromColor(Color color)
        {
            // Clamp in case values are slightly out of range due to math operations.
            byte a = ToByte(color.Alpha);
            byte r = ToByte(color.Red);
            byte g = ToByte(color.Green);
            byte b = ToByte(color.Blue);

            return (a << 24) | (r << 16) | (g << 8) | b;
        }

        private static byte ToByte(float channel01)
        {
            if (channel01 <= 0f) return 0;
            if (channel01 >= 1f) return 255;
            return (byte)MathF.Round(channel01 * 255f);
        }
    }
}
