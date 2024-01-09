using Godot;
using System;
using GeoWalle;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace GeoWalle
{
    sealed class Parser
    {
        List<string> NameVar = new List<string>();
        private readonly Token[] _tokens = new Token[50];
        private int pos;
        bool issequence = false;
        bool isfunctionvariable = false;
        bool isdrawvariable = false;
        bool check = true;
        private Token Current
        {
            get
            {
                if (pos >= _tokens.Length)
                    return new Token(TokenKind.EndToken, "1");

                return _tokens[pos];
            }

        }
        private void CheckToken(TokenKind kind)
        {
            if (Current.Kind == kind)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            pos++;
        }
        private void NextToken()
        {
            pos++;
        }
        private Color GetCurrentColor()
        {
            return (Utils.Colors.Count >= 1) ? Utils.Colors.Peek() : new Color(0, 0, 0);
        }
        private Vector2 Random()
        {
            Random random = new Random();

            return new Vector2(random.Next(320, 1000), random.Next(100, 550));
        }
        private (Point, Point) GetLinesArguments()
        {
            NextToken();
            var p1 = Utils.Points[Current.Text];
            NextToken(); NextToken();
            var p2 = Utils.Points[Current.Text];
            NextToken(); NextToken();

            return (p1, p2);
        }
        private (Point, Expression) GetCircleArguments()
        {
            NextToken();
            var p1 = Utils.Points[Current.Text];
            NextToken(); NextToken();
            if (isdrawvariable)
                isdrawvariable = false;
            var radious = Parse();
            NextToken();

            return (p1, radious);
        }
        private (Point, Point, Point, Expression) GetArcArguments()
        {
            List<Point> arguments = new List<Point>();

            for (int i = 0; i < 3; i++)
            {
                NextToken();
                var point = Utils.Points[Current.Text];
                NextToken();
                arguments.Add(point);
            }
            NextToken();
            if (isdrawvariable)
                isdrawvariable = false;
            var radious = Parse();
            NextToken();

            return (arguments[0], arguments[1], arguments[2], radious);

        }
        public Parser(Token[] tokens)
        {
            _tokens = tokens;
        }
        public object[] ParseExpressions()
        {
            var evaluator = new Evaluator();

            var expressions = new List<object>();

            do
            {
                var parse = Parse();
                var evaluate = evaluator.Evaluate(parse);
                expressions.Add(evaluate);
                CheckToken(TokenKind.FinalInstruccionToken);
            }
            while (check);

            return expressions.ToArray();

        }
        private Expression Parse() => O();

        private Expression O()
        {
            var left = A();
            if (Current.Kind != TokenKind.InvalidToken || Current.Kind != TokenKind.EndToken)
            {
                while (Current.Kind == TokenKind.LogicalOrToken)
                {
                    NextToken();

                    var right = A();

                    left = new OrBinaryExpression(left, right);

                }
            }
            return left;
        }
        private Expression A()
        {
            var left = N();
            if (Current.Kind != TokenKind.InvalidToken || Current.Kind != TokenKind.EndToken)
            {
                while (Current.Kind == TokenKind.LogicalAndToken)
                {
                    NextToken();
                    var right = N();

                    left = new AndBinaryExpression(left, right);

                }
            }
            return left;

        }
        private Expression N()
        {
            var left = T();
            if (Current.Kind != TokenKind.InvalidToken || Current.Kind != TokenKind.EndToken)
            {
                while (Current.Kind == TokenKind.EqualEqualToken || Current.Kind == TokenKind.GreaterEqualToken ||
                       Current.Kind == TokenKind.GreateThanToken || Current.Kind == TokenKind.LessEqualToken ||
                       Current.Kind == TokenKind.DiferentToken || Current.Kind == TokenKind.LessThanToken)
                {
                    var operatortoken = Current; NextToken();
                    var right = T();

                    if (operatortoken.Kind == TokenKind.EqualEqualToken)
                    {
                        left = new EqualEqualBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.GreaterEqualToken)
                    {
                        left = new GreaterEqualBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.GreateThanToken)
                    {
                        left = new GreaterBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.LessEqualToken)
                    {
                        left = new LessEqualBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.LessThanToken)
                    {
                        left = new LessBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.DiferentToken)
                    {
                        left = new DiferentBinaryExpression(left, right);
                    }
                }
            }
            return left;
        }
        private Expression T()
        {
            var left = F();
            if (Current.Kind != TokenKind.InvalidToken || Current.Kind != TokenKind.EndToken)
            {
                while (Current.Kind == TokenKind.PlusToken || Current.Kind == TokenKind.MinusToken)
                {
                    var operatortoken = Current; NextToken();


                    var right = F();
                    if (operatortoken.Kind == TokenKind.PlusToken)
                    {
                        left = new PlusBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.MinusToken)
                    {
                        left = new MinusBinaryExpression(left, right);
                    }

                }
            }

            return left;


        }
        private Expression F()
        {
            var left = E(Current);

            if (Current.Kind != TokenKind.InvalidToken || Current.Kind != TokenKind.EndToken)
            {
                while (Current.Kind == TokenKind.StarToken || Current.Kind == TokenKind.SlashToken)
                {
                    var operatortoken = Current; NextToken();

                    var right = E(Current);

                    if (operatortoken.Kind == TokenKind.StarToken)
                    {
                        left = new StarBinaryExpression(left, right);
                    }
                    else if (operatortoken.Kind == TokenKind.SlashToken)
                    {
                        left = new SlashBinaryExpression(left, right);
                    }

                }
            }

            return left;
        }

        private Expression E(Token currentToken)
        {
            NextToken();

            if (currentToken.Kind == TokenKind.DrawToken)
            {
                isdrawvariable = true;
                var arg = Parse();
                isdrawvariable = false;
                return new DrawExpression(arg);
            }
            if (currentToken.Kind == TokenKind.ColorToken)
            {
                var colortoken = Current;
                NextToken();
                switch (colortoken.Kind)
                {
                    case TokenKind.BlueToken:
                        return new ColorExpression(new Color(0, 0, 1));
                    case TokenKind.RedToken:
                        return new ColorExpression(new Color(1, 0, 0));
                    case TokenKind.YellowToken:
                        return new ColorExpression(new Color(1, 1, 0));
                    case TokenKind.GreenToken:
                        return new ColorExpression(new Color(0, 1, 0));
                    case TokenKind.CyanToken:
                        return new ColorExpression(new Color(0, 1, 1));
                    case TokenKind.MagentaToken:
                        return new ColorExpression(new Color(0.8f, 0.1f, 1));
                    case TokenKind.WhiteToken:
                        return new ColorExpression(new Color(1, 1, 1));
                    case TokenKind.GrayToken:
                        return new ColorExpression(new Color(0.5f, 0.5f, 0.5f));
                    case TokenKind.BlackToken:
                        return new ColorExpression(new Color(0, 0, 0));
                    default:
                        return new ColorExpression(new Color(1, 1, 1));
                }
            }
            if (currentToken.Kind == TokenKind.RestoreToken)
            {
                return new RestoreExpression();
            }
            if (currentToken.Kind == TokenKind.MeasureToken)
            {
                var points = GetLinesArguments();
                return new MeasuereExpression(points.Item1, points.Item2);
            }
            if (currentToken.Kind == TokenKind.PointToken)
            {
                Point point = new Point(Random(), Current.Text, GetCurrentColor());
                Utils.Points.Add(point.Name, point);
                NextToken();
                return point;
            }
            if (currentToken.Kind == TokenKind.LineToken)
            {
                var line = new Line(Current.Text, Random(), Random(), GetCurrentColor());
                NextToken();
                return line;
            }
            if (currentToken.Kind == TokenKind.RayToken)
            {
                var ray = new Ray(Current.Text, Random(), Random(), GetCurrentColor());
                NextToken();
                return ray;
            }
            if (currentToken.Kind == TokenKind.SegmentToken)
            {
                var segment = new Segment(Current.Text, Random(), Random(), GetCurrentColor());
                NextToken();
                return segment;
            }
            if (currentToken.Kind == TokenKind.CircleToken)
            {
                Random random = new Random();
                var circle = new Circle(Current.Text, Random(), random.Next(40, 100), GetCurrentColor());
                NextToken();
                return circle;
            }
            if (currentToken.Kind == TokenKind.ArcToken)
            {
                Random random = new Random();
                var arc = new Arc(Current.Text, Random(), random.Next(40, 150), random.Next(0, 3), random.Next(3, 6), GetCurrentColor());
                NextToken();
                return arc;
            }
            if (currentToken.Kind == TokenKind.GeometricLineToken)
            {
                var points = GetLinesArguments();
                return new GeometricLine(points.Item1, points.Item2, GetCurrentColor());
            }
            if (currentToken.Kind == TokenKind.GeometricRayToken)
            {
                var points = GetLinesArguments();
                return new GeometricRay(points.Item1, points.Item2, GetCurrentColor());
            }
            if (currentToken.Kind == TokenKind.GeometricSegmentToken)
            {
                var points = GetLinesArguments();
                return new GeometricSegment(points.Item1, points.Item2, GetCurrentColor());
            }
            if (currentToken.Kind == TokenKind.GeometricCircleToken)
            {
                var arguments = GetCircleArguments();
                return new GeometricCircle(arguments.Item1, arguments.Item2, GetCurrentColor());
            }
            if (currentToken.Kind == TokenKind.GeometricArcToken)
            {
                var arguments = GetArcArguments();
                return new GeometricArc(arguments.Item1, arguments.Item2, arguments.Item3, arguments.Item4, GetCurrentColor());
            }
            if (currentToken.Kind == TokenKind.NumberToken)
            {
                return new NumberExpression(currentToken.Text);
            }
            if (currentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var parenthesisexpression = Parse();
                NextToken();
                return parenthesisexpression;
            }
            if (currentToken.Kind == TokenKind.MinusToken || currentToken.Kind == TokenKind.LogicalNegationToken)
            {
                var expression = E(Current);

                return new UnaryExpression(currentToken, expression);
            }
            if (currentToken.Kind == TokenKind.TrueToken || currentToken.Kind == TokenKind.FalseToken)
            {
                return new BoolExpression(currentToken);
            }
            if (currentToken.Kind == TokenKind.VariableToken || currentToken.Kind == TokenKind.UnderScoreToken)
            {
                if (Current.Kind == TokenKind.EqualToken)
                {
                    NextToken();
                    var expression = Parse();

                    if (expression is FigureExpression || expression is GeometricExpression)
                    {
                        if (expression is Point point)
                        {
                            Utils.Points.Add(currentToken.Text, point);
                        }
                    }
                    else
                        NameVar.Add(currentToken.Text);

                    return new AsignamentExpression(currentToken.Text, expression);

                }
                if (Current.Kind == TokenKind.CommaToken && !issequence)
                {
                    var variables = new List<Token> { currentToken };

                    while (Current.Kind == TokenKind.CommaToken)
                    {
                        NextToken(); variables.Add(Current); NextToken();
                    }
                    NextToken();

                    var sequence = Parse();

                    return new AsignamentSequenceExpression(variables, sequence);
                }
                else if (Utils.Points.ContainsKey(currentToken.Text))
                {
                    return Utils.Points[currentToken.Text];
                }
                else if (isfunctionvariable && !NameVar.Contains(currentToken.Text))
                {
                    return new FunctionVariableExpression(currentToken.Text);
                }
                else if (isdrawvariable && !NameVar.Contains(currentToken.Text))
                {
                    return new DrawVariableExpression(currentToken.Text);
                }
                else
                {
                    NameVar.Add(currentToken.Text);
                    return new VariableExpression(currentToken.Text);
                }
            }
            if (currentToken.Kind == TokenKind.IfToken)
            {
                var parenthesisexpression = Parse(); NextToken();

                var thenexpression = Parse(); NextToken();

                var elseexpression = Parse();

                return new ConditionalExpression(parenthesisexpression, thenexpression, elseexpression);
            }
            if (currentToken.Kind == TokenKind.LetToken)
            {
                List<Expression> LetExpressions = new List<Expression>();
                pos--;
                do
                {
                    NextToken();
                    if (Current.Kind == TokenKind.InToken) break;

                    LetExpressions.Add(Parse());

                } while (Current.Kind == TokenKind.FinalInstruccionToken);
                NextToken();

                var expression = Parse();

                return new LetInExpression(LetExpressions, expression);
            }
            if (currentToken.Kind == TokenKind.FunctionNameToken)
            {
                if (Utils.NameFunctions.Contains(currentToken.Text))
                {
                    var arguments = new List<Expression>();
                    NextToken();
                    do
                    {
                        arguments.Add(Parse());
                    } while (Current.Kind == TokenKind.CommaToken);
                    NextToken();
                    return new CallFunctionExpression(currentToken.Text, arguments);

                }
                else
                {
                    Utils.NameFunctions.Add(currentToken.Text);
                    var parameters = new List<string>();
                    NextToken();

                    do
                    {
                        parameters.Add(Current.Text);
                        NextToken();
                    } while (Current.Kind == TokenKind.CommaToken);

                    NextToken(); NextToken();

                    isfunctionvariable = true;

                    var expression = Parse();

                    isfunctionvariable = false;

                    return new DeclarateFunctionExpression(currentToken.Text, parameters, expression);
                }
            }
            if (currentToken.Kind == TokenKind.OpenKeyToken)
            {
                List<Expression> expressions = new List<Expression>();

                if (Current.Kind != TokenKind.ClosKeyToken)
                {

                    pos--;
                    do
                    {
                        NextToken();
                        issequence = true;
                        expressions.Add(Parse());
                        issequence = false;
                    }
                    while (Current.Kind == TokenKind.CommaToken);
                    NextToken();
                }

                return new SequenceExpression(expressions);


            }
            if (currentToken.Kind == TokenKind.PointsFunctionToken)
            {

                NextToken(); var figure = Parse(); NextToken();

                return new PointsFuncExpression(figure);
            }
            if (currentToken.Kind == TokenKind.IntersectToken)
            {
                NextToken();
                var argumetn1 = Parse();
                NextToken();
                var argumetn2 = Parse();
                NextToken();
                return new IntersectExpression(argumetn1, argumetn2);

            }
            if (currentToken.Kind == TokenKind.CountToken)
            {
                NextToken();
                var e = Parse();
                NextToken();
                return new CountExpression(e);
            }
            if (currentToken.Kind == TokenKind.SamplesToken)
            {
                return new SamplesExpression();
            }
            if (currentToken.Kind == TokenKind.RandomToken)
            {
                return new RandomExpression();
            }
            return new LexicalError("asdasdasd");
        }
    }
}