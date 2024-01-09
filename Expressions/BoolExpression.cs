namespace GeoWalle
{
    class BoolExpression : Expression
    {
        public bool Value { get; }
        public BoolExpression(Token booltoken)
        {
            Value = booltoken.Text == "true";
        }

    }
}