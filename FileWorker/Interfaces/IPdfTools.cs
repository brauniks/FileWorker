using PdfFile.Common;
using System;
using System.Threading.Tasks;

namespace FileWorker.Interfaces
{
    public interface IPdfTools
    {
        string filePath { get; set; }
        Task CreatePdfFromHtmlFileAsync(string[] fileList, string outputPath = null);
        Task DeleteFilesAsync(KindOfFileEnum fileType = KindOfFileEnum.Any);
        Task SplitFile(string filePath);
        Action<FileEventArgs> eventHandler { get; set; }
    }
}