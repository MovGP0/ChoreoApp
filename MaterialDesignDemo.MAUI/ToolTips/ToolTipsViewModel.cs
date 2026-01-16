using MaterialDesignThemes.Maui;
using ThemeElevation = MaterialDesignThemes.Maui.Elevation;

namespace MaterialDesignDemo.Maui.ToolTips;

public sealed partial class ToolTipsViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new();

    public IReadOnlyList<ThemeElevation> Elevations { get; } = Enum.GetValues<ThemeElevation>();
    public IReadOnlyList<PopupBoxPlacementMode> PopupBoxPlacementModes { get; } = Enum.GetValues<PopupBoxPlacementMode>();
    public IReadOnlyList<PopupAnimation> PopupAnimations { get; } = Enum.GetValues<PopupAnimation>();
    public IReadOnlyList<PopupBoxPopupMode> PopupBoxPopupModes { get; } = Enum.GetValues<PopupBoxPopupMode>();

    [Reactive]
    private bool _isPopupOpen;

    [Reactive]
    private ThemeElevation _selectedElevation = ThemeElevation.Dp6;

    [Reactive]
    private PopupBoxPlacementMode _selectedPopupBoxPlacementMode = PopupBoxPlacementMode.BottomAndAlignCentres;

    [Reactive]
    private PopupAnimation _selectedPopupAnimation = PopupAnimation.Fade;

    [Reactive]
    private PopupBoxPopupMode _selectedPopupBoxPopupMode = PopupBoxPopupMode.Click;

    [Reactive]
    private double _popupUniformCornerRadius = 8;

    [Reactive]
    private double _popupHorizontalOffset;

    [Reactive]
    private double _popupVerticalOffset;

    [ReactiveCommand]
    private void ResetToDefaults()
    {
        IsPopupOpen = false;
        SelectedElevation = ThemeElevation.Dp6;
        SelectedPopupBoxPlacementMode = PopupBoxPlacementMode.BottomAndAlignCentres;
        SelectedPopupAnimation = PopupAnimation.Fade;
        SelectedPopupBoxPopupMode = PopupBoxPopupMode.Click;
        PopupUniformCornerRadius = 8;
        PopupHorizontalOffset = 0;
        PopupVerticalOffset = 0;
    }
}
