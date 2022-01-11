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
    internal static class Adding
    {
        public static List<KeyValuePair<string, Addition>> ProcessAdding(
            ref string context, CPPModule module, string fileName, ShapeProject shapeProject, Location location)
        {
            var processedAdditions = new List<KeyValuePair<string, Addition>>();

            var additions = GetAdditions(module, fileName, shapeProject, location);

            if (additions.Count > 0)
                foreach (var addition in additions)
                {
                    processedAdditions.Add(addition);
                    context = AddMatches(context, addition);
                }

            return processedAdditions;
        }

        public static Dictionary<string, Addition> GetAdditions(CPPModule module, string fileName, ShapeProject shapeProject, Location location)
        {
            var additions = new Dictionary<string, Addition>();

            foreach (var patch in shapeProject.Patches)
            {
                if (!Matching.MatchesFile(fileName, patch))
                    continue;

                foreach (var addition in patch.Patch.Actions.Additions)
                {
                    var add = addition.Value;

                    if (add.Location != location)
                        continue;

                    if (add.ReferenceLocation != Location.None)
                        if (!PcreRegex.IsMatch(module.Dictionary[add.ReferenceLocation], add.Reference))
                            continue;

                    additions.Add(addition.Key, addition.Value);
                }
            }

            return additions;
        }

        public static string AddMatches(string definition, KeyValuePair<string, Addition> addition)
        {
            var result = definition;

            var add = addition.Value;

            if (add.Order == "before")
                result = add.Code + result;
            else
                result += add.Code;

            Console.WriteLine(" - Added '{0}'", addition.Key);

            return result;
        }
    }
}
