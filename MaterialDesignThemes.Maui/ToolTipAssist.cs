namespace MaterialDesignThemes.Maui;

public static class ToolTipAssist
{
    public static Point GetToolTipOffset(Size popupSize, Size targetSize, Point offset)
    {
        return new Point(targetSize.Width / 2 - popupSize.Width / 2, targetSize.Height + 14);
    }
}
