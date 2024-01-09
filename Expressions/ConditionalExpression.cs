namespace GeoWalle
{
    sealed class ConditionalExpression : Expression
    {
        public Expression ParenthesisExpression { get; }
        public Expression ThenExpression { get; }
        public Expression ElseExpression { get; }

        public ConditionalExpression(Expression parenthesisExpression, Expression thenexpression,
                                    Expression elseexpression)
        {
            ParenthesisExpression = parenthesisExpression;
            ThenExpression = thenexpression;
            ElseExpression = elseexpression;
        }
    }
}