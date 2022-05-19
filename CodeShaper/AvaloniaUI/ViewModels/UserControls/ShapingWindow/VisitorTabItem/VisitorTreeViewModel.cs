// System Namespaces


// Application Namespaces

using Avalonia.Media;
using AvaloniaUI.Views.UserControls.EditTextEditor;
using Lib.Managers;


// Library Namespaces
using AvaloniaEdit.Highlighting;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow.VisitorTabItem
{
    public class VisitorTreeViewModel : ViewModelBase
    {
        public VisitorTreeViewModel()
        {
                
        }
        
        private void HighlightLocations()
        {
            
        }

        internal void HighlightCurrentLocation(TextEditorUserControl? textEditorUserControl)
        {
            var currentTargetFile = ShapingOperationsManager.ActiveShapingOperation.OperationsController.CurrentTargetFile;

            var visitorController = currentTargetFile.ShapePatchFile.PreparationController
                .ASTSet.Visitor.VisitorController;

            var currentContext = visitorController.CurrentContext;

            var start = currentContext.Start.StartIndex;
            var stop = currentContext.Stop.StopIndex;

            var textEditor = textEditorUserControl?.TextEditor;
            
            if (textEditor == null)
                return;
            
            // var offset = textEditor.Document.GetOffset(1, 5);

            // var documentHighlighter = new DocumentHighlighter(textEditor.Document, null);
            // var result = documentHighlighter.HighlightLine(1);
        }
    }
}
