using System;

namespace JsonParser.Lexer
{
    public class Token
    {
        public string Lexeme { get; set; }
        public int Column { get; }
        public int Row { get; }

        public TokenType TokenType { get; set; }

        public Token(string lexeme, int row, int column, TokenType tokenType)
        {
            TokenType = tokenType;
            Row = row;
            Column = column;
            Lexeme = lexeme;

            if (tokenType == TokenType.None)
                throw new LexerExecption($"Token: {Lexeme} in Line: {Row} Column: {Column}");
        }

        public override string ToString()
        {
            return "L: " + Lexeme + " R: " + Row + " C: " + Column + " T: " + TokenType;
        }

        public static Token operator +(Token t1, Token t2)
        {
            if (t1.TokenType != t2.TokenType)
                throw new LexerExecption(
                    $"{t1.Lexeme} and {t2.Lexeme} not compatible");

            t1.Lexeme += t2.Lexeme;

            return t1;
        }


        public static bool operator ==(Token t1, Token t2)
        {
            return t2 != null && t1 != null && t1.Lexeme == t2.Lexeme && t1.TokenType == t2.TokenType && t1.Column == t2.Column && t1.Row == t2.Row;
        }

        public static bool operator !=(Token t1, Token t2)
        {
            return !(t1 == t2);
        }

        protected bool Equals(Token other)
        {
            return string.Equals(Lexeme, other.Lexeme) && Column == other.Column && Row == other.Row && TokenType == other.TokenType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Lexeme != null ? Lexeme.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Column;
                hashCode = (hashCode * 397) ^ Row;
                hashCode = (hashCode * 397) ^ (int)TokenType;
                return hashCode;
            }
        }
    }
    
}