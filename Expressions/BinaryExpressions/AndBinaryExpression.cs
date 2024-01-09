namespace GeoWalle
{
    sealed class AndBinaryExpression : BinaryExpression, IComparate
    {
        public AndBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (bool)left && (bool)right;
        }

    }
}