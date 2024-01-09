namespace GeoWalle
{
    sealed class LessBinaryExpression : BinaryExpression
    {
        public LessBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }

        public object Comparate(object left, object right)
        {
            return (double)left < (double)right;
        }
    }
}