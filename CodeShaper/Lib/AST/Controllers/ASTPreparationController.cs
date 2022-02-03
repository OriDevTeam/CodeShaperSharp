// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;


// Library Namespaces
using Antlr4.Runtime;


namespace Lib.AST.Controllers
{
    public class ASTPreparationController
    {
        public IASTSet ASTSet { get; }

        public ASTPreparationController(IASTSet astSet)
        {
            ASTSet = astSet;
        }

        public void Prepare(string fileContent)
        {
            ASTSet.InputStream = new AntlrInputStream(fileContent);
            
            ASTSet.Lexer = (Lexer)Activator.CreateInstance(ASTSet.TypeLexer, ASTSet.InputStream);
            var tokens = new CommonTokenStream(ASTSet.Lexer);
            
            ASTSet.Parser = (Parser)Activator.CreateInstance(ASTSet.TypeParser, tokens);
            ASTSet.Parser!.BuildParseTree = true;
            
            ASTSet.Visitor = (IASTVisitor)Activator.CreateInstance(ASTSet.TypeVisitor, this);
        }
    }
}
