// System Namespaces
using System.Collections.Generic;
using System.Linq;


// Application Namespaces
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shapers.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class ResolverExpressions : IActionExpressions
    {
        private const string ResolverPattern = @"#!\{(.*?)\}\((.*?)\)$";
        private const PcreOptions ResolverPatternFlags = PcreOptions.None;

        public string ProcessExpression(string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;

            foreach (var match in PcreRegex.Matches(expression, ResolverPattern, ResolverPatternFlags))
            {
                var varmatch = match.Groups[0].Value;
                var variable = match.Groups[1].Value;
                var args = match.Groups[2].Value;

                var exprVar = variables.FirstOrDefault(v => v.Name == variable);

                if (exprVar == null)
                    continue;

                var argse = ArgumentsExpressions.ProcessGroupExpressions(ExpressionsGroup.ActionsExpressions, args, variables, arguments);

                var variableValue = exprVar.ProcessVariable(variables, argse);

                processedExpression = processedExpression.Replace(varmatch, variableValue);
            }

            return processedExpression;
        }
    }
}
