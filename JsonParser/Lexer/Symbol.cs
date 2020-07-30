namespace JsonParser.Lexer
{
    public class Symbol
    {
        public char Character { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public Symbol(char character, int column, int row)
        {
            Character = character;
            Column = column;
            Row = row;
        }

        public Symbol()
        {
        }

        public override string ToString()
        {
            return "S: " + Character + " C: " + Column + " R: " + Row;
        }

        public static bool operator ==(Symbol symbol, char c)
        {
            return symbol != null && symbol.Character == c;
        }

        public static bool operator !=(Symbol symbol, char c)
        {
            return !(symbol == c);
        }

        public static string operator +(string str, Symbol symbol)
        {
            return str + symbol.Character;
        }
    }
}