// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;

// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{
    internal static class Replacing
    {
        public static List<IShapeActionsReplacer> ProcessReplacing(
            ref string context, IASTVisitor visitor, string fileName, List<ShapePatchFile> patches, Enum location)
        {
            var processed = new List<IShapeActionsReplacer>();

            foreach (var patch in patches)
                processed.AddRange(ProcessReplacing(ref context, visitor, fileName, patch, location));
            
            return processed;
        }
        
        public static List<IShapeActionsReplacer> ProcessReplacing(
            ref string context, IASTVisitor visitor, string fileName, ShapePatchFile patch, Enum location)
        {
            var processed = new List<IShapeActionsReplacer>();
            
            foreach (var replacement in GetReplacements(visitor, fileName, patch.Patch, location))
            {
                if (!HasMatch(context, replacement))
                    continue;
                
                processed.Add(replacement);
                context = ReplaceMatches(context, replacement);
            }
            
            return processed;
        }
        
        
        private static bool HasMatch(string definition, IShapeActionsReplacer replacement)
        {
            foreach (var match in replacement.From)
                if (PcreRegex.IsMatch(definition, match))
                    return true;

            return false;
        }

        private static List<IShapeActionsReplacer> GetReplacements(
            IASTVisitor visitor, string fileName, List<IShapePatch> patches, Enum location)
        {
            var replacements = new List<IShapeActionsReplacer>();

            foreach (var patch in patches)
                replacements.AddRange(GetReplacements(visitor, fileName, patch, location));
            
            return replacements;
        }

        private static List<IShapeActionsReplacer> GetReplacements(
            IASTVisitor visitor, string fileName, IShapePatch patch, Enum location)
        {
            var replacements = new List<IShapeActionsReplacer>();

            if (!Matching.MatchesFile(fileName, patch))
                return new List<IShapeActionsReplacer>();
            
            if (patch.Actions.Replacers == null)
                return new List<IShapeActionsReplacer>();
            
            foreach (var replacer in patch.Actions.Replacers)
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
