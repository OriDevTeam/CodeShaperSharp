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
        static string pattern = @"#!\{{0}\}";
        static string pattern_resolver = @"#!\{(.*?)\}";
        static string pattern_builder = @"#!\{(.*?)\}\((.*?)\)";
        static string pattern_args = @"\b\w+(?=\s*[,()])?";

        public static bool NeedsResolving(string build)
        {
            return PcreRegex.IsMatch(build, pattern_resolver);
        }

        public static string ResolveResolverExpressions(string build, KeyValuePair<string, Builder> builderKVP)
        {
            string result = null;

            var resolver_info = PcreRegex.Match(build, pattern_builder);

            var resolver_name = resolver_info.Groups[1].Value;

            var resolver_args = new List<string>();

            foreach (var arg in PcreRegex.Matches(resolver_info.Groups[2].Value, pattern_args))
                foreach (var group in arg.Groups)
                    resolver_args.Add(group.Value);

            var resolver = FindResolver(builderKVP.Value, resolver_name);

            if (resolver.Mode == ResolverMode.List)
                result = ResolveList(resolver, result, resolver_args);
            else
                result = ResolveCases(resolver, result, resolver_args);

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
                if (PcreRegex.IsMatch(result, pattern))
                    result = new PcreRegex(pattern).Replace(result, pattern);

            }

            return result;
        }

        private static List<string> GetBuildVariables(string match)
        {

            var variables = new List<string>();

            if (PcreRegex.IsMatch(match, pattern_args))
                return variables;

            return variables;
        }
    }
}
