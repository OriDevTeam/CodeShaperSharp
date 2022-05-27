// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using AvaloniaUI.Models;
using Lib.Managers;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;


// Library Namespaces
using Avalonia.Controls;
using Lib.Shapers.Loaders;
using Lib.Shapers.Patches;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ActionsTreeViewModel : ViewModelBase
    {
        public List<ShapePatchFile> Patches { get; }

        // public List<ShapePatchModel> Patches { get; set; } = new();

        public ActionsTreeViewModel()
        {
            // Temp hack to not load lib related content
            if (!LibManager.Initialized)
                return;

            Patches = ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Patches; 
            
            // foreach (var patch in ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Patches)
            //    Patches.Add(new ShapePatchModel(patch));
            
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
        
        // public HierarchicalTreeDataGridSource<IShapePatch> Source { get; }
    }
}