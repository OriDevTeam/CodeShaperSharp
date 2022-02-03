// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Controllers;
using Lib.AST.Interfaces;
using Lib.AST.ANTLR.CSharp6.Generated;


// Library Namespaces



namespace Lib.AST.ANTLR.CSharp6
{
    public class CSharpASTVisitor : CSharpParserBaseVisitor<object>, IASTVisitor
    {
        public string Name => "CSharp 6 ANtlr4 AST Visitor";
        public string Alias => "csharp6";
        
        public ASTVisitorController<Enum> VisitorController { get; }

        public CSharpASTVisitor(ASTPreparationController preparationController)
        {
            VisitorController = new ASTVisitorController<Enum>(preparationController);
        }
    }
}
