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

        public ASTPreparationController()
        {
        }
        
        public void Prepare(string fileContent)
        {
            var inputStream = new AntlrInputStream(fileContent);
            var lexer = Activator.CreateInstance(typeof(TLexer), inputStream) as Lexer;
            var tokens = new CommonTokenStream(lexer);
            
            var parser = Activator.CreateInstance(typeof(TParser), tokens) as Parser;
            parser!.BuildParseTree = true;

            Log.Information("Logs: ");
            var contextTree = parser.GetInvokingContext(0);

            var visitor = Activator.CreateInstance(typeof(TVisitor), inputStream, tokens) as IASTVisitor;

            visitor!.Visit(contextTree);
        }

        public void Visit()
        {
            
        }
    }
}
