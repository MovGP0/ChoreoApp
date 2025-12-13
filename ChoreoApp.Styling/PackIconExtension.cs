using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;

namespace ChoreoApp.Styling;

/// <summary>
/// Markup extension returning a parsed <see cref="Geometry"/> for a given <see cref="PackIconKind"/>.
/// </summary>
[AcceptEmptyServiceProvider]
public sealed class PackIconExtension : IMarkupExtension<Geometry?>
{
    public PackIconKind Kind { get; set; }

    public Geometry? ProvideValue(IServiceProvider serviceProvider) => PackIconGeometryParser.Parse(Kind);

    object? IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
