// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping;


// Library Namespaces


namespace Lib.Shapers.Interfaces
{
    internal interface IShapeResult
    {
        public abstract string FileName { get; set; }
        public abstract string FileContent { get; set; }

        public abstract List<KeyValuePair<string, Builder>> Builders { get; set;}

        public abstract List<Tuple<KeyValuePair<string, Replacement>, string, string>> Replacements { get; set; }
        public abstract List<Tuple<KeyValuePair<string, Addition>, string, string>> Additions { get; set; }
        public abstract List<Tuple<KeyValuePair<string, Subtraction>, string, string>> Subtractions { get; set; }

        public abstract ShapeProject ShapeProject { get; set; }
    }
}
