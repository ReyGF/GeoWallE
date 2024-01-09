using System;
namespace GeoWalle
{
    sealed class EqualEqualBinaryExpression : BinaryExpression, IComparate
    {
        public EqualEqualBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public object Comparate(object left, object right)
        {
            return left.Equals(right);
        }
    }
}