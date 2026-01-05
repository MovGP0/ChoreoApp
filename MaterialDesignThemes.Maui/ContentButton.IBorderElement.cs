namespace MaterialDesignThemes.Maui;

public partial class ContentButton: IBorderElement
{
    Brush IBorderElement.Background => Background;

    Color IBorderElement.BackgroundColor => BackgroundColor;

    Color IBorderElement.BorderColor => GetBorderColor();

    private Color GetBorderColor()
    {
        return Stroke is SolidColorBrush solid
            ? solid.Color
            : Colors.Transparent;
    }

    Color IBorderElement.BorderColorDefaultValue => Colors.Transparent;

    double IBorderElement.BorderWidth => StrokeThickness;

    double IBorderElement.BorderWidthDefaultValue => 0d;

    int IBorderElement.CornerRadius => ConvertCornerRadius();

    private int ConvertCornerRadius()
    {
        return (int)Math.Round(Math.Max(0, CornerRadius), MidpointRounding.AwayFromZero);
    }

    int IBorderElement.CornerRadiusDefaultValue => 0;

    bool IBorderElement.IsBackgroundColorSet() => IsSet(BackgroundColorProperty);

    bool IBorderElement.IsBackgroundSet() => IsSet(BackgroundProperty);

    bool IBorderElement.IsBorderColorSet() => IsSet(StrokeProperty);

    bool IBorderElement.IsBorderWidthSet() => IsSet(StrokeThicknessProperty);

    bool IBorderElement.IsCornerRadiusSet() => IsSet(CornerRadiusProperty);

    void IBorderElement.OnBorderColorPropertyChanged(Color oldValue, Color newValue)
    {
        Stroke = new SolidColorBrush(newValue);
    }
}
