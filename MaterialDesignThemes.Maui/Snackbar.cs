namespace MaterialDesignThemes.Maui;

public enum SnackbarActionButtonPlacementMode
{
    Auto,
    Inline,
    SeparateLine
}

[ContentProperty(nameof(Message))]
public class Snackbar : TemplatedView
{
    public const string MessageHostPartName = "PART_MessageHost";

    private ContentView? _messageHost;
    private Action? _messageQueueRegistrationCleanUp;
    private CancellationTokenSource? _deactivateCts;

    public event EventHandler<ValueChangedEventArgs<bool>>? IsActiveChanged;

    public event EventHandler<SnackbarMessageEventArgs>? DeactivateStoryboardCompleted;

    public TimeSpan ActivateAnimationDuration { get; private set; } = TimeSpan.FromMilliseconds(300);

    public TimeSpan DeactivateAnimationDuration { get; private set; } = TimeSpan.FromMilliseconds(300);

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(SnackbarMessage),
        typeof(Snackbar),
        propertyChanged: OnMessageChanged);

    public SnackbarMessage? Message
    {
        get => (SnackbarMessage?)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty MessageQueueProperty = BindableProperty.Create(
        nameof(MessageQueue),
        typeof(SnackbarMessageQueue),
        typeof(Snackbar),
        propertyChanged: OnMessageQueueChanged);

    public SnackbarMessageQueue? MessageQueue
    {
        get => (SnackbarMessageQueue?)GetValue(MessageQueueProperty);
        set => SetValue(MessageQueueProperty, value);
    }

    public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(
        nameof(IsActive),
        typeof(bool),
        typeof(Snackbar),
        false,
        propertyChanged: OnIsActiveChanged);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly BindableProperty ActionButtonStyleProperty = BindableProperty.Create(
        nameof(ActionButtonStyle),
        typeof(Style),
        typeof(Snackbar));

    public Style? ActionButtonStyle
    {
        get => (Style?)GetValue(ActionButtonStyleProperty);
        set => SetValue(ActionButtonStyleProperty, value);
    }

    public static readonly BindableProperty ActionButtonPlacementProperty = BindableProperty.Create(
        nameof(ActionButtonPlacement),
        typeof(SnackbarActionButtonPlacementMode),
        typeof(Snackbar),
        SnackbarActionButtonPlacementMode.Auto);

    public SnackbarActionButtonPlacementMode ActionButtonPlacement
    {
        get => (SnackbarActionButtonPlacementMode)GetValue(ActionButtonPlacementProperty);
        set => SetValue(ActionButtonPlacementProperty, value);
    }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(Snackbar),
        null);

    public Color? TextColor
    {
        get => (Color?)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Snackbar),
        new CornerRadius(3));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _messageHost = GetTemplateChild(MessageHostPartName) as ContentView;
        UpdateMessageHost();
    }

    private static void OnMessageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Snackbar snackbar)
        {
            snackbar.UpdateMessageHost();
        }
    }

    private static void OnMessageQueueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Snackbar snackbar)
        {
            return;
        }

        snackbar._messageQueueRegistrationCleanUp?.Invoke();
        snackbar._messageQueueRegistrationCleanUp = (newValue as SnackbarMessageQueue)?.Pair(snackbar);
    }

    private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Snackbar snackbar)
        {
            return;
        }

        var args = new ValueChangedEventArgs<bool>((bool)oldValue, (bool)newValue);
        snackbar.IsActiveChanged?.Invoke(snackbar, args);

        if ((bool)newValue)
        {
            snackbar.CancelDeactivate();
            return;
        }

        snackbar.ScheduleDeactivateCompleted();
    }

    private void UpdateMessageHost()
    {
        if (_messageHost is null)
        {
            return;
        }

        _messageHost.Content = Message;
    }

    private void ScheduleDeactivateCompleted()
    {
        CancelDeactivate();

        if (Message is null)
        {
            return;
        }

        _deactivateCts = new CancellationTokenSource();
        var localCts = _deactivateCts;
        var token = localCts.Token;
        var message = Message;

        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(DeactivateAnimationDuration, token).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            if (token.IsCancellationRequested)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeactivateStoryboardCompleted?.Invoke(this, new SnackbarMessageEventArgs(message));
                localCts.Dispose();
                if (ReferenceEquals(_deactivateCts, localCts))
                {
                    _deactivateCts = null;
                }
            });
        });
    }

    private void CancelDeactivate()
    {
        if (_deactivateCts is null)
        {
            return;
        }

        _deactivateCts.Cancel();
        _deactivateCts.Dispose();
        _deactivateCts = null;
    }
}
