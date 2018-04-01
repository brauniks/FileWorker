namespace PdfFile.Common
{
    using FileWorker.Common;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="DialogFactory" />
    /// </summary>
    public static class DialogFactory
    {
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
    }
}
