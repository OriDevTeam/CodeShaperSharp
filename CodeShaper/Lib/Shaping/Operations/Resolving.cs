// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    internal static class Resolving
    {
        private const string Pattern = @"#!\{{0}\}";
        private const string PatternResolver = @"#!\{(.*?)\}";
        private const string PatternBuilder = @"#!\{(.*?)\}\((.*?)\)";
        private const string PatternArgs = @"\b\w+(?=\s*[,()])?";

        public static bool NeedsResolving(string build)
        {
            return PcreRegex.IsMatch(build, PatternResolver);
        }

        public static string ResolveResolverExpressions(string build, KeyValuePair<string, Builder> builderKVP)
        {
            string result = null;

            var resolverInfo = PcreRegex.Match(build, PatternBuilder);

            var resolverName = resolverInfo.Groups[1].Value;

            var resolverArgs = new List<string>();

            foreach (var arg in PcreRegex.Matches(resolverInfo.Groups[2].Value, PatternArgs))
                foreach (var group in arg.Groups)
                    resolverArgs.Add(group.Value);

            var resolver = FindResolver(builderKVP.Value, resolverName);

            if (resolver.Mode == ResolverMode.List)
                result = ResolveList(resolver, result, resolverArgs);
            else
                result = ResolveCases(resolver, result, resolverArgs);

            return result;
        }

        public static Resolver FindResolver(Builder builder, string name)
        {
            if (builder.RootBuilder.Value.Actions != null)
                if (builder.RootBuilder.Value.Actions.Resolvers.ContainsKey(name))
                    return builder.RootBuilder.Value.Actions.Resolvers[name];

            return null;
        }

        public static string ResolveList(Resolver resolver, string match, List<string> parameters)
        {
            string result = null;

            return result;
        }

        public static string ResolveCases(Resolver resolver, string match, List<string> parameters)
        {
            string result = null;

            foreach (var scase in resolver.Cases)
            {
                if (PcreRegex.IsMatch(match, scase.Key))
                {
                    result = scase.Value;
                    break;
                }

            }


            foreach (var parameter in parameters)
            {
                if (PcreRegex.IsMatch(result, Pattern))
                    result = new PcreRegex(Pattern).Replace(result, Pattern);

            }

            return result;
        }

        private static List<string> GetBuildVariables(string match)
        {

            var variables = new List<string>();

            if (PcreRegex.IsMatch(match, PatternArgs))
                return variables;

            return variables;
        }
    }
}
