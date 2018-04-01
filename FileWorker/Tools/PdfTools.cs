namespace FileWorker
{
    using Common;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.tool.xml;
    using PdfFile.Common;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PdfTools" />
    /// </summary>
    internal class PdfTools
    {
        /// <summary>
        /// Gets or sets the eventHandler
        /// </summary>
        public Action<FileEventArgs> eventHandler { get; set; }

        /// <summary>
        /// Gets or sets the args
        /// </summary>
        public FileEventArgs args { get; set; }

        /// <summary>
        /// Gets or sets the filePath
        /// </summary>
        public string filePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfTools"/> class.
        /// </summary>
        public PdfTools()
        {
            this.args = new FileEventArgs();
        }

        /// <summary>
        /// The CreatePdfFromHtmlAsync
        /// </summary>
        /// <param name="fileList">The <see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task CreatePdfFromHtmlAsync(string[] fileList)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                  {
                      var directoryPath = DirectoryDataFactory.CreateChildDirectory(fileList[0], $"pdfy{ DateTime.Now.ToString("ddHHmm")}");

                      this.args.totalFiles = fileList.Length;
                      Parallel.For(0, this.args.totalFiles, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, (i) =>
                         {
                             this.args.currentFileName = fileList[i];
                             byte[] pdf;

                             var html = File.ReadAllText(fileList[i]);

                             using (var memoryStream = new MemoryStream())
                             {
                                 var document = new Document(PageSize.A4, 50, 50, 60, 60);
                                 var writer = PdfWriter.GetInstance(document, memoryStream);
                                 StringReader sr = new StringReader(html);
                                 document.Open();

                                 using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                                 {
                                     XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
                                 }

                                 document.Close();

                                 pdf = memoryStream.ToArray();

                                 var pdfFileName = Path.GetFileName(fileList[i]).Replace(".html", ".pdf");

                                 File.WriteAllBytes($"{directoryPath}\\{pdfFileName}", pdf);

                                 this.args.currentFile++;
                                 this.eventHandler?.Invoke(this.args);
                             }
                         });
                  });
            }
            catch (Exception)
            {
                throw new ProcessFileException(this.args);
            }
        }

        /// <summary>
        /// The CreatPdfFilesAsync
        /// </summary>
        /// <param name="countOfGeneratFIles">The <see cref="int"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task CreatPdfFilesAsync(int countOfGeneratFIles)
        {
            await Task.Factory.StartNew(() =>
            {

            });
        }

        /// <summary>
        /// The DeleteFilesAsync
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteFilesAsync(KindOfFileEnum fileType = KindOfFileEnum.Any)
        {
            await Task.Factory.StartNew(() =>
            {
                var fileList = DirectoryDataFactory.GetFilesFromPattern(this.filePath, fileType);

                Parallel.For(0, fileList.Length, (i) =>
                {
                    File.Delete(fileList[i]);
                });
            });
        }

        /// <summary>
        /// The SplitPdfFile
        /// </summary>
        /// <param name="v1">The <see cref="string"/></param>
        /// <param name="v2">The <see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task SplitPdfFile(string filePath)
        {
            await Task.Factory.StartNew(() =>
            {
                var outputDirectoryPath = DirectoryDataFactory.CreateChildDirectory(filePath, $"pages{ DateTime.Now.ToString("ddHHmm")}");


                PdfReader reader = new PdfReader(filePath); ;
                Document sourceDocument = null;
                PdfCopy pdfCopyProvider = null;
                PdfImportedPage importedPage = null;
                var fileName = Path.GetFileName(filePath);
                this.args.totalFiles = reader.NumberOfPages;

                for (int i = 1; i < reader.NumberOfPages; i++)
                {
                    var outPath = $"{outputDirectoryPath}\\{fileName.Replace(".pdf","")}_{i}_page.pdf";
                    sourceDocument = new Document(reader.GetPageSizeWithRotation(i));
                    pdfCopyProvider = new PdfCopy(sourceDocument, new FileStream(outPath, FileMode.Create));
                    sourceDocument.Open();
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                    sourceDocument.Close();

                    this.args.currentFile++;
                    this.eventHandler?.Invoke(this.args);
                }
                reader.Close();
            });
        }
    }
}
