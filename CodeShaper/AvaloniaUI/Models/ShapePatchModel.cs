// System Namespaces
using System.Collections.ObjectModel;
using Lib.Shapers;

// Application Namespaces
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;


// Library Namespaces



namespace AvaloniaUI.Models
{
    
    public class ShapePatchModel
    {
        public string? Name { get; set; }
        public string FileSearch { get; set; }
        
        public ObservableCollection<ShapePatchActionsModel> Actions { get; set; } = new();

        
        public ShapePatchModel(ShapePatchFile patchFile)
        {
            FileSearch = $"File Name: {patchFile.Patch.FileSearch}";
            
            Actions.Add(new ShapePatchActionsModel(patchFile.Patch.Actions));
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
