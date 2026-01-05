namespace MaterialDesignThemes.Maui;

internal static class DoubleExtensions
{
    extension(double value)
    {
        internal double Clamp(double min, double max)
            => Math.Clamp(value, min, max);

        internal double Clamp01()
            => Math.Clamp(value, 0, 1);
    }
}
