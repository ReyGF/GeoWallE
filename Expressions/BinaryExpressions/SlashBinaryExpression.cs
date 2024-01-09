using System.Data;

namespace GeoWalle
{
    sealed class SlashBinaryExpression : BinaryExpression, ICalculate
    {
        public SlashBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public double Calculate(double left, double right)
        {
            return left / right;
        }
    }
}