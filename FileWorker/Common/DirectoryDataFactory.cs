namespace PdfFile.Common
{
    using FileWorker.Common;
    using System;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="DirectoryInfoFactory" />
    /// </summary>
    public static class DirectoryDataFactory
    {

        public  static string[] GetFilesFromPattern(string filesPath , KindOfFileEnum type = KindOfFileEnum.Any)
        {
            return Directory.GetFiles(filesPath, type.GetSearchPattern());
        }
        /// <summary>
        /// The GetFileDialog
        /// </summary>
        public static string GetFilePathFromDialog(KindOfFileEnum type)
        {
            using (var dialog = new OpenFileDialog() { Filter = Search.GetSearchPatternForDialog(type)})
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
                else
                {
                    return string.Empty;
                }
            } 
        }

        public static string[] GetDirectoryFilesFromBrowserDialog(string filterPath)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return Directory.GetFiles(fbd.SelectedPath, filterPath, SearchOption.TopDirectoryOnly);
                }
                else
                {
                    return null;
                }
            }
        }

        public static string CreateChildDirectory(string filePath, string pattern)
        {
            var directoryName = Path.GetDirectoryName(filePath);
            var outputDirectoryPath = $"{directoryName}\\{pattern}";

            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }
            return outputDirectoryPath;
        }
    }
}
