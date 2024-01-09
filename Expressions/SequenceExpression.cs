using System.Collections.Generic;
namespace GeoWalle
{
    sealed class SequenceExpression : Expression
    {
        public List<Expression> Expressions { get; }

        public SequenceExpression(List<Expression> expressions)
        {
            Expressions = expressions;
        }
    }
}