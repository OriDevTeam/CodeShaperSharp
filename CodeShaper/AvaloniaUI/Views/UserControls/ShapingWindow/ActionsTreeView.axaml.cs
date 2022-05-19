// System Namespaces


// Application Namespaces
using Avalonia.Controls;
using AvaloniaUI.ViewModels.UserControls.ShapingWindow;


// Library Namespaces
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionsTreeView : ReactiveUserControl<ActionsTreeViewModel>
    {

        public ActionsTreeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            // ActionsTreeViewControl = this.Find<TreeView>("TreeView");
            // ActionsTreeViewControl.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count < 1)
                return;

            if (ActionToolBoxView.Instance != null)
                ActionToolBoxView.Instance.SelectedAction = selectionChangedEventArgs.AddedItems[0];
        }
        
        private TreeView ActionsTreeViewControl { get; set; }
    }
}
