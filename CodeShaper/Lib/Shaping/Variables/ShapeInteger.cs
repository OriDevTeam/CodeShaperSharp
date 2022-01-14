// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces



namespace Lib.Shaping.Variables
{
    public class ShapeString : IShapeVariable
    {
        public string Name { get; set; }
        
        private string _value;
        
        public ShapeString(string value)
        {
            _value = value;
        }

        public ShapeString(string name, string value)
        {
            Name = name;
            _value = value;
        }

        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null)
        {
            return _value;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
    }
}
