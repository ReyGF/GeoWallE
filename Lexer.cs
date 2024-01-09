using System.Collections.Generic;
namespace GeoWalle
{

    sealed class Lexer
    {
        private readonly string _text;
        private int pos;

        private char Current
        {
            get
            {
                if (pos >= _text.Length)  //1+2+3
                {
                    return '\0';
                }
                return _text[pos];
            }
        }
        public Lexer(string line)
        {
            _text = line;
        }
        public Token[] GetTokens()
        {
            var tokens = new List<Token>();

            while (pos < _text.Length)
            {
                tokens.Add(GetToken());
                Next();
            }
            return tokens.ToArray();
        }

        private Token GetToken()
        {
            while (char.IsWhiteSpace(Current))
            {
                Next();
            }
            if (char.IsLetter(Current))
            {
                string word = "";

                while (char.IsLetter(Current) || char.IsDigit(Current))
                {
                    word += Current;
                    Next();
                }
                Previous();

                switch (word)
                {
                    case "true":
                        return new Token(TokenKind.TrueToken, "true");
                    case "false":
                        return new Token(TokenKind.FalseToken, "false");
                    case "if":
                        return new Token(TokenKind.IfToken, "if");
                    case "then":
                        return new Token(TokenKind.ThenToken, "then");
                    case "else":
                        return new Token(TokenKind.ElseToken, "else");
                    case "let":
                        return new Token(TokenKind.LetToken, "let");
                    case "in":
                        return new Token(TokenKind.InToken, "in");
                    case "draw":
                        return new Token(TokenKind.DrawToken, "draw");
                    case "measure":
                        return new Token(TokenKind.MeasureToken, "measure");
                    case "undefined":
                        return new Token(TokenKind.UndefinedToken, "undefined");
                    case "rest":
                        return new Token(TokenKind.RestToken, "rest");
                    case "color":
                        return new Token(TokenKind.ColorToken, "color");
                    case "blue":
                        return new Token(TokenKind.BlueToken, "blue");
                    case "red":
                        return new Token(TokenKind.RedToken, "red");
                    case "yellow":
                        return new Token(TokenKind.YellowToken, "yellow");
                    case "green":
                        return new Token(TokenKind.GreenToken, "green");
                    case "cyan":
                        return new Token(TokenKind.CyanToken, "cyan");
                    case "magenta":
                        return new Token(TokenKind.MagentaToken, "magenta");
                    case "white":
                        return new Token(TokenKind.WhiteToken, "white");
                    case "gray":
                        return new Token(TokenKind.GrayToken, "gray");
                    case "black":
                        return new Token(TokenKind.BlackToken, "black");
                    case "restore":
                        return new Token(TokenKind.RestoreToken, "restore");
                    case "point":
                        return new Token(TokenKind.PointToken, "point");
                    case "points":
                        return new Token(TokenKind.PointsFunctionToken, "point");
                    case "intersec":
                        return new Token(TokenKind.IntersectToken, "intersect");
                    case "count":
                        return new Token(TokenKind.CountToken, "count");
                    case "samples":
                        return new Token(TokenKind.SamplesToken, "samples");
                    case "random":
                        return new Token(TokenKind.RandomToken, "random");
                    case "line":
                        if (IsFunction())
                            return new Token(TokenKind.GeometricLineToken, "line()");
                        else
                            return new Token(TokenKind.LineToken, "line");
                    case "ray":
                        if (IsFunction())
                            return new Token(TokenKind.GeometricRayToken, "ray()");
                        else
                            return new Token(TokenKind.RayToken, "ray");
                    case "segment":
                        if (IsFunction())
                            return new Token(TokenKind.GeometricSegmentToken, "segment()");
                        else
                            return new Token(TokenKind.SegmentToken, "segment");
                    case "circle":
                        if (IsFunction())
                            return new Token(TokenKind.GeometricCircleToken, "circle()");
                        else
                            return new Token(TokenKind.CircleToken, "circle");
                    case "arc":
                        if (IsFunction())
                            return new Token(TokenKind.GeometricArcToken, "arc()");
                        else
                            return new Token(TokenKind.ArcToken, "arc");
                    default:
                        Next();
                        if (Current == '(')
                        {
                            Previous();
                            return new Token(TokenKind.FunctionNameToken, word);
                        }
                        Previous();
                        return new Token(TokenKind.VariableToken, word);

                }
            }
            if (char.IsDigit(Current))
            {
                string n = "";
                while (char.IsDigit(Current))
                {
                    n += Current;
                    Next();
                }
                Previous();

                return new Token(TokenKind.NumberToken, n);
            }
            switch (Current)
            {
                case '+':
                    return new Token(TokenKind.PlusToken, "+");
                case '-':
                    return new Token(TokenKind.MinusToken, "-");
                case '*':
                    return new Token(TokenKind.StarToken, "*");
                case '/':
                    return new Token(TokenKind.SlashToken, "/");
                case '(':
                    return new Token(TokenKind.OpenParenthesisToken, "(");
                case ')':
                    return new Token(TokenKind.CloseParenthesisToken, ")");
                case '|':
                    return new Token(TokenKind.LogicalOrToken, "|");
                case '&':
                    return new Token(TokenKind.LogicalAndToken, "&");
                case ';':
                    return new Token(TokenKind.FinalInstruccionToken, ";");
                case ',':
                    return new Token(TokenKind.CommaToken, ",");
                case '_':
                    return new Token(TokenKind.UnderScoreToken, "_");
                case '{':
                    return new Token(TokenKind.OpenKeyToken, "{");
                case '}':
                    return new Token(TokenKind.ClosKeyToken, "}");
                case '!':
                    if (NextCharIs('='))
                        return new Token(TokenKind.DiferentToken, "!=");
                    else
                        return new Token(TokenKind.LogicalNegationToken, "!");
                case '>':

                    if (NextCharIs('='))
                        return new Token(TokenKind.GreaterEqualToken, ">=");
                    else
                        return new Token(TokenKind.GreateThanToken, ">");
                case '<':
                    if (NextCharIs('='))
                        return new Token(TokenKind.LessEqualToken, "<=");
                    else
                        return new Token(TokenKind.LessThanToken, "<");
                case '=':
                    if (NextCharIs('='))
                        return new Token(TokenKind.EqualEqualToken, "==");
                    else
                        return new Token(TokenKind.EqualToken, "=");
                default:
                    return new Token(TokenKind.InvalidToken, Current.ToString());
            }
        }
        private void Next()
        {
            pos++;
        }
        private void Previous()
        {
            pos--;
        }
        private bool NextCharIs(char nextchar)
        {
            pos++;
            if (Current == nextchar)
            {
                return true;
            }
            else
            {
                pos--;
                return false;
            }
        }
        private bool IsFunction()
        {
            pos++;
            if (Current == '(')
            {
                pos--;
                return true;
            }
            else
            {
                pos--;
                return false;
            }
        }



    }
}
