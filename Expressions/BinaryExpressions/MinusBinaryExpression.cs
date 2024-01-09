namespace GeoWalle
{
    sealed class MinusBinaryExpression : BinaryExpression, ICalculate
    {
        public MinusBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}