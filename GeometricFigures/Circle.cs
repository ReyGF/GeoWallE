using System;
using Godot;
namespace GeoWalle
{
    sealed class Circle : FigureExpression
    {
        public Vector2 Center { get; }
        public int Radius { get; }
        public Color Color { get; }
        public string Name { get; }

        public Circle(string name, Vector2 center, int radius, Color color)
        {
            Name = name;
            Center = center;
            Radius = radius;
            Color = color;
        }
    }

}