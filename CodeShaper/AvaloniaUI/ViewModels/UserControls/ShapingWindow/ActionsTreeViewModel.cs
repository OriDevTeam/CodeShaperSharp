// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Managers;
using AvaloniaUI.Models;


// Library Namespaces



namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ActionsTreeViewModel : ViewModelBase
    {
        public List<ShapePatchModel> Patches { get; set; } = new();

        public ActionsTreeViewModel()
        {
            // Temp hack to not load lib related content
            if (!LibManager.Initialized)
                return;
                
            foreach (var patch in ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Patches)
                Patches.Add(new ShapePatchModel(patch));
        }
    }
}