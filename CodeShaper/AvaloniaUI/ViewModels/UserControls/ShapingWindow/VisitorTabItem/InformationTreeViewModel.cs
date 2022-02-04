// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Managers;


// Library Namespaces



namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow.VisitorTabItem
{
    public class InformationTreeViewModel : ViewModelBase
    {
        public InformationTreeViewModel()
        {
        }
        
        internal void Refresh()
        {
            var currentTargetFile = ShapingOperationsManager.ActiveShapingOperation.OperationsController.CurrentTargetFile;

            Locations = currentTargetFile.ShapePatch.PreparationController.ASTSet.Visitor.
                VisitorController.LocationsContent;
        }
        
        public Dictionary<Enum, string> Locations { get; private set; } = null!;
    }
}
