using GeoWalle;
namespace GeoWalle
{
    class LexicalError : ErrorExpression
    {
        public string Message { get; }

        public LexicalError(string message)
        {
            Message = message;
        }
    }
}