// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;
using Lib.Shaping.Target.Interfaces;

// Library Namespaces
using PCRE;


namespace Lib.Shaping.Operations
{

    internal static class Subtracting
    {
        public static List<IShapeActionsSubtracter> ProcessSubtractions(
            ref string context, IASTVisitor visitor, string fileName, List<ShapePatchFile> patches, Enum location)
        {
            var subtracters = new List<IShapeActionsSubtracter>();

            foreach (var patch in patches)
                subtracters.AddRange(ProcessSubtractions(ref context, visitor, fileName, patch, location));

            return subtracters;
        }
        
        public static List<IShapeActionsSubtracter> ProcessSubtractions(
            ref string context, IASTVisitor visitor, string fileName, ShapePatchFile patch, Enum location)
        {
            var processedSubtractions = new List<IShapeActionsSubtracter>();

            var subtractions = GetSubtractions(visitor, fileName, patch.Patch, location);

            if (subtractions.Count > 0)
                foreach (var subtraction in subtractions)
                {
                    processedSubtractions.Add(subtraction);
                    context = SubtractMatches(context, subtraction);
                }

            return processedSubtractions;
        }

        private static List<IShapeActionsSubtracter> GetSubtractions(
            IASTVisitor visitor, string fileName, IShapePatch patch, Enum location)
        {
            var subtractions = new List<IShapeActionsSubtracter>();
            
            if (!Matching.MatchesFile(fileName, patch))
                return new List<IShapeActionsSubtracter>();
            
            if (patch.Actions.Subtracters == null)
                return new List<IShapeActionsSubtracter>();
            
            foreach (var subtracter in patch.Actions.Subtracters)
            {
                if (!Equals(subtracter.Location, location))
                    continue;

                if (subtracter.ReferenceLocation != null)
                    if (!PcreRegex.IsMatch(
                            visitor.VisitorController.LocationsContent[subtracter.ReferenceLocation],
                            subtracter.Reference))
                        continue;

                subtractions.Add(subtracter);
            }

            return subtractions;
        }

        private static string SubtractMatches(string definition, IShapeActionsSubtracter subtraction)
        {
            return "";
        }
    }
}
