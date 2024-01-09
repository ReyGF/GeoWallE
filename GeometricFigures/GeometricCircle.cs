using Godot;
namespace GeoWalle
{
    sealed class GeometricCircle : GeometricExpression
    {
        public Point P1 { get; }
        public Expression m { get; }
        public Color Color { get; }

        public GeometricCircle(Point p1, Expression r, Color color)
        {
            P1 = p1;
            m = r;
            Color = color;
        }

    }
}