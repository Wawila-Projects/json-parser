using System;

namespace JsonParser.Lexer
{
    internal class LexerExecption : Exception
    {
        public LexerExecption()
        {
        }

        public LexerExecption(string message) : base(message)
        {
        }

        public LexerExecption(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}