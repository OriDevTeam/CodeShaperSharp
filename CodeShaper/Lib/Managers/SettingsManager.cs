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
        private static bool Initialized { get; set; }
        
        public static IASTVisitorSettings VisitorSettings { get; private set; }
        
        public static void Initialize()
        {
            if (Initialized)
                return;
            
            Initialized = true;
            
            LoadSettings();
        }
        
        private static void LoadSettings()
        {
            if (!File.Exists(Constants.UserSettingsPath))
            {
                VisitorSettings = new ASTVisitorSettings();
                return;   
            }

            var hjsonPatch = Hjson.HjsonValue.Load(Constants.UserSettingsPath).ToString();
            VisitorSettings = JsonConvert.DeserializeObject<IASTVisitorSettings>(hjsonPatch);
        }
    }
}
