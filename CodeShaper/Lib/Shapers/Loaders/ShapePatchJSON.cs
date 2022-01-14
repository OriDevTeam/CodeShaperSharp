// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shaping.Expressions;
using Lib.Utility.Extensions;


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCRE;


namespace Lib.Shapers.Loaders
{

    public class ShapePatchJSON<TLocation> : IShapePatch<TLocation> where TLocation : Enum
    {
        private string filePath;
        private string fileContent;

        public ShapePatchHeader<ShapeActions<TLocation>>? Header { get; set; }

        public ShapePatchJSON(string filePath)
        {
            this.filePath = filePath;
            fileContent = File.ReadAllText(filePath);

            var hjsonPatch = Hjson.HjsonValue.Load(this.filePath).ToString();
            
            Header = JsonConvert.DeserializeObject<ShapePatchHeader<ShapeActions<TLocation>>>(hjsonPatch);

            foreach (var shapeActionsReplacer in Header!.Actions.Replacers)
            {
                var replacer = (Replacer)shapeActionsReplacer;
                if (replacer.ExtensionData.Count > 0)
                    Console.WriteLine();
            }

            foreach (var shapeActionsAdder in Header.Actions.Adders)
            {
                var adder = (Adder)shapeActionsAdder;
                if (adder.ExtensionData.Count > 0)
                    Console.WriteLine();
            }

            foreach (var shapeActionsSubtracter in Header.Actions.Subtracters)
            {
                var subtracter = (Subtracter)shapeActionsSubtracter;
                if (subtracter.ExtensionData.Count > 0)
                    Console.WriteLine();
            }
        }
    }
    
    public class ShapePatchHeader<TActions> : IShapePatchHeader<TActions>
    {
        [JsonProperty("project")]
        public string Project { get; set; }
        
        [JsonRequired]
        [JsonProperty("file")]
        public string FileSearch { get; set; }

        [JsonRequired]
        [JsonProperty("actions")]
        public TActions Actions { get; set; }
    }
    
    public class ShapeActions<TLocation> where TLocation : Enum
    {
        [JsonConverter(typeof(BuilderConverter))]
        [JsonProperty("builders")]
        public ObservableCollection<Builder<TLocation>> Builders { get; set; }
        
        
        [JsonProperty("makers"), JsonConverter(typeof(MakerConverter))]
        public ObservableCollection<IShapeActionsMaker> Makers { get; }
        
        [JsonProperty("resolvers")]
        public ObservableCollection<IShapeActionsResolver> Resolvers { get; }
        
        [JsonProperty("replacements", ItemConverterType = typeof(ReplacementConverter))]
        public ObservableCollection<IShapeActionsReplacer> Replacers { get; }
        
        [JsonProperty("adders")]
        public ObservableCollection<IShapeActionsAdder> Adders { get; }
        
        [JsonProperty("subtractions")]
        public ObservableCollection<IShapeActionsSubtracter> Subtracters { get; }
    }
    
    
    public class Builder<TLocation> where TLocation : Enum
    {
        public string Name { get; set; }
        
        [JsonRequired]
        [JsonProperty("location")]
        public Enum? Location { get; set; }

        [JsonProperty("reference_location")]
        public Enum? ReferenceLocation { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("reference_flags")]
        public string ReferenceFlags { get; set; }

        [JsonProperty("match")]
        public string Match { get; set; }

        [JsonProperty("prepare")]
        public string Prepare { get; set; }

        [JsonRequired]
        [JsonProperty("build")]
        public string Build { get; set; }

        [JsonProperty("actions")]
        public IShapeActions<Enum> Actions { get; set; }
        
        public IShapeActionsBuilder RootBuilder { get; set; }

        public IShapeActionsBuilder ParentBuilder { get; set; }

        public IShapeActionsBuilder ActiveBuilder { get; set; }

        public string Context { get; set; }

        public string Result { get; set; }

        public bool Ready { get; set; }
        
        public List<IShapeVariable> LocalVariables { get; set; } = new();

        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null)
        {
            var built = Groups.ProcessActionsExpressions(Build, variables, arguments);

            return built;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
    }
    
    public class Resolver : IShapeActionsResolver
    {
        public string Name { get; set; }
        
        [JsonProperty("cases")]
        public Dictionary<string, string> Cases { get; set; } = new();

        [JsonProperty("list")]
        public List<string> List { get; set; } = new();

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("default")]
        public string Default { get; set;  }

        public ResolverMode Mode { get; set; }

        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null)
        {
            var result = Mode == ResolverMode.List ?
                ProcessList(variables, arguments) : ProcessCases(variables, arguments);

            return result;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }

        private string ProcessList(List<IShapeVariable> variables, List<string> arguments = null)
        {
            var localExpressions = new LocalExpressions();

            var index = Convert.ToInt32(localExpressions.ProcessExpression(Index, variables, arguments));

            var defaultVal = localExpressions.ProcessExpression(Default, variables, arguments);

            var value = defaultVal;

            if (List.Count < index)
                value = List[List.Count];
            else
                value = List[index];

            return "";
        }

        private string ProcessCases(List<IShapeVariable> variables, List<string> arguments = null)
        {
            if (arguments == null || arguments.Count < 1)
                return "";

            var value = arguments[0];

            foreach (var (key, s) in Cases)
                if (PcreRegex.IsMatch(value, key))
                    return s;

            return Default;
        }
    }
    
    public class Maker : IShapeActionsMaker
    {
        public string Name { get; set; }
        
        [JsonProperty("prepare")]
        public string Prepare { get; set; }

        [JsonProperty("locals")]
        public Dictionary<string, Dictionary<string, string>> Locals { get; set; } = new();

        [JsonRequired]
        [JsonProperty("make")]
        public string Make { get; set; }

        public Dictionary<string, IShapeVariable> LocalVariables { get; internal set; } = new();

        public string ProcessVariable(List<IShapeVariable> variables, List<string> arguments = null)
        {
            var built = Groups.ProcessActionsExpressions(Make, variables, arguments);

            return built;
        }
        
        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
    }
    
    public class Replacer : IShapeActionsReplacer
    {
        public string Name { get; set; }
        
        [JsonRequired]
        [JsonProperty("location")]
        public Enum? Location { get; set; }

        [JsonProperty("reference_location")]
        public Enum? ReferenceLocation { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonRequired]
        [JsonProperty("from")]
        public string[] From { get; set; }

        [JsonRequired]
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtensionData = new();
    }
    
    
    public class Adder : IShapeActionsAdder
    {
        public string Name { get; set; }
        
        [JsonRequired]
        [JsonProperty("location")]
        public Enum? Location { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }

        [JsonProperty("reference_location")]
        public Enum? ReferenceLocation { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("except")]
        public string Except { get; set; }

        [JsonRequired]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonExtensionData]
        public readonly Dictionary<string, JToken> ExtensionData = new();
    }
    
    public class Subtracter : IShapeActionsSubtracter
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Enum? Location { get; set; }

        [JsonRequired]
        [JsonProperty("reference_location")]
        public Enum? ReferenceLocation { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("remove_usages")]
        public bool RemoveUsages { get; set; }

        [JsonExtensionData]
        public readonly Dictionary<string, JToken> ExtensionData = new();
    }

    public class BuilderConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var builderType = objectType.GetGenericArguments()[0];
            var locationType = objectType.GetGenericArguments()[0].GetGenericArguments()[0];

            var obj = JObject.Load(reader);

            var builders = new ObservableCollection<Builder<Enum>>();

            foreach (var (key, value) in obj)
            {
                var builder = new Builder<Enum>();

                if (builder == null)
                    continue;
                
                builder.Name = key;
                builder.Reference = value.Value<string>("reference");
                builder.Location = value.Value<string>("reference").ToEnum(locationType);
                builder.ReferenceLocation = value.Value<string>("reference_location").ToEnum(locationType);
                builder.ReferenceFlags = value.Value<string>("reference_flags");
                builder.Match = value.Value<string>("match");
                builder.Prepare = value.Value<string>("prepare");
                builder.Build = value.Value<string>("build");

                // builder.Actions = value.Value<IShapeActions<Enum>>("actions")
                
                builders.Add((Builder<Enum>)builder);
            }

            return builders;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Builder<Enum>);
        }
    }
    
    
    public class MakerConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
    
    
    public class ReplacementConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Replacer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var props = new List<string> { "to", "from", "location", "reference_location", "reference" };
            var replacement = new Replacer
            {
                To = (string)obj.Property("to")?.Value,
                Reference = (string)obj.Property("reference")?.Value,
                // Location = ((string)obj.Property("location")?.Value).ToEnum<TLocation>(),
                // ReferenceLocation = ((string)obj.Property("reference_location")?.Value).ToEnum<TLocation>()
            };

            var from = obj.Property("from");

            if (from != null)
            {
                switch (@from.Value.Type)
                {
                    case JTokenType.String:
                        replacement.From = new string[] { (string)@from?.Value };
                        break;
                    case JTokenType.Array:
                        replacement.From = @from?.Value.ToObject<string[]>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var prop in obj.Properties())
            {
                if (!props.Contains(prop.Name))
                    replacement.ExtensionData.Add(prop.Name, prop.Value);
            }

            return replacement;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
