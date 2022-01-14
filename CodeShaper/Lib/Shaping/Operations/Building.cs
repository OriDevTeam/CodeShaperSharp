// System Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers.Interfaces;
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
        internal static bool IsLastBuilder(IShapeActionsBuilder builder)
        {
            return builder == GetLastBuilder(builder.RootBuilder);
        }

        private static IShapeActionsBuilder GetLastBuilder(IShapeActionsBuilder builder)
        {
            while (true)
            {
                if (builder.Actions == null) 
                    return builder;

                var tempBuilders = builder.Actions.Builders.ToList();

                var lastParentChildBuilder = tempBuilders[tempBuilders.Count - 1];

                builder = lastParentChildBuilder;
            }
        }

        private static bool ShouldEnter(IShapeActionsBuilder builder, IASTVisitor visitor, Enum location)
        {
            if (!Equals(builder.Location, location))
                return false;

            var loc = builder.ReferenceLocation ?? location;

            if (builder.Reference == null) 
                return true;
            
            var regex = new PcreRegex(builder.Reference, PcreOptionsExtensions.FromString(builder.ReferenceFlags));
            var match = regex.Match(visitor.VisitorController.LocationsContent[loc]);

            if (!match.Success)
                return false;

            for (var i = 1; i < match.Groups.Count; i++)
            {
                var groupName = regex.PatternInfo.GroupNames[i - 1];
                builder.LocalVariables.Add(
                    new ShapeString( match.Groups[i])
                    {
                        
                    }
                );
            }

            return true;
        }
    }

    public static class BuildingExtensions
    {
        private static Dictionary<TK, TV> Merge<TK, TV>(IEnumerable<Dictionary<TK, TV>> dictionaries)
        {
            return dictionaries.SelectMany(x => x)
                            .ToDictionary(x => x.Key, y => y.Value);
        }
        
        public static List<IShapeVariable> GetAllVariables(this IShapeActionsBuilder builder)
        {
            return builder.GetAllResolverVariables()
                .Concat(builder.GetAllBuilderVariables())
                .Concat(builder.GetAllMakerVariables())
                .ToList();
            
        }

        private static List<IShapeVariable> GetAllBuilderVariables(this IShapeActionsBuilder builder)
        {
            var variables = new List<IShapeVariable>();
            
            if (builder.Actions != null)
                foreach (var childBuilder in builder.Actions.Builders)
                {
                    variables.Add(childBuilder);

                    foreach (var variable in GetAllBuilderVariables(childBuilder))
                        variables.Add(variable);
                }

            foreach (var variable in builder.LocalVariables)
                variables.Add(variable);

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
