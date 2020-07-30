using JsonParser.Lexer;

namespace JsonParser.Input
{
    public interface IInput
    {
        Symbol GetNextSymbol();
    }
}