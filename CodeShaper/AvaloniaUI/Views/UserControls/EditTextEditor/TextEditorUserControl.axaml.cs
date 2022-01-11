// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.Indentation.CSharp;


namespace AvaloniaUI.Views.UserControls.EditTextEditor
{
    public class TextEditorUserControl : UserControl
    {
        private readonly TextEditor _textEditor;
        
        public TextEditorUserControl()
        {
            InitializeComponent();
            
            _textEditor = this.FindControl<TextEditor>("TextCode");
            _textEditor.ShowLineNumbers = true;
            _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
