using MaterialDesignDemo.Maui.Buttons;
using MaterialDesignDemo.Maui.Cards;
using MaterialDesignDemo.Maui.Chips;
using MaterialDesignDemo.Maui.ComboBoxes;
using MaterialDesignDemo.Maui.ColorZones;
using MaterialDesignDemo.Maui.ColorTool;
using MaterialDesignDemo.Maui.DataGrids;
using MaterialDesignDemo.Maui.Dialogs;
using MaterialDesignDemo.Maui.DocumentationLinks;
using MaterialDesignDemo.Maui.Drawers;
using MaterialDesignDemo.Maui.Elevation;
using MaterialDesignDemo.Maui.Expander;
using MaterialDesignDemo.Maui.Fields;
using MaterialDesignDemo.Maui.FieldsLineUp;
using MaterialDesignDemo.Maui.Home;
using MaterialDesignDemo.Maui.PaletteSelector;
using MaterialDesignDemo.Maui.Snackbars;
using MaterialDesignDemo.Maui.SplitButtons;
using MaterialDesignDemo.Maui.ThemeSettings;
using MaterialDesignDemo.Maui.ToolTips;
using MaterialDesignDemo.Maui.Toggles;
using MaterialDesignDemo.Maui.Transitions;
using MaterialDesignDemo.Maui.Trees;
using MaterialDesignDemo.Maui.Typography;
using Sharpnado.Shades;
#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace MaterialDesignDemo.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.UseSharpnadoShadows(enableLogging: false);

        builder.Services.AddHome();
        builder.Services.AddPaletteSelector();
        builder.Services.AddColorTool();
        builder.Services.AddButtons();
        builder.Services.AddSplitButtons();
        builder.Services.AddCards();
        builder.Services.AddChips();
        builder.Services.AddColorZones();
        builder.Services.AddComboBoxes();
        builder.Services.AddDataGrids();
        builder.Services.AddDialogs();
        builder.Services.AddDocumentationLinks();
        builder.Services.AddDrawers();
        builder.Services.AddElevation();
        builder.Services.AddExpander();
        builder.Services.AddFields();
        builder.Services.AddFieldsLineUp();
        builder.Services.AddTypography();
        builder.Services.AddTrees();
        builder.Services.AddTransitions();
        builder.Services.AddToolTips();
        builder.Services.AddToggles();
        builder.Services.AddThemeSettings();
        builder.Services.AddSnackbars();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
