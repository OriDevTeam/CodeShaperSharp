// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;


// Application Namespaces
using Lib.Settings;
using Lib.Shapers.CPP;
using Lib.Shaping;


// Library Namespaces



namespace Lib.Projects
{
    /*
    public static class ReportProcessedExtensions
    {
        public static int TotalProcessedReplacementCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Replacements.Count;

            return count;
        }

        public static int TotalProcessedAdditionCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Additions.Count;

            return count;
        }

        public static int TotalProcessedSubtractionCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Subtractions.Count;

            return count;
        }

        public static List<KeyValuePair<string, Replacement>> AppliedReplacements(this VCXSolution vcxProject)
        {
            var replacements = new List<KeyValuePair<string, Replacement>>();

            foreach (var module in vcxProject.Modules)
                foreach (var replacement in module.Result.Replacements)
                    if (!replacements.Contains(replacement.Item1))
                        replacements.Add(replacement.Item1);

            return replacements;
        }

        public static List<KeyValuePair<string, Addition>> AppliedAdditions(this VCXSolution vcxProject)
        {
            var additions = new List<KeyValuePair<string, Addition>>();

            foreach (var module in vcxProject.Modules)
                foreach (var addition in module.Result.Additions)
                    if (!additions.Contains(addition.Item1))
                        additions.Add(addition.Item1);

            return additions;
        }

        public static List<KeyValuePair<string, Subtraction>> AppliedSubtractions(this VCXSolution vcxProject)
        {
            var subtractions = new List<KeyValuePair<string, Subtraction>>();

            foreach (var module in vcxProject.Modules)
                foreach (var subtraction in module.Result.Subtractions)
                    if (!subtractions.Contains(subtraction.Item1))
                        subtractions.Add(subtraction.Item1);

            return subtractions;
        }
    }

    public static class ReportUnprocessedExtensions
    {
        public static List<KeyValuePair<string, Replacement>> UnapliedReplacements(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedReplacements();

            var replacements = new List<KeyValuePair<string, Replacement>>();

            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var replacement in patch.Patch.Actions.Replacements)
                    {
                        if(!applied.Contains(replacement))
                            if (!replacements.Contains(replacement))
                                replacements.Add(replacement);
                    }
                }

            return replacements;
        }

        public static List<KeyValuePair<string, Addition>> UnapliedAdditions(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedAdditions();

            var additions = new List<KeyValuePair<string, Addition>>();

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

            return additions;
        }

        public static List<KeyValuePair<string, Subtraction>> UnapliedSubtractions(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedSubtractions();

            var subtractions = new List<KeyValuePair<string, Subtraction>>();

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

            return subtractions;
        }
    }
    */
}
