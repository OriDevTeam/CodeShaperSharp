// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions.Interfaces
{
    public interface IActionExpressions
    {
        public string ProcessExpression(string expression, List<IShapeVariable> variables, List<string> arguments);
    }
}
