// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;


// Library Namespaces
using Antlr4.Runtime;
using Serilog;


namespace Lib.AST
{
    public class ASTPreparationController
    {
        public AntlrInputStream InputStream { get; set; }
        private Lexer Lexer { get; set; }
        private Parser Parser { get; set; }
        public IASTVisitor Visitor { get; set; }

        private Type TypeLexer { get; }
        private Type TypeParser { get; }
        private Type TypeVisitor { get; }
        
        public ParserRuleContext RootContextTree { get; set; }
        public RootFinder RootFinder { get; private set; }

        public ASTPreparationController(Type lexer, Type parser, Type visitor)
        {
            TypeLexer = lexer;
            TypeParser = parser;
            TypeVisitor = visitor;
        }
        
        public void Prepare(string fileContent)
        {
            InputStream = new AntlrInputStream(fileContent);
            
            Lexer = (Lexer)Activator.CreateInstance(TypeLexer, InputStream);
            var tokens = new CommonTokenStream(Lexer);

            RootFinder = new RootFinder();
            
            Parser = (Parser)Activator.CreateInstance(TypeParser, tokens);
            Parser?.AddParseListener(RootFinder);
            Parser!.BuildParseTree = true;
            
            Visitor = (IASTVisitor)Activator.CreateInstance(TypeVisitor, this);
        }

        public ParserRuleContext MakeRootContext()
        {
            Log.Information("Logs: ");
            
            return ((CPP14Parser)Parser).translationUnit();
            
            // return RootFinder.RootRuleContext;
        }
    }
}
