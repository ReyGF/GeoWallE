namespace GeoWalle
{
    sealed class GreaterEqualBinaryExpression : BinaryExpression
    {
        public GreaterEqualBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (double)left >= (double)right;
        }
    }
}