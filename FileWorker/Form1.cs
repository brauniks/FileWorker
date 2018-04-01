using FileWorker.Common;
using PdfFile.Common;
using PdfFile.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileWorker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Gets or sets the fileList
        /// </summary>
        private string[] fileList { get; set; }


        /// <summary>
        /// The Progress
        /// </summary>
        /// <param name="pdf">The <see cref="PdfTools"/></param>
        /// <returns>The <see cref="Task"/></returns>
        internal void ProgressBarEventHandler(FileEventArgs fileArgs)
        {
            if (fileArgs.currentFile != 0 && fileArgs.totalFiles != 0)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    progressBar1.Value = (int)Math.Ceiling((double)fileArgs.currentFile / (double)fileArgs.totalFiles * 100);

                }));
            }
        }

        /// <summary>
        /// The button2_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.fileList = DialogFactory.GetDirectoryFilesFromBrowserDialog("*html");
            if (fileList != null)
            {
                MessageBox.Show($"Files found: {fileList.Length.ToString()} files");
            }
            else 
            {
                return;
            }

            this.textBox1.Text = Path.GetDirectoryName(fileList[0]);
            this.SetGenerateButtonStatus(button2,button3,fileList.Length, true);
        }

        /// <summary>
        /// The SetGenerateButtonStatus
        /// </summary>
        /// <param name="count">The <see cref="int"/></param>
        /// <param name="isOpenFolderButtonVisible">The <see cref="bool"/></param>
        private void SetGenerateButtonStatus(object button, object buttonGenerate,int count, bool isOpenFolderButtonVisible)
        {
            if (button is Button && buttonGenerate is Button)
            {
                ((Button)button).Enabled = isOpenFolderButtonVisible;
                ((Button)buttonGenerate).Enabled = count > 0 ? true : false;
            }
        }

        /// <summary>
        /// The button3_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private async void button3_Click(object sender, EventArgs e)
        {
            var timing = Statistics.TimerFactory();
            this.SetGenerateButtonStatus(button3,button2,0, false);
            PdfTools files = new PdfTools();
            files.eventHandler += this.ProgressBarEventHandler;
            try
            {
                await files.CreatePdfFromHtmlAsync(fileList);
            }
            catch (ProcessFileException ex)
            {
                MessageBox.Show($"Error in generating PDF file {Environment.NewLine} Current File:{ex.args.currentFileName}");
            }

            Statistics.ShowTaskCompleted(timing);

            this.SetGenerateButtonStatus(button2, button3, 0, true);
        }

        /// <summary>
        /// The button4_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private async void button4_Click(object sender, EventArgs e)
        {
            PdfTools pdf = new PdfTools();
            pdf.eventHandler += this.ProgressBarEventHandler;
            var timer = Statistics.TimerFactory();
            await pdf.SplitPdfFile(@"C:\pdffolder\wzorce.pdf", @"C:\pdffolder");
            Statistics.ShowTaskCompleted(timer);
        }

        /// <summary>
        /// The button5_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.fileList = new string[] { DialogFactory.GetFilePathFromDialog(KindOfFileEnum.Pdf) };

            if (string.IsNullOrEmpty(this.fileList[0]))
            {
                MessageBox.Show("Error: please input correct .pdf file");
                return;
            }
            this.textBox2.Text = Path.GetDirectoryName(fileList[0]);
            this.SetGenerateButtonStatus(button5,button4,fileList.Length, true);
        }
    }
}
