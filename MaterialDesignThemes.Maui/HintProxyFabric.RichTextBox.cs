namespace MaterialDesignThemes.Maui;

public static partial class HintProxyFabric
{
    private sealed class EditorHintProxy : IHintProxy
    {
        private readonly Editor _editor;

        public EditorHintProxy(Editor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _editor.TextChanged += EditorTextChanged;
            _editor.Loaded += EditorLoaded;
            _editor.PropertyChanged += EditorPropertyChanged;
            _editor.Focused += EditorFocused;
            _editor.Unfocused += EditorFocused;
        }

        public bool IsLoaded => _editor.IsLoaded;

        public bool IsVisible => _editor.IsVisible;

        public bool IsEmpty => string.IsNullOrEmpty(_editor.Text);

        public bool IsFocused => _editor.IsFocused;

        public event EventHandler? ContentChanged;
        public event EventHandler? IsVisibleChanged;
        public event EventHandler? Loaded;
        public event EventHandler? FocusedChanged;

        private void EditorTextChanged(object? sender, TextChangedEventArgs e) =>
            ContentChanged?.Invoke(sender, EventArgs.Empty);

        private void EditorPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VisualElement.IsVisible))
            {
                IsVisibleChanged?.Invoke(sender, EventArgs.Empty);
            }
        }

        private void EditorLoaded(object? sender, EventArgs e) =>
            Loaded?.Invoke(sender, EventArgs.Empty);

        private void EditorFocused(object? sender, FocusEventArgs e) =>
            FocusedChanged?.Invoke(sender, EventArgs.Empty);

        public void Dispose()
        {
            _editor.TextChanged -= EditorTextChanged;
            _editor.Loaded -= EditorLoaded;
            _editor.PropertyChanged -= EditorPropertyChanged;
            _editor.Focused -= EditorFocused;
            _editor.Unfocused -= EditorFocused;
        }
    }
}
