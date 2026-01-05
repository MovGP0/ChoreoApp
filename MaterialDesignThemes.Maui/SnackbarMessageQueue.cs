namespace MaterialDesignThemes.Maui;

public sealed class SnackbarMessageQueue : ISnackbarMessageQueue, IDisposable
{
    private readonly TimeSpan _messageDuration;
    private readonly HashSet<Snackbar> _pairedSnackbars = new();
    private readonly LinkedList<SnackbarMessageQueueItem> _snackbarMessages = new();
    private readonly object _snackbarMessagesLock = new();
    private readonly SemaphoreSlim _showMessageSemaphore = new(1, 1);
    private CancellationTokenSource? _closeSnackbarCts;
    private bool _isDisposed;

    public SnackbarMessageQueue()
        : this(TimeSpan.FromSeconds(3))
    {
    }

    public SnackbarMessageQueue(TimeSpan messageDuration)
    {
        _messageDuration = messageDuration;
    }

    public IReadOnlyList<SnackbarMessageQueueItem> QueuedMessages
    {
        get
        {
            lock (_snackbarMessagesLock)
            {
                return _snackbarMessages.ToList();
            }
        }
    }

    public bool DiscardDuplicates { get; set; }

    internal Action Pair(Snackbar snackbar)
    {
        if (snackbar is null)
        {
            throw new ArgumentNullException(nameof(snackbar));
        }

        _pairedSnackbars.Add(snackbar);
        return () => _pairedSnackbars.Remove(snackbar);
    }

    public void Enqueue(object content) => Enqueue(content, false);

    public void Enqueue(object content, bool neverConsiderToBeDuplicate) =>
        Enqueue(content, null, null, null, false, neverConsiderToBeDuplicate);

    public void Enqueue(object content, object? actionContent, Action? actionHandler) =>
        Enqueue(content, actionContent, actionHandler, false);

    public void Enqueue(object content, object? actionContent, Action? actionHandler, bool promote) =>
        Enqueue(content, actionContent, _ => actionHandler?.Invoke(), null, promote, false);

    public void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler,
        TArgument? actionArgument) =>
        Enqueue(content, actionContent, actionHandler, actionArgument, false, false);

    public void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler,
        TArgument? actionArgument, bool promote) =>
        Enqueue(content, actionContent, actionHandler, actionArgument, promote, false);

    public void Enqueue<TArgument>(object content, object? actionContent, Action<TArgument?>? actionHandler,
        TArgument? actionArgument, bool promote, bool neverConsiderToBeDuplicate, TimeSpan? durationOverride = null)
    {
        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (actionContent is null ^ actionHandler is null)
        {
            throw new ArgumentNullException(actionContent != null ? nameof(actionContent) : nameof(actionHandler),
                "All action arguments must be provided if any are provided.");
        }

        Action<object?>? handler = actionHandler != null
            ? argument => actionHandler((TArgument?)argument)
            : null;
        Enqueue(content, actionContent, handler, actionArgument, promote, neverConsiderToBeDuplicate, durationOverride);
    }

    public void Enqueue(object content, object? actionContent, Action<object?>? actionHandler,
        object? actionArgument, bool promote, bool neverConsiderToBeDuplicate, TimeSpan? durationOverride = null)
    {
        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (actionContent is null ^ actionHandler is null)
        {
            throw new ArgumentNullException(actionContent != null ? nameof(actionContent) : nameof(actionHandler),
                "All action arguments must be provided if any are provided.");
        }

        var item = new SnackbarMessageQueueItem(content,
            durationOverride ?? _messageDuration,
            actionContent,
            actionHandler,
            actionArgument,
            promote,
            neverConsiderToBeDuplicate);
        InsertItem(item);
    }

    public void Clear()
    {
        lock (_snackbarMessagesLock)
        {
            _snackbarMessages.Clear();
            _closeSnackbarCts?.Cancel();
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _closeSnackbarCts?.Cancel();
        _closeSnackbarCts?.Dispose();
        _showMessageSemaphore.Dispose();
    }

    private void InsertItem(SnackbarMessageQueueItem item)
    {
        lock (_snackbarMessagesLock)
        {
            var added = false;
            var node = _snackbarMessages.First;
            while (node != null)
            {
                if (DiscardDuplicates && item.IsDuplicate(node.Value))
                {
                    return;
                }

                if (item.IsPromoted && !node.Value.IsPromoted)
                {
                    _snackbarMessages.AddBefore(node, item);
                    added = true;
                    break;
                }

                node = node.Next;
            }

            if (!added)
            {
                _snackbarMessages.AddLast(item);
            }
        }

        MainThread.BeginInvokeOnMainThread(async () => await ShowNextAsync());
    }

    private Snackbar? FindSnackbar()
    {
        return _pairedSnackbars.FirstOrDefault();
    }

    private async Task ShowNextAsync()
    {
        await _showMessageSemaphore.WaitAsync().ConfigureAwait(true);
        try
        {
            if (_isDisposed)
            {
                return;
            }

            Snackbar? snackbar;
            while (true)
            {
                snackbar = FindSnackbar();
                if (snackbar is not null)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);
            }

            LinkedListNode<SnackbarMessageQueueItem>? messageNode;
            CancellationToken closeToken;
            lock (_snackbarMessagesLock)
            {
                messageNode = _snackbarMessages.First;
                if (messageNode is null)
                {
                    return;
                }

                _closeSnackbarCts?.Cancel();
                _closeSnackbarCts?.Dispose();
                _closeSnackbarCts = new CancellationTokenSource();
                closeToken = _closeSnackbarCts.Token;
            }

            await ShowAsync(snackbar, messageNode.Value, closeToken).ConfigureAwait(true);

            lock (_snackbarMessagesLock)
            {
                if (messageNode.List == _snackbarMessages)
                {
                    _snackbarMessages.Remove(messageNode);
                }
            }
        }
        finally
        {
            _showMessageSemaphore.Release();
        }
    }

    private async Task ShowAsync(Snackbar snackbar, SnackbarMessageQueueItem item, CancellationToken closeToken)
    {
        if (_isDisposed)
        {
            return;
        }

        var actionClicked = new TaskCompletionSource();
        var message = new SnackbarMessage
        {
            Content = item.Content,
            ActionContent = item.ActionContent,
            ActionCommandParameter = item.ActionArgument
        };

        if (item.ActionHandler is not null)
        {
            message.ActionCommand = new Command<object?>(argument =>
            {
                item.ActionHandler(argument);
                actionClicked.TrySetResult();
            });
        }

        EventHandler actionClickHandler = (_, _) => actionClicked.TrySetResult();
        message.ActionClick += actionClickHandler;

        await RunOnMainThreadAsync(() =>
        {
            snackbar.Message = message;
            snackbar.IsActive = true;
        }).ConfigureAwait(false);

        var delayTask = Task.Delay(item.Duration, closeToken);
        var closeTask = AsTask(closeToken);
        await Task.WhenAny(delayTask, actionClicked.Task, closeTask).ConfigureAwait(false);

        await RunOnMainThreadAsync(() =>
        {
            snackbar.IsActive = false;
        }).ConfigureAwait(false);

        await Task.Delay(snackbar.DeactivateAnimationDuration, CancellationToken.None).ConfigureAwait(false);

        message.ActionClick -= actionClickHandler;
    }

    private static Task AsTask(CancellationToken token)
    {
        var tcs = new TaskCompletionSource();
        if (token.IsCancellationRequested)
        {
            tcs.SetResult();
            return tcs.Task;
        }

        token.Register(() => tcs.TrySetResult());
        return tcs.Task;
    }

    private static Task RunOnMainThreadAsync(Action action)
    {
        var tcs = new TaskCompletionSource();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                action();
                tcs.TrySetResult();
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        });
        return tcs.Task;
    }
}
