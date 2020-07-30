using System;
using System.Collections.Generic;
using System.Linq;
using JsonParser.Input;

namespace JsonParser.Lexer
{
    public class Lexer
    {
        private IInput _inputString;
        private Symbol _currentSymbol;
        private TokenLibrary _tokenLibrary;

        public Lexer(IInput inputString)
        {
            _inputString = inputString;
            _tokenLibrary = new TokenLibrary();
            _currentSymbol = inputString.GetNextSymbol();
        }

        public Lexer()
        {

        }

        public void InitializeLexer(IInput inputString)
        {
            _inputString = inputString;
            _tokenLibrary = new TokenLibrary();
            _currentSymbol = inputString.GetNextSymbol();
        }

        public List<Token> GetTokenList()
        {
            var tokenList = new List<Token>();
            var token = GetNextToken();

            while (token.TokenType != TokenType.EOF)
            {

                tokenList.Add(token);

                token = GetNextToken();
            }

            tokenList.Add(new Token("$EOF$", _currentSymbol.Row, _currentSymbol.Column, TokenType.EOF));

            return tokenList;
        }

        public void PrintTokenList(List<Token> tokensList)
        {
            Console.Out.WriteLine("{0,-6} {1,-6} {2,-40} {3,-10}\n", " Ln ", " Col", "Lexeme", " Type");
            foreach (var token in tokensList)
            {
                Console.Out.Write("{0,-6} {1,-6} {2,-40} {3,-10}\n",
                    token.Row, token.Column == 2 ? 1 : token.Column, token.Lexeme, token.TokenType);
            }
        }

        public Token GetNextToken()
        {
            while (_currentSymbol.WhatIs() == LexerUtils.SymbolType.WhiteSpace)
                _currentSymbol = _inputString.GetNextSymbol();

            if (_currentSymbol == '"')
                return BuildLetterToken();
            if (char.IsDigit(_currentSymbol.Character))
                return BuildNumberToken();
            if (char.IsPunctuation(_currentSymbol.Character))
                return BuildPunctuationToken();
            if (_currentSymbol == 'f' || _currentSymbol == 't')
                return BuildBoolToken();
            if (_currentSymbol == 'n')
                return BuildNullToken();
            if (_currentSymbol.WhatIs() == LexerUtils.SymbolType.EOF)
                return new Token("$EO$", _currentSymbol.Row, _currentSymbol.Column, TokenType.EOF);


            throw new LexerExecption("Invalid Token");
        }
        
        private Token BuildPunctuationToken()
        {
            if (!_tokenLibrary.SingleTokenDictionary.ContainsKey(_currentSymbol.Character))
                throw new LexerExecption($"Bad Symbol {_currentSymbol}");
            var type = _tokenLibrary.SingleTokenDictionary[_currentSymbol.Character];
            var lexeme = ""+  _currentSymbol;
            _currentSymbol = _inputString.GetNextSymbol();
            return new Token(lexeme, _currentSymbol.Row, _currentSymbol.Row, type);
        }

        private Token BuildNumberToken()
        {
            var lexeme = "";
            while (_currentSymbol.WhatIs() == LexerUtils.SymbolType.Number)
            {
                lexeme += _currentSymbol;
                _currentSymbol = _inputString.GetNextSymbol();

                if (_currentSymbol == '.')
                    return BuildFloatToken(lexeme);
            }

            return new Token(lexeme, _currentSymbol.Row, _currentSymbol.Column, TokenType.IntegerLiteral);
        }

        private Token BuildFloatToken(string lexeme)
        {
            lexeme += _currentSymbol;
            _currentSymbol = _inputString.GetNextSymbol();

            if (_currentSymbol.WhatIs() != LexerUtils.SymbolType.Number)
                throw new LexerExecption($"Expected Number found {_currentSymbol}");

            while (_currentSymbol.WhatIs() == LexerUtils.SymbolType.Number)
            {
                lexeme += _currentSymbol;
                _currentSymbol = _inputString.GetNextSymbol();
            }

            return new Token(lexeme, _currentSymbol.Row, _currentSymbol.Column, TokenType.FloatLiteral);
        }

        private Token BuildLetterToken()
        {

            var lexeme = "\"";
            var row = _currentSymbol.Row;
            var col = _currentSymbol.Column;
            _currentSymbol = _inputString.GetNextSymbol();

            var validESChars = new List<char>
            {
                'n', 'r', 't', '\'', '"', '\\', 'a', 'b',
                'f', 'v'
            };

            while (_currentSymbol.Character != '"')
            {
                if (_currentSymbol.WhatIs() == LexerUtils.SymbolType.EOF)
                    throw new LexerExecption(
                        $"String not closed in Line: {row} Column: {col}");

                if (_currentSymbol == '"' && _currentSymbol.Row != row)
                {
                    throw new LexerExecption(
                        $"New line not acceptedin Line: {row} Column: {col}");
                }

                var carryString = lexeme.Last() == '\\';
                lexeme += _currentSymbol;

                if ((!validESChars.Contains(_currentSymbol.Character) || lexeme[lexeme.Length - 2] != '\\')
                    && carryString
                )
                    throw new LexerExecption(
                        $"Escape Sequence not valid in Line: {row} Column: {col}");

                if (_currentSymbol == '"' && !carryString)
                    break;

                _currentSymbol = _inputString.GetNextSymbol();
            }

            _currentSymbol = _inputString.GetNextSymbol();

            lexeme += '"';
            if (_tokenLibrary.ReservedWordsDictionary.ContainsKey(lexeme))
            {
                var type = _tokenLibrary.ReservedWordsDictionary[lexeme];
                return new Token(lexeme, _currentSymbol.Row, _currentSymbol.Column, type);
            } 

            return new Token(lexeme, _currentSymbol.Row, _currentSymbol.Column, TokenType.StringLiteral);
        }

        private Token BuildBoolToken()
        {
            var lexeme = "";
            if (_currentSymbol == 'f')
            {
                while (char.IsLetter(_currentSymbol.Character))
                {
                    lexeme += _currentSymbol;
                    _currentSymbol = _inputString.GetNextSymbol();
                }

                if(lexeme != "false")
                    throw new LexerExecption("Not false");

                return new Token("false", _currentSymbol.Row, _currentSymbol.Column, TokenType.RwFalse);
            }

            while(char.IsLetter(_currentSymbol.Character))
            {
                lexeme += _currentSymbol;
                _currentSymbol = _inputString.GetNextSymbol();
            }

            if (lexeme != "true")
                throw new LexerExecption("Not true");

            return new Token("true", _currentSymbol.Row, _currentSymbol.Column, TokenType.RwTrue);
        }

        private Token BuildNullToken()
        {
            var lexeme = "";

                while (char.IsLetter(_currentSymbol.Character))
                {
                    lexeme += _currentSymbol;
                    _currentSymbol = _inputString.GetNextSymbol();
                }

                if (lexeme != "null")
                    throw new LexerExecption("Not null");

                return new Token("null", _currentSymbol.Row, _currentSymbol.Column, TokenType.Null);
            
        }
    }
}