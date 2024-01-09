using System.Collections.Generic;
namespace GeoWalle
{
    sealed class CallFunctionExpression : Expression
    {
        public string FunctionName { get; }

        public List<Expression> ArgumentValues { get; }

        public CallFunctionExpression(string name, List<Expression> argumentvalues)
        {
            FunctionName = name;
            ArgumentValues = argumentvalues;
        }
    }
}