using System.Linq;
using JsonParser.Lexer;

namespace JsonParser.Parser
{
    public class Parser
    {
        private readonly Lexer.Lexer _lexer;
        private Token _token;

        public Parser(Lexer.Lexer lexer)
        {
            _lexer = lexer;
            _token = _lexer.GetNextToken();
        }

        public void NextToken()
        {
            _token = _lexer.GetNextToken();
        }

        public bool ValidateToken(params TokenType[] tt)
        {
            var errors = tt.Count(tokenType => _token.TokenType != tokenType);

            return errors != tt.Length;
        }

        public void Parse()
        {
            Object();

            if (!ValidateToken(TokenType.EOF))
                throw new SyntaxException($"Expected EOF found {_token}");
        }
        
        private void Object()
        {
            KeyOpen();

            Pairs();

            KeyClose();
        }

        private void Pairs()
        {
            if (!IsValue())
                return;

            Pair();

            Pairs_tail();
        }
        
        private void Pair()
        {
            STRING();

            Colon();

            Value();
        }

        private void Pairs_tail()
        {
            if (!ValidateToken(TokenType.Comma))
                return; 

            Comma();

            Pairs();
        }


        private void Value()
        {
            if(!IsValue())
                throw new SyntaxException($"Expected Value found {_token}");
            
            if(ValidateToken(TokenType.StringLiteral))
                STRING();
            if(ValidateToken(TokenType.IntegerLiteral, TokenType.FloatLiteral))
                Number();
            if(ValidateToken(TokenType.RwTrue))
                True();
            if(ValidateToken(TokenType.RwFalse))
                False();
            if(ValidateToken(TokenType.Null))
                Null();
            if(ValidateToken(TokenType.KeyOpen))
                Object();
            if (ValidateToken(TokenType.BracketOpen))
                Array();
        }

        private void Array()
        {
            BracketOpen();

            Elements();

            BracketClose();
        }

        private void Elements()
        {
            if(!IsValue())
                return;

            Value();

            Element_tail();
        }

        private void Element_tail()
        {
            if(!ValidateToken(TokenType.Comma))
                return;

            Comma();

            Elements();
        }

        private void STRING()
        {
            if(!ValidateToken(TokenType.StringLiteral))
                throw new SyntaxException($"Expected String found {_token}");
            NextToken();
        }

        private void Null()
        {
            if(!ValidateToken(TokenType.Null))
                throw new SyntaxException($"Expected String found {_token}");
            NextToken();
        }

        private void Number()
        {
            if(!ValidateToken(TokenType.IntegerLiteral, TokenType.FloatLiteral))
                throw new SyntaxException($"Expected Number found {_token}");
            NextToken();
        }
        private void True()
        {
            if(!ValidateToken(TokenType.RwTrue))
                throw new SyntaxException($"Expected true found {_token}");
            NextToken();
        }
        private void False()
        {
            if(!ValidateToken(TokenType.RwFalse))
                throw new SyntaxException($"Expected false found {_token}");
            NextToken();
        }

        private void Colon()
        {
            if (!ValidateToken(TokenType.Colon))
                throw new SyntaxException($"Expected : found {_token}");
            NextToken();
        }

        private void BracketClose()
        {
            if (!ValidateToken(TokenType.BracketClose))
                throw new SyntaxException($"Expected ] found {_token}");
            NextToken();
        }

        private void BracketOpen()
        {
            if (!ValidateToken(TokenType.BracketOpen))
                throw new SyntaxException($"Expected [ found {_token}");
            NextToken();
        }

        private void KeyClose()
        {
            if (!ValidateToken(TokenType.KeyClose))
                throw new SyntaxException($"Expected }} found {_token}");
            NextToken();
        }

        private void KeyOpen()
        {
            if (!ValidateToken(TokenType.KeyOpen))
                throw new SyntaxException($"Expected {{ found {_token}");
            NextToken();
        }

        private void Comma()
        {
            if (!ValidateToken(TokenType.Comma))
                throw new SyntaxException($"Expected , found {_token}");
            NextToken();
        }

        private bool IsValue()
        {
            return ValidateToken(TokenType.StringLiteral, TokenType.FloatLiteral, TokenType.IntegerLiteral,
                TokenType.RwTrue, TokenType.RwFalse, TokenType.KeyOpen, TokenType.BracketOpen);
        }
    }
}