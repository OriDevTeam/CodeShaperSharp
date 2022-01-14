// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces


namespace Lib.Shaping.Operations
{
    public static class Making
    {
    }

    public static class MakingExtensions
    {
        public static List<IShapeVariable> GetAllMakerVariables(this IShapeActionsBuilder builder)
        {
            var variables = new List<IShapeVariable>();

            if (builder.Actions == null) 
                return variables;
            
            foreach (var maker in builder.Actions.Makers)
                variables.Add(maker);

            return variables;
        }
    }
}
