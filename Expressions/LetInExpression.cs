using System.Collections.Generic;
namespace GeoWalle
{
    sealed class LetInExpression : Expression
    {
        public List<Expression> ListExpression { get; }
        public Expression ToEvaluateExpreesion { get; }

        public LetInExpression(List<Expression> listexpression, Expression expression)
        {
            ListExpression = listexpression;
            ToEvaluateExpreesion = expression;
        }
    }
}