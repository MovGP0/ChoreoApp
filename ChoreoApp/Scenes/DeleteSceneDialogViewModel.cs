using System.Globalization;
using ChoreoApp.Global;
using ChoreoApp.i18n;
using ChoreoApp.Main.Messages;
using MessagePipe;

namespace ChoreoApp.Scenes;

public sealed partial class DeleteSceneDialogViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly GlobalStateModel _globalState;
    private readonly IPublisher<CloseDialogCommand> _closeDialogPublisher;
    private readonly SceneViewModel? _scene;

    public DeleteSceneDialogViewModel(
        GlobalStateModel globalState,
        IPublisher<CloseDialogCommand> closeDialogPublisher,
        SceneViewModel? scene)
    {
        _globalState = globalState;
        _closeDialogPublisher = closeDialogPublisher;
        _scene = scene;

        var name = scene?.Name;
        if (string.IsNullOrWhiteSpace(name))
        {
            name = Translations.DeleteSceneDialogDefaultName;
        }

        Message = string.Format(CultureInfo.CurrentUICulture, Translations.DeleteSceneDialogMessage, name);
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _message = string.Empty;

    [ReactiveCommand]
    private void ConfirmDelete()
    {
        if (_scene is null)
        {
            CloseDialog();
            return;
        }

        RemoveScene(_scene);
        CloseDialog();
    }

    [ReactiveCommand]
    private void Cancel()
    {
        CloseDialog();
    }

    private void CloseDialog()
    {
        _closeDialogPublisher.Publish(new CloseDialogCommand());
    }

    private void RemoveScene(SceneViewModel scene)
    {
        var index = _globalState.Scenes.IndexOf(scene);
        if (index < 0)
        {
            return;
        }

        _globalState.Scenes.RemoveAt(index);

        if (_globalState.Choreography is { } choreography)
        {
            var model = choreography.Scenes.FirstOrDefault(s => s.SceneId == scene.SceneId)
                ?? choreography.Scenes.FirstOrDefault(s => string.Equals(s.Name, scene.Name, StringComparison.Ordinal));
            if (model is not null)
            {
                choreography.Scenes.Remove(model);
            }
        }

        if (ReferenceEquals(_globalState.SelectedScene, scene))
        {
            if (_globalState.Scenes.Count == 0)
            {
                _globalState.SelectedScene = null;
            }
            else
            {
                var nextIndex = Math.Clamp(index, 0, _globalState.Scenes.Count - 1);
                _globalState.SelectedScene = _globalState.Scenes[nextIndex];
            }
        }
    }
}
