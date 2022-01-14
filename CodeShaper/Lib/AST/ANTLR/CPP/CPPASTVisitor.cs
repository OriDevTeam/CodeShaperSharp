// System Namespaces
using System;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers.CPP;


// Library Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PCRE;
using Serilog;


namespace Lib.AST.ANTLR.CPP
{
    public class CPPASTVisitor : CPP14ParserBaseVisitor<object>, IASTVisitor
    {
        public string Name => "CPP14 ANtlr4 AST Visitor";
        public ASTVisitorController<Enum> VisitorController { get; }

        public CPPASTVisitor(AntlrInputStream inputStream)
        {
            VisitorController = new ASTVisitorController<Enum>(inputStream);
        }
        
        public void Visit()
        {
            throw new NotImplementedException();
        }
        
        public override object VisitErrorNode(IErrorNode node)
        {
            VisitorController.State = VisitorState.Stop;
            
            Log.Error($"ERROR: Visitor cannot parse node {node}");

            return base.VisitErrorNode(node);
        }

        public override object Visit(IParseTree tree)
        {
            var allText = VisitorController.AllText();
            
            CustomVisitInclude(allText);
            
            return VisitorController.ProcessCustomVisit(tree,
                allText,
                Location.Module,
                new Func<IParseTree, object>(base.Visit));
        }

        private void CustomVisitInclude(string text)
        {
            var reg = new PcreRegex(@"#\s*+include\s*+[""<](.*?)["">]");

            var matches = reg.Matches(text);

            foreach (var match in matches)
                VisitorController.ProcessCustomVisit(match.Value, Location.Include);
        }
        
        public override object VisitDeclaration([NotNull] CPP14Parser.DeclarationContext context)
        {
            return VisitorController.ProcessVisit(context,
                Location.Declaration,
                new Func<CPP14Parser.DeclarationContext, object>(base.VisitDeclaration));
        }

        public override object VisitDeclarationStatement([NotNull] CPP14Parser.DeclarationStatementContext context)
        {
            return VisitorController.ProcessVisit(context,
                Location.DeclarationStatement,
                new Func<CPP14Parser.DeclarationStatementContext, object>(base.VisitDeclarationStatement));
        }
        public override object VisitFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            var con = VisitorController.GetText(context);
            var functionBodyContext = VisitorController.GetText(context.functionBody());

            var functionBody = con.Substring(0, con.Length - functionBodyContext.Length);

            VisitorController.ProcessCustomVisit(functionBody, Location.Function);
            
            return VisitorController.ProcessVisit(context,
                Location.FunctionDefinition,
                new Func<CPP14Parser.FunctionDefinitionContext, object>(base.VisitFunctionDefinition));
        }

        public override object VisitFunctionBody([NotNull] CPP14Parser.FunctionBodyContext context)
        {
            var con = VisitorController.GetText(context);

            var body = con.Substring(1, con.Length - 2);

            return VisitorController.ProcessCustomVisit(context,
                body, 
                Location.FunctionBody,
                new Func<CPP14Parser.FunctionBodyContext, object>(base.VisitFunctionBody));
        }
        public override object VisitCondition([NotNull] CPP14Parser.ConditionContext context)
        {
            return VisitorController.ProcessVisit(context,
                Location.MethodCondition,
                new Func<CPP14Parser.ConditionContext, object>(base.VisitCondition));
        }

    }
}
