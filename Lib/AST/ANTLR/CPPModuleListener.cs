// System Namespaces
using System.IO;


// Application Namespaces
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


// Library Namespaces


namespace Lib.AST.ANTLR.Generated
{
    public class CPPModuleListener : CPP14ParserBaseListener
    {
        private readonly StringWriter _writer;

        public CPPModuleListener(StringWriter writer)
        {
            _writer = writer;
        }

        public override void VisitErrorNode([NotNull] IErrorNode node)
        {
            base.VisitErrorNode(node);
        }

        public override void EnterLinkageSpecification([NotNull] CPP14Parser.LinkageSpecificationContext context)
        {
            base.EnterLinkageSpecification(context);
        }

        public override void EnterFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            base.EnterFunctionDefinition(context);
        }

        public override void EnterFunctionBody([NotNull] CPP14Parser.FunctionBodyContext context)
        {
            base.EnterFunctionBody(context);
        }
    }
}
