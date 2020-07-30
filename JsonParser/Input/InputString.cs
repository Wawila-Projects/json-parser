using JsonParser.Lexer;

namespace JsonParser.Input
{
        public class InputString : IInput
        {
            public string InitialInput { get; internal set; }
            public int CurrentChar { get; internal set; }
            public int ColumnCount { get; internal set; }
            public int RowCount { get; internal set; }

            public InputString(string input)
            {
                InitialInput = input;
                RowCount = 1;
                ColumnCount = 1;
                CurrentChar = 0;
            }

            public InputString()
            {

            }

            public void SetInput(string input)
            {
                InitialInput = input;
                RowCount = 1;
                ColumnCount = 1;
                CurrentChar = 0;
            }

            public Symbol GetNextSymbol()
            {
                if (CurrentChar >= InitialInput.Length) return new Symbol('\0', RowCount, ColumnCount);

                if (InitialInput[CurrentChar] == '\n')
                {
                    ++RowCount;
                    ColumnCount = 1;
                }

                return new Symbol
                {
                    Character = InitialInput[CurrentChar++],
                    Column = ColumnCount++,
                    Row = RowCount
                };
            }
        }
    
}