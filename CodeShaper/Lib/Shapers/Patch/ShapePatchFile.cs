// System Imports
using System.IO;


// Application Imports
using Lib.AST.Controllers;
using Lib.Shapers.Interfaces;


// Library Imports


namespace Lib.Shapers.Patch
{
    public class ShapePatchFile
    {
        protected string FilePath;
        
        protected string FileContent;
        
        protected string Name { get; set; }

        public ASTPreparationController PreparationController { get; set; }

        public IShapePatch Patch { get; protected init; }

        protected ShapePatchFile(string path)
        {
            FilePath = path;
            
            FileContent = File.ReadAllText(path);

            Name = Path.GetFileNameWithoutExtension(path);
        }

    }
}
