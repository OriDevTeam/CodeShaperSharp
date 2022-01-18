// System Namespaces


// Application Namespaces


// Library Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;


namespace Lib.AST
{
    public class RootFinder : IParseTreeListener
    {
        public ParserRuleContext RootRuleContext { get; private set; }
        
        public void VisitTerminal(ITerminalNode node)
        {
        }

        public void VisitErrorNode(IErrorNode node)
        {
        }

        public void EnterEveryRule(ParserRuleContext ctx)
        {
            
            RootRuleContext ??= ctx;
        }

        public void ExitEveryRule(ParserRuleContext ctx)
        {
        }
    }
}