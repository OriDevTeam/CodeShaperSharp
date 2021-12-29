// System Namespaces


// Application Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


// Library Namespaces


namespace Lib.AST.ANTLR
{
    public partial class CPPModuleListener : CPP14ParserBaseListener
    {
        AntlrInputStream InputStream;

        public CPPModuleListener(AntlrInputStream inputSteam)
        {
            InputStream = inputSteam;
        }

        public override void VisitErrorNode([NotNull] IErrorNode node)
        {
            base.VisitErrorNode(node);
        }

        public override void EnterTranslationUnit([NotNull] CPP14Parser.TranslationUnitContext context)
        {
            base.EnterTranslationUnit(context);
        }

        public override void EnterFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            base.EnterFunctionDefinition(context);
        }

        public override void EnterFunctionBody([NotNull] CPP14Parser.FunctionBodyContext context)
        {
            base.EnterFunctionBody(context);
        }

        public override void EnterDeclaration([NotNull] CPP14Parser.DeclarationContext context)
        {
            base.EnterDeclaration(context);
        }

    }

    public partial class CPPModuleListener
    {
        private string GetOriginalText(ParserRuleContext context)
        {
            var start = context.Start.StartIndex;

            var stop = -1;
            if (context.Stop == null)
                stop = context.Start.StopIndex;
            else
                stop = context.Stop.StopIndex;


            if (start > stop)
                return "";

            var interval = new Interval(start, stop);
            return InputStream.GetText(interval);
        }
    }
}
