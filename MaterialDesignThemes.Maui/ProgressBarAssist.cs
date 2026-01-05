namespace MaterialDesignThemes.Maui;

public static class ProgressBarAssist
{
    private const string IndeterminateAnimationName = "MaterialDesignProgressIndeterminate";
    private const string ProgressAnimationName = "MaterialDesignProgressSmooth";

    public static readonly BindableProperty IsIndeterminateProperty =
        BindableProperty.CreateAttached(
            "IsIndeterminate",
            typeof(bool),
            typeof(ProgressBarAssist),
            false,
            propertyChanged: OnIsIndeterminateChanged);

    public static bool GetIsIndeterminate(BindableObject element) =>
        (bool)element.GetValue(IsIndeterminateProperty);

    public static void SetIsIndeterminate(BindableObject element, bool value) =>
        element.SetValue(IsIndeterminateProperty, value);

    public static readonly BindableProperty IndeterminateDurationProperty =
        BindableProperty.CreateAttached(
            "IndeterminateDuration",
            typeof(uint),
            typeof(ProgressBarAssist),
            (uint)900);

    public static uint GetIndeterminateDuration(BindableObject element) =>
        (uint)element.GetValue(IndeterminateDurationProperty);

    public static void SetIndeterminateDuration(BindableObject element, uint value) =>
        element.SetValue(IndeterminateDurationProperty, value);

    public static readonly BindableProperty AnimateProgressProperty =
        BindableProperty.CreateAttached(
            "AnimateProgress",
            typeof(bool),
            typeof(ProgressBarAssist),
            false,
            propertyChanged: OnAnimateProgressChanged);

    public static bool GetAnimateProgress(BindableObject element) =>
        (bool)element.GetValue(AnimateProgressProperty);

    public static void SetAnimateProgress(BindableObject element, bool value) =>
        element.SetValue(AnimateProgressProperty, value);

    public static readonly BindableProperty ProgressAnimationDurationProperty =
        BindableProperty.CreateAttached(
            "ProgressAnimationDuration",
            typeof(uint),
            typeof(ProgressBarAssist),
            (uint)180);

    public static uint GetProgressAnimationDuration(BindableObject element) =>
        (uint)element.GetValue(ProgressAnimationDurationProperty);

    public static void SetProgressAnimationDuration(BindableObject element, uint value) =>
        element.SetValue(ProgressAnimationDurationProperty, value);

    private static readonly BindablePropertyKey IsAnimatingProgressPropertyKey =
        BindableProperty.CreateAttachedReadOnly(
            "IsAnimatingProgress",
            typeof(bool),
            typeof(ProgressBarAssist),
            false);

    private static readonly BindableProperty IsAnimatingProgressProperty = IsAnimatingProgressPropertyKey.BindableProperty;

    private static bool GetIsAnimatingProgress(BindableObject element) =>
        (bool)element.GetValue(IsAnimatingProgressProperty);

    private static void SetIsAnimatingProgress(BindableObject element, bool value) =>
        element.SetValue(IsAnimatingProgressPropertyKey, value);

    private static void OnIsIndeterminateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ProgressBar progressBar)
        {
            return;
        }

        if ((bool)newValue)
        {
            StartIndeterminate(progressBar);
        }
        else
        {
            StopIndeterminate(progressBar);
        }
    }

    private static void OnAnimateProgressChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ProgressBar progressBar)
        {
            return;
        }

        if ((bool)newValue)
        {
            progressBar.PropertyChanged -= OnProgressPropertyChanged;
            progressBar.PropertyChanged += OnProgressPropertyChanged;
        }
        else
        {
            progressBar.PropertyChanged -= OnProgressPropertyChanged;
        }
    }

    private static void OnProgressPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not ProgressBar progressBar || e.PropertyName != ProgressBar.ProgressProperty.PropertyName)
        {
            return;
        }

        if (GetIsAnimatingProgress(progressBar))
        {
            return;
        }

        var duration = GetProgressAnimationDuration(progressBar);
        progressBar.AbortAnimation(ProgressAnimationName);

        _ = AnimateProgressAsync(progressBar, progressBar.Progress, duration);
    }

    private static async Task AnimateProgressAsync(ProgressBar progressBar, double target, uint duration)
    {
        try
        {
            SetIsAnimatingProgress(progressBar, true);
            var current = progressBar.Progress;
            if (Math.Abs(current - target) < 0.0001)
            {
                return;
            }

            await progressBar.ProgressTo(target, duration, Easing.CubicOut);
        }
        finally
        {
            SetIsAnimatingProgress(progressBar, false);
        }
    }

    private static void StartIndeterminate(ProgressBar progressBar)
    {
        progressBar.AbortAnimation(IndeterminateAnimationName);

        var duration = GetIndeterminateDuration(progressBar);
        var animation = new Animation(
            callback: value => progressBar.Progress = value,
            start: 0,
            end: 1,
            easing: Easing.Linear);

        animation.Commit(progressBar, IndeterminateAnimationName, 16, duration, Easing.Linear, finished: (value, isCompleted) =>
        {
            if (GetIsIndeterminate(progressBar))
            {
                StartIndeterminate(progressBar);
            }
        });
    }

    private static void StopIndeterminate(ProgressBar progressBar)
    {
        progressBar.AbortAnimation(IndeterminateAnimationName);
    }
}
