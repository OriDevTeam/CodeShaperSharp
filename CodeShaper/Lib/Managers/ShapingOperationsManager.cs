// System Namespaces
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping;


// Library Namespaces


namespace Lib.Managers
{
    public static class ShapingOperationsManager
    {
        private static List<ShapingOperation> ShapingOperations { get; } = new();
        
        public static ShapingOperation ActiveShapingOperation { get; private set; }
        
        public static void Initialize()
        {
        }
        
        public static void SetShapingOperations(ShapingOperation shapingOperation)
        {
            ActiveShapingOperation = shapingOperation;
        }
        
        public static void AddShapingOperations(ShapingOperation shapingOperation)
        {
            ShapingOperations.Add(shapingOperation);
        }
    }
}
