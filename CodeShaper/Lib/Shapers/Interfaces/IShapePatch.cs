// System Namespaces
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.AST.Controllers;


// Library Namespaces


namespace Lib.Shapers.Interfaces
{
    public interface IShapePatch
    {
        public string Name { get; set; }
        
        public ASTPreparationController PreparationController { get; }
        
        public IShapePatchHeader Header { get; set; }
    }

    public interface IShapePatchHeader
    {
        bool Enabled { get; set; }
        string Alias { get; set; }
        string FileSearch { get; set; }
        public IShapeActions Actions { get; set; }
    }

    public interface IShapeActions
    {
        public ObservableCollection<IShapeActionsBuilder> Builders { get; }
        public ObservableCollection<IShapeActionsMaker> Makers { get; }
        public ObservableCollection<IShapeActionsResolver> Resolvers { get; }

        public ObservableCollection<IShapeActionsReplacer> Replacers { get; }
        public ObservableCollection<IShapeActionsAdder> Adders { get; }
        public ObservableCollection<IShapeActionsSubtracter> Subtracters { get; }
    }

    public interface IShapeActionsBuilder : IShapeVariable
    {
        public Enum? Location { get; set; }
        
        public Enum? ReferenceLocation { get; set; }
        
        public string Reference { get; set; }
        
        public string ReferenceFlags { get; set; }
        
        public string Match { get; set; }
        
        public string Prepare { get; set; }
        
        public string Build { get; set; }
        
        public IShapeActions Actions { get; set; }
        
        public IShapeActionsBuilder RootBuilder { get; set; }

        public IShapeActionsBuilder ParentBuilder { get; set; }

        public IShapeActionsBuilder ActiveBuilder { get; set; }

        public string Context { get; set; }

        public string Result { get; set; }
        
        
        public bool Ready { get; set; }
        
        public List<IShapeVariable> LocalVariables { get; set; }
    }

    public interface IShapeActionsResolver : IShapeVariable
    {
        public Dictionary<string, string> Cases { get; set; }
        
        public List<string> List { get; set; }
        
        public string Index { get; set; }
        
        public string Default { get; set; }

        public ResolverMode Mode { get; set; }
    }
    
    public interface IShapeActionsMaker : IShapeVariable
    {
        public string Prepare { get; set; }
        
        public Dictionary<string, Dictionary<string, string>> Locals { get; set; }
        
        public string Make { get; set; }
    }
    
    public interface IShapeActionsReplacer
    {
        public Enum? Location { get; set; }
        
        public Enum? ReferenceLocation { get; set; }
        
        public string Reference { get; set; }
        
        public string[] From { get; set; }
        
        public string To { get; set; }
    }

    public interface IShapeActionsAdder
    {
        public Enum? Location { get; set; }
        
        public string Order { get; set; }
        
        public Enum? ReferenceLocation { get; set; }
        
        public string Reference { get; set; }
        
        public string Except { get; set; }
        
        public string Code { get; set; }
    }
    
    public interface IShapeActionsSubtracter
    {
        public Enum? Location { get; set; }
        
        public Enum? ReferenceLocation { get; set; }
        
        public string Reference { get; set; }
        
        public bool RemoveUsages { get; set; }
    }
    
    public enum ResolverMode
    {
        [EnumMember(Value = "list")]
        List,
        
        [EnumMember(Value = "switch")]
        Switch
    }
}
