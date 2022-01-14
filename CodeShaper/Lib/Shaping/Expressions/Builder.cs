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
    public class BuilderExpressions : IActionExpressions
    {
        private const string BuilderPattern = @"#\{(.*?)\}";
        private const PcreOptions BuilderPatternFlags = PcreOptions.None;

        public string ProcessExpression(string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;


            foreach (var match in PcreRegex.Matches(expression, BuilderPattern, BuilderPatternFlags))
            {
                var varmatch = match.Groups[0].Value;
                var variable = match.Groups[1].Value;
                
                var exprVar = variables.FirstOrDefault(v => v.Name == variable);
                
                if (exprVar == null)
                    continue;

                var variableValue = exprVar.ProcessVariable(variables);

                processedExpression = processedExpression.Replace(varmatch, variableValue);
            }

            return processedExpression;
        }
    }
}
