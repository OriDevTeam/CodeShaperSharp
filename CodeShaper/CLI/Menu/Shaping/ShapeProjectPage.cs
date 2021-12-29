// System Namespaces
using System;
using System.Runtime.Serialization;


// Application Namespaces
using CLI.Utility.Dialogs;
using CLI.Utility.Extensions;
using Lib.Configurations;
using Lib.Utility.Extensions;


// Library Namespaces
using EasyConsole;

namespace CLI.Menu.Shaping
{
    internal class ShapeProjectPage : MenuPage
    {
        ShapingConfiguration ShapingConfiguration;

        public ShapeProjectPage(ConsoleProgram program) : base("Shape Project", program)
        {
            ShapingConfiguration = new();
        }

        public override void Display()
        {
            ((ConsoleProgram)Program).Display();

            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();

            DisplayOptions();

            if (DisplayActions())
                Program.NavigateTo<ShapeProjectPage>();
        }

        private void DisplayOptions()
        {
            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, "Paths:");

            ConsoleExtensions.Write(ConsoleColor.Cyan, " - Source Files Directory: ");
            ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(ShapingConfiguration.SourceDirectory) ?
                "None" : ShapingConfiguration.SourceDirectory);
            Console.WriteLine();

            switch (ShapingConfiguration.ResultOptions)
            {
                case ResultOptions.ReplaceOriginal:
                    break;
                case ResultOptions.BackupAndReplaceMoveOriginal:
                    ConsoleExtensions.Write(ConsoleColor.Cyan, " - Backup Directory: ");
                    ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(ShapingConfiguration.TargetDirectory) ?
                        "None" : ShapingConfiguration.TargetDirectory);
                    Console.WriteLine();
                    break;
                case ResultOptions.CreateNew:
                    ConsoleExtensions.Write(ConsoleColor.Cyan, " - Shaped Files Directory: ");
                    ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(ShapingConfiguration.BackupDirectory) ?
                        "None" : ShapingConfiguration.BackupDirectory);
                    Console.WriteLine();
                    break;
                default:
                    break;
            }
            if (ShapingConfiguration.ResultOptions != ResultOptions.ReplaceOriginal)
            {
            }

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, "Result Options:");

            ConsoleExtensions.Write(ConsoleColor.Cyan, " - On shaping files: ");
            ConsoleExtensions.Write(ConsoleColor.Magenta, ShapingConfiguration.ResultOptions.ToEnumString());
        }


        private bool DisplayActions()
        {
            Output.WriteLine("\n");

            Output.WriteLine(ConsoleColor.Yellow, "What to do?: ");
            UserAction input = InputExtension.ReadEnumAttr<UserAction>("");

            switch (input)
            {
                case UserAction.SelectSourceProjectDirectory:
                    ShapingConfiguration.SourceDirectory = FolderDialog.SelectDirectory("Select source project directory");
                    break;
                case UserAction.SelectTargetProjectDirectory:
                    var dir = FolderDialog.SelectDirectory("Select target/backup project directory");
                    ShapingConfiguration.TargetDirectory = dir;
                    ShapingConfiguration.BackupDirectory = dir;
                    break;
                case UserAction.ChangeShapingResult:
                    ShapingConfiguration.ResultOptions = InputResult();
                    break;
                case UserAction.ShapeSourceFiles:
                    ((ConsoleProgram)Program).Make(ShapingConfiguration);
                    break;
                case UserAction.GoBack:
                    Program.NavigateBack();
                    return false;
                default:
                    throw new NotImplementedException($"Option not implemented: {input}");
            }

            return true;
        }

        private ResultOptions InputResult()
        {
            Console.Clear();
            ((ConsoleProgram)Program).Display();

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Yellow, "What should be the result of shaping?");
            var input = InputExtension.ReadEnumAttr<ResultOptions>("");

            return input;
        }

        public enum UserAction
        {
            [EnumMember(Value = "Select source project directory")]
            SelectSourceProjectDirectory,

            [EnumMember(Value = "Select target/backup project directory")]
            SelectTargetProjectDirectory,

            [EnumMember(Value = "Change shaping result")]
            ChangeShapingResult,

            [EnumMember(Value = "Shape source files")]
            ShapeSourceFiles,

            [EnumMember(Value = "Go back")]
            GoBack
        }
    }
}
