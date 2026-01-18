using System.Globalization;
using ChoreoApp.i18n;
using ChoreoApp.Main.Messages;
using ChoreoApp.Scenes.Events;
using MessagePipe;

namespace ChoreoApp.Scenes;

public sealed partial class CopyScenePositionsDialogViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IPublisher<CloseDialogCommand> _closeDialogPublisher;
    private readonly IPublisher<CopyScenePositionsDecisionEvent> _decisionPublisher;
    private readonly IHapticFeedback _hapticFeedback;

    public CopyScenePositionsDialogViewModel(
        IPublisher<CloseDialogCommand> closeDialogPublisher,
        IPublisher<CopyScenePositionsDecisionEvent> decisionPublisher,
        IHapticFeedback hapticFeedback,
        SceneViewModel? selectedScene)
    {
        ArgumentNullException.ThrowIfNull(closeDialogPublisher);
        ArgumentNullException.ThrowIfNull(decisionPublisher);
        ArgumentNullException.ThrowIfNull(hapticFeedback);

        _closeDialogPublisher = closeDialogPublisher;
        _hapticFeedback = hapticFeedback;
        _decisionPublisher = decisionPublisher;

        var name = selectedScene?.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            name = Translations.DeleteSceneDialogDefaultName;
        }

        SceneName = name;
        SceneColor = selectedScene?.Color ?? Colors.Transparent;

        Message = string.Format(
            CultureInfo.CurrentUICulture,
            Translations.CopyScenePositionsDialogMessage,
            name);
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _message = string.Empty;

    [Reactive]
    private string _sceneName = string.Empty;

    [Reactive]
    private Color _sceneColor = Colors.Transparent;

    [ReactiveCommand]
    private void ConfirmCopy()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        CloseDialog();
        _decisionPublisher.Publish(new CopyScenePositionsDecisionEvent(CopyScenePositionsDecision.CopyPositions));
    }

    [ReactiveCommand]
    private void DeclineCopy()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        CloseDialog();
        _decisionPublisher.Publish(new CopyScenePositionsDecisionEvent(CopyScenePositionsDecision.KeepPositions));
    }

    private void CloseDialog()
    {
        _closeDialogPublisher.Publish(new CloseDialogCommand());
    }
}
