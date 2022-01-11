// System Namespaces
using System.IO;


// Application Namespaces
using Lib.Managers;
using AvaloniaUI.Views;
using Lib.Shaping;

// Library Namespaces
using Serilog;
using Serilog.Core;


namespace AvaloniaUI.ViewModels
{
    public class ShapingViewModel : ViewModelBase
    {
        public ShapingViewModel()
        {
            ShapingOperationsManager.ActiveShapingOperation.Start();
        }

        // public ActionToolBoxViewModel ToolBox;
    }
}
