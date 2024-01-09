namespace GeoWalle
{
    class CountExpression : Expression
    {
        public Expression SeqExpression { get; }
        public CountExpression(Expression expression)
        {
            SeqExpression = expression;
        }

    }
}