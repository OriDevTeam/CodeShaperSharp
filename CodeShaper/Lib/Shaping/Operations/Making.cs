// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    public static class Making
    {
    }

    public static class MakingExtensions
    {
        public static Dictionary<string, IShapeVariable> GetAllMakerVariables(this KeyValuePair<string, Builder> builderKVP)
        {
            var variables = new Dictionary<string, IShapeVariable>();

            var builder = builderKVP.Value;

            if (builder.Actions != null)
                foreach (var maker in builder.Actions.Makers)
                    variables.Add(maker.Key, maker.Value);

            return variables;
        }
    }
}
