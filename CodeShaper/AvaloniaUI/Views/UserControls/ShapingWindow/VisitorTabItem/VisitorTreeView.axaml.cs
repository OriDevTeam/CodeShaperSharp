// System Namespaces


// Application Namespaces
using AvaloniaUI.ViewModels.UserControls.ShapingWindow.VisitorTabItem;
using AvaloniaUI.Views.UserControls.EditTextEditor;
using Lib.Managers;


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.ShapingWindow.VisitorTabItem
{
    public class VisitorTreeView : ReactiveUserControl<VisitorTreeViewModel>
    {
        public static VisitorTreeView? Instance { get; private set; }
        
        public VisitorTreeView()
        {
            Instance = this;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            BindControls();
        }

        private void BindControls()
        {
            OriginalTextEditor = this.Find<TextEditorUserControl>("OriginalTextEditor");
            TargetTextEditor = this.Find<TextEditorUserControl>("TargetTextEditor");
        }
        
        internal void Refresh()
        {
            ViewModel ??= (VisitorTreeViewModel?)Content;

            var currentTargetFile = ShapingOperationsManager.ActiveShapingOperation.OperationsController.CurrentTargetFile;

            if (OriginalTextEditor != null) 
                OriginalTextEditor.TextEditor!.Text = currentTargetFile.FileContent;
            
            if (TargetTextEditor != null) 
                TargetTextEditor.TextEditor!.Text = currentTargetFile.FileContent;

            ViewModel?.HighlightCurrentLocation(TargetTextEditor);
        }

        private TextEditorUserControl? OriginalTextEditor { get; set; }
        private TextEditorUserControl? TargetTextEditor { get; set; }
    }
}
