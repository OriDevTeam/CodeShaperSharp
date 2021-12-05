// System Namespaces
using System.Runtime.Serialization;


// Application Namespaces


// Library Namespaces


namespace Lib.Configurations
{
    public class ShapingConfiguration
    {
        public string ShapeProjectDirectory { get; set; }

        public string SourceDirectory { get; set; }

        public string TargetDirectory { get; set; }

        public string BackupDirectory { get; set; }

        public ResultOptions ResultOptions { get; set; }

        public ShapingConfiguration()
        {
            ResultOptions = ResultOptions.BackupAndReplaceMoveOriginal;
        }
    }

    public enum ResultOptions
    {
        [EnumMember(Value = "Replace the original files")]
        ReplaceOriginal,

        [EnumMember(Value = "Backup source files, move and replace them with shaped files")]
        BackupAndReplaceMoveOriginal,

        [EnumMember(Value = "Create new shaped files in target directory")]
        CreateNew
    }
}
