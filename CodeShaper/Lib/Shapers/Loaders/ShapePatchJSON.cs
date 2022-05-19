// System Namespaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;
using Lib.Shaping.Expressions;
using Lib.Utility.Extensions;


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCRE;


namespace Lib.Shapers.Loaders
{

    public abstract class ShapePatchJSON<TLocation> : ShapePatchFile
    {
        protected ShapePatchJSON(string filePath) : base(filePath)
        {
            ShapePatchSettings.Location = typeof(TLocation);
            
            Patch = JsonConvert.DeserializeObject<ShapePatch>(Hjson.HjsonValue.Load(filePath).ToString());
        }
    }
    
    public class ShapePatch : IShapePatch
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
        
        [JsonProperty("alias")]
        public string Alias { get; set; }
        
        [JsonProperty("project")]
        public string Project { get; set; }
        
        [JsonRequired]
        [JsonProperty("file")]
        public string FileSearch { get; set; }

        [JsonRequired]
        [JsonProperty("actions")]
        public IShapeActions Actions { get; set; }

        public ShapePatch(ShapeActions actions)
        {
            Actions = actions;
        }
    }
    
    [JsonObject]
    public class ShapeActions : IShapeActions
    {
        [JsonConverter(typeof(BuilderConverter))]
        [JsonProperty("builders")]
        public ObservableCollection<IShapeActionsBuilder> Builders { get; set; }
        
        [JsonConverter(typeof(MakerConverter))]
        [JsonProperty("makers")]
        public ObservableCollection<IShapeActionsMaker> Makers { get; set; }
        
        [JsonConverter(typeof(ResolverConverter))]
        [JsonProperty("resolvers")]
        public ObservableCollection<IShapeActionsResolver> Resolvers { get; set; }
        
        [JsonConverter(typeof(ReplacementConverter))]
        [JsonProperty("replacements")]
        public ObservableCollection<IShapeActionsReplacer> Replacers { get; set; }
        
        [JsonConverter(typeof(AdderConverter))]
        [JsonProperty("adders")]
        public ObservableCollection<IShapeActionsAdder> Adders { get; set; }
        
        [JsonConverter(typeof(SubtracterConverter))]
        [JsonProperty("subtractions")]
        public ObservableCollection<IShapeActionsSubtracter> Subtracters { get; set; }
    }
    
    
    public class Builder : IShapeActionsBuilder
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
        public IShapeActions Actions { get; set; }
        
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

        public Builder(ShapeActions actions)
        {
            Actions = actions;
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

        public Maker()
        {
            
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
    }
    
    public class Subtracter : IShapeActionsSubtracter
    {
        public string Name { get; set; }
        
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
    }

    public class BuilderConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var builders = new ObservableCollection<IShapeActionsBuilder>();

            foreach (var (key, value) in obj)
            {
                var jActions = value.Value<JObject>("actions");

                if (jActions == null)
                    return null;
                
                var actions = JsonConvert.DeserializeObject<ShapeActions>(jActions.ToString());

                var builder = new Builder(actions)
                {
                    Name = key,
                    Reference = value.Value<string>("reference"),
                    Location = value.Value<string>("reference").ToEnum(ShapePatchSettings.Location),
                    ReferenceLocation = value.Value<string>("reference_location").ToEnum(ShapePatchSettings.Location),
                    ReferenceFlags = value.Value<string>("reference_flags"),
                    Match = value.Value<string>("match"),
                    Prepare = value.Value<string>("prepare"),
                    Build = value.Value<string>("build"),
                };
                
                builders.Add(builder);
            }

            return builders;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Builder);
        }
    }

    public class ResolverConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var resolvers = new ObservableCollection<IShapeActionsResolver>();

            foreach (var (key, value) in obj)
            {
                var resolver = new Resolver
                {
                    Name = key,
                    Index = value.Value<string>("index"),
                    Default = value.Value<string>("default"),
                    Mode = value.Value<ResolverMode>("make"),
                };
                
                var listStr = value.Value<JArray>("list")?.ToString();
                if (listStr != null)
                    resolver.List = JsonConvert.DeserializeObject<List<string>>(listStr);

                var casesStr = value.Value<JObject>("cases")?.ToString();
                if (casesStr != null)
                    resolver.Cases = JsonConvert.DeserializeObject<Dictionary<string, string>>(casesStr);

                resolvers.Add(resolver);
            }

            return resolvers;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Resolver);
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
            var obj = JObject.Load(reader);

            var makers = new ObservableCollection<IShapeActionsMaker>();

            foreach (var (key, value) in obj)
            {
                var maker = new Maker
                {
                    Name = key,
                    Prepare = value.Value<string>("prepare"),
                    Locals = value.Value<Dictionary<string, Dictionary<string, string>>>("reference"),
                    Make = value.Value<string>("make")
                };

                makers.Add(maker);
            }

            return makers;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Maker);
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
            var obj = JObject.Load(reader);

            var replacers = new ObservableCollection<IShapeActionsReplacer>();

            foreach (var (key, value) in obj)
            {
                var replacer = new Replacer
                {
                    Name = key,
                };

                replacer.Location = value.Value<string>("location").ToEnum(ShapePatchSettings.Location);
                replacer.Reference = value.Value<string>("reference");
                replacer.ReferenceLocation =
                    value.Value<string>("reference_location").ToEnum(ShapePatchSettings.Location);
                replacer.To = value.Value<string>("to");

                try
                {
                    replacer.From = new[]{ value.Value<string>("from") };
                }
                catch (Exception e)
                {
                    replacer.From = value.Value<string[]>("from");
                }

                replacers.Add(replacer);
            }

            return replacers;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    
    public class AdderConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var adders = new ObservableCollection<Adder>();

            foreach (var (key, value) in obj)
            {
                var adder = new Adder
                {
                    Name = key,
                    Reference = value.Value<string>("reference"),
                    Location = value.Value<string>("reference").ToEnum(ShapePatchSettings.Location),
                    ReferenceLocation = value.Value<string>("reference_location").ToEnum(ShapePatchSettings.Location),
                    Order = value.Value<string>("order"),
                    Except = value.Value<string>("except"),
                    Code = value.Value<string>("code")
                };
                
                adders.Add(adder);
            }

            return adders;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Adder);
        }
    }
    
    public class SubtracterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var subtracters = new ObservableCollection<Subtracter>();

            foreach (var (key, value) in obj)
            {
                var subtracter = new Subtracter
                {
                    Name = key,
                    Reference = value.Value<string>("reference"),
                    Location = value.Value<string>("reference").ToEnum(ShapePatchSettings.Location),
                    ReferenceLocation = value.Value<string>("reference_location").ToEnum(ShapePatchSettings.Location),
                    RemoveUsages = value.Value<bool>("remove_usages"),
                };
                
                subtracters.Add(subtracter);
            }

            return subtracters;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Subtracter);
        }
    }
}
