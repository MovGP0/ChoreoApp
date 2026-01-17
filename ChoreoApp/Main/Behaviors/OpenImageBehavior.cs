using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using ChoreoApp.Global;
using ChoreoApp.Main.Messages;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.Main.Behaviors;

public sealed class OpenImageBehavior(
    GlobalStateModel globalState,
    IPreferences preferences,
    IPublisher<OpenSvgFileCommand> publisher)
    : IBehavior<MainViewModel>
{
    public void Activate(MainViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.OpenImageCommand
            .SelectMany(_ => Observable.FromAsync(HandleOpenImageAsync))
            .Subscribe()
            .DisposeWith(disposables);
    }

    private async Task HandleOpenImageAsync()
    {
        if (globalState.SvgDocument is not null)
        {
            UnloadSvg();
            return;
        }

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Open SVG image",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.WinUI] = [".svg"],
                [DevicePlatform.MacCatalyst] = ["svg"],
                [DevicePlatform.iOS] = ["svg"],
                [DevicePlatform.Android] = ["image/svg+xml", "image/*", "*/*"],
            })
        });

        if (result is null)
        {
            return;
        }

        if (!string.Equals(Path.GetExtension(result.FileName), ".svg", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Unsupported file type: {result.FileName}");
        }

        var path = result.FullPath;
        if (string.IsNullOrWhiteSpace(path))
        {
            await using var pickedStream = await result.OpenReadAsync();
            var tempPath = Path.Combine(FileSystem.CacheDirectory, $"{Path.GetFileNameWithoutExtension(result.FileName)}.svg");
            await using var tempFile = File.Open(tempPath, FileMode.Create, FileAccess.Write);
            await pickedStream.CopyToAsync(tempFile);
            path = tempPath;
        }

        publisher.Publish(new OpenSvgFileCommand(path));
    }

    private void UnloadSvg()
    {
        var previous = globalState.SvgDocument;
        MainThread.BeginInvokeOnMainThread(() =>
        {
            globalState.SvgDocument = null;
            globalState.SvgFilePath = null;
            preferences.Remove(SettingsPreferenceKeys.LastOpenedSvgFile);
        });

        previous?.Dispose();
    }
}
