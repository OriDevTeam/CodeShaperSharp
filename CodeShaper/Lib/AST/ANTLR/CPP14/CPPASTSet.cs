// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.AST.ANTLR.CPP14.Generated;


// Library Namespaces
using Antlr4.Runtime;


namespace Lib.AST.ANTLR.CPP14
{
    public class CPPASTSet : IASTSet
    {
        public Type TypeLexer => typeof(CPP14Lexer);
        public Type TypeParser => typeof(CPP14Parser);
        public Type TypeVisitor => typeof(CPPASTVisitor);
        
        public AntlrInputStream InputStream { get; set; }
        public Lexer Lexer { get; set; }
        public Parser Parser { get; set; }
        public IASTVisitor Visitor { get; set; }
        
        public ParserRuleContext GetRootContext()
        {
            return ((CPP14Parser)Parser).translationUnit();
        }
    }
}
