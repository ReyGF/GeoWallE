using System.Linq.Expressions;
using GeoWalle;
namespace GeoWalle
{
    sealed class NumberExpression : Expression
    {
        public double Number { get; }
        public NumberExpression(string number)
        {
            Number = double.Parse(number);
        }
    }
}