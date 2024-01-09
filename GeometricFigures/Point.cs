using Godot;
namespace GeoWalle
{
    public sealed class Point : FigureExpression
    {
        public Vector2 Coordinates { get; }
        public string Name { get; }
        public Color Color { get; }
        public Point(Vector2 coordinates, string name, Color color)
        {
            Coordinates = coordinates;
            Name = name;
            Color = color;
        }
    }
}