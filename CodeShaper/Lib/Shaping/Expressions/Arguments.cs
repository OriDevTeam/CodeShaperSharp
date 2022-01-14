// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions
{
    public class ArgumentsExpressions
    {
        private const string ArgsPattern = @"[^,]+(?=,)?";
        private const PcreOptions ArgsPatternFlags = PcreOptions.Extended;

        public static List<string> ProcessExpressions(string expression)
        {
            var args = new List<string>();

            foreach (var arg in PcreRegex.Matches(expression, ArgsPattern, ArgsPatternFlags))
                foreach (var group in arg.Groups)
                    args.Add(group.Value);

            return args;
        }

        public static List<string> ProcessGroupExpressions(ExpressionsGroup group, string expression, List<IShapeVariable> variables, List<string> arguments = null)
        {
            var args = ProcessExpressions(expression);

            var processedArguments = new List<string>();

            foreach (var arg in args)
                processedArguments.Add(Groups.ProcessExpressionsGroup(group, arg, variables, arguments));

            return processedArguments;
        }
    }
}
