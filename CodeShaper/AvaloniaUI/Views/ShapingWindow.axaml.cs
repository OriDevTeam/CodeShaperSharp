// System Namespaces
using System;


// Application Namespaces
using AvaloniaUI.ViewModels;
using Lib.Projects.VCXSolution;
using Lib.Managers;


// Library Namespaces
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;


namespace AvaloniaUI.Views
{
    public class ShapingWindow : ReactiveWindow<ShapingViewModel>
    {
        public TextBox LogsBox { get; }
        private TextBlock TimeElapsedTextBox { get; }
        private DispatcherTimer UpdateTimer { get; }
        
        public ShapingWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Title = string.Format("{0} - {1}",
                ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Configuration.Configuration.Name,
                ((VCXSolutionTarget)ShapingOperationsManager.ActiveShapingOperation.ShapingTarget).SolutionInformation.Name
            );
            
            LogsBox = this.FindControl<TextBox>("LogsBox");
            
            UpdateTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(100),
            };
            
            LogsBox = this.FindControl<TextBox>("LogsBox");

            TimeElapsedTextBox = this.FindControl<TextBlock>("TimeElapsedText");
            
            UpdateTimer.Tick += UpdateEvent;
            UpdateTimer.Start();
        }

        private void UpdateEvent(object? sender, EventArgs e)
        {
            ShapingUpdate();
            LogsBox.Text = LoggingManager.Messages.ToString();
        }

        private void ShapingUpdate()
        {
            TimeElapsedTextBox.Text = $"Time Elapsed: {ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Elapsed}";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
