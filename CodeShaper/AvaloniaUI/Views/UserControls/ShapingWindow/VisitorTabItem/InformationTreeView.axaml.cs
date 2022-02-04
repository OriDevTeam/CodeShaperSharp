// System Namespaces


// Application Namespaces

using System;
using System.Collections.Generic;
using AvaloniaUI.ViewModels.UserControls.ShapingWindow.VisitorTabItem;
using AvaloniaUI.Views.UserControls.EditTextEditor;


// Library Namespaces
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.Markup.Xaml;


namespace AvaloniaUI.Views.UserControls.ShapingWindow.VisitorTabItem
{
    public class InformationTreeView : ReactiveUserControl<InformationTreeViewModel>
    {
        
        public static InformationTreeView? Instance { get; private set; }
        
        public InformationTreeView()
        {
            Instance = this;

            ViewModel = (InformationTreeViewModel?)Content;
            
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            BindControls();
        }

        private void BindControls()
        {
            LocationsTreeViewControl = this.Find<TreeView>("LocationsTreeView");
            LocationsTreeViewControl.SelectionChanged += OnSelectionChanged;
            
            LocationTextEditor = this.Find<TextEditorUserControl>("LocationTextEditor");
        }

        internal void Refresh()
        {
            ViewModel ??= (InformationTreeViewModel?)Content;
            ViewModel?.Refresh();
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count < 1)
                return;

            var location = (KeyValuePair<Enum, string>)selectionChangedEventArgs.AddedItems[0]!;

            if (LocationTextEditor.TextEditor != null)
                LocationTextEditor.TextEditor.Text = location.Value;
        }
        
        private TreeView LocationsTreeViewControl { get; set; }
        
        private TextEditorUserControl LocationTextEditor { get; set; }
    }
}
