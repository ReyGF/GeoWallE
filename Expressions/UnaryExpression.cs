
namespace GeoWalle
{
    sealed class UnaryExpression : Expression
    {
        public Token Sign { get; }

        public Expression Expression { get; }

        public UnaryExpression(Token sign, Expression expression)
        {
            Sign = sign;
            Expression = expression;
        }
    }
}