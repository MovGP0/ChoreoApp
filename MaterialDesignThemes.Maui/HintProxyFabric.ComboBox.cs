namespace MaterialDesignThemes.Maui;

public static partial class HintProxyFabric
{
    private sealed class PickerHintProxy : IHintProxy
    {
        private readonly Picker _picker;

        public PickerHintProxy(Picker picker)
        {
            _picker = picker ?? throw new ArgumentNullException(nameof(picker));
            _picker.SelectedIndexChanged += PickerSelectedIndexChanged;
            _picker.Loaded += PickerLoaded;
            _picker.PropertyChanged += PickerPropertyChanged;
            _picker.Focused += PickerFocused;
            _picker.Unfocused += PickerFocused;
        }

        public bool IsLoaded => _picker.IsLoaded;

        public bool IsVisible => _picker.IsVisible;

        public bool IsEmpty => _picker is { SelectedIndex: < 0, SelectedItem: null };

        public bool IsFocused => _picker.IsFocused;

        public event EventHandler? ContentChanged;
        public event EventHandler? IsVisibleChanged;
        public event EventHandler? Loaded;
        public event EventHandler? FocusedChanged;

        private void PickerSelectedIndexChanged(object? sender, EventArgs e) =>
            ContentChanged?.Invoke(sender, EventArgs.Empty);

        private void PickerPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VisualElement.IsVisible))
            {
                IsVisibleChanged?.Invoke(sender, EventArgs.Empty);
            }
        }

        private void PickerLoaded(object? sender, EventArgs e) =>
            Loaded?.Invoke(sender, EventArgs.Empty);

        private void PickerFocused(object? sender, FocusEventArgs e) =>
            FocusedChanged?.Invoke(sender, EventArgs.Empty);

        public void Dispose()
        {
            _picker.SelectedIndexChanged -= PickerSelectedIndexChanged;
            _picker.Loaded -= PickerLoaded;
            _picker.PropertyChanged -= PickerPropertyChanged;
            _picker.Focused -= PickerFocused;
            _picker.Unfocused -= PickerFocused;
        }
    }
}
