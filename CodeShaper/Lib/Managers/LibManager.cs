﻿// System Namespaces


// Application Namespaces


// Library Namespaces


namespace Lib.Managers
{
    public static class LibManager
    {
        public static void Initialize()
        {
            LoggingManager.Initialize();
            ShapingConfigurationsManager.Initialize();
            ShapingOperationsManager.Initialize();
        }
    }
}