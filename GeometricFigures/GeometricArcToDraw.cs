
using Godot;
namespace GeoWalle
{
    sealed class GeometricArcToDraw : GeometricExpression
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Point P3 { get; }
        public double m { get; }
        public Color Color { get; }

        public GeometricArcToDraw(Point p1, Point p2, Point p3, double r, Color color)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            m = r;
            Color = color;
        }
    }
}