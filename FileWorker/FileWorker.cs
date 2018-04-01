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
    public partial class FileWorker : Form
    {
        public FileWorker()
        {
            this.inUse = false;
            InitializeComponent();
        }
        /// <summary>
        /// Gets or sets the fileList
        /// </summary>
        private string[] fileList { get; set; }

        private bool inUse { get; set; }

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
            if (!this.CheckIfOperationPossible())
            {
                return;
            }
            var timing = StatisticsTools.TimerFactory();
            this.SetGenerateButtonStatus(button3,button2,0, false);
            PdfTools files = new PdfTools();
            files.eventHandler += this.ProgressBarEventHandler;
            try
            {
                this.inUse = true;
                await files.CreatePdfFromHtmlAsync(fileList);
            }
            catch (ProcessFileException ex)
            {
                MessageBox.Show($"Error in generating PDF file {Environment.NewLine} Current File:{ex.args.currentFileName}");
            }
            finally
            {
                this.inUse = false;
            }

            StatisticsTools.ShowTaskCompleted(timing);

            this.SetGenerateButtonStatus(button2, button3, 0, true);
        }

        private bool CheckIfOperationPossible()
        {
            if (inUse)
            {
                MessageBox.Show("There is a process running in background");
                return false;
            }
            else return true;
        }


        /// <summary>
        /// The button4_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private async void button4_Click(object sender, EventArgs e)
        {
            if (!this.CheckIfOperationPossible())
            {
                return;
            }

            this.SetGenerateButtonStatus(button5, button4, 0, false);
            PdfTools pdf = new PdfTools();
            pdf.eventHandler += this.ProgressBarEventHandler;
            var timer = StatisticsTools.TimerFactory();
            try
            {
                this.inUse = true;
                await pdf.SplitPdfFile(this.fileList[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.inUse = false;
            }
            StatisticsTools.ShowTaskCompleted(timer);
            this.SetGenerateButtonStatus(button5, button4, 0, true);
        }

        /// <summary>
        /// The button5_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.fileList = new string[] { DirectoryDataFactory.GetFilePathFromDialog(KindOfFileEnum.Pdf) };

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
