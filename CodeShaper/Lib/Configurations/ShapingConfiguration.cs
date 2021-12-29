// System Namespaces
using System.Runtime.Serialization;


// Application Namespaces


// Library Namespaces
using Newtonsoft.Json;


namespace Lib.Configurations
{
    public class ShapingConfiguration
    {
        [JsonRequired]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonRequired]
        [JsonProperty("shape_project")]
        public string ShapeProjectDirectory { get; set; }

        [JsonRequired]
        [JsonProperty("source")]
        public string SourceDirectory { get; set; }

        [JsonRequired]
        [JsonProperty("target")]
        public string TargetDirectory { get; set; }

        [JsonRequired]
        [JsonProperty("backup")]
        public string BackupDirectory { get; set; }

        [JsonRequired]
        [JsonProperty("result")]
        public ResultOptions ResultOptions { get; set; }

        public ShapingConfiguration()
        {
            ResultOptions = ResultOptions.BackupAndReplaceMoveOriginal;
        }

        public static ShapingConfiguration Load(string configurationPath)
        {
            var hjsonPatch = Hjson.HjsonValue.Load(configurationPath).ToString();
            return JsonConvert.DeserializeObject<ShapingConfiguration>(hjsonPatch);
        }
    }

    public enum ResultOptions
    {
        [EnumMember(Value = "Replace the original files")]
        ReplaceOriginal,

        [EnumMember(Value = "Backup source files, move and replace them with shaped files")]
        BackupAndReplaceMoveOriginal,

        [EnumMember(Value = "Create new shaped files in target directory")]
        CreateNew
    }
}
