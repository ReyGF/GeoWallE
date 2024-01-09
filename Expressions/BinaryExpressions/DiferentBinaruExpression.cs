using System;
namespace GeoWalle
{
    sealed class DiferentBinaryExpression : BinaryExpression, IComparate
    {
        public DiferentBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public object Comparate(object left, object right)
        {

            return !left.Equals(right);
        }
    }
}