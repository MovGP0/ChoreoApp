using System.Collections;
using System.Globalization;
using System.Resources;
using ChoreoApp.ColorPicker;
using ChoreoApp.i18n;
using ChoreoApp.Models;
using DynamicData.Binding;

namespace ChoreoApp.Dancers;

public sealed partial class DancerSettingsViewModel : ReactiveObject, IActivatableViewModel
{
    private const string ResourcePrefix = "ChoreoApp.i18n.";

    public DancerSettingsViewModel(
        IEnumerable<IBehavior<DancerSettingsViewModel>> behaviors)
    {
        IconOptions = LoadIconOptions();

        this.WhenActivated(disposables =>
        {
            foreach (var behavior in behaviors)
            {
                behavior.Activate(this, disposables);
            }
        });
    }

    public ViewModelActivator Activator { get; } = new();

    [ReactiveCollection]
    private ObservableCollectionExtended<DancerModel> _dancers = [];

    [ReactiveCollection]
    private ObservableCollectionExtended<RoleModel> _roles = [];

    [Reactive]
    private DancerModel? _selectedDancer;

    [Reactive]
    private RoleModel? _selectedRole;

    [Reactive]
    private IconOption? _selectedIconOption;

    [Reactive]
    private bool _hasSelectedDancer;

    [Reactive]
    private bool _canDeleteDancer;

    [Reactive]
    private bool _isDancerListOpen;

    public IReadOnlyList<MaterialColorGroup> ColorGroups { get; } = MaterialColorPalette.DefaultGroups;
    public IReadOnlyList<IconOption> IconOptions { get; }

    [ReactiveCommand]
    private void AddDancer()
    {
    }

    [ReactiveCommand(CanExecute = nameof(CanDeleteDancer))]
    private void DeleteDancer()
    {
    }

    [ReactiveCommand]
    private async Task CancelAsync()
    {
        await Task.CompletedTask;
    }

    [ReactiveCommand]
    private async Task SaveAsync()
    {
        await Task.CompletedTask;
    }

    private static IReadOnlyList<IconOption> LoadIconOptions()
    {
        var options = new List<IconOption>();
        var manager = new ResourceManager("ChoreoApp.i18n.Icons", typeof(Translations).Assembly);
        var resourceSet = manager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        if (resourceSet is null)
        {
            return options;
        }

        foreach (DictionaryEntry entry in resourceSet)
        {
            if (entry.Key is not string key || entry.Value is not string path)
            {
                continue;
            }

            var displayName = ToDisplayName(key);
            var resourceName = BuildResourceName(path);
            var imageSource = ImageSource.FromResource(resourceName, typeof(Translations).Assembly);
            options.Add(new IconOption(key, displayName, path, imageSource));
        }

        return options
            .OrderBy(option => option.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string ToDisplayName(string key)
    {
        if (key.StartsWith("Icon", StringComparison.Ordinal))
        {
            key = key[4..];
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            return "Icon";
        }

        var builder = new List<char>(key.Length + 4);
        for (int i = 0; i < key.Length; i++)
        {
            var current = key[i];
            if (i > 0 && char.IsUpper(current) && !char.IsUpper(key[i - 1]))
            {
                builder.Add(' ');
            }

            builder.Add(current);
        }

        return new string(builder.ToArray());
    }

    private static string BuildResourceName(string path)
    {
        var normalized = path.Replace('\\', '/');
        if (!normalized.StartsWith("Icons/", StringComparison.OrdinalIgnoreCase))
        {
            normalized = $"Icons/{normalized}";
        }

        normalized = normalized.Replace('/', '.');
        return $"{ResourcePrefix}{normalized}";
    }
}
