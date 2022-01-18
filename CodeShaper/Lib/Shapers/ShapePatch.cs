// System Namespaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;


// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shapers.Loaders;


// Library Namespaces



namespace Lib.Shapers
{
    /// <summary>
    /// Holds static information for shape patch loading, which works sort
    /// of like an hack to make passing Location enum easier for the converters
    /// </summary>
    public static class ShapePatchSettings
    {
        public static Type Location;
    }
    
    /*
    public class ShapePatch : IShapePatch
    {
        private readonly string filePath;
        private string fileContent;

        private ShapeHeader Header;
        
        public ShapePatch(string filePath)
        {
            this.filePath = filePath;
            fileContent = File.ReadAllText(filePath);
            
            LoadJSON();
        }

        private void LoadJSON()
        {
            var hjsonPatch = Hjson.HjsonValue.Load(filePath).ToString();
            // header = JsonConvert.DeserializeObject<ShapeHeader>(hjsonPatch);
        }

        private class ShapeHeader : IShapePatchHeader<ShapeActions>
        {
            public string FileSearch { get; set; }

            public ShapeActions Actions { get; set; }
        }
        
        public class ShapeActions
        {
            public ObservableCollection<Builder> Builders = new();
            public ObservableCollection<Resolver> Resolvers = new();
            public ObservableCollection<Maker> Makers = new();

            public ObservableCollection<Replacer> Replacers = new();
            public ObservableCollection<Adder> Adders = new();
            public ObservableCollection<Subtracter> Subtracters = new();
        }

        public class Builder
        {
            public Enum? Location;
            
            public Enum? ReferenceLocation;
            
            public string Reference;
            
            public string[] From = Array.Empty<string>();
            
            public string To;
        }

        public class Maker
        {
            public string Prepare;
            
            public Dictionary<string, Dictionary<string, string>> Locals = new();
            
            public string Make;
        }

        public class Resolver
        {
            public Dictionary<string, string> Cases = new();
            
            public List<string> List = new();
            
            public string Index;
            
            public string Default;

            public ResolverMode Mode;
            
            
            public enum ResolverMode
            {
                List,
                Switch
            }
        }
    
        public class Replacer
        {
            public Enum? Location;
            
            public Enum? ReferenceLocation;
            
            public string Reference;
            
            public string[] From = Array.Empty<string>();
            
            public string To;
        }

        public class Adder
        {
            public Enum? Location;
            
            public string Order;
            
            public Enum? ReferenceLocation;
            
            public string Reference;
            
            public string Except;
            
            public string Code;
        }

        public class Subtracter
        {
            public Enum? Location;

            public Enum? ReferenceLocation;
            
            public string Reference;
            
            public bool RemoveUsages;
        }
    }
    */
}
