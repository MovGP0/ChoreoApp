using System.Globalization;

namespace MaterialDesignThemes.Maui;

internal static class StringExtensions
{
    public static string ToTitleCase(this string text, CultureInfo culture, string separator = " ")
    {
        TextInfo textInfo = culture.TextInfo;
        string lowerText = textInfo.ToLower(text);
        string[] words = lowerText.Split([separator], StringSplitOptions.None);

        return string.Join(separator, words.Select(v => textInfo.ToTitleCase(v)));
    }
}
