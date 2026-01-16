using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Maui;

namespace ChoreoApp.Scenes;

public partial class SceneItemView: ReactiveContentView<SceneViewModel>
{
    public static readonly BindableProperty ShowTimestampsProperty = BindableProperty.Create(
        nameof(ShowTimestamps),
        typeof(bool),
        typeof(SceneItemView),
        false);

    public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(
        nameof(SelectedBackgroundColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty UnselectedBackgroundColorProperty = BindableProperty.Create(
        nameof(UnselectedBackgroundColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty SelectedStrokeColorProperty = BindableProperty.Create(
        nameof(SelectedStrokeColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty UnselectedStrokeColorProperty = BindableProperty.Create(
        nameof(UnselectedStrokeColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create(
        nameof(SelectedTextColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty UnselectedTextColorProperty = BindableProperty.Create(
        nameof(UnselectedTextColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty SelectedTimestampTextColorProperty = BindableProperty.Create(
        nameof(SelectedTimestampTextColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public static readonly BindableProperty UnselectedTimestampTextColorProperty = BindableProperty.Create(
        nameof(UnselectedTimestampTextColor),
        typeof(Color),
        typeof(SceneItemView),
        Colors.Transparent);

    public SceneItemView()
    {
        InitializeComponent();

        if (Resources.TryGetValue("SceneItemViewStyle", out var style))
        {
            Style = (Style)style;
        }

        this.WhenActivated(disposables =>
        {
            var selectionChanges = this.WhenAnyValue(view => view.ViewModel)
                .Where(viewModel => viewModel is not null)
                .Select(viewModel => viewModel!.WhenAnyValue(vm => vm.IsSelected))
                .Switch();

            var selectedColors = this.WhenAnyValue(
                view => view.SelectedBackgroundColor,
                view => view.SelectedStrokeColor,
                view => view.SelectedTextColor,
                view => view.SelectedTimestampTextColor,
                (background, stroke, text, timestamp) => (background, stroke, text, timestamp));

            var unselectedColors = this.WhenAnyValue(
                view => view.UnselectedBackgroundColor,
                view => view.UnselectedStrokeColor,
                view => view.UnselectedTextColor,
                view => view.UnselectedTimestampTextColor,
                (background, stroke, text, timestamp) => (background, stroke, text, timestamp));

            selectionChanges
                .CombineLatest(selectedColors, unselectedColors, (isSelected, selected, unselected) => (isSelected, selected, unselected))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(state =>
                {
                    var (isSelected, selected, unselected) = state;
                    SceneCard.BackgroundColor = isSelected ? selected.background : unselected.background;
                    SceneCard.Stroke = isSelected ? selected.stroke : unselected.stroke;
                    SceneCard.StrokeThickness = isSelected ? 2 : 1;
                    SceneName.TextColor = isSelected ? selected.text : unselected.text;
                    SceneTimestamp.TextColor = isSelected ? selected.timestamp : unselected.timestamp;
                })
                .DisposeWith(disposables);
        });
    }

    public bool ShowTimestamps
    {
        get => (bool)GetValue(ShowTimestampsProperty);
        set => SetValue(ShowTimestampsProperty, value);
    }

    public Color SelectedBackgroundColor
    {
        get => (Color)GetValue(SelectedBackgroundColorProperty);
        set => SetValue(SelectedBackgroundColorProperty, value);
    }

    public Color UnselectedBackgroundColor
    {
        get => (Color)GetValue(UnselectedBackgroundColorProperty);
        set => SetValue(UnselectedBackgroundColorProperty, value);
    }

    public Color SelectedStrokeColor
    {
        get => (Color)GetValue(SelectedStrokeColorProperty);
        set => SetValue(SelectedStrokeColorProperty, value);
    }

    public Color UnselectedStrokeColor
    {
        get => (Color)GetValue(UnselectedStrokeColorProperty);
        set => SetValue(UnselectedStrokeColorProperty, value);
    }

    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }

    public Color UnselectedTextColor
    {
        get => (Color)GetValue(UnselectedTextColorProperty);
        set => SetValue(UnselectedTextColorProperty, value);
    }

    public Color SelectedTimestampTextColor
    {
        get => (Color)GetValue(SelectedTimestampTextColorProperty);
        set => SetValue(SelectedTimestampTextColorProperty, value);
    }

    public Color UnselectedTimestampTextColor
    {
        get => (Color)GetValue(UnselectedTimestampTextColorProperty);
        set => SetValue(UnselectedTimestampTextColorProperty, value);
    }
}
