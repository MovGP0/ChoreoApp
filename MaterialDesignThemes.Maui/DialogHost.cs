using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

/// <summary>
/// Hosts modal dialogs with an overlay and optional background blur.
/// </summary>
[ContentProperty(nameof(MainContent))]
public sealed class DialogHost : ContentView
{
    private static readonly HashSet<WeakReference<DialogHost>> LoadedInstances = [];
    private const double OverlayOpenOpacity = 0.56;
    private const uint DialogOpenDuration = 220;
    private const uint DialogCloseDuration = 180;
    private const string DialogAnimationName = "MaterialDesignDialogHostAnimation";

    private readonly Grid _root;
    private readonly ContentView _mainPresenter;
    private readonly Grid _overlay;
    private readonly Grid _dialogContainer;
    private readonly Border _dialogBorder;
    private readonly ContentPresenter _dialogPresenter;
    private readonly Command<object?> _openDialogCommand;
    private readonly Command<object?> _closeDialogCommand;

    private TaskCompletionSource<object?>? _dialogTaskCompletionSource;
    private DialogOpenedEventHandler? _asyncShowOpenedEventHandler;
    private DialogClosingEventHandler? _asyncShowClosingEventHandler;
    private DialogClosedEventHandler? _asyncShowClosedEventHandler;
    private bool _isClosingInternally;
    private bool _isDialogContentBindingContextInherited;
    private int _dialogAnimationToken;

    public DialogHost()
    {
        _mainPresenter = new ContentView();

        _overlay = new Grid
        {
            IsVisible = false,
            InputTransparent = false,
            Background = new SolidColorBrush(Color.FromArgb("#66000000"))
        };

        _overlay.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(OnOverlayTapped)
        });

        _dialogPresenter = new ContentPresenter();

        _dialogBorder = new Border
        {
            Background = new SolidColorBrush(Colors.White),
            StrokeThickness = 0,
            Padding = new Thickness(24),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) }
        };

        _dialogBorder.SetBinding(Border.BackgroundProperty, new Binding(nameof(DialogBackground), source: this));
        _dialogBorder.SetBinding(Border.PaddingProperty, new Binding(nameof(DialogPadding), source: this));
        _dialogBorder.Content = _dialogPresenter;

        _dialogContainer = new Grid
        {
            IsVisible = false,
            InputTransparent = false,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        _dialogContainer.SetBinding(MarginProperty, new Binding(nameof(DialogMargin), source: this));
        _dialogContainer.Children.Add(_dialogBorder);

        _root = new Grid
        {
            RowDefinitions = { new RowDefinition(GridLength.Star) }
        };

        _root.Children.Add(_mainPresenter);
        _root.Children.Add(_overlay);
        _root.Children.Add(_dialogContainer);

        _mainPresenter.ZIndex = 0;
        _overlay.ZIndex = 1;
        _dialogContainer.ZIndex = 2;

        Content = _root;

        _openDialogCommand = new Command<object?>(ExecuteOpenDialog, CanExecuteOpenDialog);
        _closeDialogCommand = new Command<object?>(ExecuteCloseDialog, CanExecuteCloseDialog);

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public static readonly BindableProperty IdentifierProperty = BindableProperty.Create(
        nameof(Identifier),
        typeof(object),
        typeof(DialogHost));

    public object? Identifier
    {
        get => GetValue(IdentifierProperty);
        set => SetValue(IdentifierProperty, value);
    }

    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
        nameof(IsOpen),
        typeof(bool),
        typeof(DialogHost),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsOpenChanged);

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly BindableProperty MainContentProperty = BindableProperty.Create(
        nameof(MainContent),
        typeof(View),
        typeof(DialogHost),
        propertyChanged: OnMainContentChanged);

    public View? MainContent
    {
        get => (View?)GetValue(MainContentProperty);
        set => SetValue(MainContentProperty, value);
    }

    public static readonly BindableProperty DialogContentProperty = BindableProperty.Create(
        nameof(DialogContent),
        typeof(object),
        typeof(DialogHost),
        propertyChanged: OnDialogContentChanged);

    public object? DialogContent
    {
        get => GetValue(DialogContentProperty);
        set => SetValue(DialogContentProperty, value);
    }

    public static readonly BindableProperty DialogContentTemplateProperty = BindableProperty.Create(
        nameof(DialogContentTemplate),
        typeof(DataTemplate),
        typeof(DialogHost),
        propertyChanged: OnDialogContentTemplateChanged);

    public DataTemplate? DialogContentTemplate
    {
        get => (DataTemplate?)GetValue(DialogContentTemplateProperty);
        set => SetValue(DialogContentTemplateProperty, value);
    }

    public static readonly BindableProperty DialogContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(DialogContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(DialogHost),
        propertyChanged: OnDialogContentTemplateChanged);

    public DataTemplateSelector? DialogContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(DialogContentTemplateSelectorProperty);
        set => SetValue(DialogContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty OverlayBackgroundProperty = BindableProperty.Create(
        nameof(OverlayBackground),
        typeof(Brush),
        typeof(DialogHost),
        new SolidColorBrush(Color.FromArgb("#66000000")),
        propertyChanged: OnOverlayBackgroundChanged);

    public Brush OverlayBackground
    {
        get => (Brush)GetValue(OverlayBackgroundProperty);
        set => SetValue(OverlayBackgroundProperty, value);
    }

    public static readonly BindableProperty DialogBackgroundProperty = BindableProperty.Create(
        nameof(DialogBackground),
        typeof(Brush),
        typeof(DialogHost),
        new SolidColorBrush(Colors.White),
        propertyChanged: OnDialogBackgroundChanged);

    public Brush DialogBackground
    {
        get => (Brush)GetValue(DialogBackgroundProperty);
        set => SetValue(DialogBackgroundProperty, value);
    }

    public static readonly BindableProperty DialogMarginProperty = BindableProperty.Create(
        nameof(DialogMargin),
        typeof(Thickness),
        typeof(DialogHost),
        new Thickness(24));

    public Thickness DialogMargin
    {
        get => (Thickness)GetValue(DialogMarginProperty);
        set => SetValue(DialogMarginProperty, value);
    }

    public static readonly BindableProperty DialogPaddingProperty = BindableProperty.Create(
        nameof(DialogPadding),
        typeof(Thickness),
        typeof(DialogHost),
        new Thickness(24));

    public Thickness DialogPadding
    {
        get => (Thickness)GetValue(DialogPaddingProperty);
        set => SetValue(DialogPaddingProperty, value);
    }

    public static readonly BindableProperty DialogCornerRadiusProperty = BindableProperty.Create(
        nameof(DialogCornerRadius),
        typeof(CornerRadius),
        typeof(DialogHost),
        new CornerRadius(12),
        propertyChanged: OnDialogCornerRadiusChanged);

    public CornerRadius DialogCornerRadius
    {
        get => (CornerRadius)GetValue(DialogCornerRadiusProperty);
        set => SetValue(DialogCornerRadiusProperty, value);
    }

    public static readonly BindableProperty CloseOnClickAwayProperty = BindableProperty.Create(
        nameof(CloseOnClickAway),
        typeof(bool),
        typeof(DialogHost),
        false);

    public bool CloseOnClickAway
    {
        get => (bool)GetValue(CloseOnClickAwayProperty);
        set => SetValue(CloseOnClickAwayProperty, value);
    }

    public static readonly BindableProperty CloseOnClickAwayParameterProperty = BindableProperty.Create(
        nameof(CloseOnClickAwayParameter),
        typeof(object),
        typeof(DialogHost));

    public object? CloseOnClickAwayParameter
    {
        get => GetValue(CloseOnClickAwayParameterProperty);
        set => SetValue(CloseOnClickAwayParameterProperty, value);
    }

    public static readonly BindableProperty ApplyBlurBackgroundProperty = BindableProperty.Create(
        nameof(ApplyBlurBackground),
        typeof(bool),
        typeof(DialogHost),
        false);

    public bool ApplyBlurBackground
    {
        get => (bool)GetValue(ApplyBlurBackgroundProperty);
        set => SetValue(ApplyBlurBackgroundProperty, value);
    }

    public static readonly BindableProperty BlurRadiusProperty = BindableProperty.Create(
        nameof(BlurRadius),
        typeof(double),
        typeof(DialogHost),
        16d);

    public double BlurRadius
    {
        get => (double)GetValue(BlurRadiusProperty);
        set => SetValue(BlurRadiusProperty, value);
    }

    public ICommand OpenDialogCommand => _openDialogCommand;

    public ICommand CloseDialogCommand => _closeDialogCommand;

    public DialogSession? CurrentSession { get; private set; }

    public event DialogOpenedEventHandler? DialogOpened;

    public event DialogClosingEventHandler? DialogClosing;

    public event DialogClosedEventHandler? DialogClosed;

    public static Task<object?> Show(object content)
        => Show(content, null, null, null, null);

    public static Task<object?> Show(
        object content,
        DialogOpenedEventHandler openedEventHandler)
        => Show(content, null, openedEventHandler, null, null);

    public static Task<object?> Show(
        object content,
        DialogClosingEventHandler closingEventHandler)
        => Show(content, null, null, closingEventHandler, null);

    public static Task<object?> Show(
        object content,
        DialogOpenedEventHandler? openedEventHandler,
        DialogClosingEventHandler? closingEventHandler)
        => Show(content, null, openedEventHandler, closingEventHandler, null);

    public static Task<object?> Show(
        object content,
        DialogOpenedEventHandler? openedEventHandler,
        DialogClosingEventHandler? closingEventHandler,
        DialogClosedEventHandler? closedEventHandler)
        => Show(content, null, openedEventHandler, closingEventHandler, closedEventHandler);

    public static Task<object?> Show(
        object content,
        object? dialogIdentifier)
        => Show(content, dialogIdentifier, null, null, null);

    public static Task<object?> Show(
        object content,
        object? dialogIdentifier,
        DialogOpenedEventHandler openedEventHandler)
        => Show(content, dialogIdentifier, openedEventHandler, null, null);

    public static Task<object?> Show(
        object content,
        object? dialogIdentifier,
        DialogClosingEventHandler closingEventHandler)
        => Show(content, dialogIdentifier, null, closingEventHandler, null);

    public static Task<object?> Show(
        object content,
        object? dialogIdentifier,
        DialogOpenedEventHandler? openedEventHandler,
        DialogClosingEventHandler? closingEventHandler)
        => Show(content, dialogIdentifier, openedEventHandler, closingEventHandler, null);

    public static async Task<object?> Show(
        object content,
        object? dialogIdentifier,
        DialogOpenedEventHandler? openedEventHandler,
        DialogClosingEventHandler? closingEventHandler,
        DialogClosedEventHandler? closedEventHandler)
    {
        ArgumentNullException.ThrowIfNull(content);
        var dialogHost = GetInstance(dialogIdentifier);
        return await dialogHost.ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);
    }

    public static void Close(object? dialogIdentifier)
        => Close(dialogIdentifier, null);

    public static void Close(object? dialogIdentifier, object? parameter)
    {
        var dialogHost = GetInstance(dialogIdentifier);
        if (dialogHost.CurrentSession is { } currentSession)
        {
            currentSession.Close(parameter);
            return;
        }

        throw new InvalidOperationException("DialogHost is not open.");
    }

    public static DialogSession? GetDialogSession(object? dialogIdentifier)
    {
        var dialogHost = GetInstance(dialogIdentifier);
        return dialogHost.CurrentSession;
    }

    public static bool IsDialogOpen(object? dialogIdentifier) => GetDialogSession(dialogIdentifier)?.IsEnded == false;

    internal async Task<object?> ShowInternal(
        object? content,
        DialogOpenedEventHandler? openedEventHandler,
        DialogClosingEventHandler? closingEventHandler,
        DialogClosedEventHandler? closedEventHandler)
    {
        if (IsOpen)
        {
            throw new InvalidOperationException("DialogHost is already open.");
        }

        _dialogTaskCompletionSource = new TaskCompletionSource<object?>();

        if (content is not null)
        {
            DialogContent = content;
        }

        _asyncShowOpenedEventHandler = openedEventHandler;
        _asyncShowClosingEventHandler = closingEventHandler;
        _asyncShowClosedEventHandler = closedEventHandler;

        IsOpen = true;

        var result = await _dialogTaskCompletionSource.Task;

        _asyncShowOpenedEventHandler = null;
        _asyncShowClosingEventHandler = null;
        _asyncShowClosedEventHandler = null;

        return result;
    }

    internal void InternalClose(object? parameter)
    {
        var currentSession = CurrentSession ?? throw new InvalidOperationException("DialogHost does not have a current session.");

        currentSession.CloseParameter = parameter;
        currentSession.IsEnded = true;

        var dialogClosingEventArgs = new DialogClosingEventArgs(currentSession);
        DialogClosing?.Invoke(this, dialogClosingEventArgs);
        _asyncShowClosingEventHandler?.Invoke(this, dialogClosingEventArgs);

        if (dialogClosingEventArgs.IsCancelled)
        {
            currentSession.IsEnded = false;
            return;
        }

        _isClosingInternally = true;
        IsOpen = false;
        _isClosingInternally = false;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (_isDialogContentBindingContextInherited && _dialogPresenter.Content is View content)
        {
            content.BindingContext = BindingContext;
        }
    }

    protected override void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == ApplyBlurBackgroundProperty.PropertyName || propertyName == BlurRadiusProperty.PropertyName)
        {
            UpdateBlurState();
        }
    }

    private static void OnIsOpenChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost && newValue is bool isOpen)
        {
            dialogHost.HandleIsOpenChanged(isOpen);
        }
    }

    private static void OnMainContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost)
        {
            dialogHost._mainPresenter.Content = newValue as View;
        }
    }

    private static void OnDialogContentChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost)
        {
            dialogHost.UpdateDialogContent(newValue);
        }
    }

    private static void OnDialogContentTemplateChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost)
        {
            dialogHost.UpdateDialogContent(dialogHost.DialogContent);
        }
    }

    private static void OnOverlayBackgroundChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost)
        {
            dialogHost._overlay.Background = newValue as Brush ?? new SolidColorBrush(Color.FromArgb("#66000000"));
        }
    }

    private static void OnDialogBackgroundChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost)
        {
            dialogHost._dialogBorder.Background = newValue as Brush ?? new SolidColorBrush(Colors.White);
        }
    }

    private static void OnDialogCornerRadiusChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is DialogHost dialogHost && newValue is CornerRadius cornerRadius)
        {
            dialogHost._dialogBorder.StrokeShape = new RoundRectangle
            {
                CornerRadius = cornerRadius
            };
        }
    }

    private void HandleIsOpenChanged(bool isOpen)
    {
        UpdateOverlayVisibility();
        UpdateDialogVisibility();
        UpdateBlurState();
        UpdateCommandStates();
        _ = AnimateDialogAsync(isOpen);

        if (isOpen)
        {
            CurrentSession = new DialogSession(this);
            var dialogOpenedEventArgs = new DialogOpenedEventArgs(CurrentSession);
            DialogOpened?.Invoke(this, dialogOpenedEventArgs);
            _asyncShowOpenedEventHandler?.Invoke(this, dialogOpenedEventArgs);
            return;
        }

        if (CurrentSession is { } session)
        {
            if (!session.IsEnded && !_isClosingInternally)
            {
                session.Close(session.CloseParameter);
                if (!session.IsEnded)
                {
                    throw new InvalidOperationException($"Cannot cancel dialog closing after {nameof(IsOpen)} property has been set to false.");
                }
            }

            var dialogClosedEventArgs = new DialogClosedEventArgs(session);
            DialogClosed?.Invoke(this, dialogClosedEventArgs);
            _asyncShowClosedEventHandler?.Invoke(this, dialogClosedEventArgs);

            var closeParameter = session.CloseParameter;
            CurrentSession = null;
            _dialogTaskCompletionSource?.TrySetResult(closeParameter);
        }
    }

    private void UpdateDialogContent(object? content)
    {
        if (content is null)
        {
            _dialogPresenter.Content = null;
            return;
        }

        if (content is View view)
        {
            switch (view.Parent)
            {
                case Layout layout:
                    layout.Children.Remove(view);
                    break;
                case ContentView contentView:
                    contentView.Content = null;
                    break;
                case Border border:
                    border.Content = null;
                    break;
                case ContentPresenter presenter:
                    presenter.Content = null;
                    break;
            }

            _dialogPresenter.Content = view;

            if (view.IsSet(BindingContextProperty))
            {
                _isDialogContentBindingContextInherited = false;
                return;
            }

            _isDialogContentBindingContextInherited = true;
            view.BindingContext = BindingContext;
            return;
        }

        var templatedView = CreateTemplatedView(content) ?? new Label
        {
            Text = content.ToString() ?? string.Empty
        };

        _dialogPresenter.Content = templatedView;
        _isDialogContentBindingContextInherited = false;
    }

    private View? CreateTemplatedView(object content)
    {
        var selector = DialogContentTemplateSelector;
        var template = selector?.SelectTemplate(content, this) ?? DialogContentTemplate;
        if (template is null)
        {
            return null;
        }

        var created = template.CreateContent();
        switch (created)
        {
            case View view:
                view.BindingContext = content;
                return view;

            case ViewCell { View: not null } cell:
                cell.View.BindingContext = content;
                return cell.View;

            default:
                return null;
        }
    }

    private void UpdateOverlayVisibility()
    {
        if (IsOpen)
        {
            _overlay.IsVisible = true;
        }

        _overlay.InputTransparent = !IsOpen;
    }

    private void UpdateDialogVisibility()
    {
        if (IsOpen)
        {
            _dialogContainer.IsVisible = true;
        }

        _dialogContainer.InputTransparent = !IsOpen;
    }

    private void UpdateBlurState()
    {
        var shouldBlur = IsOpen && ApplyBlurBackground;
        _mainPresenter.SetDialogBackgroundBlur(shouldBlur, BlurRadius);
    }

    private void UpdateCommandStates()
    {
        _openDialogCommand.ChangeCanExecute();
        _closeDialogCommand.ChangeCanExecute();
    }

    private void ExecuteOpenDialog(object? parameter)
    {
        if (IsOpen)
        {
            return;
        }

        if (parameter is not null)
        {
            DialogContent = parameter;
        }

        IsOpen = true;
    }

    private async Task AnimateDialogAsync(bool isOpen)
    {
        var token = ++_dialogAnimationToken;
        _overlay.AbortAnimation(DialogAnimationName);
        _dialogContainer.AbortAnimation(DialogAnimationName);

        if (isOpen)
        {
            _overlay.IsVisible = true;
            _dialogContainer.IsVisible = true;
            _overlay.Opacity = 0;
            _dialogContainer.Opacity = 0;
            _dialogContainer.Scale = 0.9;

            await Task.WhenAll(
                _overlay.FadeTo(OverlayOpenOpacity, DialogOpenDuration, Easing.CubicOut),
                _dialogContainer.FadeTo(1, DialogOpenDuration, Easing.CubicOut),
                _dialogContainer.ScaleTo(1, DialogOpenDuration, Easing.CubicOut));
            return;
        }

        await Task.WhenAll(
            _overlay.FadeTo(0, DialogCloseDuration, Easing.CubicIn),
            _dialogContainer.FadeTo(0, DialogCloseDuration, Easing.CubicIn),
            _dialogContainer.ScaleTo(0.9, DialogCloseDuration, Easing.CubicIn));

        if (token != _dialogAnimationToken)
        {
            return;
        }

        _overlay.IsVisible = false;
        _dialogContainer.IsVisible = false;
        _dialogContainer.Scale = 1;
    }

    private bool CanExecuteOpenDialog(object? parameter) => !IsOpen;

    private void ExecuteCloseDialog(object? parameter)
    {
        if (CurrentSession is not { } currentSession)
        {
            return;
        }

        currentSession.Close(parameter);
    }

    private bool CanExecuteCloseDialog(object? parameter) => CurrentSession is not null;

    private void OnOverlayTapped()
    {
        if (CloseOnClickAway && CurrentSession is not null)
        {
            CurrentSession.Close(CloseOnClickAwayParameter);
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        foreach (var weakRef in LoadedInstances.ToList())
        {
            if (weakRef.TryGetTarget(out var dialogHost) && ReferenceEquals(dialogHost, this))
            {
                return;
            }
        }

        LoadedInstances.Add(new WeakReference<DialogHost>(this));
        UpdateOverlayVisibility();
        UpdateDialogVisibility();
        UpdateBlurState();
        UpdateCommandStates();
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        foreach (var weakRef in LoadedInstances.ToList())
        {
            if (!weakRef.TryGetTarget(out var dialogHost) || ReferenceEquals(dialogHost, this))
            {
                LoadedInstances.Remove(weakRef);
                break;
            }
        }
    }

    private static DialogHost GetInstance(object? dialogIdentifier)
    {
        if (LoadedInstances.Count == 0)
        {
            throw new InvalidOperationException("No loaded DialogHost instances.");
        }

        List<DialogHost> targets = [];
        foreach (var instance in LoadedInstances.ToList())
        {
            if (instance.TryGetTarget(out var dialogInstance))
            {
                if (Equals(dialogIdentifier, dialogInstance.Identifier))
                {
                    targets.Add(dialogInstance);
                }
            }
            else
            {
                LoadedInstances.Remove(instance);
            }
        }

        return targets.Count switch
        {
            0 => throw new InvalidOperationException($"No loaded DialogHost have an {nameof(Identifier)} property matching {nameof(dialogIdentifier)} ('{dialogIdentifier}') argument."),
            > 1 => throw new InvalidOperationException("Multiple viable DialogHosts. Specify a unique Identifier on each DialogHost."),
            _ => targets[0]
        };
    }
}
