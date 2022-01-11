// System Namespaces
using System;


// Application Namespaces


// Library Namespaces
using Avalonia;
using AvaloniaEdit;
using Avalonia.Xaml.Interactivity;


namespace AvaloniaUI.Views.UserControls.EditTextEditor
{
    public class DocumentTextBindingBehavior : Behavior<TextEditor>
    {
        private TextEditor? textEditor;

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<DocumentTextBindingBehavior, string>(nameof(Text));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not TextEditor textEditor) return;
            
            this.textEditor = textEditor;
            this.textEditor.TextChanged += TextChanged;
            this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (textEditor != null)
            {
                textEditor.TextChanged -= TextChanged;
            }
        }

        private void TextChanged(object sender, EventArgs eventArgs)
        {
            if (textEditor != null && textEditor.Document != null)
            {
                Text = textEditor.Document.Text;
            }
        }

        private void TextPropertyChanged(string text)
        {
            if (textEditor != null && textEditor.Document != null && text != null)
            {
                var caretOffset = textEditor.CaretOffset;
                textEditor.Document.Text = text;
                textEditor.CaretOffset = caretOffset;
            }
        }
    }
}
