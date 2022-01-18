// System Namespaces


// Application Namespaces


// Library Namespaces


namespace Lib.Managers
{
    public static class LibManager
    {
        public static bool Initialized { get; set; } = false;
        
        public static void Initialize()
        {
            LoggingManager.Initialize();
            ShapingConfigurationsManager.Initialize();
            ShapingOperationsManager.Initialize();

            Initialized = true;
        }
        
    }
}
