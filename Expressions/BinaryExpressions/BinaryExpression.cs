namespace GeoWalle
{
    interface ICalculate
    {
        double Calculate(double left, double right);
    }
    interface IComparate
    {
        object Comparate(object left, object right);
    }

    abstract class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

    }
}