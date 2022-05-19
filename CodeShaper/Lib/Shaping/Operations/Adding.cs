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
    internal static class Adding
    {
        public static List<IShapeActionsAdder> ProcessAdding(
            ref string context, IASTVisitor visitor, string fileName, List<ShapePatchFile> patches, Enum location)
        {
            var adders = new List<IShapeActionsAdder>();

            foreach (var patch in patches)
                adders.AddRange(ProcessAdding(ref context, visitor, fileName, patch.Patch, location));
            
            return adders;
        }
        
        public static List<IShapeActionsAdder> ProcessAdding(
            ref string context, IASTVisitor visitor, string fileName, IShapePatch patch, Enum location)
        {
            var processedAdditions = new List<IShapeActionsAdder>();

            var adders = GetAdditions(visitor, fileName, patch, location);

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
            IASTVisitor visitor, string fileName, IShapePatch patch, Enum location)
        {
            var additions = new List<IShapeActionsAdder>();
            
            if (!Matching.MatchesFile(fileName, patch))
                return new List<IShapeActionsAdder>();
            
            if (patch.Actions.Adders == null)
                return new List<IShapeActionsAdder>();

            foreach (var adder in patch.Actions.Adders)
            {
                if (!Equals(adder.Location, location))
                    continue;

                if (adder.ReferenceLocation != null)
                    if (!PcreRegex.IsMatch(visitor.VisitorController.LocationsContent[adder.ReferenceLocation],
                            adder.Reference))
                        continue;

                additions.Add(adder);
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
