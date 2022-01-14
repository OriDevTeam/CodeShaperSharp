// System Namespaces

using System.Collections.Generic;


// Application Namespaces



// Library Namespaces



namespace Lib.Shapers.Interfaces
{
    public interface IShapeVariable
    {
        public string Name { get; set; }
        
        public string ProcessVariable();
        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null);
    }
}
