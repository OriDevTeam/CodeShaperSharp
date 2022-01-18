// System Namespaces


// Application Namespaces

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaUI.ViewModels.UserControls.ShapingWindow;


// Library Namespaces
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionsTreeView : ReactiveUserControl<ActionsTreeViewModel>
    {
        private TreeView actionsTreeViewControl { get; set; }
        
        public ActionsTreeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            actionsTreeViewControl = this.Find<TreeView>("treeView");
            actionsTreeViewControl.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count < 1)
                return;

            ActionToolBox.Instance.SelectedAction = selectionChangedEventArgs.AddedItems[0];
        }
    }
}
