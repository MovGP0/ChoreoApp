using System.Globalization;
using ChoreoApp.Dancers.Messages;
using ChoreoApp.i18n;
using ChoreoApp.Models;
using MessagePipe;

namespace ChoreoApp.Dancers;

public sealed partial class SwapDancersDialogViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IPublisher<CloseDancerDialogCommand> _closeDialogPublisher;
    private readonly DancerModel _firstDancer;
    private readonly DancerModel _secondDancer;
    private readonly IHapticFeedback _hapticFeedback;

    public SwapDancersDialogViewModel(
        IPublisher<CloseDancerDialogCommand> closeDialogPublisher,
        IHapticFeedback hapticFeedback,
        DancerModel firstDancer,
        DancerModel secondDancer)
    {
        _closeDialogPublisher = closeDialogPublisher;
        _hapticFeedback = hapticFeedback;
        _firstDancer = firstDancer;
        _secondDancer = secondDancer;

        FirstDancerName = GetDisplayName(firstDancer);
        SecondDancerName = GetDisplayName(secondDancer);
        FirstDancerColor = firstDancer.Color;
        SecondDancerColor = secondDancer.Color;
        Message = string.Format(
            CultureInfo.CurrentUICulture,
            Translations.DancerSwapDialogMessage,
            FirstDancerName,
            SecondDancerName);
    }

    public ViewModelActivator Activator { get; } = new();

    [Reactive]
    private string _message = string.Empty;

    [Reactive]
    private string _firstDancerName = string.Empty;

    [Reactive]
    private string _secondDancerName = string.Empty;

    [Reactive]
    private Color _firstDancerColor = Colors.Transparent;

    [Reactive]
    private Color _secondDancerColor = Colors.Transparent;

    [ReactiveCommand]
    private void ConfirmSwap()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        SwapProperties(_firstDancer, _secondDancer);
        CloseDialog();
    }

    [ReactiveCommand]
    private void Cancel()
    {
        if (_hapticFeedback.IsSupported)
        {
            _hapticFeedback.Perform(HapticFeedbackType.Click);
        }

        CloseDialog();
    }

    private void CloseDialog()
    {
        _closeDialogPublisher.Publish(new CloseDancerDialogCommand());
    }

    private static void SwapProperties(DancerModel first, DancerModel second)
    {
        var firstRole = first.Role;
        var firstName = first.Name;
        var firstShortcut = first.Shortcut;
        var firstColor = first.Color;
        var firstIcon = first.Icon;

        first.Role = second.Role;
        first.Name = second.Name;
        first.Shortcut = second.Shortcut;
        first.Color = second.Color;
        first.Icon = second.Icon;

        second.Role = firstRole;
        second.Name = firstName;
        second.Shortcut = firstShortcut;
        second.Color = firstColor;
        second.Icon = firstIcon;
    }

    private static string GetDisplayName(DancerModel dancer)
    {
        if (!string.IsNullOrWhiteSpace(dancer.Name))
        {
            return dancer.Name;
        }

        if (!string.IsNullOrWhiteSpace(dancer.Shortcut))
        {
            return dancer.Shortcut;
        }

        return $"#{dancer.DancerId.Value}";
    }
}
