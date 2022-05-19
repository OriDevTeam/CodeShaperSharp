// System Namespaces
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;

// Application Namespaces
using Lib.Managers;
using AvaloniaUI.Models;
using Lib.Shapers;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;


// Library Namespaces



namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ActionsTreeViewModel : ViewModelBase
    {
        public List<ShapePatchFile> patches { get; } =
            ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Patches; 
        
        public List<ShapePatchModel> Patches { get; set; } = new();

        public ActionsTreeViewModel()
        {
            // Temp hack to not load lib related content
            if (!LibManager.Initialized)
                return;
                
            foreach (var patch in ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Patches)
                Patches.Add(new ShapePatchModel(patch));
            
            /*
            Source = new HierarchicalTreeDataGridSource<IShapePatch>(patches)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<IShapePatch>(
                        new TextColumn<IShapePatch, string>("Action", x => x.Name),
                        x => x.Header.)
                },
            };
            */
        }
        
        public HierarchicalTreeDataGridSource<IShapePatch> Source { get; }
    }
}