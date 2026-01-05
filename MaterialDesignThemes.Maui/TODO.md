# TODO

## Missing MAUI theme styles (WPF has these)

### Controls available in MAUI (same name)
- MaterialDesign3.CheckBox.xaml
- MaterialDesign3.DatePicker.xaml
- MaterialDesign3.Expander.xaml
- MaterialDesign3.ListView.xaml
- MaterialDesign3.ProgressBar.xaml
- MaterialDesign3.RadioButton.xaml

### Controls available in MAUI but different name
- MaterialDesign3.ComboBox.xaml (use `Picker`)
- MaterialDesign3.ListBox.xaml (use `CollectionView` or `ListView`)
- MaterialDesign3.PasswordBox.xaml (use `Entry` with `IsPassword`)
- MaterialDesign3.TabControl.xaml (use `TabbedPage` / `TabView` equivalent)

### Controls that need manual implementation if needed
- MaterialDesign3.AutoSuggestBox.xaml
- MaterialDesign3.Calendar.xaml
- MaterialDesign3.DataGrid.xaml
- MaterialDesign3.DataGrid.ComboBox.xaml
- MaterialDesign3.DialogHost.xaml
- MaterialDesign3.Flipper.xaml
- MaterialDesign3.FlipperClassic.xaml
- MaterialDesign3.GridSplitter.xaml
- MaterialDesign3.GroupBox.xaml
- MaterialDesign3.Hyperlink.xaml
- MaterialDesign3.Menu.xaml
- MaterialDesign3.PopupBox.xaml
- MaterialDesign3.RichTextBox.xaml
- MaterialDesign3.Thumb.xaml
- MaterialDesign3.ToolBar.xaml
- MaterialDesign3.ToolBarTray.xaml
- MaterialDesign3.ToolTip.xaml
- MaterialDesign3.TreeListView.xaml
- MaterialDesign3.TreeView.xaml

### Non-control theme resources

The following files do not need to be ported:
- MaterialDesign3.Dark.xaml
- MaterialDesign3.Font.xaml
- MaterialDesign3.Light.xaml
- MaterialDesign3.ObsoleteBrushes.xaml
- MaterialDesign3.ObsoleteStyles.xaml
- MaterialDesign3.ValidationErrorTemplate.xaml

> [!note]
> Use the keys defined in `MaterialDesignColorKey.cs` instead to retrieve the colors from the resource dictionary.
