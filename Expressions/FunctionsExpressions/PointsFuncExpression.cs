namespace GeoWalle
{
    class PointsFuncExpression : Expression
    {
        public Expression Figure { get; set; }

        public PointsFuncExpression(Expression figure)
        {
            Figure = figure;
        }

    }
}