// System Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Shaping.Interfaces;
using Lib.Utility.Extensions;
using Lib.Shaping.Expressions;


// Library Namespaces
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCRE;

namespace Lib.Shapers.CPP
{
    public class CPPPatch : IShapePatch
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

        [JsonProperty("makers")]
        public Dictionary<string, Maker> Makers = new();
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

    public class Builder : IShapeVariable
    {
        [JsonRequired]
        [JsonProperty("location")]
        public Location Location;

        [JsonProperty("reference_location")]
        public Location ReferenceLocation;

        [JsonProperty("reference")]
        public string Reference;

        [JsonProperty("reference_flags")]
        public string ReferenceFlags;

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

        public string Result = "";

        public bool Ready { get; internal set; }
        public Dictionary<string, IShapeVariable> LocalVariables { get; internal set; } = new();

        public string ProcessVariable(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var built = Groups.ProcessActionsExpressions(Build, variables, arguments);

            return built;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
    }

    public class Maker : IShapeVariable
    {
        [JsonProperty("prepare")]
        public string Prepare;

        [JsonProperty("locals")]
        public Dictionary<string, Dictionary<string, string>> Locals = new();

        [JsonRequired]
        [JsonProperty("make")]
        public string Make;

        public Dictionary<string, IShapeVariable> LocalVariables { get; internal set; } = new();

        public string ProcessVariable(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            var built = Groups.ProcessActionsExpressions(Make, variables, arguments);

            return built;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
    }

    public class Resolver : IShapeVariable
    {
        [JsonProperty("cases")]
        public Dictionary<string, string> Cases = new();

        [JsonProperty("list")]
        public List<string> List = new();

        [JsonProperty("index")]
        public string Index;

        [JsonProperty("default")]
        public string Default;

        public ResolverMode Mode;

        public string ProcessVariable(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            string result = null;

            if (Mode == ResolverMode.List)
                result = ProcessList(variables, arguments);
            else
                result = ProcessCases(variables, arguments);

            return result;
        }

        public string ProcessVariable()
        {
            throw new NotImplementedException();
        }
        
        public string ProcessList(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
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
        public string ProcessCases(Dictionary<string, IShapeVariable> variables, List<string> arguments = null)
        {
            if (arguments == null || arguments.Count < 1)
                return "";

            var value = arguments[0];

            foreach (var acase in Cases)
            {
                if (PcreRegex.IsMatch(value, acase.Key))
                {
                    return acase.Value;
                }
            }

            return Default;
        }
        
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
