// System Imports


using System.IO;
using Lib.AST.Controllers;
using Lib.Shapers.Interfaces;
// Application Imports


// Library Imports


namespace Lib.Shapers.Patch
{
    public class ShapePatchFile
    {
        protected string FilePath;
        
        protected string FileContent;

        public ShapePatchFile(string path)
        {
            FilePath = path;
            
            FileContent = File.ReadAllText(path);

            Name = Path.GetFileNameWithoutExtension(path);
        }

        protected string Name { get; set; }

        public ASTPreparationController PreparationController { get; set; }

        public IShapePatch Patch { get; init; }
    }
}
