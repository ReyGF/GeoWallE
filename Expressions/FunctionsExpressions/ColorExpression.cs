using Godot;
namespace GeoWalle
{
    sealed class ColorExpression : Expression
    {
        public Color Color { get; }

        public ColorExpression(Color color)
        {
            Color = color;
        }
    }
}
