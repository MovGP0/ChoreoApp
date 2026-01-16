using System.Collections.ObjectModel;

namespace MaterialDesignDemo.Maui.DocumentationLinks;

public sealed partial class DocumentationLinksViewModel : ReactiveObject, IActivatableViewModel
{
    public DocumentationLinksViewModel()
    {
        Documentation = new ObservableCollection<DocumentationLink>
        {
            new("Material 3 overview", DocumentationLinkType.Specs, "https://m3.material.io/"),
            new("Material Design in XAML Toolkit", DocumentationLinkType.ControlSource, "https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit"),
            new("MAUI docs", DocumentationLinkType.Documentation, "https://learn.microsoft.com/dotnet/maui/")
        };
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<DocumentationLink> Documentation { get; }

    [ReactiveCommand]
    private async Task OpenLinkAsync(DocumentationLink link)
    {
        if (string.IsNullOrWhiteSpace(link.Url))
        {
            return;
        }

        await Launcher.OpenAsync(new Uri(link.Url));
    }
}

public enum DocumentationLinkType
{
    Documentation,
    DemoPageSource,
    StyleSource,
    Video,
    ControlSource,
    Specs
}

public sealed record DocumentationLink(string Label, DocumentationLinkType Type, string Url);
