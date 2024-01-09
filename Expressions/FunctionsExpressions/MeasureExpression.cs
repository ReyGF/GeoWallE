using System;
using Godot;

namespace GeoWalle
{
    class MeasuereExpression : Expression
    {
        Point P1 { get; }
        Point P2 { get; }

        public MeasuereExpression(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }
        public double Distance()
        {
            return Math.Sqrt(Mathf.Pow(P2.Coordinates.x - P1.Coordinates.x, 2) + Math.Pow(P2.Coordinates.y - P1.Coordinates.y, 2));
        }
    }
}