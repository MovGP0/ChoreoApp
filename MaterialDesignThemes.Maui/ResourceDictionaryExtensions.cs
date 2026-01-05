namespace MaterialDesignThemes.Maui;

public static partial class ResourceDictionaryExtensions
{
    extension (ResourceDictionary dict)
    {
        public void SetColor(string key, Color value)
        {
            ArgumentNullException.ThrowIfNull(key);
            if (!dict.TryGetColor(key, out var existing) || existing != value)
            {
                dict[key] = value;
            }
        }

        public bool TryGetColor(string key, out Color color)
        {
            if (dict.ContainsKey(key) && dict[key] is Color found)
            {
                color = found;
                return true;
            }

            color = null;
            return false;
        }
    }
}
