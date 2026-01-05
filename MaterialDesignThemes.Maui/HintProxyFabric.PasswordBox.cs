namespace MaterialDesignThemes.Maui;

public static partial class HintProxyFabric
{
    private sealed class PasswordEntryHintProxy : IHintProxy
    {
        private readonly Entry _entry;

        public PasswordEntryHintProxy(Entry entry)
        {
            _entry = entry ?? throw new ArgumentNullException(nameof(entry));
            _entry.TextChanged += EntryTextChanged;
            _entry.Loaded += EntryLoaded;
            _entry.PropertyChanged += EntryPropertyChanged;
            _entry.Focused += EntryFocused;
            _entry.Unfocused += EntryFocused;
        }

        public bool IsLoaded => _entry.IsLoaded;

        public bool IsVisible => _entry.IsVisible;

        public bool IsEmpty => string.IsNullOrEmpty(_entry.Text);

        public bool IsFocused => _entry.IsFocused;

        public event EventHandler? ContentChanged;
        public event EventHandler? IsVisibleChanged;
        public event EventHandler? Loaded;
        public event EventHandler? FocusedChanged;

        private void EntryTextChanged(object? sender, TextChangedEventArgs e) =>
            ContentChanged?.Invoke(sender, EventArgs.Empty);

        private void EntryPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VisualElement.IsVisible))
            {
                IsVisibleChanged?.Invoke(sender, EventArgs.Empty);
            }
        }

        private void EntryLoaded(object? sender, EventArgs e) =>
            Loaded?.Invoke(sender, EventArgs.Empty);

        private void EntryFocused(object? sender, FocusEventArgs e) =>
            FocusedChanged?.Invoke(sender, EventArgs.Empty);

        public void Dispose()
        {
            _entry.TextChanged -= EntryTextChanged;
            _entry.Loaded -= EntryLoaded;
            _entry.PropertyChanged -= EntryPropertyChanged;
            _entry.Focused -= EntryFocused;
            _entry.Unfocused -= EntryFocused;
        }
    }
}
