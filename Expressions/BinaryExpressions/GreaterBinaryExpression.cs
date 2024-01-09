namespace GeoWalle
{
    sealed class GreaterBinaryExpression : BinaryExpression
    {
        public GreaterBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (double)left > (double)right;
        }
    }
}