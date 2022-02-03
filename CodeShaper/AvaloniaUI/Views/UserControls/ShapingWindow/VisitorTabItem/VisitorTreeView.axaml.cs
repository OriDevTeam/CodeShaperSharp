// System Namespaces


// Application Namespaces
using AvaloniaUI.Views.UserControls.EditTextEditor;
using Lib.Managers;


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaUI.ViewModels.UserControls.ShapingWindow;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
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
        
        public void Refresh()
        {
            var currentTargetFile = ShapingOperationsManager.ActiveShapingOperation.OperationsController.CurrentTargetFile;

            OriginalTextEditor.TextEditor!.Text = currentTargetFile.FileContent;
            TargetTextEditor.TextEditor!.Text = currentTargetFile.FileContent;

        }
        
        private TextEditorUserControl OriginalTextEditor { get; set; }
        private TextEditorUserControl TargetTextEditor { get; set; }
    }
}
