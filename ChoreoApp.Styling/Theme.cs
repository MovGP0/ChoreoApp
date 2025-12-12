using Microsoft.Maui.Graphics;

namespace ChoreoApp;

/// <summary>
/// Automatically generated color theme.
/// </summary>
public sealed partial class Theme: ReactiveObject
{
    /// <summary>
    /// High-emphasis brand color. Use as the fill for primary actions
    /// (e.g. main Buttons, focused toggle controls, selected tabs, primary
    /// selection indicators). Pair with <see cref="Resources.Styles.Theme.OnPrimary"/> for any text
    /// or icons drawn on top.
    /// </summary>
    [Reactive] private Color _primary = new();

    /// <summary>
    /// Tint color for elevated surfaces. When custom-drawing raised Panels,
    /// toolbars, or card-like controls, apply a translucent overlay of this
    /// color on the base surface/surface container to express elevation instead
    /// of using pure opacity changes.
    /// </summary>
    [Reactive] private Color _surfaceTint = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.Primary"/> backgrounds.
    /// Use for text, glyphs, and icons on primary-filled controls
    /// (e.g. primary Buttons, primary Tab headers).
    /// </summary>
    [Reactive] private Color _onPrimary = new();

    /// <summary>
    /// Background color for lower-emphasis primary elements. Use for controls
    /// that belong to the primary family but are less prominent than
    /// <see cref="Resources.Styles.Theme.Primary"/> fills, such as secondary/tonal Buttons, chips,
    /// or selected ListView items.
    /// Pair with <see cref="Resources.Styles.Theme.OnPrimaryContainer"/> for content on top.
    /// </summary>
    [Reactive] private Color _primaryContainer = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.PrimaryContainer"/> surfaces.
    /// Use for text and icons on tonal primary controls (e.g. filled chips,
    /// secondary primary buttons, selected list items).
    /// </summary>
    [Reactive] private Color _onPrimaryContainer = new();

    /// <summary>
    /// Secondary accent color. Use for less prominent interactive elements
    /// (e.g. secondary Buttons, filter chips, accent icons) and to extend
    /// brand expression beyond primary without competing with it.
    /// Pair with <see cref="Resources.Styles.Theme.OnSecondary"/> for content on top.
    /// </summary>
    [Reactive] private Color _secondary = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.Secondary"/> backgrounds.
    /// Use for text and icons on secondary-filled controls.
    /// </summary>
    [Reactive] private Color _onSecondary = new();

    /// <summary>
    /// Background for low-emphasis secondary elements. Use for tonal variants
    /// of secondary components (e.g. secondary chips, toggle backgrounds,
    /// selected items in secondary areas). Pair with <see cref="Resources.Styles.Theme.OnSecondaryContainer"/>.
    /// </summary>
    [Reactive] private Color _secondaryContainer = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.SecondaryContainer"/> surfaces.
    /// Use for text and icons on tonal secondary components.
    /// </summary>
    [Reactive] private Color _onSecondaryContainer = new();

    /// <summary>
    /// Tertiary accent color. Use as an additional accent for highlighting
    /// specific information (e.g. input focus rings, special status markers,
    /// accent icons or graphs) when primary and secondary are already in use.
    /// Pair with <see cref="Resources.Styles.Theme.OnTertiary"/> for content on top.
    /// </summary>
    [Reactive] private Color _tertiary = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.Tertiary"/> backgrounds.
    /// Use for text and icons on tertiary-filled controls.
    /// </summary>
    [Reactive] private Color _onTertiary = new();

    /// <summary>
    /// Background color for low-emphasis tertiary elements. Use for tonal
    /// tertiary components such as special cards, highlighted list items, or
    /// tertiary chips. Pair with <see cref="Resources.Styles.Theme.OnTertiaryContainer"/>.
    /// </summary>
    [Reactive] private Color _tertiaryContainer = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.TertiaryContainer"/> surfaces.
    /// Use for text and icons on tonal tertiary components.
    /// </summary>
    [Reactive] private Color _onTertiaryContainer = new();

    /// <summary>
    /// High-emphasis error color. Use for destructive actions and critical
    /// validation states: error icons, error text, and filled error controls
    /// (e.g. destructive Buttons). Pair with <see cref="Resources.Styles.Theme.OnError"/> for content
    /// on error-filled backgrounds.
    /// </summary>
    [Reactive] private Color _error = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.Error"/> backgrounds.
    /// Use for text and icons on error-filled buttons or banners.
    /// </summary>
    [Reactive] private Color _onError = new();

    /// <summary>
    /// Background color for error containers. Use for error states that should
    /// be visible but not as intense as pure <see cref="Resources.Styles.Theme.Error"/>, such as
    /// highlighted TextBox backgrounds, error panels, or validation summary
    /// areas. Pair with <see cref="Resources.Styles.Theme.OnErrorContainer"/>.
    /// </summary>
    [Reactive] private Color _errorContainer = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.ErrorContainer"/> surfaces.
    /// Use for error messages, icons, and links inside error panels or
    /// highlighted input backgrounds.
    /// </summary>
    [Reactive] private Color _onErrorContainer = new();

    /// <summary>
    /// Overall app background color. Use for the main Form background and any
    /// full-window regions behind scrollable content or surfaces.
    /// Typical WinForms usage: Form.BackColor, main MDI background.
    /// </summary>
    [Reactive] private Color _background = new();

    /// <summary>
    /// Default foreground color for content on <see cref="VisualStyleElement.TrayNotify.Background"/>.
    /// Use for primary text, icons, and glyphs drawn directly on the main
    /// window background.
    /// </summary>
    [Reactive] private Color _onBackground = new();

    /// <summary>
    /// Primary surface color for UI elements that sit above the background.
    /// Use for Panel, GroupBox, UserControl, card-like backgrounds, and other
    /// content surfaces. Container variants (SurfaceContainer*) can be used
    /// to express elevation and hierarchy relative to this.
    /// </summary>
    [Reactive] private Color _surface = new();

    /// <summary>
    /// Default foreground color for content on <see cref="Resources.Styles.Theme.Surface"/>.
    /// Use for primary text and icons drawn on Panels, GroupBoxes, cards,
    /// tool windows, and other surfaces.
    /// </summary>
    [Reactive] private Color _onSurface = new();

    /// <summary>
    /// Variant of <see cref="Resources.Styles.Theme.Surface"/> for differentiated but related
    /// surfaces. Use for secondary surfaces that should be distinct from the
    /// main surface, such as data grid backgrounds, status panels, or
    /// navigation panes. Pair with <see cref="Resources.Styles.Theme.OnSurfaceVariant"/> for content.
    /// </summary>
    [Reactive] private Color _surfaceVariant = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.SurfaceVariant"/> surfaces.
    /// Use for text and icons on secondary surfaces, subtle labels, and
    /// supporting information.
    /// </summary>
    [Reactive] private Color _onSurfaceVariant = new();

    /// <summary>
    /// Main stroke/divider color. Use for borders and outlines of controls
    /// (e.g. TextBox borders, GroupBox borders, separator lines, grid lines)
    /// where standard emphasis is required.
    /// </summary>
    [Reactive] private Color _outline = new();

    /// <summary>
    /// Lower-emphasis stroke/divider color. Use for subtle separators,
    /// low-contrast grid lines, and disabled or de-emphasised borders,
    /// where <see cref="Android.Graphics.Outline"/> would be too strong.
    /// </summary>
    [Reactive] private Color _outlineVariant = new();

    /// <summary>
    /// Shadow color for elevated components. Use when custom-rendering drop
    /// shadows for raised Panels, floating tool windows, popups, or menus.
    /// The alpha is typically controlled separately from the color.
    /// </summary>
    [Reactive] private Color _shadow = new();

    /// <summary>
    /// Scrim (backdrop) color for modal elements. Use as a semi-transparent
    /// overlay behind dialogs, popups, or side panels to dim and de-emphasize
    /// the underlying UI while preserving context.
    /// </summary>
    [Reactive] private Color _scrim = new();

    /// <summary>
    /// Surface color for inverted regions. Use for areas that visually invert
    /// the normal surfaces, such as status bars, snackbars, or banners that
    /// sit on top of typical surfaces. Pair with <see cref="Resources.Styles.Theme.InverseOnSurface"/>
    /// for content on top.
    /// </summary>
    [Reactive] private Color _inverseSurface = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.InverseSurface"/> backgrounds.
    /// Use for text, icons, and action glyphs in inverted regions such as
    /// status bars or snackbars.
    /// </summary>
    [Reactive] private Color _inverseOnSurface = new();

    /// <summary>
    /// Primary accent color for use inside inverse regions. For example, use
    /// as the fill for an action Button in a snackbar or banner that uses
    /// <see cref="Resources.Styles.Theme.InverseSurface"/> as its background.
    /// </summary>
    [Reactive] private Color _inversePrimary = new();

    /// <summary>
    /// Light/dark invariant primary container color. Use when you need a
    /// primary-family container that must look consistent across light and
    /// dark themes (e.g. persistent brand panels, logo title bars, or
    /// non-theme-switching controls). Pair with <see cref="Resources.Styles.Theme.OnPrimaryFixed"/>.
    /// </summary>
    [Reactive] private Color _primaryFixed = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.PrimaryFixed"/> surfaces.
    /// Use for text and icons on brand surfaces that should not change between
    /// light and dark themes.
    /// </summary>
    [Reactive] private Color _onPrimaryFixed = new();

    /// <summary>
    /// Darker variant of <see cref="Resources.Styles.Theme.PrimaryFixed"/> for increased emphasis.
    /// Use for hover/pressed states or for higher-emphasis primary-fixed
    /// areas such as focused brand buttons on a primary-fixed panel.
    /// Pair with <see cref="Resources.Styles.Theme.OnPrimaryFixedVariant"/> or <see cref="Resources.Styles.Theme.OnPrimaryFixed"/>
    /// depending on contrast.
    /// </summary>
    [Reactive] private Color _primaryFixedDim = new();

    /// <summary>
    /// Foreground color for content on stronger primary-fixed surfaces,
    /// typically <see cref="Resources.Styles.Theme.PrimaryFixedDim"/>. Use when you need appropriate
    /// contrast on the darker fixed primary surfaces.
    /// </summary>
    [Reactive] private Color _onPrimaryFixedVariant = new();

    /// <summary>
    /// Light/dark invariant secondary container color. Use when you need a
    /// secondary-family container that should look the same in light and dark
    /// themes (e.g. secondary brand panels, stable sidebar backgrounds).
    /// Pair with <see cref="Resources.Styles.Theme.OnSecondaryFixed"/>.
    /// </summary>
    [Reactive] private Color _secondaryFixed = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.SecondaryFixed"/> surfaces.
    /// Use for text and icons on fixed secondary containers.
    /// </summary>
    [Reactive] private Color _onSecondaryFixed = new();

    /// <summary>
    /// Darker variant of <see cref="Resources.Styles.Theme.SecondaryFixed"/> for more emphasis.
    /// Use for hover/pressed states or more prominent secondary-fixed
    /// components. Pair with <see cref="Resources.Styles.Theme.OnSecondaryFixedVariant"/>.
    /// </summary>
    [Reactive] private Color _secondaryFixedDim = new();

    /// <summary>
    /// Foreground color for content on stronger secondary-fixed surfaces,
    /// typically <see cref="Resources.Styles.Theme.SecondaryFixedDim"/>. Use when higher contrast is
    /// required than <see cref="Resources.Styles.Theme.OnSecondaryFixed"/> provides.
    /// </summary>
    [Reactive] private Color _onSecondaryFixedVariant = new();

    /// <summary>
    /// Light/dark invariant tertiary container color. Use for tertiary-family
    /// surfaces that should not change with theme (e.g. special feature areas,
    /// analytic views, or domain-specific accent panels). Pair with
    /// <see cref="Resources.Styles.Theme.OnTertiaryFixed"/>.
    /// </summary>
    [Reactive] private Color _tertiaryFixed = new();

    /// <summary>
    /// Foreground color for content on <see cref="Resources.Styles.Theme.TertiaryFixed"/> surfaces.
    /// Use for text and icons on fixed tertiary containers.
    /// </summary>
    [Reactive] private Color _onTertiaryFixed = new();

    /// <summary>
    /// Darker variant of <see cref="Resources.Styles.Theme.TertiaryFixed"/> for more emphasis.
    /// Use for hover/pressed states or highly emphasized tertiary-fixed
    /// components. Pair with <see cref="Resources.Styles.Theme.OnTertiaryFixedVariant"/>.
    /// </summary>
    [Reactive] private Color _tertiaryFixedDim = new();

    /// <summary>
    /// Foreground color for content on stronger tertiary-fixed surfaces,
    /// typically <see cref="Resources.Styles.Theme.TertiaryFixedDim"/>. Use when higher contrast is
    /// needed than <see cref="Resources.Styles.Theme.OnTertiaryFixed"/> provides.
    /// </summary>
    [Reactive] private Color _onTertiaryFixedVariant = new();

    /// <summary>
    /// Darkest overall surface tone in the scheme. In dark themes, use for
    /// the main window background or deeply recessed areas. In light themes,
    /// use sparingly for strongly de-emphasized regions.
    /// </summary>
    [Reactive] private Color _surfaceDim = new();

    /// <summary>
    /// Lightest overall surface tone in the scheme. In light themes, use as
    /// the default high-level background (e.g. main Form surface). In dark
    /// themes, use for highly elevated sheets such as floating tool windows
    /// or popups that must stand out against darker surfaces.
    /// </summary>
    [Reactive] private Color _surfaceBright = new();

    /// <summary>
    /// Surface container with the least emphasis (lightest in light theme,
    /// darkest in dark theme among container tones). Use for the outermost
    /// background container, such as scrollable content hosts or the area
    /// behind cards and Panels.
    /// </summary>
    [Reactive] private Color _surfaceContainerLowest = new();

    /// <summary>
    /// Slightly more emphasized container than <see cref="Resources.Styles.Theme.SurfaceContainerLowest"/>.
    /// Use for base content containers such as primary Panels, tab pages, or
    /// standard card backgrounds.
    /// </summary>
    [Reactive] private Color _surfaceContainerLow = new();

    /// <summary>
    /// Default surface container tone. Use for the main card or Panel level
    /// in a layered interface, sitting above <see cref="Resources.Styles.Theme.SurfaceContainerLow"/>
    /// and below the High/Highest variants. Good default for standard cards
    /// and list item backgrounds.
    /// </summary>
    [Reactive] private Color _surfaceContainer = new();

    /// <summary>
    /// Higher-emphasis surface container tone. Use for surfaces that should
    /// stand out above regular content, such as modal Panels, pinned tool
    /// windows, or important cards in a dashboard.
    /// </summary>
    [Reactive] private Color _surfaceContainerHigh = new();

    /// <summary>
    /// Highest-emphasis container tone. Use for the most prominent surfaces
    /// in the hierarchy, such as dialogs, modal sheets, or critical overlays.
    /// Typically used together with <see cref="Resources.Styles.Theme.Scrim"/> behind modal UI.
    /// </summary>
    [Reactive] private Color _surfaceContainerHighest = new();
}
