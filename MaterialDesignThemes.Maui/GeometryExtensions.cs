using Microsoft.Maui.Controls.Shapes;

namespace MaterialDesignThemes.Maui;

public static class GeometryExtensions
{
    extension (Geometry)
    {
        public static Geometry Parse(string data)
        {
            var figures = new PathFigureCollection();
            PathFigureCollectionConverter.ParseStringToPathFigureCollection(figures, data);
            return new PathGeometry
            {
                Figures = figures
            };
        }
    }
}
