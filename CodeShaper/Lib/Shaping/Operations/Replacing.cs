// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.AST.ANTLR;
using Lib.AST.ANTLR.CPP14;

// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    internal static class Replacing
    {
        public static List<KeyValuePair<string, Replacement>> ProcessReplacing(ref string context, CPPModule module, string fileName, ShapeProject shapeProject, Location location)
        {
            var processedReplacements = new List<KeyValuePair<string, Replacement>>();

            var replacements = GetReplacements(module, fileName, shapeProject, location);

            if (replacements.Count > 0)
                foreach (var replacement in replacements)
                {
                    if (HasMatch(module, context, replacement.Value))
                    {
                        processedReplacements.Add(replacement);
                        context = ReplaceMatches(context, replacement);
                    }
                }

            return processedReplacements;
        }


        public static bool HasMatch(CPPModule module, string definition, Replacement replacement)
        {
            foreach (var match in replacement.From)
                if (PcreRegex.IsMatch(definition, match))
                    return true;

            return false;
        }


        public static Dictionary<string, Replacement> GetReplacements(CPPModule module, string FileName, ShapeProject shapeProject, Location location)
        {
            var replacements = new Dictionary<string, Replacement>();

            foreach (var patch in shapeProject.Patches)
            {
                if (!Matching.MatchesFile(FileName, patch))
                    continue;

                foreach (var replacement in patch.Patch.Actions.Replacements)
                {
                    var rep = replacement.Value;

                    if (rep.Location != location)
                        continue;

                    if (rep.ReferenceLocation != Location.None)
                        if (!PcreRegex.IsMatch(module.Dictionary[rep.ReferenceLocation], rep.Reference))
                            continue;

                    replacements.Add(replacement.Key, replacement.Value);
                }
            }

            return replacements;
        }

        public static string ReplaceMatches(string definition, KeyValuePair<string, Replacement> replacement)
        {
            var result = definition;

            foreach (var match in replacement.Value.From)
            {
                var regex = new PcreRegex(match);

                result = regex.Replace(result, replacement.Value.To);
            }

            Console.WriteLine(" - Applied '{0}'", replacement.Key);

            return result;
        }
    }
}
