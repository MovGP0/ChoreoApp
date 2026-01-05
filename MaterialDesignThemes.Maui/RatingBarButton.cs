namespace MaterialDesignThemes.Maui;

public sealed class RatingBarButton : ContentButton
{
    private readonly TapGestureRecognizer _tap;
    private readonly PointerGestureRecognizer _pointer;

    public RatingBarButton()
    {
        _tap = new TapGestureRecognizer();
        _tap.Tapped += OnTapped;
        GestureRecognizers.Add(_tap);

        _pointer = new PointerGestureRecognizer();
        _pointer.PointerMoved += OnPointerMoved;
        _pointer.PointerExited += OnPointerExited;
        GestureRecognizers.Add(_pointer);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(RatingBarButton),
        0);

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    internal RatingBar? RatingBar { get; private set; }

    internal void AttachRatingBar(RatingBar ratingBar)
    {
        RatingBar = ratingBar;
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        var point = e.GetPosition(this);
        RatingBar?.HandleButtonTapped(this, point);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetPosition(this);
        RatingBar?.HandlePointerMoved(this, point);
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        RatingBar?.ClearPreview();
    }
}
