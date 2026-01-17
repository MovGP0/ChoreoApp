using System.Windows.Input;

namespace MaterialDesignDemo.Maui;

public partial class AppShell : Shell
{
    public static readonly BindableProperty CanNavigateBackProperty = BindableProperty.Create(
        nameof(CanNavigateBack),
        typeof(bool),
        typeof(AppShell),
        false);

    public static readonly BindableProperty CanNavigateForwardProperty = BindableProperty.Create(
        nameof(CanNavigateForward),
        typeof(bool),
        typeof(AppShell),
        false);

    public AppShell()
    {
        InitializeComponent();

        ToggleFlyoutCommand = new Command(() => FlyoutIsPresented = !FlyoutIsPresented);
        NavigateBackCommand = new Command(async () => await NavigateBackAsync(), () => CanNavigateBack);
        NavigateForwardCommand = new Command(() => { }, () => CanNavigateForward);
        NavigateHomeCommand = new Command(async () => await GoToAsync("//Home"));
        NavigateThemeCommand = new Command(async () => await GoToAsync("//Theme"));

        Navigated += OnNavigated;
        UpdateNavigationState();
    }

    public ICommand ToggleFlyoutCommand { get; }

    public ICommand NavigateBackCommand { get; }

    public ICommand NavigateForwardCommand { get; }

    public ICommand NavigateHomeCommand { get; }

    public ICommand NavigateThemeCommand { get; }

    public bool CanNavigateBack
    {
        get => (bool)GetValue(CanNavigateBackProperty);
        set => SetValue(CanNavigateBackProperty, value);
    }

    public bool CanNavigateForward
    {
        get => (bool)GetValue(CanNavigateForwardProperty);
        set => SetValue(CanNavigateForwardProperty, value);
    }

    private async Task NavigateBackAsync()
    {
        if (!CanNavigateBack)
        {
            return;
        }

        await GoToAsync("..");
    }

    private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        UpdateNavigationState();
    }

    private void UpdateNavigationState()
    {
        CanNavigateBack = Navigation.NavigationStack.Count > 1;
        CanNavigateForward = false;

        if (NavigateBackCommand is Command navigateBackCommand)
        {
            navigateBackCommand.ChangeCanExecute();
        }

        if (NavigateForwardCommand is Command navigateForwardCommand)
        {
            navigateForwardCommand.ChangeCanExecute();
        }
    }
}
