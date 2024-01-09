using System.Collections.Generic;
namespace GeoWalle
{
    public sealed class DeclarateFunctionExpression : Expression
    {
        public string FunctionName { get; }
        public List<string> Parameters { get; }

        public Expression Expression { get; }

        public DeclarateFunctionExpression(string functionname, List<string> parameters, Expression expression)
        {
            FunctionName = functionname;
            Parameters = parameters;
            Expression = expression;
        }
    }
}