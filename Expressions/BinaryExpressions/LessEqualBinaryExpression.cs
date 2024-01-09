namespace GeoWalle
{
    sealed class LessEqualBinaryExpression : BinaryExpression
    {
        public LessEqualBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (double)left <= (double)right;
        }
    }
}