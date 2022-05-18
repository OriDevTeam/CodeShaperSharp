// System Namespaces


// Application Namespaces


// Library Namespaces


namespace Lib.Managers
{
    public static class LibManager
    {
        public static bool Initialized { get; set; }
        
        public static void Initialize(bool console = false)
        {
            if (Initialized)
                return;
            
            LoggingManager.Initialize(console);
            ShapingConfigurationsManager.Initialize();
            ShapingOperationsManager.Initialize();
            SettingsManager.Initialize();

            Initialized = true;
        }
        
    }
}
