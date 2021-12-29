// System Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Utility.Extensions;


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Lib.Shapers.CPP
{
    public class CPPPatch
    {
        [JsonRequired]
        [JsonProperty("file")]
        public string FileSearch;

        [JsonRequired]
        [JsonProperty("actions")]
        public Actions Actions;
    }

    public class Actions
    {
        [JsonProperty("replacements", ItemConverterType = typeof(ReplacementConverter))]
        public Dictionary<string, Replacement> Replacements = new();

        [JsonProperty("additions")]
        public Dictionary<string, Addition> Additions = new();

        [JsonProperty("subtractions")]
        public Dictionary<string, Subtraction> Subtractions = new();

        [JsonProperty("builders")]
        public Dictionary<string, Builder> Builders = new();

        [JsonProperty("resolvers")]
        public Dictionary<string, Resolver> Resolvers = new();
    }

    public class Replacement
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("reference_location")]
        public Location ReferenceLocation;

        [JsonProperty("reference")]
        public string Reference;

        [JsonRequired]
        [JsonProperty("from")]
        public string[] From = Array.Empty<string>();

        [JsonRequired]
        [JsonProperty("to")]
        public string To;

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtensionData = new();
    }

    public class Subtraction
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Location Location;

        [JsonRequired]
        [JsonProperty("reference_location")]
        public Location ReferenceLocation;

        [JsonProperty("reference")]
        public string Reference;

        [JsonProperty("remove_usages")]
        public bool RemoveUsages;

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtensionData = new();
    }

    public class Addition
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("order")]
        public string Order;

        [JsonProperty("reference_location")]
        public Location ReferenceLocation;

        [JsonProperty("reference")]
        public string Reference;

        [JsonProperty("except")]
        public string Except;

        [JsonRequired]
        [JsonProperty("code")]
        public string Code;

        [JsonExtensionData]
        public Dictionary<string, JToken> ExtensionData = new();
    }

    public class Builder
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("reference_location")]
        public Location ReferenceLocation;

        [JsonProperty("reference")]
        public string Reference;

        [JsonProperty("match")]
        public string Match;

        [JsonProperty("prepare")]
        public string Prepare;

        [JsonRequired]
        [JsonProperty("build")]
        public string Build;

        [JsonProperty("actions")]
        public Actions Actions;


        public KeyValuePair<string, Builder> RootBuilder;

        public KeyValuePair<string, Builder> ParentBuilder;

        public KeyValuePair<string, Builder> ActiveBuilder;

        public string Context;

        public string Vars;

        public string Result = "";

        public bool Ready { get; internal set; }
    }

    public class Resolver
    {
        [JsonProperty("cases")]
        public Dictionary<string, string> Cases = new();

        [JsonProperty("list")]
        public List<string> List = new();

        public ResolverMode Mode;
    }

    public enum ResolverMode
    {
        List,
        Switch
    }

    public enum Location
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "module")]
        Module,

        [EnumMember(Value = "include")]
        Include,

        [EnumMember(Value = "var")]
        ModuleVariable,

        [EnumMember(Value = "var.def")]
        ModuleVariableDefinition,

        [EnumMember(Value = "declaration")]
        Declaration,

        [EnumMember(Value = "declaration.statement")]
        DeclarationStatement,

        [EnumMember(Value = "method")]
        Function,

        [EnumMember(Value = "method.def")]
        FunctionDefinition,

        [EnumMember(Value = "method.body")]
        FunctionBody,

        [EnumMember(Value = "method.condition")]
        MethodCondition,
    }


    ////////////////////////////////////////
    /// Converters
    /// 

    public class ReplacementConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Replacement);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var props = new List<string> { "to", "from", "location", "reference_location", "reference" };
            var replacement = new Replacement
            {
                To = (string)obj.Property("to")?.Value,
                Reference = (string)obj.Property("reference")?.Value,
                Location = ((string)obj.Property("location")?.Value).ToEnum<Location>(),
                ReferenceLocation = ((string)obj.Property("reference_location")?.Value).ToEnum<Location>()
            };

            var from = obj.Property("from");

            if (from != null)
            {
                if (from.Value.Type == JTokenType.String)
                    replacement.From = new string[] { (string)from?.Value };
                else if (from.Value.Type == JTokenType.Array)
                    replacement.From = from?.Value.ToObject<string[]>();
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
