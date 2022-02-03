// System Namespaces
using System.IO;
using Lib.AST.Settings;

// Application Namespaces
using Lib.AST.Settings.Interfaces;


// Library Namespaces
using Newtonsoft.Json;


namespace Lib.Managers
{
    public static class SettingsManager
    {
        public static bool Initialized { get; set; }
        
        public static IASTVisitorSettings VisitorSettings { get; private set; }
        
        public static void Initialize()
        {
            Initialized = true;
            
            LoadSettings();
        }
        
        private static void LoadSettings()
        {
            var settingsDirectory = @"settings/";
            var userSettingsFilePath = settingsDirectory + "user_settings.hjson";

            if (!File.Exists(userSettingsFilePath))
            {
                VisitorSettings = new ASTVisitorSettings();
                return;   
            }

            var hjsonPatch = Hjson.HjsonValue.Load(userSettingsFilePath).ToString();
            VisitorSettings = JsonConvert.DeserializeObject<IASTVisitorSettings>(hjsonPatch);
        }
    }
}
