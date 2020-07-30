namespace JsonParser.Lexer
{
    public enum TokenType
    {
        None,
        Comma,
        Colon,
        KeyClose,
        KeyOpen,
        BracketClose,
        BracketOpen,
        RwTrue,
        RwFalse,
        StringLiteral,
        IntegerLiteral, 
        FloatLiteral,
        Null,
        EOF
    }
}