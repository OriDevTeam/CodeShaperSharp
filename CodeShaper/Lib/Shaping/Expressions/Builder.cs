// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class BuilderExpressions : IActionExpressions
    {
        private const string BuilderPattern = @"#\{(.*?)\}";
        private const PcreOptions BuilderPatternFlags = PcreOptions.None;

        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;


            foreach (var match in PcreRegex.Matches(expression, BuilderPattern, BuilderPatternFlags))
            {
                var varmatch = match.Groups[0].Value;
                var variable = match.Groups[1].Value;

                if (!variables.ContainsKey(variable))
                    continue;

                var variableValue = variables[variable].ProcessVariable(variables);

                processedExpression = processedExpression.Replace(varmatch, variableValue);
            }

            return processedExpression;
        }
    }
}
