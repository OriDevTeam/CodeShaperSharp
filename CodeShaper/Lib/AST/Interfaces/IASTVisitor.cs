﻿// System Namespaces


// Application Namespaces
using Lib.Shapers.CPP;


// Library Namespaces
using Antlr4.Runtime.Tree;


namespace Lib.AST.Interfaces
{
    public interface IASTVisitor : IParseTreeVisitor<object>
    {
        public string Name { get; }
        public ASTVisitorController<Location> VisitorController { get; }
        
        public void Visit();
    }
}
