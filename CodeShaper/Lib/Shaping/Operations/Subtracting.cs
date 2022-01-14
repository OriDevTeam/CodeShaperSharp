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

    internal static class Subtracting
    {
        public static List<IShapeActionsSubtracter> ProcessSubtractions(
            ref string context, IASTVisitor visitor,
            string fileName, List<IShapePatch<Enum>> patches, Enum location)
        {
            var processedSubtractions = new List<IShapeActionsSubtracter>();

            var subtractions = GetSubtractions(visitor, fileName, patches, location);

            if (subtractions.Count > 0)
                foreach (var subtraction in subtractions)
                {
                    processedSubtractions.Add(subtraction);
                    context = SubtractMatches(context, subtraction);
                }

            return processedSubtractions;
        }

        private static List<IShapeActionsSubtracter> GetSubtractions(
            IASTVisitor visitor, string fileName,
            List<IShapePatch<Enum>> patches, Enum location)
        {
            var subtractions = new List<IShapeActionsSubtracter>();

            foreach (var patch in patches)
            {
                if (!Matching.MatchesFile(fileName, patch))
                    continue;

                foreach (var subtracter in patch.Header.Actions.Subtracters)
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
            }

            return subtractions;
        }

        private static string SubtractMatches(string definition, IShapeActionsSubtracter subtraction)
        {
            var result = ""; // definition.Replace(sub.Reference, definition);
            
            return result;
        }
    }
}
