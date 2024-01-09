using System.Collections.Generic;
namespace GeoWalle
{
    sealed class AsignamentSequenceExpression : Expression
    {
        public List<Token> Vars { get; }
        public Expression SequenceExpressions { get; }

        public AsignamentSequenceExpression(List<Token> vars, Expression sequence)
        {
            Vars = vars;
            SequenceExpressions = sequence;
        }

    }
}