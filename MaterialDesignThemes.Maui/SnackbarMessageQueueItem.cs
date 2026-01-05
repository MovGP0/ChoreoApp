namespace MaterialDesignThemes.Maui;

public sealed class SnackbarMessageQueueItem
{
    public SnackbarMessageQueueItem(object content,
        TimeSpan duration,
        object? actionContent = null,
        Action<object?>? actionHandler = null,
        object? actionArgument = null,
        bool isPromoted = false,
        bool alwaysShow = false)
    {
        Content = content;
        Duration = duration;
        ActionContent = actionContent;
        ActionHandler = actionHandler;
        ActionArgument = actionArgument;
        IsPromoted = isPromoted;
        AlwaysShow = alwaysShow;
    }

    public object Content { get; }

    public TimeSpan Duration { get; }

    public object? ActionContent { get; }

    public Action<object?>? ActionHandler { get; }

    public object? ActionArgument { get; }

    public bool IsPromoted { get; }

    public bool AlwaysShow { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not SnackbarMessageQueueItem message)
        {
            return false;
        }

        return EqualityComparer<object>.Default.Equals(Content, message.Content)
               && EqualityComparer<object?>.Default.Equals(ActionContent, message.ActionContent);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = Content.GetHashCode();
            result = (result * 397) ^ (ActionContent?.GetHashCode() ?? 0);
            return result;
        }
    }

    public bool IsDuplicate(SnackbarMessageQueueItem value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (AlwaysShow)
        {
            return false;
        }

        return Equals(value);
    }
}
