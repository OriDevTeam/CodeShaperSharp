// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping.Expressions.Interfaces;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Expressions.Interfaces
{
    public interface IActionExpressions
    {
        public string ProcessExpression(string expression, Dictionary<string, IShapeVariable> variables, List<string> arguments);
    }
}
