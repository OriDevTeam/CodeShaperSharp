// System Namespaces
using System;


// Application Namespaces
using AvaloniaUI.ViewModels;
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
        private DispatcherTimer UpdateTimer { get; }
        
        public ShapingWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Title = string.Format("{0} - {1}",
                ShapingOperationsManager.ActiveShapingOperation.ShapeProject.Configuration.Configuration.Name,
                ShapingOperationsManager.ActiveShapingOperation.ShapingTarget.Name
            );
            
            BindControls();
            
            UpdateTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(100),
            };
            
            UpdateTimer.Tick += UpdateEvent;
            UpdateTimer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void BindControls()
        {
            LogsBox = this.FindControl<TextBox>("LogsBox");
        }
        
        private void UpdateEvent(object? sender, EventArgs e)
        {
            var messages = LoggingManager.Messages.ToString();

            if (LogsBox.Text != null && messages == LogsBox.Text)
                return;
            
            var gotoEnd = LogsBox.CaretIndex == LogsBox.Text?.Length || LogsBox.Text == null;

            LogsBox.Text = LoggingManager.Messages.ToString();
            
            if (gotoEnd)
                LogsBox.CaretIndex = LogsBox.Text.Length;
        }

        private TextBox LogsBox { get; set; }
    }
}
