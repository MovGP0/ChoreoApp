namespace MaterialDesignThemes.Maui;

/// <summary>
/// Minimal abstraction used by SmartHint to observe target input controls.
/// </summary>
public interface IHintProxy : IDisposable
{
    event EventHandler? IsVisibleChanged;
    event EventHandler? ContentChanged;
    event EventHandler? Loaded;
    event EventHandler? FocusedChanged;

    bool IsVisible { get; }
    bool IsFocused { get; }
    bool IsLoaded { get; }

    /// <summary>Should return true when the target has no content.</summary>
    bool IsEmpty();
}
