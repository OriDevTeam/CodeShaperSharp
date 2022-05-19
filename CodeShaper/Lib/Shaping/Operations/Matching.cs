// System Namespaces
using System;
using System.Collections.Generic;
using System.Linq;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;
using Lib.Utility.Extensions;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{

    public static class Matching
    {
        public static bool MatchesFile(string fileName, IShapePatch patchOld)
        {
            return PcreRegex.IsMatch(fileName, patchOld.FileSearch);
        }
    }

    public static partial class Building
    {
        public static List<IShapeActionsBuilder> GetRootBuilders(string fileName, List<ShapePatchFile> patches)
        {
            var builders = new List<IShapeActionsBuilder>();
            
            foreach (var patch in patches)
                builders.AddRange(GetRootBuilders(fileName, patch));
            
            return builders;
        }
        
        public static List<IShapeActionsBuilder> GetRootBuilders(string fileName, ShapePatchFile patch)
        {
            if (!Matching.MatchesFile(fileName, patch.Patch))
                return new List<IShapeActionsBuilder>();

            var builders = patch.Patch.Actions.Builders.ToList();

            // TODO: Builders hierarchical preparation should be done somewhere else, as this is a simple getter
            foreach (var builder in builders)
            {
                builder.RootBuilder = builder;

                PrepareChildrenBuilders(builder);
            }

            return builders;
        }

        private static void PrepareChildrenBuilders(IShapeActionsBuilder parent)
        {
            if (parent.Actions?.Builders == null)
                return;

            foreach (var childBuilder in parent.Actions.Builders)
            {
                childBuilder.ParentBuilder = parent;
                childBuilder.RootBuilder = parent.RootBuilder;

                PrepareChildrenBuilders(childBuilder);
            }
        }

        public static bool ProcessBuilder(this IShapeActionsBuilder builder, IASTVisitor visitor, Enum location)
        {
            var context = visitor.VisitorController.LocationsContent[location];
            
            var build = false;

            if (builder.ActiveBuilder == null)
            {
                if (ShouldEnter(builder, visitor, location))
                {
                    builder.ActiveBuilder = builder;

                    builder.Context = context;
                }
            }

            if (builder.ActiveBuilder != null)
            {
                builder.ActiveBuilder = ProcessNextBuilder(builder.ActiveBuilder, visitor, context, location, ref build);
            }

            return build;
        }

        private static IShapeActionsBuilder ProcessNextBuilder(
            this IShapeActionsBuilder builder, IASTVisitor visitor,
            string context, Enum location, ref bool build)
        {
            build = false;

            var activeBuilder = builder.ActiveBuilder;

            IShapeActionsBuilder nextBuilder;

            if (builder.ActiveBuilder != null)
            {
                nextBuilder = builder.ActiveBuilder.GetNextBuilder(visitor, location);

                if (nextBuilder != null)
                {
                    if (ShouldEnter(nextBuilder, visitor, location))
                    {
                        activeBuilder = nextBuilder;
                        activeBuilder.Context = context;
                        activeBuilder.ActiveBuilder = activeBuilder;
                        activeBuilder = ProcessNextBuilder(activeBuilder, visitor, context, location, ref build);
                    }
                }
            }

            if (builder.ActiveBuilder.IsLastDepthBuilder())
            {
                var vars = builder.GetAllVariables();

                builder.ActiveBuilder.Result = builder.ProcessVariable(vars);
                build = true;
            }

            if (builder.ActiveBuilder.IsLastBranchBuilder())
            {
                var vars = builder.GetAllVariables();

                builder.RootBuilder.Result = builder.RootBuilder.ProcessVariable(vars);
                builder.ActiveBuilder = null;
                build = true;
                return builder.ActiveBuilder;
            }

            return activeBuilder;
        }

        private static IShapeActionsBuilder GetNextBuilder(this IShapeActionsBuilder builder,
            IASTVisitor visitor, Enum location)
        {
            if (builder.Actions != null)
                return builder.Actions.Builders.FirstOrDefault();
            else if (builder.ParentBuilder != null)
                return builder.ParentBuilder.Actions.Builders.Next(x => x == builder);

            return null;
        }

        private static bool IsLastBranchBuilder(this IShapeActionsBuilder builder)
        {
            if (builder.ParentBuilder == null)
                return false;

            var tempBuilders = new List<IShapeActionsBuilder>();
            foreach (var childBuilders in builder.ParentBuilder.Actions.Builders)
                tempBuilders.Add(childBuilders);

            var lastBuilder = tempBuilders[tempBuilders.Count - 1];

            return builder == lastBuilder;
        }

        private static bool IsLastDepthBuilder(this IShapeActionsBuilder builder)
        {
            if (builder.Actions == null)
                return true;

            return builder.Actions.Builders == null || builder.Actions.Builders.Count < 1;
        }
    }
}
