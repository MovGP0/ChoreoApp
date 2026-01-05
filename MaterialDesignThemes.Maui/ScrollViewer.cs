namespace MaterialDesignThemes.Maui;

public sealed class ScrollViewer : TemplatedView
{
    public const string ScrollViewPartName = "PART_ScrollView";
    public const string VerticalScrollBarPartName = "PART_VerticalScrollBar";
    public const string HorizontalScrollBarPartName = "PART_HorizontalScrollBar";

    private ScrollView? _scrollView;
    private ScrollBar? _verticalScrollBar;
    private ScrollBar? _horizontalScrollBar;
    private bool _isUpdatingScrollBar;
    private bool _isUpdatingScrollView;

    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content),
        typeof(View),
        typeof(ScrollViewer),
        propertyChanged: OnContentChanged);

    public View? Content
    {
        get => (View?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly BindableProperty HorizontalScrollBarVisibilityProperty = BindableProperty.Create(
        nameof(HorizontalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(ScrollViewer),
        ScrollBarVisibility.Default);

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

    public static readonly BindableProperty VerticalScrollBarVisibilityProperty = BindableProperty.Create(
        nameof(VerticalScrollBarVisibility),
        typeof(ScrollBarVisibility),
        typeof(ScrollViewer),
        ScrollBarVisibility.Default);

    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
        set => SetValue(VerticalScrollBarVisibilityProperty, value);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (_scrollView is not null)
        {
            _scrollView.Scrolled -= OnScrollViewScrolled;
            _scrollView.SizeChanged -= OnScrollViewSizeChanged;
            if (_scrollView.Content is VisualElement oldContent)
            {
                oldContent.SizeChanged -= OnContentSizeChanged;
            }
        }

        _scrollView = GetTemplateChild(ScrollViewPartName) as ScrollView;
        _verticalScrollBar = GetTemplateChild(VerticalScrollBarPartName) as ScrollBar;
        _horizontalScrollBar = GetTemplateChild(HorizontalScrollBarPartName) as ScrollBar;

        if (_scrollView is not null)
        {
            _scrollView.Scrolled += OnScrollViewScrolled;
            _scrollView.Content = Content;
            _scrollView.SizeChanged += OnScrollViewSizeChanged;
            if (_scrollView.Content is VisualElement content)
            {
                content.SizeChanged += OnContentSizeChanged;
            }
        }

        if (_verticalScrollBar is not null)
        {
            _verticalScrollBar.Orientation = StackOrientation.Vertical;
            _verticalScrollBar.ValueChanged += OnVerticalScrollBarValueChanged;
        }

        if (_horizontalScrollBar is not null)
        {
            _horizontalScrollBar.Orientation = StackOrientation.Horizontal;
            _horizontalScrollBar.ValueChanged += OnHorizontalScrollBarValueChanged;
        }

        UpdateScrollBars();
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ScrollViewer viewer)
        {
            if (viewer._scrollView is not null)
            {
                if (viewer._scrollView.Content is VisualElement oldContent)
                {
                    oldContent.SizeChanged -= viewer.OnContentSizeChanged;
                }

                viewer._scrollView.Content = newValue as View;
                if (viewer._scrollView.Content is VisualElement content)
                {
                    content.SizeChanged += viewer.OnContentSizeChanged;
                }
            }

            viewer.UpdateScrollBars();
        }
    }

    private void OnScrollViewScrolled(object? sender, ScrolledEventArgs e)
    {
        if (_isUpdatingScrollView)
        {
            return;
        }

        _isUpdatingScrollBar = true;
        if (_verticalScrollBar is not null)
        {
            _verticalScrollBar.Value = e.ScrollY;
        }

        if (_horizontalScrollBar is not null)
        {
            _horizontalScrollBar.Value = e.ScrollX;
        }

        _isUpdatingScrollBar = false;
        UpdateScrollBars();
    }

    private void OnScrollViewSizeChanged(object? sender, EventArgs e)
    {
        UpdateScrollBars();
    }

    private void OnContentSizeChanged(object? sender, EventArgs e)
    {
        UpdateScrollBars();
    }

    private void OnVerticalScrollBarValueChanged(object? sender, ValueChangedEventArgs<double> e)
    {
        if (_isUpdatingScrollBar || _scrollView is null)
        {
            return;
        }

        _isUpdatingScrollView = true;
        _scrollView.ScrollToAsync(_scrollView.ScrollX, e.NewValue, false);
        _isUpdatingScrollView = false;
    }

    private void OnHorizontalScrollBarValueChanged(object? sender, ValueChangedEventArgs<double> e)
    {
        if (_isUpdatingScrollBar || _scrollView is null)
        {
            return;
        }

        _isUpdatingScrollView = true;
        _scrollView.ScrollToAsync(e.NewValue, _scrollView.ScrollY, false);
        _isUpdatingScrollView = false;
    }

    private void UpdateScrollBars()
    {
        if (_scrollView is null)
        {
            return;
        }

        var content = _scrollView.Content;
        if (content is null)
        {
            return;
        }

        var contentWidth = content.Width;
        var contentHeight = content.Height;
        var viewportWidth = _scrollView.Width;
        var viewportHeight = _scrollView.Height;

        if (_verticalScrollBar is not null)
        {
            var max = Math.Max(0, contentHeight - viewportHeight);
            _verticalScrollBar.Minimum = 0;
            _verticalScrollBar.Maximum = max;
            _verticalScrollBar.ViewportSize = viewportHeight;
            _verticalScrollBar.IsVisible = ShouldShowBar(VerticalScrollBarVisibility, max);
        }

        if (_horizontalScrollBar is not null)
        {
            var max = Math.Max(0, contentWidth - viewportWidth);
            _horizontalScrollBar.Minimum = 0;
            _horizontalScrollBar.Maximum = max;
            _horizontalScrollBar.ViewportSize = viewportWidth;
            _horizontalScrollBar.IsVisible = ShouldShowBar(HorizontalScrollBarVisibility, max);
        }
    }

    private static bool ShouldShowBar(ScrollBarVisibility visibility, double max)
    {
        return visibility switch
        {
            ScrollBarVisibility.Always => true,
            ScrollBarVisibility.Never => false,
            _ => max > 0
        };
    }
}
