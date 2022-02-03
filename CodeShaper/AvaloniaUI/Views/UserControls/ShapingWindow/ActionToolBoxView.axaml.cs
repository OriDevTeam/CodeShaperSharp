// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaUI.ViewModels.UserControls.ShapingWindow;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionToolBoxView : ReactiveUserControl<ActionToolBoxView>
    {
        public static ActionToolBoxView? Instance { get; private set; }

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

        public ActionToolBoxView()
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
            ActionNameText = this.Find<TextBlock>("ActionNameText");
            FieldsListBox = this.Find<ListBox>("FieldsListBox");
        }

        private void RefreshToolBox()
        {
            Fields.Clear();

            switch (selectedAction)
            {
                case IShapeActionsBuilder builder:
                    ActionNameText.Text = builder.Name;
                
                    Fields = new ObservableCollection<Tuple<string, string>>
                    {
                        new("Name:", builder.Name ?? string.Empty),
                        new("Type:", "Builder"),
                        new("Location:", builder.Location?.ToString() ?? string.Empty),
                        new("Reference Location:", builder.ReferenceLocation?.ToString() ?? string.Empty),
                        new("Reference:", builder.Reference ?? string.Empty),
                        new("Reference Flags:", builder.ReferenceFlags ?? string.Empty),
                        new("Build:", builder.Build ?? string.Empty),
                        new("Result:", builder.Result ?? string.Empty),
                        new("Root:", builder.RootBuilder?.Name ?? string.Empty),
                        new("Parent:", builder.ParentBuilder?.Name ?? string.Empty),
                        new("Active:", builder.ActiveBuilder?.Name ?? string.Empty),
                        new("Context:", builder.Context ?? string.Empty)
                    };
                    
                    break;
                
                case IShapeActionsMaker maker:
                    ActionNameText.Text = maker.Name;
                
                    Fields = new ObservableCollection<Tuple<string, string>>
                    {
                        new("Name:", maker.Name ?? string.Empty),
                        new("Type:", "Maker"),
                        new("Prepare:", maker.Prepare),
                        new("Make:", maker.Make),
                    };
                    
                    break;
            }
        }

        private ObservableCollection<Tuple<string, string>> Fields
        {
            get => ActionToolBoxViewModel.Instance.Fields;
            set => ActionToolBoxViewModel.Instance.Fields = value;
        }

        private ListBox FieldsListBox { get; set; }
        private TextBlock ActionNameText { get; set; }
    }
}
