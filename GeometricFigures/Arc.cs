using Godot;
namespace GeoWalle
{
    sealed class Arc : FigureExpression
    {
        public Vector2 Center { get; }
        public int Radius { get; }
        public float StartAngle { get; }
        public float EndAngle { get; }
        public Color Color { get; }
        public string Name { get; }

        public Arc(string name, Vector2 center, int radius, float startangle, float endangle, Color color)
        {
            Name = name;
            Center = center;
            Radius = radius;
            StartAngle = startangle;
            EndAngle = endangle;
            Color = color;

        }

    }
}