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
        
        public static void Initialize(bool console)
        {
            LoggerConfiguration = new LoggerConfiguration()
                .WriteTo.TextWriter(Messages)
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);

            if (!console)
                LoggerConfiguration = LoggerConfiguration.WriteTo.Console();
            
            Log.Logger = LoggerConfiguration.CreateLogger();

            Log.Information("Logging initialized");
        }
    }
}
