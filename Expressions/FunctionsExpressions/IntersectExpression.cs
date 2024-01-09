namespace GeoWalle
{
    class IntersectExpression : Expression
    {
        public Expression Figure1 { get; }

        public Expression Figure2 { get; }

        public IntersectExpression(Expression fig1, Expression fig2)
        {
            Figure1 = fig1;
            Figure2 = fig2;
        }
    }
}