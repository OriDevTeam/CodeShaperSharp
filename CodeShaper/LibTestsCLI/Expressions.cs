// System Namespaces
using System.Collections.Generic;
using Lib.Shapers.Interfaces;

// Applications Namespaces
using Lib.Shaping.Expressions;
using Lib.Shaping.Variables;


// Library Namespaces



namespace LibTestsCLI
{
    internal static class TestExpressions
    {
        public static bool TestBuilderExpressions()
        {
            var rawExpression = "#{test_builder}";

            var variables = new List<IShapeVariable>()
            {
                new ShapeString("test_builder", "vartest"),
                new ShapeString("var2","vartest2")
            };

            var builderExpression = new BuilderExpressions();

            var result = builderExpression.ProcessExpression(rawExpression, variables);

            return true;
        }

        public static bool TestResolverExpressions()
        {
            var rawExpression = "#!{test_resolver}(arg1) #!{test_resolver}(arg1, #{var2}, #{test_resolver})";

            var variables = new List<IShapeVariable>()
            {
                new ShapeString("test_resolver", "vartest"),
                new ShapeString("var2", "vartest2")
            };

            var resolverExpression = new ResolverExpressions();

            var result = resolverExpression.ProcessExpression(rawExpression, variables);

            return true;
        }
    }
}
