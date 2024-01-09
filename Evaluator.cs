using Godot;
using System;
using System.Collections.Generic;
using System.IO;

namespace GeoWalle
{
    sealed class Evaluator
    {
        Dictionary<string, object> VarVAlues = new Dictionary<string, object>();
        Stack<Dictionary<string, object>> Stack = new Stack<Dictionary<string, object>>();
        Stack<List<object>> Rest = new Stack<List<object>>();
        public Evaluator()
        {

        }
        public object Evaluate(Expression expression)
        {
            if (expression is DrawExpression drawExpression)
            {
                if (drawExpression.Argument is VariableExpression variableExpression2)
                {
                    try
                    {
                        if ((Expression)VarVAlues[variableExpression2.Name] is SequenceExpression)
                        {
                            drawExpression.Argument = (Expression)VarVAlues[variableExpression2.Name];
                        }
                    }
                    catch (KeyNotFoundException)
                    {

                    }
                }
                if (drawExpression.Argument is SequenceExpression sequenceExpression2)
                {
                    foreach (var item in sequenceExpression2.Expressions)
                    {
                        Utils.GeometricDraw.Add((Expression)Evaluate(item));
                    }
                }
                else
                    Utils.GeometricDraw.Add((Expression)Evaluate(drawExpression.Argument));

                return "";
            }
            if (expression is ColorExpression colorExpression)
            {
                Utils.Colors.Push(colorExpression.Color);
                return "";
            }
            if (expression is RestoreExpression)
            {
                Utils.Colors.Pop();
                return "";
            }
            if (expression is FigureExpression figureExpression)
            {
                if (figureExpression is Point point)
                {
                    Utils.FiguresVar.Add(point.Name, point);
                    return point;
                }
                if (figureExpression is Line line)
                {
                    Utils.FiguresVar.Add(line.Name, line);
                    return line;
                }
                if (figureExpression is Ray ray)
                {
                    Utils.FiguresVar.Add(ray.Name, ray);
                    return ray;
                }
                if (figureExpression is Segment segment)
                {
                    Utils.FiguresVar.Add(segment.Name, segment);
                    return segment;
                }
                if (figureExpression is Circle circle)
                {
                    Utils.FiguresVar.Add(circle.Name, circle);
                    return circle;
                }
                if (figureExpression is Arc arc)
                {
                    Utils.FiguresVar.Add(arc.Name, arc);
                    return arc;
                }
            }
            if (expression is GeometricExpression geometricExpression)
            {
                if (geometricExpression is GeometricLine geometricLine)
                {
                    return geometricLine;
                }
                if (geometricExpression is GeometricRay geometricRay)
                {
                    return geometricRay;
                }
                if (geometricExpression is GeometricSegment geometricSegment)
                {
                    return geometricSegment;
                }
                if (geometricExpression is GeometricCircle geometricCircle)
                {
                    return new GeometricCircleToDraw(geometricCircle.P1, (double)Evaluate(geometricCircle.m), geometricCircle.Color);
                }
                if (geometricExpression is GeometricArc geometricArc)
                {
                    return new GeometricArcToDraw(geometricArc.P1, geometricArc.P2, geometricArc.P3,
                                                (double)Evaluate(geometricArc.m), geometricArc.Color);
                }
            }
            if (expression is NumberExpression numberExpression)
            {
                return numberExpression.Number;
            }
            if (expression is BoolExpression boolExpression)
            {
                return boolExpression.Value;
            }
            if (expression is VariableExpression variableExpression)
            {
                if (variableExpression is FunctionVariableExpression functionVariableExpression)
                {
                    return Stack.Peek()[functionVariableExpression.Name];
                }
                if (variableExpression is DrawVariableExpression drawVariableExpression)
                {
                    if (Utils.FiguresVar[drawVariableExpression.Name] is GeometricCircle geometricCircle)
                    {
                        return new GeometricCircleToDraw(geometricCircle.P1, (double)Evaluate(geometricCircle.m), geometricCircle.Color);
                    }
                    else if (Utils.FiguresVar[drawVariableExpression.Name] is GeometricArc geometricArc)
                    {
                        return new GeometricArcToDraw(geometricArc.P1, geometricArc.P2, geometricArc.P3,
                                               (double)Evaluate(geometricArc.m), geometricArc.Color);
                    }
                    return Utils.FiguresVar[drawVariableExpression.Name];
                }
                try
                {
                    return VarVAlues[variableExpression.Name];
                }
                catch (KeyNotFoundException)
                {
                    return Utils.FiguresVar[variableExpression.Name];
                }
            }
            if (expression is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Sign.Kind == TokenKind.MinusToken)
                {
                    return -(double)Evaluate(unaryExpression.Expression);
                }
                else if (unaryExpression.Sign.Kind == TokenKind.LogicalNegationToken)
                {
                    return !(bool)Evaluate(unaryExpression.Expression);
                }
                else
                    return Evaluate(unaryExpression.Expression);
            }
            if (expression is AsignamentExpression asignamentExpression)
            {
                if (asignamentExpression.Expression is FigureExpression || asignamentExpression.Expression is GeometricExpression)
                {
                    if (!Utils.FiguresVar.ContainsKey(asignamentExpression.VariableName))
                        Utils.FiguresVar.Add(asignamentExpression.VariableName, asignamentExpression.Expression);
                    else
                        Utils.FiguresVar[asignamentExpression.VariableName] = asignamentExpression.Expression;
                }
                else if (asignamentExpression.Expression is SequenceExpression)
                {
                    if (!VarVAlues.ContainsKey(asignamentExpression.VariableName))
                        VarVAlues.Add(asignamentExpression.VariableName, asignamentExpression.Expression);
                    else
                        VarVAlues[asignamentExpression.VariableName] = asignamentExpression.Expression;
                }
                else
                {
                    if (!VarVAlues.ContainsKey(asignamentExpression.VariableName))
                        VarVAlues.Add(asignamentExpression.VariableName, Evaluate(asignamentExpression.Expression));
                    else
                        VarVAlues[asignamentExpression.VariableName] = Evaluate(asignamentExpression.Expression);
                }
                return "";
            }

            if (expression is AsignamentSequenceExpression asignamentSequenceExpression)
            {
                var values = new List<Expression>();
                if (asignamentSequenceExpression.SequenceExpressions is SequenceExpression sequenceExpression1)
                {
                    values = sequenceExpression1.Expressions;
                }
                var rest = new List<object>();

                for (int i = 0; i < asignamentSequenceExpression.Vars.Count; i++)
                {
                    if (asignamentSequenceExpression.Vars[i].Kind == TokenKind.UnderScoreToken) continue;

                    if (i < values.Count)
                    {
                        if (asignamentSequenceExpression.Vars[i].Kind != TokenKind.RestToken)
                        {
                            if (values[i] is FigureExpression || values[i] is GeometricExpression)
                            {
                                if (values[i] is Point point)
                                {
                                    Utils.Points.Add(asignamentSequenceExpression.Vars[i].Text, point);
                                }
                                if (!Utils.FiguresVar.ContainsKey(asignamentSequenceExpression.Vars[i].Text))
                                    Utils.FiguresVar.Add(asignamentSequenceExpression.Vars[i].Text, values[i]);
                                else
                                    Utils.FiguresVar[asignamentSequenceExpression.Vars[i].Text] = values[i];
                            }
                            else
                            {
                                if (!VarVAlues.ContainsKey(asignamentSequenceExpression.Vars[i].Text))
                                    VarVAlues.Add(asignamentSequenceExpression.Vars[i].Text, Evaluate(values[i]));
                                else
                                    VarVAlues[asignamentSequenceExpression.Vars[i].Text] = Evaluate(values[i]);
                            }
                        }
                        else
                            rest.Add(Evaluate(values[i]));
                    }
                    else
                    {
                        if (!VarVAlues.ContainsKey(asignamentSequenceExpression.Vars[i].Text))
                            VarVAlues.Add(asignamentSequenceExpression.Vars[i].Text, new UndefinedExpression());
                        else
                            VarVAlues[asignamentSequenceExpression.Vars[i].Text] = new UndefinedExpression();
                    }
                }
                Rest.Push(rest);

                return "";
            }
            if (expression is MeasuereExpression measuereExpression)
            {
                return measuereExpression.Distance();
            }
            if (expression is PointsFuncExpression pointsFuncExpression)
            {
                if (pointsFuncExpression.Figure is VariableExpression variableExpression1)
                {
                    pointsFuncExpression.Figure = (Expression)Evaluate(variableExpression1);
                }
                if (pointsFuncExpression.Figure is FigureExpression figureExpression1)
                {
                    if (figureExpression1 is Point point)
                    {
                        Utils.PointsFunctionsFigures.Add(point);
                        return point;
                    }
                    if (figureExpression1 is Line line)
                    {
                        Utils.PointsFunctionsFigures.Add(line);
                        return line;
                    }
                    if (figureExpression1 is Ray ray)
                    {
                        Utils.PointsFunctionsFigures.Add(ray);
                        return ray;
                    }
                    if (figureExpression1 is Segment segment)
                    {
                        Utils.PointsFunctionsFigures.Add(segment);
                        return segment;
                    }
                    if (figureExpression1 is Circle circle)
                    {
                        Utils.PointsFunctionsFigures.Add(circle);
                        return circle;
                    }
                    if (figureExpression1 is Arc arc)
                    {
                        Utils.PointsFunctionsFigures.Add(arc);
                        return arc;
                    }
                }
                if (pointsFuncExpression.Figure is GeometricExpression geometricExpression1)
                {
                    if (geometricExpression1 is GeometricLine geometricLine)
                    {
                        Utils.PointsFunctionsFigures.Add(geometricLine);
                        return geometricLine;
                    }
                    if (geometricExpression1 is GeometricRay geometricRay)
                    {
                        Utils.PointsFunctionsFigures.Add(geometricRay);
                        return geometricRay;
                    }
                    if (geometricExpression1 is GeometricSegment geometricSegment)
                    {
                        Utils.PointsFunctionsFigures.Add(geometricSegment);
                        return geometricSegment;
                    }
                    if (geometricExpression1 is GeometricCircle geometricCircle)
                    {
                        Utils.PointsFunctionsFigures.Add(geometricCircle);
                        return geometricCircle;
                    }
                    if (geometricExpression1 is GeometricArcToDraw geometricArc)
                    {
                        Utils.PointsFunctionsFigures.Add(geometricArc);
                        return geometricArc;
                    }
                }
            }
            if (expression is IntersectExpression intersectExpression)
            {
                return new List<Expression> { intersectExpression.Figure1, intersectExpression.Figure2 };
            }
            if (expression is CountExpression countExpression)
            {
                if (countExpression.SeqExpression is SequenceExpression sequenceExpression1)
                {
                    return sequenceExpression1.Expressions.Count;
                }
                else
                {
                    var s = (SequenceExpression)Evaluate(countExpression.SeqExpression);

                    return s.Expressions.Count;

                }
            }
            if (expression is SamplesExpression samplesExpression)
            {
                Utils.GeometricDraw.Add(samplesExpression);
            }
            if (expression is RandomExpression random)
            {
                var r = new Random();
                var l = new List<int>();
                int i = 0;
                while (i < 100000)
                {
                    l.Add(r.Next(0, 10000));
                    i++;
                }
                return l;
            }
            if (expression is DeclarateFunctionExpression declarateFunctionExpression)
            {
                Utils.Functions.Add(declarateFunctionExpression.FunctionName, declarateFunctionExpression);
                return "";
            }
            if (expression is CallFunctionExpression callFunctionExpression)
            {
                var function = Utils.Functions[callFunctionExpression.FunctionName];
                if (function.Parameters.Count == callFunctionExpression.ArgumentValues.Count)
                {
                    var CurrentParementers = new Dictionary<string, object>();
                    for (int i = 0; i < function.Parameters.Count; i++)
                    {
                        CurrentParementers.Add(function.Parameters[i], Evaluate(callFunctionExpression.ArgumentValues[i]));
                    }
                    Stack.Push(CurrentParementers);

                    var functionexpression = Evaluate(function.Expression);

                    Stack.Pop();

                    return functionexpression;

                }
            }
            if (expression is SequenceExpression sequenceExpression)
            {
                var evaluatesexpreesions = new List<object>();

                foreach (var item in sequenceExpression.Expressions)
                {
                    evaluatesexpreesions.Add(Evaluate(item));
                }
                return evaluatesexpreesions;
            }
            if (expression is ConditionalExpression conditionalExpression)
            {
                if ((bool)Evaluate(conditionalExpression.ParenthesisExpression))
                    return Evaluate(conditionalExpression.ThenExpression);
                else
                    return Evaluate(conditionalExpression.ElseExpression);
            }
            if (expression is LetInExpression letInExpression)
            {
                for (int i = 0; i < letInExpression.ListExpression.Count; i++)
                {
                    Evaluate(letInExpression.ListExpression[i]);
                }
                return Evaluate(letInExpression.ToEvaluateExpreesion);
            }
            if (expression is BinaryExpression binaryExpression)
            {
                var left = Evaluate(binaryExpression.Left);
                var right = Evaluate(binaryExpression.Right);

                if (binaryExpression is StarBinaryExpression starBinaryExpression)
                {
                    return starBinaryExpression.Calculate((double)left, (double)right);
                }
                if (binaryExpression is SlashBinaryExpression slashBinaryExpression)
                {
                    return slashBinaryExpression.Calculate((double)left, (double)right);
                }
                if (binaryExpression is PlusBinaryExpression plusBinaryExpression)
                {
                    return plusBinaryExpression.Calculate((double)left, (double)right);
                }
                if (binaryExpression is MinusBinaryExpression minusBinaryExpression)
                {
                    return minusBinaryExpression.Calculate((double)left, (double)right);
                }
                if (binaryExpression is OrBinaryExpression orBinaryExpression)
                {
                    return orBinaryExpression.Comparate((bool)left, (bool)right);
                }
                if (binaryExpression is AndBinaryExpression andBinaryExpression)
                {
                    return andBinaryExpression.Comparate((bool)left, (bool)right);
                }
                if (binaryExpression is EqualEqualBinaryExpression equalEqualBinaryExpression)
                {
                    return equalEqualBinaryExpression.Comparate(left, right);
                }
                if (binaryExpression is GreaterBinaryExpression greaterBinaryExpression)
                {
                    return greaterBinaryExpression.Comparate(left, right);
                }
                if (binaryExpression is GreaterEqualBinaryExpression greaterEqualBinaryExpression)
                {
                    return greaterEqualBinaryExpression.Comparate(left, right);
                }
                if (binaryExpression is LessEqualBinaryExpression lessEqualBinaryExpression)
                {
                    return lessEqualBinaryExpression.Comparate(left, right);
                }
                if (binaryExpression is LessBinaryExpression lessBinaryExpression)
                {
                    return lessBinaryExpression.Comparate(left, right);
                }
                if (binaryExpression is DiferentBinaryExpression diferentBinaryExpression)
                {
                    return diferentBinaryExpression.Comparate(left, right);
                }
            }
            return "Unexpected expression";
        }
    }
}
