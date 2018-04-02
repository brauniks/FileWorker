using Castle.Windsor;
using Castle.Windsor.Installer;
using FileWorker.Common;
using FileWorker.Interfaces;
using FileWorker.Tools;
using PdfFile.Common;
using PdfFile.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileWorker.Controllers
{
    public class FileWorkerController
    {
        private IXmlTools xml;
        private IPdfTools pdf;

        public string[] fileList { get; private set; }

        public Action<TextBox,string> txtBoxEventHandler { get; set; }
        public string fileXslt { get; private set; }
        
        public FileWorkerController(IPdfTools pdf , IXmlTools xml)
        {
            this.xml = xml;
            this.pdf = pdf;            
        }


        public void OpenFileDialogSplitPdf(TextBox sender)
        {
            this.fileList = new string[] { DirectoryDataFactory.GetFilePathFromDialog(KindOfFileEnum.Pdf) };

            if (string.IsNullOrEmpty(this.fileList[0]))
            {
                MessageBox.Show("Error: please input correct .pdf file");
                return;
            }
            
                this.txtBoxEventHandler?.Invoke(sender, Path.GetDirectoryName(this.fileList[0]));
        }

        internal async void GenerateSplitPdf(Action<FileEventArgs> progressBarEventHandler)
        {
            var timing = StatisticsTools.TimerFactory();
            pdf.eventHandler += progressBarEventHandler;

            var timer = StatisticsTools.TimerFactory();
            try
            {
                await pdf.SplitFile(this.fileList[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            StatisticsTools.ShowTaskCompleted(timer);
        }

        internal void OpenFileDialogConvertHtml(TextBox sender)
        {
            this.fileList = DirectoryDataFactory.GetDirectoryFilesFromBrowserDialog("*html");
            if (fileList != null)
            {
                var count = fileList.Length;
                MessageBox.Show($"Files found: {count.ToString()} files");
                if (count == 0)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            this.txtBoxEventHandler?.Invoke(sender, Path.GetDirectoryName(fileList[0]));
        }

        internal void OpenFileDialogXml(TextBox sender)
        {
            this.fileList = DirectoryDataFactory.GetDirectoryFilesFromBrowserDialog("*xml");
            if (fileList != null)
            {
                var count = fileList.Length;
                MessageBox.Show($"Files found: {count.ToString()} files");
                if (count == 0)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            this.txtBoxEventHandler?.Invoke(sender, Path.GetDirectoryName(fileList[0]));
        }

        internal void OpenFileDialogXslt(TextBox sender)
        {
            this.fileXslt = DirectoryDataFactory.GetFilePathFromDialog(KindOfFileEnum.Xslt);
            if (string.IsNullOrEmpty(fileXslt))
            {
                return;
            }
            else
            {
                MessageBox.Show($"Files found: {fileXslt}");
            }
            this.txtBoxEventHandler(sender, Path.GetDirectoryName(fileXslt));
        }

        internal async Task GenerateXmlToPdfFile()
        {
            var timer = StatisticsTools.TimerFactory();
            var directoryXMLPath = DirectoryDataFactory.CreateChildDirectory(this.fileList[0], $"xml{ DateTime.Now.ToString("ddHHmm")}");

            var directoryPdfPath = DirectoryDataFactory.CreateChildDirectory(this.fileList[0], $"htmle{ DateTime.Now.ToString("ddHHmm")}");

            await xml.TransformXML(this.fileList, this.fileXslt, directoryXMLPath);            
            
            await pdf.CreatePdfFromHtmlFileAsync(DirectoryDataFactory.GetDirectoryFilesFromPath(directoryXMLPath), directoryPdfPath);

            DirectoryDataFactory.DeleteDirectory(directoryXMLPath);
            StatisticsTools.ShowTaskCompleted(timer);
        }

        internal async void GenerateHtmlToPdfFile(Action<FileEventArgs> progressBarEventHandler)
        {
            var timing = StatisticsTools.TimerFactory();

            pdf.eventHandler += progressBarEventHandler;
            try
            {
                await pdf.CreatePdfFromHtmlFileAsync(fileList);
            }
            catch (ProcessFileException ex)
            {
                MessageBox.Show($"Error in generating PDF file {Environment.NewLine} Current File:{ex.args.currentFileName}");
            }

            StatisticsTools.ShowTaskCompleted(timing);
        }
    }
}
