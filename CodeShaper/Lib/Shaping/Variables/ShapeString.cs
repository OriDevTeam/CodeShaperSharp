// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces



namespace Lib.Shaping.Variables
{
    public class ShapeInteger : IShapeVariable
    {
        public string Name { get; set; }
        
        private int _value;

        public ShapeInteger(int value)
        {
            _value = value;
        }

        public string ProcessVariable(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            return Convert.ToString(_value);
        }
        

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }

        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null)
        {
            throw new NotImplementedException();
        }
    }
}
