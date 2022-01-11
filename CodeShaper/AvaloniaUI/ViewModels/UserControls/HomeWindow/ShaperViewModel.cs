// System Namespaces
using System;
using System.Windows.Input;


// Application Namespaces
using AvaloniaUI.Views;
using Lib.Configurations;
using Lib.Managers;
using Lib.Shaping;


// Library Namespaces
using ReactiveUI;


namespace AvaloniaUI.ViewModels.UserControls.HomeWindow
{
    public class ShaperViewModel : ViewModelBase
    {
        private readonly Tuple<ShapingConfiguration, ShapeProject> shapingConfiguration;

        public string Name => shapingConfiguration.Item1.Name;
        public string Summary => "A short description of this project";
        public string Description => shapingConfiguration.Item2.Configuration.Configuration.Description;
        public string ShapeProjectDirectory => "Shaper Project Location: " + shapingConfiguration.Item1.ShapeProjectDirectory;
        public string SourceDirectory => "Source Location: " + shapingConfiguration.Item1.SourceDirectory;
        
        public ShaperViewModel(Tuple<ShapingConfiguration, ShapeProject> shapingConfiguration)
        {
            this.shapingConfiguration = shapingConfiguration;
            
            MakeCommands();
            
            
        }

        public void MakeCommands()
        {
            GoBackCommand = ReactiveCommand.Create(async () =>
            {
                HomePage.Instance.ToggleUserArea(false);
            });
            
            StartShapeProjectCommand = ReactiveCommand.Create(async () =>
            {
                var shapingOperation = new ShapingOperation(shapingConfiguration.Item1, shapingConfiguration.Item2);
                
                ShapingOperationsManager.AddShapingOperations(shapingOperation);
                ShapingOperationsManager.SetShapingOperations(shapingOperation);
                
                _ShapingWindow = new Views.ShapingWindow()
                {
                    DataContext = new ShapingViewModel()
                };
                
                _ShapingWindow.Show();
                HomePage.Instance.Hide();
            });
        }
        
        public Views.ShapingWindow _ShapingWindow { get; private set; }
        
        public ICommand GoBackCommand { get; private set; }
        public ICommand StartShapeProjectCommand { get; private set; }
    }
}
