// System Namespaces


// Application Namespaces


// Library Namespaces
using Newtonsoft.Json;



namespace Lib.Configurations
{
    public class RapidConfiguration
    {
        [JsonRequired]
        [JsonProperty("name")]
        public string Name;

        [JsonRequired]
        [JsonProperty("shape_project")]
        public string ShapeProjectPath;

        [JsonRequired]
        [JsonProperty("source")]
        public string SourcePath;

        [JsonRequired]
        [JsonProperty("target")]
        public string Target;

        public static RapidConfiguration Load(string configurationPath)
        {
            var hjsonPatch = Hjson.HjsonValue.Load(configurationPath).ToString();
            return JsonConvert.DeserializeObject<RapidConfiguration>(hjsonPatch);
        }
    }
}
