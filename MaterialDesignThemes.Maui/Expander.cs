using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace MaterialDesignThemes.Maui;

public enum ExpandDirection
{
    Down,
    Up
}

public sealed class ExpandedChangedEventArgs(bool isExpanded) : EventArgs
{
    public bool IsExpanded { get; } = isExpanded;
}

[ContentProperty(nameof(Content))]
[RequiresUnreferencedCode("Calls Microsoft.Maui.Controls.Binding.Binding(String, BindingMode, IValueConverter, Object, String, Object)")]
public partial class Expander : ContentView
{
    private readonly WeakEventManager _expandedChangedEventManager = new();
    private const string ExpandAnimationName = "MaterialDesignExpanderExpand";

    public Expander()
    {
        HandleHeaderTapped = ResizeExpanderInItemsView;
        HeaderTapGestureRecognizer.Tapped += OnHeaderTapGestureRecognizerTapped;
        base.Content = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            }
        };
    }

    public event EventHandler<ExpandedChangedEventArgs> ExpandedChanged
    {
        add => _expandedChangedEventManager.AddEventHandler(value);
        remove => _expandedChangedEventManager.RemoveEventHandler(value);
    }

    public static readonly BindableProperty DirectionProperty = BindableProperty.Create(
        nameof(Direction),
        typeof(ExpandDirection),
        typeof(Expander),
        ExpandDirection.Down,
        propertyChanging: OnExpandDirectionChanging,
        propertyChanged: OnDirectionPropertyChanged);

    public ExpandDirection Direction
    {
        get => (ExpandDirection)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(Expander));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(Expander));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded),
        typeof(bool),
        typeof(Expander),
        false,
        propertyChanged: OnIsExpandedPropertyChanged);

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public static readonly BindableProperty AnimationDurationProperty = BindableProperty.Create(
        nameof(AnimationDuration),
        typeof(uint),
        typeof(Expander),
        (uint)180);

    public uint AnimationDuration
    {
        get => (uint)GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create(
        nameof(AnimationEasing),
        typeof(Easing),
        typeof(Expander),
        Easing.CubicOut);

    public Easing AnimationEasing
    {
        get => (Easing)GetValue(AnimationEasingProperty);
        set => SetValue(AnimationEasingProperty, value);
    }

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(Expander),
        propertyChanged: OnContentPropertyChanged);

    public new View? Content
    {
        get => (View?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
        nameof(Header),
        typeof(View),
        typeof(Expander),
        propertyChanged: OnHeaderPropertyChanged);

    public View? Header
    {
        get => (View?)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public Action<TappedEventArgs>? HandleHeaderTapped { get; set; }

    internal TapGestureRecognizer HeaderTapGestureRecognizer { get; } = new();

    private Grid ContentGrid => (Grid)base.Content;

    private static void OnExpandDirectionChanging(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is not ExpandDirection enumValue || !Enum.IsDefined(enumValue))
        {
            throw new InvalidEnumArgumentException(nameof(newValue), newValue is int intValue ? intValue : -1, typeof(ExpandDirection));
        }

        ((Expander)bindable).Direction = enumValue;
    }

    private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        if (newValue is View newView)
        {
            if (oldValue is IView oldView)
            {
                expander.ContentGrid.Remove(oldView);
            }
            expander.ContentGrid.Add(newView);
            expander.ContentGrid.SetRow(newView, expander.Direction switch
            {
                ExpandDirection.Down => 1,
                ExpandDirection.Up => 0,
                _ => throw new NotSupportedException($"{nameof(ExpandDirection)} {expander.Direction} is not yet supported")
            });
            expander.UpdateContentVisualState(expander.IsExpanded);
        }
    }

    private static void OnHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        if (newValue is View newView)
        {
            expander.SetHeaderGestures(newView);
            if (oldValue is IView oldView)
            {
                expander.ContentGrid.Remove(oldView);
            }
            expander.ContentGrid.Add(newView);
            expander.ContentGrid.SetRow(newView, expander.Direction switch
            {
                ExpandDirection.Down => 0,
                ExpandDirection.Up => 1,
                _ => throw new NotSupportedException($"{nameof(ExpandDirection)} {expander.Direction} is not yet supported")
            });
        }
    }

    private static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        _ = expander.AnimateContentAsync((bool)newValue);
        expander.OnExpandedChanged((bool)newValue);
    }

    private static void OnDirectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((Expander)bindable).HandleDirectionChanged((ExpandDirection)newValue);

    private void HandleDirectionChanged(ExpandDirection expandDirection)
    {
        if (Header is null || Content is null)
        {
            return;
        }

        switch (expandDirection)
        {
            case ExpandDirection.Down:
                ContentGrid.SetRow(Header, 0);
                ContentGrid.SetRow(Content, 1);
                Content.AnchorY = 0;
                break;
            case ExpandDirection.Up:
                ContentGrid.SetRow(Header, 1);
                ContentGrid.SetRow(Content, 0);
                Content.AnchorY = 1;
                break;
            default:
                throw new NotSupportedException($"{nameof(ExpandDirection)} {expandDirection} is not yet supported");
        }
    }

    private void SetHeaderGestures(in View header)
    {
        header.GestureRecognizers.Remove(HeaderTapGestureRecognizer);
        header.GestureRecognizers.Add(HeaderTapGestureRecognizer);
    }

    private void OnHeaderTapGestureRecognizerTapped(object? sender, TappedEventArgs tappedEventArgs)
    {
        IsExpanded = !IsExpanded;
        HandleHeaderTapped?.Invoke(tappedEventArgs);
    }

    private void ResizeExpanderInItemsView(TappedEventArgs tappedEventArgs)
    {
        if (Header is null)
        {
            return;
        }

        Element? element = this;
#if WINDOWS
        var size = IsExpanded
            ? Measure(double.PositiveInfinity, double.PositiveInfinity)
            : Header.Measure(double.PositiveInfinity, double.PositiveInfinity);
#endif
        while (element is not null)
        {
#if IOS || MACCATALYST
            if (element is ListView listView)
            {
                (listView.Handler?.PlatformView as UIKit.UITableView)?.ReloadData();
            }
#endif
#if WINDOWS
            if (element.Parent is ListView listView && element is Cell cell)
            {
                cell.ForceUpdateSize();
            }
            else if (element is CollectionView collectionView)
            {
                var tapLocation = tappedEventArgs.GetPosition(collectionView);
                if (tapLocation is { } tapPosition)
                {
                    ForceUpdateCellSize(collectionView, size, tapPosition);
                }
            }
#endif
            element = element.Parent;
        }
    }

    private void OnExpandedChanged(bool isExpanded)
    {
        if (Command?.CanExecute(CommandParameter) is true)
        {
            Command.Execute(CommandParameter);
        }

        _expandedChangedEventManager.HandleEvent(this, new ExpandedChangedEventArgs(isExpanded), nameof(ExpandedChanged));
    }

    private void UpdateContentVisualState(bool isExpanded)
    {
        if (Content is null)
        {
            return;
        }

        Content.AbortAnimation(ExpandAnimationName);
        Content.AnchorY = Direction == ExpandDirection.Down ? 0 : 1;
        Content.IsVisible = isExpanded;
        Content.Opacity = isExpanded ? 1 : 0;
        Content.ScaleY = isExpanded ? 1 : 0;
    }

    private async Task AnimateContentAsync(bool isExpanded)
    {
        if (Content is null)
        {
            return;
        }

        Content.AbortAnimation(ExpandAnimationName);
        Content.AnchorY = Direction == ExpandDirection.Down ? 0 : 1;

        var duration = AnimationDuration;
        var easing = AnimationEasing;

        if (isExpanded)
        {
            Content.IsVisible = true;
            Content.Opacity = 0;
            Content.ScaleY = 0;

            var opacityTask = Content.FadeTo(1, duration, easing);
            var scaleTask = Content.ScaleYTo(1, duration, easing);
            await Task.WhenAll(opacityTask, scaleTask);
        }
        else
        {
            if (!Content.IsVisible)
            {
                return;
            }

            var opacityTask = Content.FadeTo(0, duration, easing);
            var scaleTask = Content.ScaleYTo(0, duration, easing);
            await Task.WhenAll(opacityTask, scaleTask);

            Content.IsVisible = false;
        }
    }
}
