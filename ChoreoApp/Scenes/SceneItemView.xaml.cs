using ReactiveUI.Maui;

namespace ChoreoApp.Scenes;

public partial class SceneItemView: ReactiveContentView<SceneViewModel>
{
    public static readonly BindableProperty ShowTimestampsProperty = BindableProperty.Create(
        nameof(ShowTimestamps),
        typeof(bool),
        typeof(SceneItemView),
        false);

    public SceneItemView()
    {
        InitializeComponent();
    }

    public bool ShowTimestamps
    {
        get => (bool)GetValue(ShowTimestampsProperty);
        set => SetValue(ShowTimestampsProperty, value);
    }
}
