// System Namespaces
using System;
using System.IO;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Configurations;
using Lib.Shaping;


// Library Namespaces


namespace Lib.Managers
{
    public static class ShapingConfigurationsManager
    {
        public static ObservableCollection<Tuple<ShapingConfiguration, ShapeProject>> LocalShapingConfigurations { get; private set; }
        
        public static void Initialize()
        {
            LocalShapingConfigurations = GetLocalShapingConfigurations();
        }
        
        private static ObservableCollection<Tuple<ShapingConfiguration, ShapeProject>> GetLocalShapingConfigurations()
        {
            const string configsDir = @"configs\";

            var configs = new ObservableCollection<Tuple<ShapingConfiguration, ShapeProject>>();

            foreach (var filePath in Directory.EnumerateFiles(configsDir, "*.hjson", SearchOption.AllDirectories))
            {
                var config = ShapingConfiguration.Load(filePath);
                var shapeProject = new ShapeProject(config);

                configs.Add(new Tuple<ShapingConfiguration, ShapeProject>(config, shapeProject));
            }

            return configs;
        }
    }
}
