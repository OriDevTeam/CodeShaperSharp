// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;


namespace AvaloniaUI.Views.UserControls.EditTextEditor
{
    public class TextEditorUserControl : UserControl
    {
        public TextEditorUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            BindControls();
            PrepareControls();
        }

        private void BindControls()
        {
            TextEditor = this.FindControl<TextEditor>("TextCode");
        }

        private void PrepareControls()
        {
            TextEditor!.ShowLineNumbers = true;
        }
        
        public TextEditor? TextEditor { get; private set; }
    }
}
