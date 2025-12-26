using System.Collections.ObjectModel;

namespace ChoreoApp.Settings.Models;

public sealed class MaterialColorGroup(string name) : ObservableCollection<MaterialColorOption>
{
    public string Name { get; } = name;
}
