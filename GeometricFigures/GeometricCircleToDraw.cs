using Godot;
namespace GeoWalle
{
    sealed class GeometricCircleToDraw : GeometricExpression
    {
        public Point P1 { get; }
        public double Radius { get; }
        public Color Color { get; }
        public GeometricCircleToDraw(Point p1, double m, Color color)
        {
            P1 = p1;
            Radius = m;
            Color = color;
        }
    }

}