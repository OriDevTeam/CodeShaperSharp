// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;


// Library Namespaces
using Antlr4.Runtime;
using Serilog;


namespace Lib.AST
{
    public class ASTPreparationController<TLexer, TParser, TVisitor>
    {
        private AntlrInputStream InputStream { get; set; }
        private Lexer Lexer { get; set; }
        private Parser Parser { get; set; }
        public IASTVisitor Visitor { get; set; }
        
        public ASTPreparationController()
        {
        }
        
        public void Prepare(string fileContent)
        {
            InputStream = new AntlrInputStream(fileContent);
            
            Lexer = Activator.CreateInstance(typeof(TLexer), InputStream) as Lexer;
            var tokens = new CommonTokenStream(Lexer);
            
            Parser = Activator.CreateInstance(typeof(TParser), tokens) as Parser;
            Parser!.BuildParseTree = true;
            
            Visitor = Activator.CreateInstance(typeof(TVisitor), InputStream, tokens) as IASTVisitor;
        }

        public void Visit()
        {
            Log.Information("Logs: ");
            var contextTree = Parser.GetInvokingContext(0);

            Visitor!.Visit(contextTree);
        }
    }
}
