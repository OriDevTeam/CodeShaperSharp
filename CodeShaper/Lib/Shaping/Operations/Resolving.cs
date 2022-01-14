// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shaping.Expressions.Interfaces;


// Library Namespaces



namespace Lib.Shaping.Operations
{
    public class ResolverExpressions : IActionExpressions
    {
        public string ProcessExpression(string expression, List<IShapeVariable> variables, List<string> arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public static class ResolvingExtensions
    {
        public static List<IShapeVariable> GetAllResolverVariables(this IShapeActionsBuilder builder)
        {
            var variables = new List<IShapeVariable>();

            if (builder.Actions == null)
                return variables;
            
            foreach (var resolver in builder.Actions.Resolvers)
                variables.Add(resolver);

            return variables;
        }
    }
}
