namespace GeoWalle
{
    sealed class OrBinaryExpression : BinaryExpression, IComparate
    {
        public OrBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (bool)left || (bool)right;
        }

    }
}