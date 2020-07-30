using System.Collections.Generic;

namespace JsonParser.Lexer
{
    public class TokenLibrary
    {
        public Dictionary<char, TokenType> SingleTokenDictionary { get; set; }
        public Dictionary<string, TokenType> ReservedWordsDictionary { get; set; }

        public TokenLibrary()
        {
            PopulateDictionary();
        }

        private void PopulateDictionary()
        {
            SingleTokenDictionary = new Dictionary<char, TokenType>
            {
                [':'] = TokenType.Colon,
                [','] = TokenType.Comma,
                ['{'] = TokenType.KeyOpen,
                ['}'] = TokenType.KeyClose,
                ['['] = TokenType.BracketOpen,
                [']'] = TokenType.BracketClose
            };

            ReservedWordsDictionary = new Dictionary<string, TokenType>
            {
                ["true"] = TokenType.RwTrue,
                ["false"] = TokenType.RwFalse
            };
        }
    }
}