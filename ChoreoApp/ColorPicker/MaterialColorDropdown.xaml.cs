using System.Collections.ObjectModel;
//using ChoreoApp.Settings.Models;

namespace ChoreoApp.ColorPicker;

public partial class MaterialColorDropdown : ContentView
{
    private bool _isUpdatingSelection;

    public MaterialColorDropdown()
    {
        InitializeComponent();
    }

    public ObservableCollection<MaterialColorOption> FlatItems { get; } = new();

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IReadOnlyList<MaterialColorGroup>),
        typeof(MaterialColorDropdown),
        defaultValue: Array.Empty<MaterialColorGroup>(),
        propertyChanged: OnItemsSourceChanged);

    public IReadOnlyList<MaterialColorGroup> ItemsSource
    {
        get => (IReadOnlyList<MaterialColorGroup>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(
        nameof(SelectedColor),
        typeof(Color),
        typeof(MaterialColorDropdown),
        Colors.Transparent,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedColorChanged);

    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    public static readonly BindableProperty SelectedOptionProperty = BindableProperty.Create(
        nameof(SelectedOption),
        typeof(MaterialColorOption),
        typeof(MaterialColorDropdown),
        defaultValue: null,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedOptionChanged);

    public MaterialColorOption? SelectedOption
    {
        get => (MaterialColorOption?)GetValue(SelectedOptionProperty);
        set => SetValue(SelectedOptionProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(MaterialColorDropdown),
        "Select color",
        propertyChanged: OnPlaceholderChanged);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(MaterialColorDropdown),
        false);

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public string SelectedDisplayName => SelectedOption?.DisplayName ?? Placeholder;

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialColorDropdown dropdown)
        {
            dropdown.RebuildFlatItems();
            dropdown.UpdateSelectedOptionFromColor();
        }
    }

    private static void OnSelectedColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialColorDropdown dropdown)
        {
            dropdown.UpdateSelectedOptionFromColor();
        }
    }

    private static void OnSelectedOptionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialColorDropdown dropdown)
        {
            dropdown.UpdateSelectedColorFromOption();
            dropdown.OnPropertyChanged(nameof(SelectedDisplayName));
        }
    }

    private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MaterialColorDropdown dropdown)
        {
            dropdown.OnPropertyChanged(nameof(SelectedDisplayName));
        }
    }

    private void UpdateSelectedOptionFromColor()
    {
        if (_isUpdatingSelection)
        {
            return;
        }

        var option = FindOptionByColor(SelectedColor);
        if (option is null || ReferenceEquals(option, SelectedOption))
        {
            return;
        }

        _isUpdatingSelection = true;
        SelectedOption = option;
        _isUpdatingSelection = false;
    }

    private void UpdateSelectedColorFromOption()
    {
        if (_isUpdatingSelection || SelectedOption is null)
        {
            return;
        }

        _isUpdatingSelection = true;
        SelectedColor = SelectedOption.Color;
        _isUpdatingSelection = false;
    }

    private MaterialColorOption? FindOptionByColor(Color color)
    {
        foreach (var group in ItemsSource)
        {
            foreach (var option in group)
            {
                if (option.Color.Equals(color))
                {
                    return option;
                }
            }
        }

        return null;
    }

    private void RebuildFlatItems()
    {
        FlatItems.Clear();
        foreach (var group in ItemsSource)
        {
            foreach (var option in group)
            {
                FlatItems.Add(option);
            }
        }
    }

    private void OnColorTapped(object? sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is MaterialColorOption option)
        {
            SelectedOption = option;
        }

        IsExpanded = false;
    }
}
