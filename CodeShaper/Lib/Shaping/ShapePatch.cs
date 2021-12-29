// System Namespaces
using System;
using System.IO;


// Application Namespaces
using Lib.Shapers.CPP;


// Library Namespaces
using Newtonsoft.Json;


namespace Lib.Shaping
{

    public class ShapePatch
    {
        public string FilePath;
        public string FileContent;
        public CPPPatch Patch;

        public ShapePatch(string filePath)
        {
            FilePath = filePath;
            FileContent = File.ReadAllText(filePath);

            var hjsonPatch = Hjson.HjsonValue.Load(FilePath).ToString();
            Patch = JsonConvert.DeserializeObject<CPPPatch>(hjsonPatch);

            foreach (var replacement in Patch.Actions.Replacements)
            {
                if (replacement.Value.ExtensionData.Count > 0)
                    Console.WriteLine();
            }

            foreach (var addition in Patch.Actions.Additions)
            {
                if (addition.Value.ExtensionData.Count > 0)
                    Console.WriteLine();
            }

            foreach (var subtraction in Patch.Actions.Subtractions)
            {
                if (subtraction.Value.ExtensionData.Count > 0)
                    Console.WriteLine();
            }
        }
    }
}
