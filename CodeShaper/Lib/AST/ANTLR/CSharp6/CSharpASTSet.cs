// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.AST.ANTLR.CSharp6.Generated;


// Library Namespaces
using Antlr4.Runtime;


namespace Lib.AST.ANTLR.CSharp6
{
    public class CSharpASTSet : IASTSet
    {
        public Type TypeLexer => typeof(CSharpLexer);
        public Type TypeParser => typeof(CSharpParser);
        public Type TypeVisitor => typeof(CSharpASTVisitor);
        
        public AntlrInputStream InputStream { get; set; }
        public Lexer Lexer { get; set; }
        public Parser Parser { get; set; }
        public IASTVisitor Visitor { get; set; }
        
        public ParserRuleContext GetRootContext()
        {
            return ((CSharpParser)Parser).compilation_unit();
        }
    }
}
