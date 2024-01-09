using Godot;
namespace GeoWalle
{
    sealed class GeometricRay : GeometricExpression
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Color Color { get; }

        public GeometricRay(Point p1, Point p2, Color color)
        {
            P1 = p1;
            P2 = p2;
            Color = color;
        }
    }
}