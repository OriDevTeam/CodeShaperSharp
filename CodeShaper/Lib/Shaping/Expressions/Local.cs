// System Namespaces
using System.Collections.Generic;
using System.Linq;


// Application Namespaces
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Variables;
using Lib.Shapers.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class LocalExpressions : IActionExpressions
    {

        private const string LocalPattern = @"#@\{(.*?)\}";
        private const PcreOptions LocalPatternFlags = PcreOptions.Ungreedy;

        public string ProcessExpression(string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;

            foreach (var match in PcreRegex.Matches(expression, LocalPattern, LocalPatternFlags))
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

        public static string ProcessExpressionArgs(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            if (arguments != null)
            {
                string joinedArgs = "";

                foreach (var arg in arguments)
                    joinedArgs += arg;

                
                variables.Add("args", new ShapeString(joinedArgs));
                variables.Add("args_count", new ShapeInteger(arguments.Count)); 
            }

            return "";
        }
    }
}
