// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Lib.Shapers.Loaders;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionToolBox : UserControl
    {
        public static ActionToolBox? Instance { get; private set; }

        private object? selectedAction;
        public object? SelectedAction
        {
            get => selectedAction;

            set
            {
                selectedAction = value;
                RefreshToolBox();
            }
        }

        public ActionToolBox()
        {
            Instance = this;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            CreateComponents();
        }

        private void CreateComponents()
        {
            NameTextBox = new TextBox
            {
                Text = "",
                IsVisible = true
            };

            ActionTextBox = new TextBox
            {
                Text = "",
                IsVisible = true
            };

            ActionNameText = this.Find<TextBlock>("actionNameText");
        }

        private void RefreshToolBox()
        {
            if (selectedAction is Builder builder)
                ActionNameText.Text = builder.Name;
        }

        private TextBox NameTextBox { get; set; }
        private TextBox ActionTextBox { get; set; }
        private TextBlock ActionNameText { get; set; }
    }
}
