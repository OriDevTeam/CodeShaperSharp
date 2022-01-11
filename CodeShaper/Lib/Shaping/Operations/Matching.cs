// System Namespaces
using System.Linq;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.AST.ANTLR;
using Lib.AST.ANTLR.CPP14;
using Lib.Utility.Extensions;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{

    public static class Matching
    {
        public static bool MatchesFile(string fileName, ShapePatch patch)
        {
            return PcreRegex.IsMatch(fileName, patch.Patch.FileSearch);
        }
    }

    public static partial class Building
    {
        public static Dictionary<string, Builder> GetTopBuilders(string fileName, ShapeProject shapeProject)
        {
            var builders = new Dictionary<string, Builder>();

            foreach (var patch in shapeProject.Patches)
            {
                if (!Matching.MatchesFile(fileName, patch))
                    continue;

                foreach (var builder in patch.Patch.Actions.Builders)
                {
                    var bui = builder.Value;
                    builders.Add(builder.Key, builder.Value);
                }
            }

            foreach (var builder in builders)
            {
                var bui = builder.Value;

                bui.RootBuilder = builder;

                PrepareChildrenBuilders(builder);
            }

            return builders;
        }

        private static void PrepareChildrenBuilders(KeyValuePair<string, Builder> parentKVP)
        {
            var parent = parentKVP.Value;

            if (parent.Actions == null)
                return;

            foreach (var childBuilder in parent.Actions.Builders)
            {
                var childBui = childBuilder.Value;

                childBui.ParentBuilder = parentKVP;
                childBui.RootBuilder = parent.RootBuilder;

                PrepareChildrenBuilders(childBuilder);
            }
        }

        public static bool ProcessBuilder(this KeyValuePair<string, Builder> builderKVP, CPPModule module, string context, Location location)
        {
            var build = false;

            var builder = builderKVP.Value;

            if (builder.ActiveBuilder.Value == null)
            {
                if (ShouldEnter(builder, module, location))
                {
                    builder.ActiveBuilder = builderKVP;

                    builder.Context = context;
                }
            }

            if (builder.ActiveBuilder.Value != null)
            {
                builder.ActiveBuilder = ProcessNextBuilder(builderKVP.Value.ActiveBuilder, module, context, location, ref build);
            }

            return build;
        }

        public static KeyValuePair<string, Builder> ProcessNextBuilder(this KeyValuePair<string, Builder> builderKVP, CPPModule module, string context, Location location, ref bool build)
        {
            build = false;

            var activeBuilder = builderKVP.Value.ActiveBuilder;

            var builder = builderKVP.Value;


            KeyValuePair<string, Builder> nextBuilder = new();

            if (builder.ActiveBuilder.Value != null)
            {
                nextBuilder = builder.ActiveBuilder.GetNextBuilder(module, location);

                if (nextBuilder.Value != null)
                {
                    if (ShouldEnter(nextBuilder.Value, module, location))
                    {
                        activeBuilder = nextBuilder;
                        activeBuilder.Value.Context = context;
                        activeBuilder.Value.ActiveBuilder = activeBuilder;
                        activeBuilder = ProcessNextBuilder(activeBuilder, module, context, location, ref build);
                    }
                }
            }

            if (builder.ActiveBuilder.Value.IsLastDepthBuilder())
            {
                var vars = builderKVP.GetAllVariables();

                builder.ActiveBuilder.Value.Result = builderKVP.Value.ProcessVariable(vars);
                build = true;
            }

            if (builder.ActiveBuilder.Value.IsLastBranchBuilder())
            {
                var vars = builderKVP.GetAllVariables();

                builder.RootBuilder.Value.Result = builderKVP.Value.RootBuilder.Value.ProcessVariable(vars);
                builder.ActiveBuilder = new KeyValuePair<string, Builder>();
                build = true;
                return builder.ActiveBuilder;
            }

            return activeBuilder;
        }


        public static KeyValuePair<string, Builder> GetNextBuilder(this KeyValuePair<string, Builder> builderKVP, CPPModule module, Location location)
        {
            var builder = builderKVP.Value;

            if (builder.Actions != null)
                return builder.Actions.Builders.FirstOrDefault();
            else if (builder.ParentBuilder.Key != null)
                return builder.ParentBuilder.Value.Actions.Builders.Next(x => x.Value == builder);

            return new KeyValuePair<string, Builder>();
        }

        public static bool IsLastBranchBuilder(this Builder builder)
        {
            if (builder.ParentBuilder.Value == null)
                return false;

            var tempBuilders = new List<Builder>();
            foreach (var childBuilders in builder.ParentBuilder.Value.Actions.Builders)
                tempBuilders.Add(childBuilders.Value);

            var lastBuilder = tempBuilders[tempBuilders.Count - 1];

            if (builder == lastBuilder)
                return true;

            return false;
        }

        public static bool IsLastDepthBuilder(this Builder builder)
        {
            if (builder.Actions == null)
                return true;

            return builder.Actions.Builders == null || builder.Actions.Builders.Count < 1;
        }
    }
}
