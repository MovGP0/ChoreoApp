namespace MaterialDesignThemes.Maui;

public static class ResourceDictionaryExtensions
{
    extension (ResourceDictionary dict)
    {
        public void SetColor(string key, Color value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

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

            color = default;
            return false;
        }
    }
}
