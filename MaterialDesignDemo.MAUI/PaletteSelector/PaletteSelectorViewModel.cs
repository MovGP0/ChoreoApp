using System.Collections.ObjectModel;
using MaterialDesignThemes.Maui;

namespace MaterialDesignDemo.Maui.PaletteSelector;

public sealed partial class PaletteSelectorViewModel : ReactiveObject, IActivatableViewModel
{
    public PaletteSelectorViewModel()
    {
        Swatches = new ObservableCollection<PaletteSwatch>(CreateSwatches());
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<PaletteSwatch> Swatches { get; }

    [Reactive]
    private string _lastApplied = "No palette applied yet.";

    [ReactiveCommand]
    private void ApplyPrimary(PaletteSwatch swatch)
    {
        LastApplied = $"Primary set to {swatch.Name}";
    }

    [ReactiveCommand]
    private void ApplySecondary(PaletteSwatch swatch)
    {
        if (swatch.SecondaryColorKey is null)
        {
            return;
        }

        LastApplied = $"Secondary set to {swatch.Name}";
    }

    private static IEnumerable<PaletteSwatch> CreateSwatches()
    {
        return
        [
            new PaletteSwatch("Red", MaterialDesignColorKey.Red500, MaterialDesignColorKey.Red200),
            new PaletteSwatch("Pink", MaterialDesignColorKey.Pink500, MaterialDesignColorKey.Pink200),
            new PaletteSwatch("Purple", MaterialDesignColorKey.Purple500, MaterialDesignColorKey.Purple200),
            new PaletteSwatch("Indigo", MaterialDesignColorKey.Indigo500, MaterialDesignColorKey.Indigo200),
            new PaletteSwatch("Blue", MaterialDesignColorKey.Blue500, MaterialDesignColorKey.Blue200),
            new PaletteSwatch("Teal", MaterialDesignColorKey.Teal500, MaterialDesignColorKey.Teal200),
            new PaletteSwatch("Green", MaterialDesignColorKey.Green500, MaterialDesignColorKey.Green200),
            new PaletteSwatch("Orange", MaterialDesignColorKey.Orange500, MaterialDesignColorKey.Orange200),
            new PaletteSwatch("Brown", MaterialDesignColorKey.Brown500, MaterialDesignColorKey.Brown200),
            new PaletteSwatch("Blue Grey", MaterialDesignColorKey.BlueGrey500, MaterialDesignColorKey.BlueGrey200)
        ];
    }
}
