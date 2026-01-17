using System.Collections;
using System.Globalization;
using System.Resources;
using ChoreoApp.i18n;
using ChoreoApp.Models;
using DynamicData.Binding;

namespace ChoreoApp.Dancers;

public sealed partial class DancerSettingsViewModel : ReactiveObject, IActivatableViewModel
{
    private const string ResourcePrefix = "ChoreoApp.i18n.";
    private readonly IHapticFeedback _hapticFeedback;

    public DancerSettingsViewModel(
        IEnumerable<IBehavior<DancerSettingsViewModel>> behaviors,
        IHapticFeedback hapticFeedback)
    {
        _hapticFeedback = hapticFeedback;
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

    [Reactive]
    private DancerModel? _swapFromDancer;

    [Reactive]
    private DancerModel? _swapToDancer;

    [Reactive]
    private bool _canSwapDancers;

    [Reactive]
    private bool _isDialogOpen;

    [Reactive]
    private View? _dialogContentView;

    public IReadOnlyList<IconOption> IconOptions { get; }

    [ReactiveCommand]
    private void AddDancer()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
    }

    [ReactiveCommand(CanExecute = nameof(CanDeleteDancer))]
    private void DeleteDancer()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
    }

    [ReactiveCommand(CanExecute = nameof(CanSwapDancers))]
    private void SwapDancers()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
    }

    [ReactiveCommand]
    private async Task CancelAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
        await Task.CompletedTask;
    }

    [ReactiveCommand]
    private async Task SaveAsync()
    {
        _hapticFeedback.Perform(HapticFeedbackType.Click);
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
