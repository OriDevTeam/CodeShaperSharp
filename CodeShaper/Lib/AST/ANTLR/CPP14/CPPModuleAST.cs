// System Namespaces
using System;
using System.Collections.Generic;
using System.IO;


// Application Namespaces

// Library Namespaces
using Antlr4.Runtime;
using Lib.Shaping;
using static CPP14Parser;


namespace Lib.AST.ANTLR.CPP14
{
    public class CPPModuleAST
    {
        string FileName;
        string FileContent;

        ShapeProject ShapeProject;

        public CPPModuleAST(ShapeProject shapeProject, string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FileContent = File.ReadAllText(filePath);
            ShapeProject = shapeProject;
        }

        public ShapeResult ParseAndProcessModule()
        {
            var inputStream = new AntlrInputStream(FileContent);
            var lexer = new CPP14Lexer(inputStream);
            var tokens = new CommonTokenStream(lexer);

            var result = new ShapeResult(ShapeProject, inputStream.ToString(), FileName);

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

        public ShapeResult ParseAndProcess()
        {
            var inputStream = new AntlrInputStream(FileContent);
            var lexer = new CPP14Lexer(inputStream);
            var tokens = new CommonTokenStream(lexer);

            var result = new ShapeResult(ShapeProject, inputStream.ToString(), FileName);

            var listener = new CPPModuleListener(inputStream);

            var parser = new CPP14Parser(tokens)
            {
                BuildParseTree = true
            };

            parser.AddParseListener(listener);

            Console.WriteLine("Logs: ");
            var tree = parser.translationUnit();

            return result;
        }
    }
}
