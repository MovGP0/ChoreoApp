namespace MaterialDesignThemes.Maui;

public enum ClockDisplayMode
{
    Hours,
    Minutes,
    Seconds
}

public enum ClockDisplayAutomation
{
    None,
    Cycle,
    ToMinutesOnly,
    ToSeconds,
    CycleWithSeconds
}

public sealed class Clock : TemplatedView
{
    public const string HoursCanvasPartName = "PART_HoursCanvas";
    public const string MinutesCanvasPartName = "PART_MinutesCanvas";
    public const string SecondsCanvasPartName = "PART_SecondsCanvas";
    public const string HourReadOutPartName = "PART_HourReadOut";
    public const string MinuteReadOutPartName = "PART_MinuteReadOut";
    public const string SecondReadOutPartName = "PART_SecondReadOut";
    public const string HourLinePartName = "PART_HourLine";
    public const string MinuteLinePartName = "PART_MinuteLine";
    public const string SecondLinePartName = "PART_SecondLine";

    public const string HoursVisualStateName = "Hours";
    public const string MinutesVisualStateName = "Minutes";
    public const string SecondsVisualStateName = "Seconds";

    private AbsoluteLayout? _hoursCanvas;
    private AbsoluteLayout? _minutesCanvas;
    private AbsoluteLayout? _secondsCanvas;
    private View? _hourReadOutPart;
    private View? _minuteReadOutPart;
    private View? _secondReadOutPart;
    private View? _hourLine;
    private View? _minuteLine;
    private View? _secondLine;

    private Point _centreCanvas = new(0, 0);
    private Point _currentStartPosition = new(0, 0);
    private bool _suppressMeridiemUpdate;

    public event EventHandler<TimeChangedEventArgs>? TimeChanged;
    public event EventHandler<ClockChoiceMadeEventArgs>? ClockChoiceMade;

    public static readonly BindableProperty TimeProperty = BindableProperty.Create(
        nameof(Time),
        typeof(DateTime),
        typeof(Clock),
        default(DateTime),
        BindingMode.TwoWay,
        propertyChanged: OnTimeChanged);

    public DateTime Time
    {
        get => (DateTime)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public static readonly BindableProperty IsPostMeridiemProperty = BindableProperty.Create(
        nameof(IsPostMeridiem),
        typeof(bool),
        typeof(Clock),
        false,
        propertyChanged: OnIsPostMeridiemChanged);

    public bool IsPostMeridiem
    {
        get => (bool)GetValue(IsPostMeridiemProperty);
        set => SetValue(IsPostMeridiemProperty, value);
    }

    public static readonly BindableProperty Is24HoursProperty = BindableProperty.Create(
        nameof(Is24Hours),
        typeof(bool),
        typeof(Clock),
        false,
        propertyChanged: OnIs24HoursChanged);

    public bool Is24Hours
    {
        get => (bool)GetValue(Is24HoursProperty);
        set => SetValue(Is24HoursProperty, value);
    }

    public static readonly BindableProperty DisplayModeProperty = BindableProperty.Create(
        nameof(DisplayMode),
        typeof(ClockDisplayMode),
        typeof(Clock),
        ClockDisplayMode.Hours,
        propertyChanged: OnDisplayModeChanged);

    public ClockDisplayMode DisplayMode
    {
        get => (ClockDisplayMode)GetValue(DisplayModeProperty);
        set => SetValue(DisplayModeProperty, value);
    }

    public static readonly BindableProperty DisplayAutomationProperty = BindableProperty.Create(
        nameof(DisplayAutomation),
        typeof(ClockDisplayAutomation),
        typeof(Clock),
        ClockDisplayAutomation.None,
        propertyChanged: OnDisplayAutomationChanged);

    public ClockDisplayAutomation DisplayAutomation
    {
        get => (ClockDisplayAutomation)GetValue(DisplayAutomationProperty);
        set => SetValue(DisplayAutomationProperty, value);
    }

    public static readonly BindableProperty ButtonStyleProperty = BindableProperty.Create(
        nameof(ButtonStyle),
        typeof(Style),
        typeof(Clock));

    public Style? ButtonStyle
    {
        get => (Style?)GetValue(ButtonStyleProperty);
        set => SetValue(ButtonStyleProperty, value);
    }

    public static readonly BindableProperty LesserButtonStyleProperty = BindableProperty.Create(
        nameof(LesserButtonStyle),
        typeof(Style),
        typeof(Clock));

    public Style? LesserButtonStyle
    {
        get => (Style?)GetValue(LesserButtonStyleProperty);
        set => SetValue(LesserButtonStyleProperty, value);
    }

    public static readonly BindableProperty BorderBrushProperty = BindableProperty.Create(
        nameof(BorderBrush),
        typeof(Brush),
        typeof(Clock),
        default(Brush));

    public Brush? BorderBrush
    {
        get => (Brush?)GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(
        nameof(BorderThickness),
        typeof(Thickness),
        typeof(Clock),
        new Thickness(0));

    public Thickness BorderThickness
    {
        get => (Thickness)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly BindableProperty ButtonRadiusRatioProperty = BindableProperty.Create(
        nameof(ButtonRadiusRatio),
        typeof(double),
        typeof(Clock),
        0.835d,
        propertyChanged: OnLayoutPropertyChanged);

    public double ButtonRadiusRatio
    {
        get => (double)GetValue(ButtonRadiusRatioProperty);
        set => SetValue(ButtonRadiusRatioProperty, value);
    }

    public static readonly BindableProperty ButtonRadiusInnerRatioProperty = BindableProperty.Create(
        nameof(ButtonRadiusInnerRatio),
        typeof(double),
        typeof(Clock),
        0.6d,
        propertyChanged: OnLayoutPropertyChanged);

    public double ButtonRadiusInnerRatio
    {
        get => (double)GetValue(ButtonRadiusInnerRatioProperty);
        set => SetValue(ButtonRadiusInnerRatioProperty, value);
    }

    public static readonly BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(
        nameof(IsHeaderVisible),
        typeof(bool),
        typeof(Clock),
        true);

    public bool IsHeaderVisible
    {
        get => (bool)GetValue(IsHeaderVisibleProperty);
        set => SetValue(IsHeaderVisibleProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Clock),
        new CornerRadius(2));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private static readonly BindablePropertyKey IsMidnightHourPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsMidnightHour),
        typeof(bool),
        typeof(Clock),
        false);

    public static readonly BindableProperty IsMidnightHourProperty = IsMidnightHourPropertyKey.BindableProperty;

    public bool IsMidnightHour
    {
        get => (bool)GetValue(IsMidnightHourProperty);
        private set => SetValue(IsMidnightHourPropertyKey, value);
    }

    private static readonly BindablePropertyKey IsMiddayHourPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsMiddayHour),
        typeof(bool),
        typeof(Clock),
        false);

    public static readonly BindableProperty IsMiddayHourProperty = IsMiddayHourPropertyKey.BindableProperty;

    public bool IsMiddayHour
    {
        get => (bool)GetValue(IsMiddayHourProperty);
        private set => SetValue(IsMiddayHourPropertyKey, value);
    }

    private static readonly BindablePropertyKey HourLineAnglePropertyKey = BindableProperty.CreateReadOnly(
        nameof(HourLineAngle),
        typeof(double),
        typeof(Clock),
        0d);

    public static readonly BindableProperty HourLineAngleProperty = HourLineAnglePropertyKey.BindableProperty;

    public double HourLineAngle
    {
        get => (double)GetValue(HourLineAngleProperty);
        private set => SetValue(HourLineAnglePropertyKey, value);
    }

    private static readonly BindablePropertyKey MinuteLineAnglePropertyKey = BindableProperty.CreateReadOnly(
        nameof(MinuteLineAngle),
        typeof(double),
        typeof(Clock),
        0d);

    public static readonly BindableProperty MinuteLineAngleProperty = MinuteLineAnglePropertyKey.BindableProperty;

    public double MinuteLineAngle
    {
        get => (double)GetValue(MinuteLineAngleProperty);
        private set => SetValue(MinuteLineAnglePropertyKey, value);
    }

    private static readonly BindablePropertyKey SecondLineAnglePropertyKey = BindableProperty.CreateReadOnly(
        nameof(SecondLineAngle),
        typeof(double),
        typeof(Clock),
        0d);

    public static readonly BindableProperty SecondLineAngleProperty = SecondLineAnglePropertyKey.BindableProperty;

    public double SecondLineAngle
    {
        get => (double)GetValue(SecondLineAngleProperty);
        private set => SetValue(SecondLineAnglePropertyKey, value);
    }

    private static readonly BindablePropertyKey HourTextPropertyKey = BindableProperty.CreateReadOnly(
        nameof(HourText),
        typeof(string),
        typeof(Clock),
        "00");

    public static readonly BindableProperty HourTextProperty = HourTextPropertyKey.BindableProperty;

    public string HourText
    {
        get => (string)GetValue(HourTextProperty);
        private set => SetValue(HourTextPropertyKey, value);
    }

    private static readonly BindablePropertyKey HourTextOpacityPropertyKey = BindableProperty.CreateReadOnly(
        nameof(HourTextOpacity),
        typeof(double),
        typeof(Clock),
        0.56d);

    public static readonly BindableProperty HourTextOpacityProperty = HourTextOpacityPropertyKey.BindableProperty;

    public double HourTextOpacity
    {
        get => (double)GetValue(HourTextOpacityProperty);
        private set => SetValue(HourTextOpacityPropertyKey, value);
    }

    private static readonly BindablePropertyKey MinuteTextPropertyKey = BindableProperty.CreateReadOnly(
        nameof(MinuteText),
        typeof(string),
        typeof(Clock),
        "00");

    public static readonly BindableProperty MinuteTextProperty = MinuteTextPropertyKey.BindableProperty;

    public string MinuteText
    {
        get => (string)GetValue(MinuteTextProperty);
        private set => SetValue(MinuteTextPropertyKey, value);
    }

    private static readonly BindablePropertyKey MinuteTextOpacityPropertyKey = BindableProperty.CreateReadOnly(
        nameof(MinuteTextOpacity),
        typeof(double),
        typeof(Clock),
        0.56d);

    public static readonly BindableProperty MinuteTextOpacityProperty = MinuteTextOpacityPropertyKey.BindableProperty;

    public double MinuteTextOpacity
    {
        get => (double)GetValue(MinuteTextOpacityProperty);
        private set => SetValue(MinuteTextOpacityPropertyKey, value);
    }

    private static readonly BindablePropertyKey SecondTextPropertyKey = BindableProperty.CreateReadOnly(
        nameof(SecondText),
        typeof(string),
        typeof(Clock),
        "00");

    public static readonly BindableProperty SecondTextProperty = SecondTextPropertyKey.BindableProperty;

    public string SecondText
    {
        get => (string)GetValue(SecondTextProperty);
        private set => SetValue(SecondTextPropertyKey, value);
    }

    private static readonly BindablePropertyKey SecondTextOpacityPropertyKey = BindableProperty.CreateReadOnly(
        nameof(SecondTextOpacity),
        typeof(double),
        typeof(Clock),
        0.56d);

    public static readonly BindableProperty SecondTextOpacityProperty = SecondTextOpacityPropertyKey.BindableProperty;

    public double SecondTextOpacity
    {
        get => (double)GetValue(SecondTextOpacityProperty);
        private set => SetValue(SecondTextOpacityPropertyKey, value);
    }

    private static readonly BindablePropertyKey AmPmTextPropertyKey = BindableProperty.CreateReadOnly(
        nameof(AmPmText),
        typeof(string),
        typeof(Clock),
        "AM");

    public static readonly BindableProperty AmPmTextProperty = AmPmTextPropertyKey.BindableProperty;

    public string AmPmText
    {
        get => (string)GetValue(AmPmTextProperty);
        private set => SetValue(AmPmTextPropertyKey, value);
    }

    private static readonly BindablePropertyKey IsSecondsDisplayedPropertyKey = BindableProperty.CreateReadOnly(
        nameof(IsSecondsDisplayed),
        typeof(bool),
        typeof(Clock),
        false);

    public static readonly BindableProperty IsSecondsDisplayedProperty = IsSecondsDisplayedPropertyKey.BindableProperty;

    public bool IsSecondsDisplayed
    {
        get => (bool)GetValue(IsSecondsDisplayedProperty);
        private set => SetValue(IsSecondsDisplayedPropertyKey, value);
    }

    public Clock()
    {
        UpdateReadout();
        UpdateLineAngles();
        UpdateDisplayModeStates();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        DetachReadoutHandlers();

        _hoursCanvas = GetTemplateChild(HoursCanvasPartName) as AbsoluteLayout;
        _minutesCanvas = GetTemplateChild(MinutesCanvasPartName) as AbsoluteLayout;
        _secondsCanvas = GetTemplateChild(SecondsCanvasPartName) as AbsoluteLayout;
        _hourReadOutPart = GetTemplateChild(HourReadOutPartName) as View;
        _minuteReadOutPart = GetTemplateChild(MinuteReadOutPartName) as View;
        _secondReadOutPart = GetTemplateChild(SecondReadOutPartName) as View;
        _hourLine = GetTemplateChild(HourLinePartName) as View;
        _minuteLine = GetTemplateChild(MinuteLinePartName) as View;
        _secondLine = GetTemplateChild(SecondLinePartName) as View;

        AttachReadoutHandlers();
        AttachCanvasHandlers(_hoursCanvas);
        AttachCanvasHandlers(_minutesCanvas);
        AttachCanvasHandlers(_secondsCanvas);

        GenerateButtons();
        UpdateButtonStates();
        UpdateLineAngles();
        UpdateDisplayModeStates();
        GotoVisualState(false);
    }

    private void AttachCanvasHandlers(AbsoluteLayout? canvas)
    {
        if (canvas is null)
        {
            return;
        }

        canvas.SizeChanged -= CanvasOnSizeChanged;
        canvas.SizeChanged += CanvasOnSizeChanged;
    }

    private void CanvasOnSizeChanged(object? sender, EventArgs e)
    {
        GenerateButtons();
        UpdateButtonStates();
    }

    private void AttachReadoutHandlers()
    {
        if (_hourReadOutPart is not null)
        {
            AddTapHandler(_hourReadOutPart, () => DisplayMode = ClockDisplayMode.Hours);
        }

        if (_minuteReadOutPart is not null)
        {
            AddTapHandler(_minuteReadOutPart, () => DisplayMode = ClockDisplayMode.Minutes);
        }

        if (_secondReadOutPart is not null)
        {
            AddTapHandler(_secondReadOutPart, () => DisplayMode = ClockDisplayMode.Seconds);
        }
    }

    private void DetachReadoutHandlers()
    {
        RemoveTapHandler(_hourReadOutPart);
        RemoveTapHandler(_minuteReadOutPart);
        RemoveTapHandler(_secondReadOutPart);
    }

    private static void AddTapHandler(View view, Action action)
    {
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => action();
        view.GestureRecognizers.Add(tap);
    }

    private static void RemoveTapHandler(View? view)
    {
        if (view is null)
        {
            return;
        }

        for (var i = view.GestureRecognizers.Count - 1; i >= 0; i--)
        {
            if (view.GestureRecognizers[i] is TapGestureRecognizer)
            {
                view.GestureRecognizers.RemoveAt(i);
            }
        }
    }

    private static void OnTimeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Clock clock)
        {
            return;
        }

        clock.UpdateFlags();
        clock.UpdateReadout();
        clock.UpdateLineAngles();
        clock.UpdateButtonStates();
        clock.TimeChanged?.Invoke(clock, new TimeChangedEventArgs((DateTime)oldValue, (DateTime)newValue));
    }

    private static void OnIsPostMeridiemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Clock clock || clock._suppressMeridiemUpdate)
        {
            return;
        }

        if ((bool)newValue && clock.Time.Hour < 12)
        {
            clock.Time = new DateTime(clock.Time.Year, clock.Time.Month, clock.Time.Day, clock.Time.Hour + 12, clock.Time.Minute, clock.Time.Second);
        }
        else if (!(bool)newValue && clock.Time.Hour >= 12)
        {
            clock.Time = new DateTime(clock.Time.Year, clock.Time.Month, clock.Time.Day, clock.Time.Hour - 12, clock.Time.Minute, clock.Time.Second);
        }
    }

    private static void OnIs24HoursChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Clock clock)
        {
            clock.GenerateButtons();
            clock.UpdateButtonStates();
            clock.UpdateReadout();
        }
    }

    private static void OnDisplayModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Clock clock)
        {
            clock.UpdateDisplayModeStates();
            clock.GotoVisualState(true);
        }
    }

    private static void OnDisplayAutomationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Clock clock)
        {
            clock.UpdateDisplayAutomation();
        }
    }

    private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Clock clock)
        {
            clock.GenerateButtons();
            clock.UpdateButtonStates();
        }
    }

    private void UpdateFlags()
    {
        _suppressMeridiemUpdate = true;
        IsPostMeridiem = Time.Hour >= 12;
        _suppressMeridiemUpdate = false;
        IsMidnightHour = Time.Hour == 0;
        IsMiddayHour = Time.Hour == 12;
    }

    private void UpdateReadout()
    {
        var hour = Is24Hours ? Time.ToString("HH") : Time.ToString("hh");
        HourText = hour;
        MinuteText = Time.ToString("mm");
        SecondText = Time.ToString("ss");
        AmPmText = Time.ToString("tt");
        UpdateDisplayAutomation();
    }

    private void UpdateDisplayAutomation()
    {
        IsSecondsDisplayed = DisplayAutomation == ClockDisplayAutomation.ToSeconds ||
                             DisplayAutomation == ClockDisplayAutomation.CycleWithSeconds;
    }

    private void UpdateDisplayModeStates()
    {
        HourTextOpacity = DisplayMode == ClockDisplayMode.Hours ? 1 : 0.56;
        MinuteTextOpacity = DisplayMode == ClockDisplayMode.Minutes ? 1 : 0.56;
        SecondTextOpacity = DisplayMode == ClockDisplayMode.Seconds ? 1 : 0.56;
    }

    private void UpdateLineAngles()
    {
        var hour = Time.Hour > 13 ? Time.Hour - 12 : Time.Hour;
        HourLineAngle = hour * 30;
        MinuteLineAngle = (Time.Minute == 0 ? 60 : Time.Minute) * 6;
        SecondLineAngle = (Time.Second == 0 ? 60 : Time.Second) * 6;

        if (_hourLine is not null)
        {
            _hourLine.Rotation = HourLineAngle;
        }

        if (_minuteLine is not null)
        {
            _minuteLine.Rotation = MinuteLineAngle;
        }

        if (_secondLine is not null)
        {
            _secondLine.Rotation = SecondLineAngle;
        }
    }

    private void GotoVisualState(bool useTransitions)
    {
        VisualStateManager.GoToState(
            this,
            DisplayMode == ClockDisplayMode.Hours
                ? HoursVisualStateName
                : DisplayMode == ClockDisplayMode.Minutes
                    ? MinutesVisualStateName
                    : SecondsVisualStateName);
    }

    private void GenerateButtons()
    {
        GenerateButtonsForCanvas(_hoursCanvas, ClockDisplayMode.Hours);
        GenerateButtonsForCanvas(_minutesCanvas, ClockDisplayMode.Minutes);
        GenerateButtonsForCanvas(_secondsCanvas, ClockDisplayMode.Seconds);
    }

    private void GenerateButtonsForCanvas(AbsoluteLayout? canvas, ClockDisplayMode mode)
    {
        if (canvas is null)
        {
            return;
        }

        if (canvas.Width <= 10 || Math.Abs(canvas.Height - canvas.Width) > 0)
        {
            return;
        }

        RemoveExistingButtons(canvas);

        _centreCanvas = new Point(canvas.Width / 2, canvas.Height / 2);

        if (mode == ClockDisplayMode.Hours)
        {
            if (Is24Hours)
            {
                GenerateButtons(canvas, Enumerable.Range(13, 12).ToList(), ButtonRadiusRatio, ButtonStyle, "00", mode);
                GenerateButtons(canvas, Enumerable.Range(1, 12).ToList(), ButtonRadiusInnerRatio, ButtonStyle, "#", mode);
            }
            else
            {
                GenerateButtons(canvas, Enumerable.Range(1, 12).ToList(), ButtonRadiusRatio, ButtonStyle, "0", mode);
            }
        }
        else
        {
            var range = Enumerable.Range(1, 60).ToList();
            GenerateButtons(canvas, range, ButtonRadiusRatio, null, "0", mode, i =>
                (i / 5.0) % 1 == 0 ? ButtonStyle : LesserButtonStyle);
        }
    }

    private void GenerateButtons(
        AbsoluteLayout canvas,
        ICollection<int> range,
        double radiusRatio,
        Style? defaultStyle,
        string format,
        ClockDisplayMode mode,
        Func<int, Style?>? styleSelector = null)
    {
        var anglePerItem = 360.0 / range.Count;
        var radiansPerItem = anglePerItem * (Math.PI / 180);
        var hypotenuseRadius = _centreCanvas.X * radiusRatio;

        foreach (var i in range)
        {
            var button = new ClockItemButton
            {
                Value = i,
                Style = styleSelector is null ? defaultStyle : styleSelector(i)
            };

            var label = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 12,
                Text = (i == 60 ? 0 : (i == 24 && mode == ClockDisplayMode.Hours ? 0 : i)).ToString(format)
            };
            label.SetBinding(Label.TextColorProperty, new Binding(nameof(ContentButton.Foreground), source: button));
            button.ButtonContent = label;

            var adjacent = Math.Cos(i * radiansPerItem) * hypotenuseRadius;
            var opposite = Math.Sin(i * radiansPerItem) * hypotenuseRadius;
            button.CentreX = _centreCanvas.X + opposite;
            button.CentreY = _centreCanvas.Y - adjacent;

            button.Clicked += OnClockItemClicked;
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += (_, e) => OnClockItemPanUpdated(button, e);
            button.GestureRecognizers.Add(pan);

            canvas.Children.Add(button);
        }
    }

    private static void RemoveExistingButtons(AbsoluteLayout canvas)
    {
        for (var i = canvas.Children.Count - 1; i >= 0; i--)
        {
            if (canvas.Children[i] is ClockItemButton)
            {
                canvas.Children.RemoveAt(i);
            }
        }
    }

    private void OnClockItemClicked(object? sender, EventArgs e)
    {
        if (sender is not ClockItemButton button)
        {
            return;
        }

        SetTimeFromButton(button);
        OnClockChoiceMade(DisplayMode);
        ApplyDisplayAutomation();
    }

    private void OnClockItemPanUpdated(ClockItemButton button, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _currentStartPosition = new Point(button.CentreX, button.CentreY);
                break;
            case GestureStatus.Running:
                var currentDragPosition = new Point(_currentStartPosition.X + e.TotalX, _currentStartPosition.Y + e.TotalY);
                UpdateTimeFromPosition(currentDragPosition);
                break;
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                OnClockChoiceMade(DisplayMode);
                ApplyDisplayAutomation();
                break;
        }
    }

    private void UpdateTimeFromPosition(Point currentDragPosition)
    {
        var delta = new Point(currentDragPosition.X - _centreCanvas.X, currentDragPosition.Y - _centreCanvas.Y);
        var angle = Math.Atan2(delta.X, -delta.Y);
        if (angle < 0)
        {
            angle += 2 * Math.PI;
        }

        DateTime time;
        if (DisplayMode == ClockDisplayMode.Hours)
        {
            if (Is24Hours)
            {
                var outerBoundary = _centreCanvas.X * ButtonRadiusInnerRatio +
                                    (_centreCanvas.X * ButtonRadiusRatio - _centreCanvas.X * ButtonRadiusInnerRatio) / 2;
                var distance = Math.Sqrt(
                    (_centreCanvas.X - currentDragPosition.X) * (_centreCanvas.X - currentDragPosition.X) +
                    (_centreCanvas.Y - currentDragPosition.Y) * (_centreCanvas.Y - currentDragPosition.Y));
                var isOuter = distance > outerBoundary;
                var hour = (int)Math.Round(6 * angle / Math.PI, MidpointRounding.AwayFromZero) % 12 + (isOuter ? 12 : 0);
                if (hour == 12)
                {
                    hour = 0;
                }
                else if (hour == 0)
                {
                    hour = 12;
                }

                time = new DateTime(Time.Year, Time.Month, Time.Day, hour, Time.Minute, Time.Second);
            }
            else
            {
                var hour = (int)Math.Round(6 * angle / Math.PI, MidpointRounding.AwayFromZero) % 12 + (IsPostMeridiem ? 12 : 0);
                time = new DateTime(Time.Year, Time.Month, Time.Day, hour, Time.Minute, Time.Second);
            }
        }
        else
        {
            var value = (int)Math.Round(30 * angle / Math.PI, MidpointRounding.AwayFromZero) % 60;
            time = DisplayMode == ClockDisplayMode.Minutes
                ? new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, value, Time.Second)
                : new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, value);
        }

        Time = time;
    }

    private void SetTimeFromButton(ClockItemButton button)
    {
        if (DisplayMode == ClockDisplayMode.Hours)
        {
            var hour = MassageHour(button.Value, Is24Hours);
            hour = ReverseMassageHour(hour, Time, Is24Hours);
            Time = new DateTime(Time.Year, Time.Month, Time.Day, hour, Time.Minute, Time.Second);
        }
        else if (DisplayMode == ClockDisplayMode.Minutes)
        {
            var minute = ReverseMassageMinuteSecond(button.Value);
            Time = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, minute, Time.Second);
        }
        else
        {
            var second = ReverseMassageMinuteSecond(button.Value);
            Time = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, second);
        }
    }

    private void UpdateButtonStates()
    {
        UpdateButtonStates(_hoursCanvas, ClockDisplayMode.Hours);
        UpdateButtonStates(_minutesCanvas, ClockDisplayMode.Minutes);
        UpdateButtonStates(_secondsCanvas, ClockDisplayMode.Seconds);
    }

    private void UpdateButtonStates(AbsoluteLayout? canvas, ClockDisplayMode mode)
    {
        if (canvas is null)
        {
            return;
        }

        foreach (var child in canvas.Children)
        {
            if (child is not ClockItemButton button)
            {
                continue;
            }

            var value = mode switch
            {
                ClockDisplayMode.Hours => MassageHour(Time.Hour, Is24Hours),
                ClockDisplayMode.Minutes => MassageMinuteSecond(Time.Minute),
                _ => MassageMinuteSecond(Time.Second)
            };

            button.IsChecked = value == button.Value;
        }
    }

    private void OnClockChoiceMade(ClockDisplayMode displayMode)
    {
        ClockChoiceMade?.Invoke(this, new ClockChoiceMadeEventArgs(displayMode));
    }

    private void ApplyDisplayAutomation()
    {
        switch (DisplayAutomation)
        {
            case ClockDisplayAutomation.None:
                break;
            case ClockDisplayAutomation.Cycle:
                DisplayMode = DisplayMode == ClockDisplayMode.Hours ? ClockDisplayMode.Minutes : ClockDisplayMode.Hours;
                break;
            case ClockDisplayAutomation.CycleWithSeconds:
                DisplayMode = DisplayMode == ClockDisplayMode.Hours
                    ? ClockDisplayMode.Minutes
                    : DisplayMode == ClockDisplayMode.Minutes
                        ? ClockDisplayMode.Seconds
                        : ClockDisplayMode.Hours;
                break;
            case ClockDisplayAutomation.ToMinutesOnly:
                if (DisplayMode == ClockDisplayMode.Hours)
                {
                    DisplayMode = ClockDisplayMode.Minutes;
                }

                break;
            case ClockDisplayAutomation.ToSeconds:
                if (DisplayMode == ClockDisplayMode.Hours)
                {
                    DisplayMode = ClockDisplayMode.Minutes;
                }
                else if (DisplayMode == ClockDisplayMode.Minutes)
                {
                    DisplayMode = ClockDisplayMode.Seconds;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static int MassageHour(int value, bool is24Hours)
    {
        if (is24Hours)
        {
            return value == 0 ? 24 : value;
        }

        if (value == 0)
        {
            return 12;
        }

        return value > 12 ? value - 12 : value;
    }

    private static int MassageMinuteSecond(int value) =>
        value == 0 ? 60 : value;

    private static int ReverseMassageHour(int value, DateTime currentTime, bool is24Hours)
    {
        if (is24Hours)
        {
            return value == 24 ? 0 : value;
        }

        return currentTime.Hour < 12
            ? value == 12 ? 0 : value
            : value == 12 ? 12 : value + 12;
    }

    private static int ReverseMassageMinuteSecond(int value) =>
        value == 60 ? 0 : value;
}
