// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces


namespace Lib.Shaping.Interfaces
{
    internal interface IShapeResult
    {
        public abstract string FileName { get; set; }
        public abstract string FileContent { get; set; }

        public abstract List<IShapeActionsBuilder> Builders { get; set;}

        public abstract List<Tuple<IShapeActionsReplacer, string, string>> Replacements { get; set; }
        public abstract List<Tuple<IShapeActionsAdder, string, string>> Additions { get; set; }
        public abstract List<Tuple<IShapeActionsSubtracter, string, string>> Subtractions { get; set; }
        
    }
}
