// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.Controllers;
using Lib.AST.Interfaces;
using Lib.AST.ANTLR.CPP14.Generated;
using Lib.Shapers.Patches;


// Library Namespaces
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PCRE;


namespace Lib.AST.ANTLR.CPP14
{
    public partial class CPPASTVisitor : CPP14ParserBaseVisitor<object>, IASTVisitor
    {
        public string Name => "CPP14 ANtlr4 AST Visitor";
        public string Alias => "cpp14";
        
        public ASTVisitorController<Enum> VisitorController { get; }

        public CPPASTVisitor(ASTPreparationController preparationController)
        {
            VisitorController = new ASTVisitorController<Enum>(preparationController);
        }
        
        public override object Visit(IParseTree tree)
        {
            var allText = VisitorController.AllText();
            
            VisitorController.ProcessCustomVisit(tree,
                allText, Location.Module,
                CustomVisitInclude(allText),
                new Func<IParseTree, object>(base.Visit));

            return null;
        }

        public override object VisitErrorNode(IErrorNode node)
        {
            VisitorController.ProcessVisitError(node,
                new Func<IErrorNode, object>(base.VisitErrorNode)
            );
            
            return null;
        }

        public override object VisitDeclaration([NotNull] CPP14Parser.DeclarationContext context)
        {
            VisitorController.ProcessVisit(context,
                Location.Declaration,
                new Func<CPP14Parser.DeclarationContext, object>(base.VisitDeclaration));

            return null;
        }

        public override object VisitDeclarationStatement([NotNull] CPP14Parser.DeclarationStatementContext context)
        {
            VisitorController.ProcessVisit(context,
                Location.DeclarationStatement,
                new Func<CPP14Parser.DeclarationStatementContext, object>(base.VisitDeclarationStatement));

            return null;
        }
        public override object VisitFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            VisitorController.ProcessCustomVisit(context,
                Location.FunctionDefinition,
                CustomFunctionBodyInclude(context),
                new Func<CPP14Parser.FunctionDefinitionContext, object>(base.VisitFunctionDefinition));

            return null;
        }

        public override object VisitFunctionBody([NotNull] CPP14Parser.FunctionBodyContext context)
        {
            var con = VisitorController.GetText(context);

            var functionBody = con.Substring(1, con.Length - 2);

            VisitorController.ProcessCustomVisit(context,
                functionBody, Location.FunctionBody,
                null,
                new Func<CPP14Parser.FunctionBodyContext, object>(base.VisitFunctionBody));

            return null;
        }
        public override object VisitCondition([NotNull] CPP14Parser.ConditionContext context)
        {
            VisitorController.ProcessVisit(context,
                Location.MethodCondition,
                new Func<CPP14Parser.ConditionContext, object>(base.VisitCondition));

            return null;
        }
    }

    public partial class CPPASTVisitor
    {
        private IEnumerable<Tuple<Enum, string>> CustomVisitInclude(string text)
        {
            var locationIncludes = new List<Tuple<Enum, string>>();
            
            var reg = new PcreRegex(@"#\s*+include\s*+[""<](.*?)["">]");

            var matches = reg.Matches(text);

            foreach (var match in matches)
                locationIncludes.Add(new Tuple<Enum, string>( Location.Include, match.Value));
            
            return locationIncludes;
        }

        private IEnumerable<Tuple<Enum, string>> CustomFunctionBodyInclude(CPP14Parser.FunctionDefinitionContext context)
        {
            var functionLocations = new List<Tuple<Enum, string>>();
            
            var functionContext = VisitorController.GetText(context);
            
            var functionBodyContext = VisitorController.GetText(context.functionBody());

            var functionBody = functionContext.Substring(0, functionContext.Length - functionBodyContext.Length);

            functionLocations.Add(new Tuple<Enum, string>(Location.Function, functionBody));

            return functionLocations;
        }
    }
}
