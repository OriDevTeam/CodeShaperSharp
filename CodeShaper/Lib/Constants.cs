// System Imports
using System;


// Application Imports


// Library Imports


namespace Lib
{
    public static class Constants
    {
        private static readonly string SettingsDirectory = 
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"/CodeShaper";

        public static readonly string ConfigurationsDirectory = $@"{SettingsDirectory}/configurations/";
        
        public static readonly string UserSettingsPath = $@"{SettingsDirectory}/user_settings.hjson";
    }
}
