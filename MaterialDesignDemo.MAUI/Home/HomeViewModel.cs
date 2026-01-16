namespace MaterialDesignDemo.Maui.Home;

public sealed partial class HomeViewModel : ReactiveObject, IActivatableViewModel
{
    private const string GitHubUrl = "https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit";
    private const string TwitterUrl = "https://twitter.com/James_Willock";
    private const string ChatUrl = "https://gitter.im/ButchersBoy/MaterialDesignInXamlToolkit";
    private const string EmailUrl = "mailto:james@dragablz.net";
    private const string DonateUrl = "https://opencollective.com/materialdesigninxaml";

    public ViewModelActivator Activator { get; } = new();

    [ReactiveCommand]
    private Task OpenExploreAsync() => OpenLinkAsync(GitHubUrl);

    [ReactiveCommand]
    private Task OpenGitHubAsync() => OpenLinkAsync(GitHubUrl);

    [ReactiveCommand]
    private Task OpenTwitterAsync() => OpenLinkAsync(TwitterUrl);

    [ReactiveCommand]
    private Task OpenChatAsync() => OpenLinkAsync(ChatUrl);

    [ReactiveCommand]
    private Task OpenEmailAsync() => OpenLinkAsync(EmailUrl);

    [ReactiveCommand]
    private Task OpenDonateAsync() => OpenLinkAsync(DonateUrl);

    private static Task OpenLinkAsync(string url)
    {
        return Microsoft.Maui.ApplicationModel.Launcher.Default.OpenAsync(new Uri(url));
    }
}
