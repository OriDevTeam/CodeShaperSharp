// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using AvaloniaUI.ViewModels.UserControls.HomeWindow;
using AvaloniaUI.Views;
using Lib.Managers;


// Library Namespaces
using ReactiveUI;


namespace AvaloniaUI.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel()
        {
            SearchResults = GenerateViewModels();
            
            MakeCommands();
        }

        private void MakeCommands()
        {
        }
        
        public ObservableCollection<ShaperViewModel> GenerateViewModels()
        {
            var shapingConfigurations = new ObservableCollection<ShaperViewModel>();
            
            foreach (var shapingConfiguration in ShapingConfigurationsManager.LocalShapingConfigurations)
                shapingConfigurations.Add(new ShaperViewModel(shapingConfiguration));

            return shapingConfigurations;
        }

        private ShaperViewModel? _selectedShaper;

        public ObservableCollection<ShaperViewModel> SearchResults { get; }

        public ShaperViewModel? SelectedShaper
        {
            get => _selectedShaper;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedShaper, value);
                HomePage.Instance.ToggleUserArea(true);
            }
        }
    }
}
