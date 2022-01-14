// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shaping.Expressions.Interfaces;


// Library Namespaces



namespace Lib.Shaping.Expressions
{
    public static class Groups
    {
        public static string ProcessExpressionsGroup(ExpressionsGroup expressionsGroup, string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            if (expressionsGroup == ExpressionsGroup.ActionsExpressions)
                return ProcessActionsExpressions(expression, variables, arguments);

            return ProcessRegexExpressions(expression);
        
        }

        public static string ProcessActionsExpressions(string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            var expressionProcessors = new List<IActionExpressions>
            {
                new BuilderExpressions(),
                new ResolverExpressions(),
                new MakerExpressions(),
                new LocalExpressions()
            };

            var result = expression;

            foreach (var processor in expressionProcessors)
                result = processor.ProcessExpression(result, variables, arguments);

            return result;
        }

        public static string ProcessRegexExpressions(string expression)
        {
            return "";
        }
    }

    public enum ExpressionsGroup
    {
        ActionsExpressions,
        RegexExpressions
    }
}
