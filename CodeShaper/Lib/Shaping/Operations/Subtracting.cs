// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.AST.ANTLR;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{

    internal static class Subtracting
    {
        public static List<KeyValuePair<string, Subtraction>> ProcessSubtractions(ref string context, CPPModule module, string fileName, ShapeProject shapeProject, Location location)
        {
            var processedSubtractions = new List<KeyValuePair<string, Subtraction>>();

            var subtractions = GetSubtractions(module, fileName, shapeProject, location);

            if (subtractions.Count > 0)
                foreach (var subtraction in subtractions)
                {
                    processedSubtractions.Add(subtraction);
                    context = SubtractMatches(context, subtraction);
                }

            return processedSubtractions;
        }

        public static Dictionary<string, Subtraction> GetSubtractions(CPPModule module, string FileName, ShapeProject shapeProject, Location location)
        {
            var subtractions = new Dictionary<string, Subtraction>();

            foreach (var patch in shapeProject.Patches)
            {
                if (!Matching.MatchesFile(FileName, patch))
                    continue;

                foreach (var subtraction in patch.Patch.Actions.Subtractions)
                {
                    var sub = subtraction.Value;

                    if (sub.Location != location)
                        continue;

                    if (sub.ReferenceLocation != Location.None)
                        if (!PcreRegex.IsMatch(module.Dictionary[sub.ReferenceLocation], sub.Reference))
                            continue;

                    subtractions.Add(subtraction.Key, subtraction.Value);
                }
            }

            return subtractions;
        }

        public static string SubtractMatches(string definition, KeyValuePair<string, Subtraction> subtraction)
        {
            var sub = subtraction.Value;
            var result = ""; // definition.Replace(sub.Reference, definition);

            Console.WriteLine(" - Subtracted '{0}'", subtraction.Key);

            return result;
        }
    }
}
