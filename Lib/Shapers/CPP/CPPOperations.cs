// System Namespaces
using Lib.AST.ANTLR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


// Application Namespaces


// Library Namespaces


namespace Lib.Shapers.CPP
{

    internal class Matching
    {
        public static bool MatchesFile(string fileName, ShapePatch patch)
        {
            return Regex.IsMatch(fileName, patch.Patch.FileSearch);
        }
    }

    internal class Adding
    {
        public static List<KeyValuePair<string, Addition>> ProcessAdding(ref string context, CPPModule module, string fileName, ShapeProject shapeProject, Location location)
        {
            var processedAdditions = new List<KeyValuePair<string, Addition>>();

            var additions = GetAdditions(module, fileName, shapeProject, location);

            if (additions.Count > 0)
                foreach (var addition in additions)
                {
                    if (addition.Key == "pybind_lib_init_external_code")
                        Console.WriteLine();

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
                        if (!Regex.IsMatch(module.Dictionary[add.ReferenceLocation], add.Reference))
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

    internal class Replacing
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
                if (Regex.IsMatch(definition, match))
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
                        if (!Regex.IsMatch(module.Dictionary[rep.ReferenceLocation], rep.Reference))
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
                var regex = new Regex(match);

                result = regex.Replace(result, replacement.Value.To);
            }

            Console.WriteLine(" - Applied '{0}'", replacement.Key);

            return result;
        }
    }

    internal class Subtracting
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
                        if (!Regex.IsMatch(module.Dictionary[sub.ReferenceLocation], sub.Reference))
                            continue;

                    subtractions.Add(subtraction.Key, subtraction.Value);
                }
            }

            return subtractions;
        }

        public static string SubtractMatches(string definition, KeyValuePair<string, Subtraction> subtraction)
        {
            var result = "";
            var sub = subtraction.Value;

            Console.WriteLine(" - Subtracted '{0}'", subtraction.Key);

            return result;
        }
    }
}
