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
    internal static class Adding
    {
        public static List<IShapeActionsAdder> ProcessAdding(
            ref string context, IASTVisitor visitor, string fileName,
            List<IShapePatch> patches, Enum location)
        {
            var processedAdditions = new List<IShapeActionsAdder>();

            var adders = GetAdditions(visitor, fileName, patches, location);

            if (adders.Count <= 0)
                return processedAdditions;
            
            foreach (var adder in adders)
            {
                processedAdditions.Add(adder);
                context = AddMatches(context, adder);
            }

            return processedAdditions;
        }

        private static List<IShapeActionsAdder> GetAdditions(
            IASTVisitor visitor, string fileName,
            List<IShapePatch> patches, Enum location)
        {
            var additions = new List<IShapeActionsAdder>();

            foreach (var patch in patches)
            {
                if (!Matching.MatchesFile(fileName, patch))
                    continue;
                
                if (patch.Header.Actions.Adders == null)
                    continue;

                foreach (var adder in patch.Header.Actions.Adders)
                {
                    if (!Equals(adder.Location, location))
                        continue;

                    if (adder.ReferenceLocation != null)
                        if (!PcreRegex.IsMatch(visitor.VisitorController.LocationsContent[adder.ReferenceLocation],
                                adder.Reference))
                            continue;

                    additions.Add(adder);
                }
            }

            return additions;
        }

        private static string AddMatches(string definition, IShapeActionsAdder adder)
        {
            var result = definition;

            if (adder.Order == "before")
                result = adder.Code + result;
            else
                result += adder.Code;
            
            return result;
        }
    }
}
