// System Namespaces
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers;


// Library Namespaces
using Antlr4.Runtime;
using static CPP14Parser;


namespace Lib.AST.ANTLR
{
    public class CPPModuleAST
    {
        string FilePath;
        string FileName;
        string FileContent;

        ShapeProject ShapeProject;

        public CPPModuleAST(ShapeProject shapeProject, string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            FileContent = File.ReadAllText(filePath);
            ShapeProject = shapeProject;
        }

        public CPPResult ParseAndProcessModule()
        {
            var inputStream = new AntlrInputStream(FileContent);
            var lexer = new CPP14Lexer(inputStream);
            var tokens = new CommonTokenStream(lexer);

            var result = new CPPResult(ShapeProject, inputStream.ToString(), FileName);

            Console.WriteLine(string.Concat(Enumerable.Repeat("-", Console.WindowWidth)));

            Console.WriteLine();

            var parser = new CPP14Parser(tokens)
            {
                BuildParseTree = true
            };

            Console.WriteLine("Logs: ");
            var tree = parser.translationUnit();

            Console.WriteLine("");

            CPPModuleVisitor superVisitor = new(ShapeProject, result, inputStream, tokens);

            var trees = new List<TranslationUnitContext>
            {
                parser.translationUnit()
            };

            superVisitor.Visit(tree);

            Console.WriteLine();

            return result;
        }
    }
}
