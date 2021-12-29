// System Namespaces
using System.Linq;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.AST.ANTLR;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    public static partial class Building
    {

        internal static bool IsLastBuilder(Builder builder, CPPModule module, Location location)
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
                if (!PcreRegex.IsMatch(module.Dictionary[loc], builder.Reference))
                    return false;

            return true;
        }

        public static string BuildMatches(KeyValuePair<string, Builder> builderKVP)
        {
            var builder = builderKVP.Value;

            var built = "";

            if (builder.Match != null)
            {
                var regex = new PcreRegex(builder.Match);

                var build = regex.Matches(builder.Context);

                foreach (var match in build)
                {
                    built += PcreRegex.Replace(match.Value, builder.Match, builder.Build);

                    if (build.Count() > 1)
                        built += "\n";
                }
            }

            return built;
        }


        public static string ResolveBuildExpression(KeyValuePair<string, Builder> builderKVP)
        {
            var build = ResolveBuilderExpressions(builderKVP.Value.Build, builderKVP);

            if (Resolving.NeedsResolving(build))
                build = Resolving.ResolveResolverExpressions(build, builderKVP);

            return build;
        }

        static string pattern_builder = @"#\{(.*?)\}";

        public static string ResolveBuilderExpressions(string build, KeyValuePair<string, Builder> builderKVP)
        {
            var builderVariables = builderKVP.GetVariables();

            foreach (var match in PcreRegex.Matches(build, pattern_builder))
            {
                var variable = match.Groups[1].Value;

                var var_pattern = @"#{" + variable + @"}";

                if (builderVariables.ContainsKey(variable))
                    build = PcreRegex.Replace(build, builderVariables[variable], var_pattern);
            }

            return build;
        }
    }

    public static class BuildingExtensions
    {

        public static Dictionary<string, string> GetVariables(this KeyValuePair<string, Builder> builderKVP)
        {
            var variables = new Dictionary<string, string>();

            var builder = builderKVP.Value;

            if (builder.Actions != null)
                foreach (var childBuilder in builder.Actions.Builders)
                    foreach (var var in GetVariables(childBuilder))
                        variables.Add(var.Key, var.Value);

            return variables;
        }

        /*
        public static List<Tuple<string, string>> GetVariables(this Builder builder, CPPModule module, Location location)
        {
            var variables = new List<Tuple<string, string>>();

            if (builder.Match != null)
            {
                var regex = new PcreRegex(builder.Match);

                var build = regex.Matches(module.Dictionary[location]);

                foreach (var match in build)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        var group = match.Groups[i];
                        
                        if (group.Success)
                            variables.Add(new Tuple<string, string>(group., group.Value));
                    }
                }
            }

            return variables;
        }
        

        public static List<Tuple<string, string>> GetVariablesRecursive(this Builder builder, CPPModule module, Location location)
        {
            var variables = new List<Tuple<string, string>>();

            foreach (var var in GetVariables(builder, module, location))
                variables.Add(var);

            if (builder.Actions != null)
                foreach (var child_builder in builder.Actions.Builders)
                    foreach (var var in GetVariablesRecursive(child_builder.Value, module, location))
                        variables.Add(var);

            return variables;
        }
        */
    }
}
