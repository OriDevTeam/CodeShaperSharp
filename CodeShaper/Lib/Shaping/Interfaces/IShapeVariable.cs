// System Namespaces
using System.Collections.Generic;


// Application Namespaces



// Library Namespaces



namespace Lib.Shaping.Interfaces
{
    public interface IShapeVariable
    {
        public string ProcessVariable();
        public string ProcessVariable(Dictionary<string, IShapeVariable> variables, List<string> arguments = null);
    }
}
