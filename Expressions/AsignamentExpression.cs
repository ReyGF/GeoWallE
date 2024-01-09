namespace GeoWalle
{
    sealed class AsignamentExpression : Expression
    {
        public string VariableName { get; }
        public Expression Expression { get; }

        public AsignamentExpression(string variablename, Expression expression)
        {
            VariableName = variablename;
            Expression = expression;
        }

    }
}