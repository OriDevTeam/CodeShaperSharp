// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;
using Lib.Shaping.Variables;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{

    public class MakerExpressions : IActionExpressions
    {
        private const string MakerPattern = @"#%\{(.*?)\}\((.*?)\)";
        private const PcreOptions MakerPatternFlags = PcreOptions.None;

        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var processedExpression = expression;

            var regex = new PcreRegex(MakerPattern, MakerPatternFlags);

            foreach (var match in regex.Matches(expression))
            {
                var varmatch = match.Groups[0].Value;
                var variable = match.Groups[1].Value;
                var args = match.Groups[2].Value;

                if (!variables.ContainsKey(variable))
                    continue;

                var maker = (Maker)variables[variable];

                var processedLocalArguments = ArgumentsExpressions.ProcessGroupExpressions(ExpressionsGroup.ActionsExpressions, args, variables);

                ProcessPrepareExpression(maker, variables, processedLocalArguments);

                foreach (var localVariable in maker.LocalVariables)
                    variables.Add(localVariable.Key, localVariable.Value);

                var processedMake = ProcessMakeExpression(maker, variables, arguments);

                /*
                var regexPrepare = new PcreRegex(args, maker.Prepare);
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    var groupName = regex.PatternInfo.GroupNames[i - 1];
                    maker.LocalVariables.Add(groupName, new ShapeString(match.Groups[i]));
                }
                */

                var variableValue = variables[variable].ProcessVariable(variables, processedLocalArguments);

                processedExpression = processedExpression.Replace(varmatch, variableValue);
            }

            return processedExpression;
        }

        
        public string ProcessPrepareExpression(Maker maker, Dictionary<string, IShapeVariable> variables, List<string> arguments)
        {
            var regex = new PcreRegex(maker.Prepare);

            foreach (var arg in arguments)
            {
                var match = regex.Match(arg);

                if (!match.Success)
                    continue;


                for (int i = 1; i < match.Groups.Count; i++)
                {
                    var groupName = regex.PatternInfo.GroupNames[i - 1];
                    maker.LocalVariables.Add(groupName, new ShapeString(match.Groups[i]));
                }
            }

            return "";
        }

        public string ProcessMakeExpression(Maker maker, Dictionary<string, IShapeVariable> variables, List<string> arguments)
        {
            return "";
        }
        
    }
}
