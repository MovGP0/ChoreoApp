#if WINDOWS
using Microsoft.Maui;

namespace MaterialDesignThemes.Maui;

public static partial class TextBlockAssist
{
    static partial void OnAutoToolTipChangedPartial(BindableObject bindable, bool enabled)
    {
        if (bindable is not View view)
        {
            return;
        }

        if (bindable is Label label && enabled)
        {
            label.LineBreakMode = LineBreakMode.TailTruncation;
        }

        var tooltipText = enabled switch
        {
            true when bindable is Label l => l.Text,
            true when bindable is Entry e => e.Text,
            true => view.AutomationId ?? string.Empty,
            _ => null
        };

        TrySetTooltip(view, tooltipText);
    }

    private static void TrySetTooltip(View view, string? text)
    {
        ToolTipProperties.SetText(view, text);
    }
}
#endif
