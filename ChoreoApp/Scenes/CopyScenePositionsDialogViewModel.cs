using System.Globalization;
using ChoreoApp.i18n;
using ChoreoApp.Main.Messages;
using MessagePipe;

namespace ChoreoApp.Scenes;

public sealed partial class CopyScenePositionsDialogViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IPublisher<CloseDialogCommand> _closeDialogPublisher;
    private readonly Action<bool> _onDecision;

    public CopyScenePositionsDialogViewModel(
        IPublisher<CloseDialogCommand> closeDialogPublisher,
        SceneViewModel? selectedScene,
        Action<bool> onDecision)
    {
        ArgumentNullException.ThrowIfNull(closeDialogPublisher);
        ArgumentNullException.ThrowIfNull(onDecision);

        _closeDialogPublisher = closeDialogPublisher;
        _onDecision = onDecision;

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
        CloseDialog();
        _onDecision(true);
    }

    [ReactiveCommand]
    private void DeclineCopy()
    {
        CloseDialog();
        _onDecision(false);
    }

    private void CloseDialog()
    {
        _closeDialogPublisher.Publish(new CloseDialogCommand());
    }
}
