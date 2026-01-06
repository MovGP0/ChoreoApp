using System.Globalization;
using System.Windows.Input;

namespace MaterialDesignThemes.Maui;

public class Flipper : TemplatedView
{
    public const string TemplateFlipGroupName = "FlipStates";
    public const string TemplateFlippedStateName = "Flipped";
    public const string TemplateUnflippedStateName = "Unflipped";
    public const string FrontContentPartName = "PART_FrontContent";
    public const string BackContentPartName = "PART_BackContent";
    private const uint FlipAnimationDuration = 200;
    private const string FlipFrontAnimationName = "MaterialDesignFlipperFrontScaleX";
    private const string FlipBackAnimationName = "MaterialDesignFlipperBackScaleX";

    private static readonly BindablePropertyKey FlipCommandPropertyKey = BindableProperty.CreateReadOnly(
        nameof(FlipCommand),
        typeof(ICommand),
        typeof(Flipper),
        null);

    public static readonly BindableProperty FlipCommandProperty = FlipCommandPropertyKey.BindableProperty;

    public ICommand FlipCommand => (ICommand)GetValue(FlipCommandProperty);

    public static readonly BindableProperty FrontContentProperty = BindableProperty.Create(
        nameof(FrontContent),
        typeof(object),
        typeof(Flipper),
        propertyChanged: OnFrontContentChanged);

    public object? FrontContent
    {
        get => GetValue(FrontContentProperty);
        set => SetValue(FrontContentProperty, value);
    }

    public static readonly BindableProperty FrontContentTemplateProperty = BindableProperty.Create(
        nameof(FrontContentTemplate),
        typeof(DataTemplate),
        typeof(Flipper),
        propertyChanged: OnFrontContentChanged);

    public DataTemplate? FrontContentTemplate
    {
        get => (DataTemplate?)GetValue(FrontContentTemplateProperty);
        set => SetValue(FrontContentTemplateProperty, value);
    }

    public static readonly BindableProperty FrontContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(FrontContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(Flipper),
        propertyChanged: OnFrontContentChanged);

    public DataTemplateSelector? FrontContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(FrontContentTemplateSelectorProperty);
        set => SetValue(FrontContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty FrontContentStringFormatProperty = BindableProperty.Create(
        nameof(FrontContentStringFormat),
        typeof(string),
        typeof(Flipper),
        propertyChanged: OnFrontContentChanged);

    public string? FrontContentStringFormat
    {
        get => (string?)GetValue(FrontContentStringFormatProperty);
        set => SetValue(FrontContentStringFormatProperty, value);
    }

    public static readonly BindableProperty BackContentProperty = BindableProperty.Create(
        nameof(BackContent),
        typeof(object),
        typeof(Flipper),
        propertyChanged: OnBackContentChanged);

    public object? BackContent
    {
        get => GetValue(BackContentProperty);
        set => SetValue(BackContentProperty, value);
    }

    public static readonly BindableProperty BackContentTemplateProperty = BindableProperty.Create(
        nameof(BackContentTemplate),
        typeof(DataTemplate),
        typeof(Flipper),
        propertyChanged: OnBackContentChanged);

    public DataTemplate? BackContentTemplate
    {
        get => (DataTemplate?)GetValue(BackContentTemplateProperty);
        set => SetValue(BackContentTemplateProperty, value);
    }

    public static readonly BindableProperty BackContentTemplateSelectorProperty = BindableProperty.Create(
        nameof(BackContentTemplateSelector),
        typeof(DataTemplateSelector),
        typeof(Flipper),
        propertyChanged: OnBackContentChanged);

    public DataTemplateSelector? BackContentTemplateSelector
    {
        get => (DataTemplateSelector?)GetValue(BackContentTemplateSelectorProperty);
        set => SetValue(BackContentTemplateSelectorProperty, value);
    }

    public static readonly BindableProperty BackContentStringFormatProperty = BindableProperty.Create(
        nameof(BackContentStringFormat),
        typeof(string),
        typeof(Flipper),
        propertyChanged: OnBackContentChanged);

    public string? BackContentStringFormat
    {
        get => (string?)GetValue(BackContentStringFormatProperty);
        set => SetValue(BackContentStringFormatProperty, value);
    }

    public static readonly BindableProperty IsFlippedProperty = BindableProperty.Create(
        nameof(IsFlipped),
        typeof(bool),
        typeof(Flipper),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsFlippedChanged);

    private ContentView? _frontContentHost;
    private ContentView? _backContentHost;
    private int _flipAnimationToken;

    public Flipper()
    {
        SetValue(FlipCommandPropertyKey, new Command(OnFlipRequested));
    }

    public bool IsFlipped
    {
        get => (bool)GetValue(IsFlippedProperty);
        set => SetValue(IsFlippedProperty, value);
    }

    public event EventHandler<ValueChangedEventArgs<bool>>? IsFlippedChanged;
    protected ContentView? FrontContentHost => _frontContentHost;
    protected ContentView? BackContentHost => _backContentHost;

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _frontContentHost = GetTemplateChild(FrontContentPartName) as ContentView;
        _backContentHost = GetTemplateChild(BackContentPartName) as ContentView;

        UpdateFrontContent();
        UpdateBackContent();
        UpdateVisualStates();
    }

    private static void OnFrontContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Flipper flipper)
        {
            flipper.UpdateFrontContent();
        }
    }

    private static void OnBackContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Flipper flipper)
        {
            flipper.UpdateBackContent();
        }
    }

    private static void OnIsFlippedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Flipper flipper)
        {
            flipper.UpdateVisualStates();
            flipper.IsFlippedChanged?.Invoke(
                flipper,
                new ValueChangedEventArgs<bool>((bool)oldValue, (bool)newValue));
        }
    }

    private void UpdateFrontContent()
    {
        if (_frontContentHost is null)
        {
            return;
        }

        _frontContentHost.Content = CreateContentView(
            FrontContent,
            FrontContentTemplate,
            FrontContentTemplateSelector,
            FrontContentStringFormat);
    }

    private void UpdateBackContent()
    {
        if (_backContentHost is null)
        {
            return;
        }

        _backContentHost.Content = CreateContentView(
            BackContent,
            BackContentTemplate,
            BackContentTemplateSelector,
            BackContentStringFormat);
    }

    private void UpdateVisualStates()
    {
        VisualStateManager.GoToState(
            this,
            IsFlipped ? TemplateFlippedStateName : TemplateUnflippedStateName);
        _ = AnimateFlipAsync(IsFlipped);
    }

    protected virtual async Task AnimateFlipAsync(bool isFlipped)
    {
        var front = _frontContentHost;
        var back = _backContentHost;
        if (front is null || back is null)
        {
            return;
        }

        var token = ++_flipAnimationToken;
        front.AbortAnimation(FlipFrontAnimationName);
        back.AbortAnimation(FlipBackAnimationName);

        front.IsVisible = true;
        back.IsVisible = true;

        if (isFlipped)
        {
            front.ScaleX = 1;
            back.ScaleX = 0;

            await AnimateScaleX(front, 1, 0, FlipAnimationDuration / 2, FlipFrontAnimationName);
            if (token != _flipAnimationToken)
            {
                return;
            }

            front.IsVisible = false;
            await AnimateScaleX(back, 0, 1, FlipAnimationDuration / 2, FlipBackAnimationName);
        }
        else
        {
            back.ScaleX = 1;
            front.ScaleX = 0;

            await AnimateScaleX(back, 1, 0, FlipAnimationDuration / 2, FlipBackAnimationName);
            if (token != _flipAnimationToken)
            {
                return;
            }

            back.IsVisible = false;
            await AnimateScaleX(front, 0, 1, FlipAnimationDuration / 2, FlipFrontAnimationName);
        }
    }

    private static Task AnimateScaleX(VisualElement element, double from, double to, uint duration, string animationName)
    {
        var tcs = new TaskCompletionSource();
        element.ScaleX = from;

        var animation = new Animation(
            callback: value => element.ScaleX = value,
            start: from,
            end: to,
            easing: Easing.Linear);

        animation.Commit(element, animationName, 16, duration, Easing.Linear, (_, _) =>
        {
            element.ScaleX = to;
            tcs.TrySetResult();
        });

        return tcs.Task;
    }

    private void OnFlipRequested()
    {
        IsFlipped = !IsFlipped;
    }

    private static View? CreateContentView(
        object? content,
        DataTemplate? template,
        DataTemplateSelector? templateSelector,
        string? stringFormat)
    {
        if (content is null)
        {
            return null;
        }

        var resolvedTemplate = templateSelector?.SelectTemplate(content, null) ?? template;
        if (resolvedTemplate is not null)
        {
            var created = resolvedTemplate.CreateContent();
            if (created is View view)
            {
                view.BindingContext = content;
                return view;
            }

            if (created is ViewCell cell)
            {
                cell.BindingContext = content;
                return cell.View;
            }
        }

        if (content is View contentView)
        {
            return contentView;
        }

        if (!string.IsNullOrWhiteSpace(stringFormat))
        {
            return new Label
            {
                Text = string.Format(CultureInfo.CurrentCulture, stringFormat, content)
            };
        }

        return new Label
        {
            Text = Convert.ToString(content, CultureInfo.CurrentCulture) ?? string.Empty
        };
    }
}
