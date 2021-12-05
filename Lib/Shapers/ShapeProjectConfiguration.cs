// System Namespaces
using System.IO;
using System.Collections.Generic;


// Application Namespaces


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lib.Shapers
{
    public class ShapeProjectConfiguration
    {
        public string FilePath;
        public string FileContent;
        public ShapeProjectConfigurationFile Configuration;

        public ShapeProjectConfiguration(string configurationPath)
        {
            FilePath = configurationPath;

            if (File.Exists(configurationPath))
                FileContent = File.ReadAllText(configurationPath);

            var hjsonPatch = Hjson.HjsonValue.Load(FilePath).ToString();
            Configuration = JsonConvert.DeserializeObject<ShapeProjectConfigurationFile>(hjsonPatch);
        }
    }

    public class ShapeProjectConfigurationFile
    {
        [JsonProperty("name")]
        public string Name = "No Name";

        [JsonProperty("projects")]
        public Projects projects;
    }

    public class Projects
    {
        [JsonExtensionData]
        private readonly Dictionary<string, JToken> projects = new();
    }
}