using System;
using System.Collections.Generic;
using JsonParser.Input;

namespace JsonParser
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path0 = @"c:\users\david chavarria\documents\visual studio 2017\Projects\JsonParser\JsonParser\JsonTest0.json";
            const string path1 = @"c:\users\david chavarria\documents\visual studio 2017\Projects\JsonParser\JsonParser\JsonTest1.json";
            const string path2 = @"c:\users\david chavarria\documents\visual studio 2017\Projects\JsonParser\JsonParser\JsonTest2.json";

            var test = new List<IInput>
            {
                new InputFile(path0),
                new InputFile(path1),
                new InputFile(path2)
            };

            foreach (var input in test)
            {
                Console.Out.WriteLine($"Start");

                var lexer = new Lexer.Lexer(input);
                var parser = new Parser.Parser(lexer);
                parser.Parse();

                Console.Out.WriteLine("Done");
            }
            
            Console.ReadKey();
        }
    }
}