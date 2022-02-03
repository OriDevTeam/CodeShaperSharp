// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    internal static class Replacing
    {
        public static List<IShapeActionsReplacer> ProcessReplacing(
            ref string context, IASTVisitor visitor, string fileName,
            List<IShapePatch> patches, Enum location)
        {
            var processedReplacements = new List<IShapeActionsReplacer>();

            var replacements = GetReplacements(visitor, fileName, patches, location);

            if (replacements.Count > 0)
                foreach (var replacement in replacements)
                {
                    if (HasMatch(context, replacement))
                    {
                        processedReplacements.Add(replacement);
                        context = ReplaceMatches(context, replacement);
                    }
                }

            return processedReplacements;
        }


        private static bool HasMatch(string definition, IShapeActionsReplacer replacement)
        {
            foreach (var match in replacement.From)
                if (PcreRegex.IsMatch(definition, match))
                    return true;

            return false;
        }


        private static List<IShapeActionsReplacer> GetReplacements(
            IASTVisitor visitor, string FileName,
            List<IShapePatch> patches, Enum location)
        {
            var replacements = new List<IShapeActionsReplacer>();

            foreach (var patch in patches)
            {
                if (!Matching.MatchesFile(FileName, patch))
                    continue;

                foreach (var replacer in patch.Header.Actions.Replacers)
                {
                    if (!Equals(replacer.Location, location))
                        continue;

                    if (replacer.ReferenceLocation != null)
                        if (replacer.Reference != null)
                            if (!PcreRegex.IsMatch(visitor.VisitorController.LocationsContent[replacer.ReferenceLocation],
                                    replacer.Reference))
                                continue;

                    replacements.Add(replacer);
                }
            }

            return replacements;
        }

        private static string ReplaceMatches(string definition, IShapeActionsReplacer replacement)
        {
            var result = definition;

            foreach (var match in replacement.From)
            {
                var regex = new PcreRegex(match);

                result = regex.Replace(result, replacement.To);
            }
            
            return result;
        }
    }
}
