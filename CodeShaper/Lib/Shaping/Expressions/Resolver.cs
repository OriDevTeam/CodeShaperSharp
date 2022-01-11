// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class ResolverExpressions : IActionExpressions
    {
        private const string ResolverPattern = @"#!\{(.*?)\}\((.*?)\)$";
        private const PcreOptions ResolverPatternFlags = PcreOptions.None;

        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;

            foreach (var match in PcreRegex.Matches(expression, ResolverPattern, ResolverPatternFlags))
            {
                var varmatch = match.Groups[0].Value;
                var variable = match.Groups[1].Value;
                var args = match.Groups[2].Value;

                if (!variables.ContainsKey(variable))
                    continue;

                var argse = ArgumentsExpressions.ProcessGroupExpressions(ExpressionsGroup.ActionsExpressions, args, variables, arguments);

                var variableValue = variables[variable].ProcessVariable(variables, argse);

                processedExpression = processedExpression.Replace(varmatch, variableValue);
            }

            return processedExpression;
        }
    }
}
