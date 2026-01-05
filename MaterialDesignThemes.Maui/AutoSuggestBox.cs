using System.Collections;

namespace MaterialDesignThemes.Maui;

public sealed class AutoSuggestBoxSuggestionChosenEventArgs(
    string? oldText,
    string? newText,
    object? selectedItem,
    object? selectedValue)
    : EventArgs
{
    public string? OldText { get; } = oldText;

    public string? NewText { get; } = newText;

    public object? SelectedItem { get; } = selectedItem;

    public object? SelectedValue { get; } = selectedValue;
}

[ContentProperty(nameof(Text))]
public sealed class AutoSuggestBox : ContentView
{
    private readonly Entry _textEntry;
    private readonly CollectionView _suggestionsList;
    private readonly Border _dropDownContainer;
    private bool _suppressTextSync;
    private bool _suppressSelectionSync;

    public AutoSuggestBox()
    {
        _textEntry = new Entry();
        _textEntry.TextChanged += OnEntryTextChanged;
        _textEntry.Focused += OnEntryFocused;
        _textEntry.Unfocused += OnEntryUnfocused;
        _textEntry.Completed += OnEntryCompleted;

        _suggestionsList = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
        };
        _suggestionsList.SelectionChanged += OnSuggestionsSelectionChanged;

        _dropDownContainer = new Border
        {
            IsVisible = false,
            Content = _suggestionsList
        };

        var layout = new Grid
        {
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            ]
        };
        layout.Add(_textEntry);
        layout.Add(_dropDownContainer, 0, 1);

        Content = layout;

        UpdateItemTemplate();
        UpdateDropDownAppearance();
        UpdateDropDownVisibility();
    }

    public static bool? GetIsInteractiveElement(BindableObject obj)
        => (bool?)obj.GetValue(IsInteractiveElementProperty);

    public static void SetIsInteractiveElement(BindableObject obj, bool? value)
        => obj.SetValue(IsInteractiveElementProperty, value);

    public static readonly BindableProperty IsInteractiveElementProperty =
        BindableProperty.CreateAttached(
            "IsInteractiveElement",
            typeof(bool?),
            typeof(AutoSuggestBox),
            null);

    public IEnumerable? Suggestions
    {
        get => (IEnumerable?)GetValue(SuggestionsProperty);
        set => SetValue(SuggestionsProperty, value);
    }

    public static readonly BindableProperty SuggestionsProperty =
        BindableProperty.Create(
            nameof(Suggestions),
            typeof(IEnumerable),
            typeof(AutoSuggestBox),
            propertyChanged: OnSuggestionsChanged);

    public string? ValueMember
    {
        get => (string?)GetValue(ValueMemberProperty);
        set => SetValue(ValueMemberProperty, value);
    }

    public static readonly BindableProperty ValueMemberProperty =
        BindableProperty.Create(
            nameof(ValueMember),
            typeof(string),
            typeof(AutoSuggestBox));

    public string? DisplayMember
    {
        get => (string?)GetValue(DisplayMemberProperty);
        set => SetValue(DisplayMemberProperty, value);
    }

    public static readonly BindableProperty DisplayMemberProperty =
        BindableProperty.Create(
            nameof(DisplayMember),
            typeof(string),
            typeof(AutoSuggestBox),
            propertyChanged: OnItemTemplateRelatedChanged);

    public Brush? DropDownBackground
    {
        get => (Brush?)GetValue(DropDownBackgroundProperty);
        set => SetValue(DropDownBackgroundProperty, value);
    }

    public static readonly BindableProperty DropDownBackgroundProperty =
        BindableProperty.Create(
            nameof(DropDownBackground),
            typeof(Brush),
            typeof(AutoSuggestBox),
            propertyChanged: OnDropDownAppearanceChanged);

    public DataTemplate? ItemTemplate
    {
        get => (DataTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(AutoSuggestBox),
            propertyChanged: OnItemTemplateRelatedChanged);

    public Style? ItemContainerStyle
    {
        get => (Style?)GetValue(ItemContainerStyleProperty);
        set => SetValue(ItemContainerStyleProperty, value);
    }

    public static readonly BindableProperty ItemContainerStyleProperty =
        BindableProperty.Create(
            nameof(ItemContainerStyle),
            typeof(Style),
            typeof(AutoSuggestBox),
            propertyChanged: OnItemTemplateRelatedChanged);

    public Elevation DropDownElevation
    {
        get => (Elevation)GetValue(DropDownElevationProperty);
        set => SetValue(DropDownElevationProperty, value);
    }

    public static readonly BindableProperty DropDownElevationProperty =
        BindableProperty.Create(
            nameof(DropDownElevation),
            typeof(Elevation),
            typeof(AutoSuggestBox),
            Elevation.Dp0,
            propertyChanged: OnDropDownAppearanceChanged);

    public double DropDownMaxHeight
    {
        get => (double)GetValue(DropDownMaxHeightProperty);
        set => SetValue(DropDownMaxHeightProperty, value);
    }

    public static readonly BindableProperty DropDownMaxHeightProperty =
        BindableProperty.Create(
            nameof(DropDownMaxHeight),
            typeof(double),
            typeof(AutoSuggestBox),
            200d,
            propertyChanged: OnDropDownAppearanceChanged);

    public bool IsSuggestionOpen
    {
        get => (bool)GetValue(IsSuggestionOpenProperty);
        set => SetValue(IsSuggestionOpenProperty, value);
    }

    public static readonly BindableProperty IsSuggestionOpenProperty =
        BindableProperty.Create(
            nameof(IsSuggestionOpen),
            typeof(bool),
            typeof(AutoSuggestBox),
            false,
            propertyChanged: OnIsSuggestionOpenChanged);

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(AutoSuggestBox),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnSelectedItemChanged);

    public object? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static readonly BindableProperty SelectedValueProperty =
        BindableProperty.Create(
            nameof(SelectedValue),
            typeof(object),
            typeof(AutoSuggestBox),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnSelectedValueChanged);

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(AutoSuggestBox),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnTextPropertyChanged);

    public event EventHandler<AutoSuggestBoxSuggestionChosenEventArgs>? SuggestionChosen;

    private static void OnSuggestionsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox._suggestionsList.ItemsSource = autoSuggestBox.Suggestions;
            autoSuggestBox.UpdateDropDownVisibility();
        }
    }

    private static void OnItemTemplateRelatedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.UpdateItemTemplate();
        }
    }

    private static void OnDropDownAppearanceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.UpdateDropDownAppearance();
        }
    }

    private static void OnIsSuggestionOpenChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox && newValue is bool isOpen)
        {
            autoSuggestBox._dropDownContainer.IsVisible = isOpen;
        }
    }

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            if (autoSuggestBox._suppressSelectionSync)
            {
                return;
            }

            autoSuggestBox.SyncSelectedItemToList(newValue);
            autoSuggestBox.SyncSelectedItemToValue(newValue);
        }
    }

    private static void OnSelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.SyncSelectedValueToItem(newValue);
        }
    }

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoSuggestBox autoSuggestBox)
        {
            autoSuggestBox.SyncTextToEntry(newValue as string);
        }
    }

    private void OnEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_suppressTextSync)
        {
            return;
        }

        SetValue(TextProperty, e.NewTextValue);
        UpdateDropDownVisibility();
    }

    private void OnEntryFocused(object? sender, FocusEventArgs e)
    {
        UpdateDropDownVisibility();
    }

    private void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        CloseAutoSuggestionPopUp();
    }

    private void OnEntryCompleted(object? sender, EventArgs e)
    {
        CommitValueSelection(_suggestionsList.SelectedItem ?? SelectedItem);
    }

    private void OnSuggestionsSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_suppressSelectionSync)
        {
            return;
        }

        var selectedItem = e.CurrentSelection.FirstOrDefault();
        if (selectedItem is null)
        {
            return;
        }

        SetValue(SelectedItemProperty, selectedItem);
        CommitValueSelection(selectedItem);
    }

    private void UpdateItemTemplate()
    {
        if (ItemTemplate is not null)
        {
            _suggestionsList.ItemTemplate = ItemTemplate;
            return;
        }

        _suggestionsList.ItemTemplate = CreateDefaultItemTemplate();
    }

    private DataTemplate CreateDefaultItemTemplate()
    {
        return new DataTemplate(() =>
        {
            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(12, 8)
            };

            var bindingPath = string.IsNullOrWhiteSpace(DisplayMember) ? "." : DisplayMember;
            label.SetBinding(Label.TextProperty, bindingPath);

            var container = new ContentView
            {
                Content = label
            };

            if (ItemContainerStyle is not null)
            {
                container.Style = ItemContainerStyle;
            }

            return container;
        });
    }

    private void UpdateDropDownAppearance()
    {
        _dropDownContainer.Background = DropDownBackground;
        _dropDownContainer.MaximumHeightRequest = DropDownMaxHeight;
        ElevationAssist.SetElevation(_dropDownContainer, DropDownElevation);
    }

    private void UpdateDropDownVisibility()
    {
        if (string.IsNullOrEmpty(Text) || !_textEntry.IsFocused || !HasSuggestions())
        {
            IsSuggestionOpen = false;
            return;
        }

        IsSuggestionOpen = true;
    }

    private bool HasSuggestions()
    {
        if (Suggestions is null)
        {
            return false;
        }

        if (Suggestions is ICollection collection)
        {
            return collection.Count > 0;
        }

        var enumerator = Suggestions.GetEnumerator();
        return enumerator.MoveNext();
    }

    private void CloseAutoSuggestionPopUp()
    {
        IsSuggestionOpen = false;
    }

    private bool CommitValueSelection(object? selectedItem)
    {
        if (!IsSuggestionOpen)
        {
            return false;
        }

        var selectedValue = GetSelectedValue(selectedItem);
        var oldText = Text;
        var newText = selectedValue?.ToString();

        SetValue(TextProperty, newText);

        if (!string.IsNullOrEmpty(_textEntry.Text))
        {
            _textEntry.CursorPosition = _textEntry.Text.Length;
        }

        SetValue(SelectedItemProperty, selectedItem);
        SetValue(SelectedValueProperty, selectedValue);
        CloseAutoSuggestionPopUp();
        ClearListSelection();

        SuggestionChosen?.Invoke(
            this,
            new AutoSuggestBoxSuggestionChosenEventArgs(oldText, newText, selectedItem, selectedValue));

        return true;
    }

    private object? GetSelectedValue(object? selectedItem)
    {
        if (selectedItem is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(ValueMember))
        {
            return selectedItem;
        }

        var property = selectedItem.GetType().GetProperty(ValueMember);
        return property?.GetValue(selectedItem) ?? selectedItem;
    }

    private void SyncSelectedItemToList(object? selectedItem)
    {
        _suppressSelectionSync = true;
        _suggestionsList.SelectedItem = selectedItem;
        _suppressSelectionSync = false;
    }

    private void SyncSelectedItemToValue(object? selectedItem)
    {
        _suppressSelectionSync = true;
        SetValue(SelectedValueProperty, GetSelectedValue(selectedItem));
        _suppressSelectionSync = false;
    }

    private void SyncSelectedValueToItem(object? selectedValue)
    {
        if (_suppressSelectionSync || Suggestions is null)
        {
            return;
        }

        if (selectedValue is null)
        {
            SetValue(SelectedItemProperty, null);
            SyncSelectedItemToList(null);
            return;
        }

        foreach (var item in Suggestions)
        {
            var itemValue = GetSelectedValue(item);
            if (Equals(itemValue, selectedValue))
            {
                SetValue(SelectedItemProperty, item);
                SyncSelectedItemToList(item);
                return;
            }
        }
    }

    private void SyncTextToEntry(string? text)
    {
        if (_textEntry.Text == text)
        {
            UpdateDropDownVisibility();
            return;
        }

        _suppressTextSync = true;
        _textEntry.Text = text;
        _suppressTextSync = false;
        UpdateDropDownVisibility();
    }

    private void ClearListSelection()
    {
        _suppressSelectionSync = true;
        _suggestionsList.SelectedItem = null;
        _suppressSelectionSync = false;
    }
}
