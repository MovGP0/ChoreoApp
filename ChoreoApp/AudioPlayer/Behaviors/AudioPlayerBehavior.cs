using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Plugin.Maui.Audio;

namespace ChoreoApp.AudioPlayer.Behaviors;

public sealed class AudioPlayerBehavior(IAudioManager audioManager) : IBehavior<AudioPlayerViewModel>
{
    public void Activate(AudioPlayerViewModel viewModel, CompositeDisposable disposables)
    {
        var playerDisposable = new SerialDisposable().DisposeWith(disposables);
        var positionDisposable = new SerialDisposable().DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.StreamFactory)
            .Where(factory => factory is not null)
            .SelectMany(async factory =>
            {
                positionDisposable.Disposable?.Dispose();
                playerDisposable.Disposable?.Dispose();

                var stream = await factory!();
                var player = audioManager.CreatePlayer(stream);

                return player;
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(player =>
            {
                playerDisposable.Disposable = player;
                viewModel.Player = player;

                SyncCapabilities(viewModel, player);
                SyncParameters(viewModel, player);

                Observable
                    .FromEventPattern(h => player.PlaybackEnded += h, h => player.PlaybackEnded -= h)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                        viewModel.IsPlaying = false;
                        viewModel.Position = 0d;
                    })
                    .DisposeWith(disposables);

                positionDisposable.Disposable = Observable
                    .Interval(TimeSpan.FromMilliseconds(200))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                        viewModel.Duration = player.Duration;

                        if (!player.IsPlaying)
                        {
                            return;
                        }

                        viewModel.Position = player.CurrentPosition;
                    });
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Speed)
            .Skip(1)
            .Subscribe(speed =>
            {
                if (viewModel.Player is { CanSetSpeed: true } player)
                {
                    player.Speed = speed;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Volume)
            .Skip(1)
            .Subscribe(volume =>
            {
                if (viewModel.Player is { } player)
                {
                    player.Volume = volume;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Balance)
            .Skip(1)
            .Subscribe(balance =>
            {
                if (viewModel.Player is { } player)
                {
                    player.Balance = balance;
                }
            })
            .DisposeWith(disposables);

        viewModel
            .WhenAnyValue(vm => vm.Loop)
            .Skip(1)
            .Subscribe(loop =>
            {
                if (viewModel.Player is { } player)
                {
                    player.Loop = loop;
                }
            })
            .DisposeWith(disposables);
    }

    private static void SyncCapabilities(AudioPlayerViewModel viewModel, IAudioPlayer player)
    {
        viewModel.CanSeek = player.CanSeek;
        viewModel.CanSetSpeed = player.CanSetSpeed;
        viewModel.Duration = player.Duration;
    }

    private static void SyncParameters(AudioPlayerViewModel viewModel, IAudioPlayer player)
    {
        player.Speed = viewModel.Speed;
        player.Volume = viewModel.Volume;
        player.Balance = viewModel.Balance;
        player.Loop = viewModel.Loop;
    }
}
