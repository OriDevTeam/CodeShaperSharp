// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    public class ResolverExpressions : IActionExpressions
    {

        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class ResolvingExtensions
    {
        public static Dictionary<string, IShapeVariable> GetAllResolverVariables(this KeyValuePair<string, Builder> builderKVP)
        {
            var variables = new Dictionary<string, IShapeVariable>();

            var builder = builderKVP.Value;

            if (builder.Actions != null)
                foreach (var resolver in builder.Actions.Resolvers)
                    variables.Add(resolver.Key, resolver.Value);

            return variables;
        }
    }
}
