using System.Collections.Generic;

namespace JsonParser.Lexer
{
    public static class LexerUtils
    {
        public enum SymbolType
        {
            None, WhiteSpace, Letter, Number, Symbol, Punctuation, EscapeSequence, EOF 
        }

        public static bool IsEscapeSequence(this Symbol symbol)
        {
            var escapeSequences = new List<char>
            {
                '\n', '\r', '\t', '\'', '\"', '\\', '\a', '\b',
                '\f', '\v'
            };


            return escapeSequences.Contains(symbol.Character);
        }

        public static SymbolType WhatIs(this Symbol symbol)
        {
            if (symbol.Character == '\0')
                return SymbolType.EOF;

            if (char.IsWhiteSpace(symbol.Character))
                return SymbolType.WhiteSpace;

            if (char.IsLetter(symbol.Character))
                return SymbolType.Letter;

            if (char.IsSymbol(symbol.Character))
                return SymbolType.Symbol;

            if (char.IsPunctuation(symbol.Character))
                return SymbolType.Punctuation;

            if (symbol.IsEscapeSequence())
                return SymbolType.EscapeSequence;

            return char.IsDigit(symbol.Character) ? SymbolType.Number : SymbolType.None;
        }
    }
}