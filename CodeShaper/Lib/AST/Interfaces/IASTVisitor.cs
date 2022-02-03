// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Controllers;


// Library Namespaces
using Antlr4.Runtime.Tree;


namespace Lib.AST.Interfaces
{
    public interface IASTVisitor
    {
        public string Name { get; }
        
        public string Alias { get; }
        
        public ASTVisitorController<Enum> VisitorController { get; }

        public object Visit(IParseTree tree);
    }
}
