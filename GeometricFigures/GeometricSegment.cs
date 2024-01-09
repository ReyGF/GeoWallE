
using Godot;
namespace GeoWalle
{
    sealed class GeometricSegment : GeometricExpression
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Color Color { get; }

        public GeometricSegment(Point p1, Point p2, Color color)
        {
            P1 = p1;
            P2 = p2;
            Color = color;
        }
    }
}