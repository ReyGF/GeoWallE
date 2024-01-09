using Godot;
namespace GeoWalle
{
    sealed class Segment : FigureExpression
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public Color Color { get; }
        public string Name { get; }
        public Segment(string name, Vector2 start, Vector2 end, Color color)
        {
            Name = name;
            Start = start;
            End = end;
            Color = color;
        }
    }
}
