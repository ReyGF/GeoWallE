using Godot;
namespace GeoWalle
{
    sealed class Line : FigureExpression
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public Color Color { get; }
        public string Name { get; }
        public Line(string name, Vector2 start, Vector2 end, Color color)
        {
            Name = name;
            Start = start;
            End = end;
            Color = color;
        }
    }
}