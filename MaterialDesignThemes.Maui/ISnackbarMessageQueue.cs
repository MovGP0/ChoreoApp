namespace MaterialDesignThemes.Maui;

public interface ISnackbarMessageQueue
{
    void Enqueue(object content);

    void Enqueue(object content, object? actionContent, Action? actionHandler);

    void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler, TArgument? actionArgument);

    void Enqueue(object content, bool neverConsiderToBeDuplicate);

    void Enqueue(object content, object? actionContent, Action? actionHandler, bool promote);

    void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler, TArgument? actionArgument, bool promote);

    void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler,
        TArgument? actionArgument, bool promote, bool neverConsiderToBeDuplicate, TimeSpan? durationOverride = null);

    void Enqueue(object content, object? actionContent, Action<object?>? actionHandler, object? actionArgument,
        bool promote, bool neverConsiderToBeDuplicate, TimeSpan? durationOverride = null);
}
