// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces



namespace Lib.Miscelaneous
{
    
    public static class ReportProcessedExtensions
    {
        public static int TotalProcessedReplacementCount(this IShapingTarget shapingTarget)
        {
            var count = 0;

            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    count += file.Result.Replacements.Count;

            return count;
        }

        public static int TotalProcessedAdditionCount(this IShapingTarget shapingTarget)
        {
            var count = 0;

            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    count += file.Result.Additions.Count;

            return count;
        }

        public static int TotalProcessedSubtractionCount(this IShapingTarget shapingTarget)
        {
            var count = 0;

            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    count += file.Result.Subtractions.Count;

            return count;
        }

        public static List<IShapeActionsReplacer> AppliedReplacements(this IShapingTarget shapingTarget)
        {
            var replacements = new List<IShapeActionsReplacer>();
            
            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    foreach (var (item1, item2, item3) in file.Result.Replacements)
                        if (!replacements.Contains(item1))
                            replacements.Add(item1);
            
            return replacements;
        }

        public static List<IShapeActionsAdder> AppliedAdditions(this IShapingTarget shapingTarget)
        {
            var additions = new List<IShapeActionsAdder>();
            
            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    foreach (var (item1, item2, item3) in file.Result.Additions)
                        if (!additions.Contains(item1))
                            additions.Add(item1);
            
            return additions;
        }

        public static List<IShapeActionsSubtracter> AppliedSubtractions(this IShapingTarget shapingTarget)
        {
            var subtractions = new List<IShapeActionsSubtracter>();

            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                    foreach (var (item1, item2, item3) in file.Result.Subtractions)
                        if (!subtractions.Contains(item1))
                            subtractions.Add(item1);

            return subtractions;
        }
    }

    public static class ReportUnprocessedExtensions
    {
        public static List<IShapeActionsReplacer> UnusedReplacements(this IShapingTarget shapingTarget)
        {
            var applied = shapingTarget.AppliedReplacements();

            var replacements = new List<IShapeActionsReplacer>();
            
            /*
            foreach (var group in shapingTarget.ShapingTargetGroups)
                foreach (var file in group.ShapingTargetFiles)
                        foreach (var patch in file.)
                        {
                            foreach (var replacement in patch.Patch.Actions.Replacements)
                            {
                                if(!applied.Contains(replacement))
                                    if (!replacements.Contains(replacement))
                                        replacements.Add(replacement);
                            }
                        }
            */
            
            return replacements;
        }

        public static List<IShapeActionsAdder> UnusedAdditions(this IShapingTarget shapingTarget)
        {
            var applied = shapingTarget.AppliedAdditions();

            var additions = new List<IShapeActionsAdder>();
            
            /*
            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var addition in patch.Patch.Actions.Additions)
                    {
                        if (!applied.Contains(addition))
                            if (!additions.Contains(addition))
                                additions.Add(addition);
                    }
                }
            */
            
            return additions;
        }

        public static List<IShapeActionsSubtracter> UnusedSubtractions(this IShapingTarget shapingTarget)
        {
            var applied = shapingTarget.AppliedSubtractions();

            var subtractions = new List<IShapeActionsSubtracter>();
            
            /*
            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var subtraction in patch.Patch.Actions.Subtractions)
                    {
                        if (!applied.Contains(subtraction))
                            if (!subtractions.Contains(subtraction))
                                subtractions.Add(subtraction);
                    }
                }
            */
            
            return subtractions;
        }
    }
}
