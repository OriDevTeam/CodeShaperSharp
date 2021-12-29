// System Namespaces
using System;
using System.IO;


// Application Namespaces
using Lib.Configurations;


// Library Namespaces


namespace Lib.Shaping
{
    public partial class ShapeResult
    {
        public void Save(string filePath, ShapingConfiguration configuration)
        {
            var targetFilePath = "";

            switch (configuration.ResultOptions)
            {
                case ResultOptions.ReplaceOriginal:
                    if (File.Exists(targetFilePath))
                        File.Delete(targetFilePath);

                    targetFilePath = filePath;

                    break;

                case ResultOptions.BackupAndReplaceMoveOriginal:
                    if (File.Exists(filePath))
                        File.Move(filePath, configuration.BackupDirectory);

                    targetFilePath = filePath;
                    break;

                case ResultOptions.CreateNew:
                    var amount = filePath.Length - configuration.SourceDirectory.Length;
                    var sub = filePath.Substring(configuration.SourceDirectory.Length, amount);

                    targetFilePath = configuration.TargetDirectory + sub;

                    if (File.Exists(targetFilePath))
                        File.Delete(targetFilePath);

                    break;
                default:
                    throw new NotImplementedException("Result Options not implemented");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

            File.WriteAllText(targetFilePath, FileContent);
            SaveLogs(targetFilePath);
        }

        private void SaveLogs(string filePath, ShapingConfiguration configuration)
        {
            var targetFilePath = "";
        }

        private void SaveLogs(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath);

            SaveReplacementsLogs(dir);
            SaveAdditionsLogs(dir);
            SaveSubtractionsLogs(dir);
        }

        private void SaveReplacementsLogs(string directory)
        {

            for (int i = 0; i < Replacements.Count; i++)
            {
                var replacement = Replacements[i];

                Directory.CreateDirectory(directory);

                var path = string.Format(@"{0}\{1}_{2}.rep.log", directory, i, replacement.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             replacement.Item2, replacement.Item3);

                File.WriteAllText(path, content);
            }
        }

        private void SaveAdditionsLogs(string directory)
        {
            for (int i = 0; i < Additions.Count; i++)
            {
                var addition = Additions[i];

                Directory.CreateDirectory(directory);

                var path = string.Format(@"{0}\{1}_{2}.add.log", directory, i, addition.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             addition.Item2, addition.Item3);

                File.WriteAllText(path, content);
            }
        }

        private void SaveSubtractionsLogs(string directory)
        {
            for (int i = 0; i < Subtractions.Count; i++)
            {
                var subtraction = Subtractions[i];

                Directory.CreateDirectory(directory);

                var path = string.Format(@"{0}\{1}_{2}.sub.log", directory, i, subtraction.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             subtraction.Item2, subtraction.Item3);

                File.WriteAllText(path, content);
            }
        }
    }
}
