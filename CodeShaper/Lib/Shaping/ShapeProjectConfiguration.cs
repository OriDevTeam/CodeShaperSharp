// System Namespaces
using System.IO;
using System.Collections.Generic;


// Application Namespaces


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lib.Shaping
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
        [JsonRequired]
        [JsonProperty("name")]
        public string Name = "No Name";
        
        [JsonRequired]
        [JsonProperty("target")]
        public string Target;

        [JsonRequired]
        [JsonProperty("patch")]
        public string Patch;
        
        [JsonProperty("projects")]
        public Projects Projects;
        
        [JsonRequired]
        [JsonProperty("description")]
        public string Description;
    }

    public class Projects
    {
        [JsonExtensionData]
        private readonly Dictionary<string, JToken> projects = new();
    }
}
