using System.Linq.Expressions;

namespace GeoWalle
{
    sealed class StarBinaryExpression : BinaryExpression, ICalculate
    {
        public StarBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public double Calculate(double left, double right)
        {
            return left * right;
        }

    }
}