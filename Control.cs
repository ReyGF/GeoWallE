using Godot;
using System;
using System.Collections.Generic;
using GeoWalle;

public class Control : Godot.Control
{
    bool IsButtonPressed = false;
    private TextEdit Input
    {
        get
        {
            return GetNode<TextEdit>("TextEdit");
        }
    }
    private Label Ouput
    {
        get
        {
            return GetNode<Label>("Label");
        }
    }
    private (float, float) CalcuLateEcuacion(Vector2 p1, Vector2 p2)
    {
        float m = (p1.y - p2.y) / (p1.x - p2.x);
        float b = p1.y - m * p1.x;

        return (m, b);
    }
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Update();
    }
    public void Compile()
    {
        Reset();
        var lexer = new Lexer(Input.Text);
        var tokens = lexer.GetTokens();

        var parser = new Parser(tokens);
        var expressions = parser.ParseExpressions();

    }
    public void DrawObjects()
    {
        IsButtonPressed = true;
    }
    public void Reset()
    {
        Utils.GeometricDraw.Clear();
        Utils.Colors.Clear();
        Utils.Functions.Clear();
        Utils.NameFunctions.Clear();
        Utils.FiguresVar.Clear();
        Utils.Points.Clear();
        Utils.PointsFunctionsFigures.Clear();
        IsButtonPressed = false;
        Ouput.Text = "";
    }

    public override void _Draw()
    {
        if (IsButtonPressed)
        {
            for (int i = 0; i < Utils.GeometricDraw.Count; i++)
            {
                DrawExpresion(Utils.GeometricDraw[i]);
            }
            for (int j = 0; j < Utils.PointsFunctionsFigures.Count; j++)
            {

            }
        }
    }
    private void DrawPointsInFigures(GeoWalle.Expression expression)
    {

        if (expression is Point point)
        {
            while (true)
            {
                DrawCircle(point.Coordinates, 5, point.Color);
            }
        }
        if (expression is Line line)
        {
            while (true)
            {

            }
        }
        if (expression is Ray ray)
        {
            while (true)
            {

            }
        }
        if (expression is Segment segment)
        {
            while (true) ;

        }
        if (expression is Circle circle)
        {
            while (true) ;
        }
        if (expression is Arc arc)
        {
            while (true) ;
        }
        if (expression is GeometricLine geometricLine)
        {
            var mb = CalcuLateEcuacion(geometricLine.P1.Coordinates, geometricLine.P2.Coordinates);

            float m = mb.Item1;
            float b = mb.Item2;

            //y = mx + b

            var start = new Vector2(-10000, m * -10000 + b);
            var end = new Vector2(10000, m * 10000 + b);
            Random r = new Random();


            while (true)
            {
                var x = r.Next(-10000, 10000);
                DrawCircle(new Vector2(x, m * x + b), 5, geometricLine.Color);
            }


        }
        if (expression is GeometricRay geometricRay)
        {
            var mb = CalcuLateEcuacion(geometricRay.P1.Coordinates, geometricRay.P2.Coordinates);

            float m = mb.Item1;
            float b = mb.Item2;

            var start = new Vector2(geometricRay.P1.Coordinates);

            var x = (geometricRay.P1.Coordinates.x > geometricRay.P2.Coordinates.x) ? -10000 : 10000;
            var end = new Vector2(x, m * x + b);

            while (true)
            {
                Random r = new Random();
                var q = r.Next((int)geometricRay.P1.Coordinates.x, x);
                DrawCircle(new Vector2(q, m * q + b), 5, geometricRay.Color);
            }
        }
        if (expression is GeometricSegment geometricSegment)
        {
            var mb = CalcuLateEcuacion(geometricSegment.P1.Coordinates, geometricSegment.P2.Coordinates);

            float m = mb.Item1;
            float b = mb.Item2;


            var start = new Vector2(geometricSegment.P1.Coordinates);
            var end = new Vector2(geometricSegment.P2.Coordinates);

            while (true)
            {
                Random r = new Random();
                var q = r.Next((int)geometricSegment.P1.Coordinates.x, (int)geometricSegment.P2.Coordinates.y);
                DrawCircle(new Vector2(q, m * q + b), 5, geometricSegment.Color);
            }

        }

        if (expression is GeometricCircleToDraw geometricCircleToDraw)
        {

            var dist = new Random();
            var l = dist.Next(0, (int)geometricCircleToDraw.Radius);
            while (true)
            {
                DrawCircle(new Vector2(geometricCircleToDraw.P1.Coordinates.x + l, geometricCircleToDraw.P1.Coordinates.y - l), 5, geometricCircleToDraw.Color);
            }
        }
        if (expression is GeometricArcToDraw geometricArcToDraw)
        {
            var mb1 = CalcuLateEcuacion(geometricArcToDraw.P1.Coordinates, geometricArcToDraw.P2.Coordinates);
            var startangle = (geometricArcToDraw.P1.Coordinates.x < geometricArcToDraw.P2.Coordinates.x) ? Mathf.Atan(mb1.Item1) : Mathf.Pi + Mathf.Atan(mb1.Item1);
            var mb2 = CalcuLateEcuacion(geometricArcToDraw.P1.Coordinates, geometricArcToDraw.P3.Coordinates);
            var endangle = (geometricArcToDraw.P1.Coordinates.x < geometricArcToDraw.P3.Coordinates.x) ? Mathf.Atan(mb2.Item1) : Mathf.Pi + Mathf.Atan(mb2.Item1);

            DrawArc(geometricArcToDraw.P1.Coordinates, (float)geometricArcToDraw.m, startangle, endangle, 60, geometricArcToDraw.Color, 4);
        }
    }
    private void DrawExpresion(GeoWalle.Expression expression)
    {
        if (expression is SamplesExpression samplesExpression)
        {
            while (true)
            {
                var r = new Random();
                DrawCircle(new Vector2(r.Next(300, 1000), r.Next(0, 1000)), 5, new Color(1, 1, 1));
            }

        }
        if (expression is Point point)
        {
            DrawCircle(point.Coordinates, 5, point.Color);
        }
        if (expression is Line line)
        {
            DrawLine(line.Start, line.End, line.Color, 3);
        }
        if (expression is Ray ray)
        {
            DrawLine(ray.Start, ray.End, ray.Color, 3);
        }
        if (expression is Segment segment)
        {
            DrawLine(segment.Start, segment.End, segment.Color, 3);
        }
        if (expression is Circle circle)
        {
            DrawArc(circle.Center, circle.Radius, 0, 6.28f, 60, circle.Color, 4);
        }
        if (expression is Arc arc)
        {
            DrawArc(arc.Center, arc.Radius, arc.StartAngle, arc.EndAngle, 30, arc.Color, 4);
        }
        if (expression is GeometricLine geometricLine)
        {
            var mb = CalcuLateEcuacion(geometricLine.P1.Coordinates, geometricLine.P2.Coordinates);

            float m = mb.Item1;
            float b = mb.Item2;

            //y = mx + b

            var start = new Vector2(-10000, m * -10000 + b);
            var end = new Vector2(10000, m * 10000 + b);

            DrawLine(start, end, geometricLine.Color, 3);
        }
        if (expression is GeometricRay geometricRay)
        {
            var mb = CalcuLateEcuacion(geometricRay.P1.Coordinates, geometricRay.P2.Coordinates);

            float m = mb.Item1;
            float b = mb.Item2;

            var start = new Vector2(geometricRay.P1.Coordinates);

            var x = (geometricRay.P1.Coordinates.x > geometricRay.P2.Coordinates.x) ? -10000 : 10000;
            var end = new Vector2(x, m * x + b);

            DrawLine(start, end, geometricRay.Color, 3);
        }
        if (expression is GeometricSegment geometricSegment)
        {
            var start = new Vector2(geometricSegment.P1.Coordinates);
            var end = new Vector2(geometricSegment.P2.Coordinates);

            DrawLine(start, end, geometricSegment.Color, 3);
        }

        if (expression is GeometricCircleToDraw geometricCircleToDraw)
            DrawArc(geometricCircleToDraw.P1.Coordinates, (float)geometricCircleToDraw.Radius, 0, 6.28f, 60, geometricCircleToDraw.Color, 4);


        if (expression is GeometricArcToDraw geometricArcToDraw)
        {
            var mb1 = CalcuLateEcuacion(geometricArcToDraw.P1.Coordinates, geometricArcToDraw.P2.Coordinates);
            var startangle = (geometricArcToDraw.P1.Coordinates.x < geometricArcToDraw.P2.Coordinates.x) ? Mathf.Atan(mb1.Item1) : Mathf.Pi + Mathf.Atan(mb1.Item1);
            var mb2 = CalcuLateEcuacion(geometricArcToDraw.P1.Coordinates, geometricArcToDraw.P3.Coordinates);
            var endangle = (geometricArcToDraw.P1.Coordinates.x < geometricArcToDraw.P3.Coordinates.x) ? Mathf.Atan(mb2.Item1) : Mathf.Pi + Mathf.Atan(mb2.Item1);

            Ouput.Text = geometricArcToDraw.Color.ToString();


            DrawArc(geometricArcToDraw.P1.Coordinates, (float)geometricArcToDraw.m, startangle, endangle, 60, geometricArcToDraw.Color, 4);
        }
        if (expression is SequenceExpression sequenceExpression)
        {
            foreach (var item in sequenceExpression.Expressions)
            {
                DrawExpresion(item);
            }
        }


    }
}


