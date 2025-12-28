using System.Collections.ObjectModel;

namespace ChoreoApp.ColorPicker;

public sealed class MaterialColorGroup(string name) : ObservableCollection<MaterialColorOption>
{
    public string Name { get; } = name;
}
