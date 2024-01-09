using System.Collections.Generic;

namespace GeoWalle
{
    class DrawExpression : Expression
    {
        public Expression Argument { get; set; }

        public DrawExpression(Expression expression)
        {
            Argument = expression;
        }
    }
}