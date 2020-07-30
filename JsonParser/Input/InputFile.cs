using System.IO;
using JsonParser.Lexer;

namespace JsonParser.Input
{
        public class InputFile : IInput
        {
            public string InitialInput { get; internal set; }
            public int CurrentChar { get; internal set; }
            public int ColumnCount { get; internal set; }
            public int RowCount { get; internal set; }

            public InputFile(string path)
            {
                string content;

                if (File.Exists(path))
                    content = File.ReadAllText(path);
                else
                    throw new FileNotFoundException(path);

                InitialInput = content;
                RowCount = 1;
                ColumnCount = 1;
                CurrentChar = 0;
            }

            public InputFile()
            {

            }


            public void SetFile(string path)
            {
                string content;

                if (File.Exists(path))
                    content = File.ReadAllText(path);
                else
                    throw new FileNotFoundException(path);

                InitialInput = content;
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