// System Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.AST.ANTLR;
using Lib.AST.ANTLR.CPP14;
using Lib.Shaping.Interfaces;
using Lib.Shaping.Variables;
using Lib.Utility.Extensions;



// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    public static partial class Building
    {

        /// <summary>
        /// Checks if the given builder is the last builder in the root builer tree
        /// </summary>
        /// <param name="builder">Builder to check</param>
        /// <returns>Whether its the last builder or not</returns>
        internal static bool IsLastBuilder(Builder builder)
        {
            if (builder == GetLastBuilder(builder.RootBuilder.Value))
                return true;

            return false;
        }

        internal static Builder GetLastBuilder(Builder builder)
        {
            if (builder.Actions == null)
                return builder;

            List<KeyValuePair<string, Builder>> tempBuilders = new();

            foreach (var childBuilder in builder.Actions.Builders)
                tempBuilders.Add(childBuilder);


            KeyValuePair<string, Builder> lastParentChildBuilder = tempBuilders[tempBuilders.Count - 1];


            return GetLastBuilder(lastParentChildBuilder.Value);
        }

        public static bool ShouldEnter(Builder builder, CPPModule module, Location location)
        {
            if (builder.Location != location)
                return false;

            Location loc;
            if (builder.ReferenceLocation != Location.None)
                loc = builder.ReferenceLocation;
            else
                loc = location;

            if (builder.Reference != null)
            {

                var regex = new PcreRegex(builder.Reference, PcreOptionsExtensions.FromString(builder.ReferenceFlags));
                var match = regex.Match(module.Dictionary[loc]);

                if (!match.Success)
                    return false;

                for (int i = 1; i < match.Groups.Count; i++)
                {
                    var groupName = regex.PatternInfo.GroupNames[i - 1];
                    builder.LocalVariables.Add(groupName, new ShapeString(match.Groups[i]));
                }
            }

            return true;
        }
    }

    public static class BuildingExtensions
    {
        public static Dictionary<K, V> Merge<K, V>(IEnumerable<Dictionary<K, V>> dictionaries)
        {
            return dictionaries.SelectMany(x => x)
                            .ToDictionary(x => x.Key, y => y.Value);
        }

        public static Dictionary<string, IShapeVariable> GetAllVariables(this KeyValuePair<string, Builder> builderKVP)
        {
            return Merge(new List<Dictionary<string, IShapeVariable>>()
            {
                builderKVP.GetAllResolverVariables(),
                builderKVP.GetAllBuilderVariables(),
                builderKVP.GetAllMakerVariables()
            });
        }

        public static Dictionary<string, IShapeVariable> GetAllBuilderVariables(this KeyValuePair<string, Builder> builderKVP)
        {
            var variables = new Dictionary<string, IShapeVariable>();

            var builder = builderKVP.Value;

            if (builder.Actions != null)
                foreach (var childBuilder in builder.Actions.Builders)
                {
                    variables.Add(childBuilder.Key, childBuilder.Value);

                    foreach (var var in GetAllBuilderVariables(childBuilder))
                        variables.Add(var.Key, var.Value);
                }

            foreach (var localVariable in builder.LocalVariables)
                variables.Add(localVariable.Key, localVariable.Value);

            return variables;
        }
    }

    public static class PcreOptionsExtensions
    {
        public static PcreOptions FromString(string flags)
        {
            var options = PcreOptions.None;

            if (flags == null)
                return options;

            foreach (var option in flags)
                options |= (PcreOptions)(option.ToString().ToEnum<PcreOptionsExtension>());

            return options;
        }

        [Flags]
        public enum PcreOptionsExtension : long
        {
            None = 0x0L,
            IgnoreCase = 0x8L,
            Singleline = 0x20L,
            ExplicitCapture = 0x2000L,
            IgnorePatternWhitespace = 0x80L,
            Unicode = 0x20000L,
            JavaScript = 0x202L,
            Compiled = 0x100000000L,
            CompiledPartial = 0x200000000L,
            Caseless = 0x8L,
            MultiLine = 0x400L,
            DotAll = 0x20L,
            NoAutoCapture = 0x2000L,

            [EnumMember(Value = "x")]
            Extended = 0x80L,

            ExtendedMore = 0x1000000L,
            AltBsUX = 0x2L,
            MatchUnsetBackref = 0x200L,
            Literal = 0x2000000L,
            Ucp = 0x20000L,
            MatchInvalidUtf = 0x4000000L,
            Anchored = 0x80000000L,
            EndAnchored = 0x20000000L,

            [EnumMember(Value = "U")]
            Ungreedy = 0x40000L,

            FirstLine = 0x100L,
            DupNames = 0x40L,
            AutoCallout = 0x4L,
            NoStartOptimize = 0x10000L,
            NoAutoPossess = 0x4000L,
            DollarEndOnly = 0x10L,
            AltCircumflex = 0x200000L,
            AltVerbNames = 0x400000L,
            AllowEmptyClass = 0x1L,
            NoDotStarAnchor = 0x8000L,
            NoUtfCheck = 0x40000000L,
            NeverUcp = 0x800L,
            NeverBackslashC = 0x100000L,
            UseOffsetLimit = 0x800000L,
            Utf = 0x80000L
        }
    }
}
