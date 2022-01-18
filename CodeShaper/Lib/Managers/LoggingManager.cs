// System Namespaces
using System.IO;


// Application Namespaces


// Library Namespaces
using Serilog;


namespace Lib.Managers
{
    public static class LoggingManager
    {
        public static StringWriter Messages { get; } = new();

        private static LoggerConfiguration LoggerConfiguration { get; set; }
        
        public static void Initialize()
        {
            LoggerConfiguration = new LoggerConfiguration()
                .WriteTo.TextWriter(Messages)
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
            
            Log.Logger = LoggerConfiguration.CreateLogger();

            Log.Information("Logging initialized");
        }
    }
}
