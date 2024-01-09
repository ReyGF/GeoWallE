using System;
using System.Collections.Generic;
namespace GeoWalle
{
    sealed class PlusBinaryExpression : BinaryExpression, ICalculate
    {
        public PlusBinaryExpression(Expression left, Expression right) : base(left, right)
        {

        }
        public double Calculate(double left, double right)
        {
            return left + right;
        }
        public SequenceExpression UnionSequence(SequenceExpression left, SequenceExpression right)
        {
            var newSequence = new List<Expression>();
            for (int i = 0; i < left.Expressions.Count; i++)
            {
                newSequence.Add(left.Expressions[i]);
            }
            for (int i = 0; i < right.Expressions.Count; i++)
            {
                newSequence.Add(right.Expressions[i]);
            }
            return new SequenceExpression(newSequence);
        }
    }
}