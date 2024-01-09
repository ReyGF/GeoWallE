using Godot;
namespace GeoWalle
{
    sealed class GeometricArc : GeometricExpression
    {
        public Point P1 { get; }
        public Point P2 { get; }
        public Point P3 { get; }
        public Expression m { get; }
        public Color Color { get; }

        public GeometricArc(Point p1, Point p2, Point p3, Expression r, Color color)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            m = r;
            Color = color;
        }

    }
}