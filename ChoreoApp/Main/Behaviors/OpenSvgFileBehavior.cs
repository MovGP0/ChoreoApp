using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Global;
using ChoreoApp.Main.Messages;
using ChoreoApp.Settings;
using MessagePipe;
using Svg.Skia;

namespace ChoreoApp.Main.Behaviors;

public sealed class OpenSvgFileBehavior(
    GlobalStateModel globalState,
    Floor.IFloorRenderGate renderGate,
    ISubscriber<OpenSvgFileCommand> subscriber) : IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        subscriber
            .Subscribe(async message => await HandleOpenAsync(message))
            .DisposeWith(disposables);

        var cancellationTokenSource = new CancellationTokenSource();
        Disposable
            .Create(() => cancellationTokenSource.Cancel())
            .DisposeWith(disposables);

        _ = LoadLastOpenedAsync(cancellationTokenSource.Token);
    }

    private async Task HandleOpenAsync(OpenSvgFileCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.FilePath) || !File.Exists(command.FilePath))
        {
            return;
        }

        await LoadSvgAsync(command.FilePath);
    }

    private async Task LoadLastOpenedAsync(CancellationToken cancellationToken)
    {
        await renderGate.WaitForFirstRenderAsync(cancellationToken);

        var storedPath = Preferences.Default.Get(SettingsPreferenceKeys.LastOpenedSvgFile, string.Empty);
        if (string.IsNullOrWhiteSpace(storedPath) || !File.Exists(storedPath))
        {
            return;
        }

        await LoadSvgAsync(storedPath);
    }

    private async Task LoadSvgAsync(string path)
    {
        var document = await Task.Run(() => LoadSvgDocument(path), CancellationToken.None);
        if (document is null)
        {
            return;
        }

        var previous = globalState.SvgDocument;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            globalState.SvgDocument = document;
            globalState.SvgFilePath = path;
            Preferences.Default.Set(SettingsPreferenceKeys.LastOpenedSvgFile, path);
        });

        previous?.Dispose();
    }

    private static SvgDocument? LoadSvgDocument(string path)
    {
        using var stream = File.OpenRead(path);
        var svg = new SKSvg();
        svg.Load(stream);

        var picture = svg.Picture;
        if (picture is null)
        {
            return null;
        }

        var bounds = picture.CullRect;

        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return null;
        }

        return new SvgDocument(picture, bounds);
    }
}
