using System.Collections.ObjectModel;
using MaterialDesignThemes.Maui;

namespace MaterialDesignDemo.Maui.ColorTool;

public sealed partial class ColorToolViewModel : ReactiveObject, IActivatableViewModel
{
    public ColorToolViewModel()
    {
        Swatches = new ObservableCollection<ColorToolSwatch>(CreateSwatches());
        SelectedColor = Colors.Blue;
    }

    public ViewModelActivator Activator { get; } = new();

    public ObservableCollection<ColorToolSwatch> Swatches { get; }

    [Reactive]
    private Color _selectedColor;

    [ReactiveCommand]
    private void SelectColor(string colorKey)
    {
        if (Application.Current?.Resources.TryGetValue(colorKey, out var resource) == true && resource is Color color)
        {
            SelectedColor = color;
        }
    }

    private static IEnumerable<ColorToolSwatch> CreateSwatches()
    {
        return
        [
            new ColorToolSwatch("Red", CreateKeys(MaterialDesignColorKey.Red50, MaterialDesignColorKey.Red100, MaterialDesignColorKey.Red200,
                MaterialDesignColorKey.Red300, MaterialDesignColorKey.Red400, MaterialDesignColorKey.Red500,
                MaterialDesignColorKey.Red600, MaterialDesignColorKey.Red700, MaterialDesignColorKey.Red800, MaterialDesignColorKey.Red900)),
            new ColorToolSwatch("Pink", CreateKeys(MaterialDesignColorKey.Pink50, MaterialDesignColorKey.Pink100, MaterialDesignColorKey.Pink200,
                MaterialDesignColorKey.Pink300, MaterialDesignColorKey.Pink400, MaterialDesignColorKey.Pink500,
                MaterialDesignColorKey.Pink600, MaterialDesignColorKey.Pink700, MaterialDesignColorKey.Pink800, MaterialDesignColorKey.Pink900)),
            new ColorToolSwatch("Purple", CreateKeys(MaterialDesignColorKey.Purple50, MaterialDesignColorKey.Purple100, MaterialDesignColorKey.Purple200,
                MaterialDesignColorKey.Purple300, MaterialDesignColorKey.Purple400, MaterialDesignColorKey.Purple500,
                MaterialDesignColorKey.Purple600, MaterialDesignColorKey.Purple700, MaterialDesignColorKey.Purple800, MaterialDesignColorKey.Purple900)),
            new ColorToolSwatch("Indigo", CreateKeys(MaterialDesignColorKey.Indigo50, MaterialDesignColorKey.Indigo100, MaterialDesignColorKey.Indigo200,
                MaterialDesignColorKey.Indigo300, MaterialDesignColorKey.Indigo400, MaterialDesignColorKey.Indigo500,
                MaterialDesignColorKey.Indigo600, MaterialDesignColorKey.Indigo700, MaterialDesignColorKey.Indigo800, MaterialDesignColorKey.Indigo900)),
            new ColorToolSwatch("Blue", CreateKeys(MaterialDesignColorKey.Blue50, MaterialDesignColorKey.Blue100, MaterialDesignColorKey.Blue200,
                MaterialDesignColorKey.Blue300, MaterialDesignColorKey.Blue400, MaterialDesignColorKey.Blue500,
                MaterialDesignColorKey.Blue600, MaterialDesignColorKey.Blue700, MaterialDesignColorKey.Blue800, MaterialDesignColorKey.Blue900)),
            new ColorToolSwatch("Teal", CreateKeys(MaterialDesignColorKey.Teal50, MaterialDesignColorKey.Teal100, MaterialDesignColorKey.Teal200,
                MaterialDesignColorKey.Teal300, MaterialDesignColorKey.Teal400, MaterialDesignColorKey.Teal500,
                MaterialDesignColorKey.Teal600, MaterialDesignColorKey.Teal700, MaterialDesignColorKey.Teal800, MaterialDesignColorKey.Teal900)),
            new ColorToolSwatch("Green", CreateKeys(MaterialDesignColorKey.Green50, MaterialDesignColorKey.Green100, MaterialDesignColorKey.Green200,
                MaterialDesignColorKey.Green300, MaterialDesignColorKey.Green400, MaterialDesignColorKey.Green500,
                MaterialDesignColorKey.Green600, MaterialDesignColorKey.Green700, MaterialDesignColorKey.Green800, MaterialDesignColorKey.Green900)),
            new ColorToolSwatch("Orange", CreateKeys(MaterialDesignColorKey.Orange50, MaterialDesignColorKey.Orange100, MaterialDesignColorKey.Orange200,
                MaterialDesignColorKey.Orange300, MaterialDesignColorKey.Orange400, MaterialDesignColorKey.Orange500,
                MaterialDesignColorKey.Orange600, MaterialDesignColorKey.Orange700, MaterialDesignColorKey.Orange800, MaterialDesignColorKey.Orange900)),
            new ColorToolSwatch("Brown", CreateKeys(MaterialDesignColorKey.Brown50, MaterialDesignColorKey.Brown100, MaterialDesignColorKey.Brown200,
                MaterialDesignColorKey.Brown300, MaterialDesignColorKey.Brown400, MaterialDesignColorKey.Brown500,
                MaterialDesignColorKey.Brown600, MaterialDesignColorKey.Brown700, MaterialDesignColorKey.Brown800, MaterialDesignColorKey.Brown900)),
            new ColorToolSwatch("Blue Grey", CreateKeys(MaterialDesignColorKey.BlueGrey50, MaterialDesignColorKey.BlueGrey100, MaterialDesignColorKey.BlueGrey200,
                MaterialDesignColorKey.BlueGrey300, MaterialDesignColorKey.BlueGrey400, MaterialDesignColorKey.BlueGrey500,
                MaterialDesignColorKey.BlueGrey600, MaterialDesignColorKey.BlueGrey700, MaterialDesignColorKey.BlueGrey800, MaterialDesignColorKey.BlueGrey900))
        ];
    }

    private static IReadOnlyList<string> CreateKeys(params string[] keys)
    {
        return keys;
    }
}
