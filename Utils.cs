using Godot;
using System;
using System.Collections.Generic;
namespace GeoWalle
{
    public static class Utils
    {
        public static List<GeometricFigureExpression> PointsFunctionsFigures = new List<GeometricFigureExpression>();
        public static Dictionary<string, Point> Points = new Dictionary<string, Point>();
        public static Dictionary<string, DeclarateFunctionExpression> Functions = new Dictionary<string, DeclarateFunctionExpression>();
        public static List<string> NameFunctions = new List<string>();
        public static Stack<Color> Colors = new Stack<Color>();
        public static List<Expression> GeometricDraw = new List<Expression>();
        public static string prove = "";
        public static Dictionary<string, Expression> FiguresVar = new Dictionary<string, Expression>();
    }
}