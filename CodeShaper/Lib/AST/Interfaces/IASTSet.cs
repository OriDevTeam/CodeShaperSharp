// System Namespaces
using System;


// Application Namespaces


// Library Namespaces
using Antlr4.Runtime;


namespace Lib.AST.Interfaces
{
    public interface IASTSet
    {
        public Type TypeLexer { get; }
        public Type TypeParser { get; }
        public Type TypeVisitor { get; }
        
        public AntlrInputStream InputStream { get; set; }
        public Lexer Lexer { get; set; }
        public Parser Parser { get; set; }
        
        public IASTVisitor Visitor { get; set; }

        public ParserRuleContext GetRootContext();
    }
}
