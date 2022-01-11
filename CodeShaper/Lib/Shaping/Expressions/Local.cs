// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;
using Lib.Shaping.Variables;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class LocalExpressions : IActionExpressions
    {

        private const string LocalPattern = @"#@\{(.*?)\}";
        private const PcreOptions LocalPatternFlags = PcreOptions.Ungreedy;

        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;

            foreach (var match in PcreRegex.Matches(expression, LocalPattern, LocalPatternFlags))
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
