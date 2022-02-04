// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces



namespace AvaloniaUI.Models
{
    public class ShapePatchModel
    {
        public string Name { get; set; }
        public ObservableCollection<ShapePatchHeaderModel> Header { get; set; } = new();
        public ShapePatchModel(IShapePatch patch)
        {
            Name = patch.Name;
            
            Header.Add(new ShapePatchHeaderModel(patch.Header));
        }
    }
    
    public class ShapePatchHeaderModel
    {
        public string? Name { get; set; }
        public string FileSearch { get; set; }
        
        public ObservableCollection<ShapePatchActionsModel> Actions { get; set; } = new();

        
        public ShapePatchHeaderModel(IShapePatchHeader header)
        {
            FileSearch = $"File Name: {header.FileSearch}";
            
            Actions.Add(new ShapePatchActionsModel(header.Actions));
        }
    }

    public class ShapePatchActionsModel
    {
        public ObservableCollection<IShapeActionsBuilder> Builders { get; set; }

        public ShapePatchActionsModel(IShapeActions actions)
        {
            Builders = actions.Builders;
        }
    }

}
