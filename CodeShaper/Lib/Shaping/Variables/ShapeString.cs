// System Namespaces
using System;
using System.Collections.Generic;
using System.IO;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping.Interfaces;


// Library Namespaces
using Newtonsoft.Json;


namespace Lib.Shaping.Variables
{
    public class ShapeInteger : IShapeVariable
    {
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
    }
}
