namespace GeoWalle
{
    sealed class Token
    {
        public TokenKind Kind { get; }
        public string Text { get; }

        public Token(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }
    }

}