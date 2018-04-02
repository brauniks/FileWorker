using Castle.Windsor;
using Castle.Windsor.Installer;
using FileWorker.Common;
using FileWorker.Controllers;
using FileWorker.Interfaces;
using FileWorker.Tools;
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
            container = new WindsorContainer();
            container.Install(FromAssembly.This());
            this.controller = this.container.Resolve<FileWorkerController>();
            this.inUse = false;
            InitializeComponent();
        }
        /// <summary>
        /// Gets or sets the fileList
        /// </summary>
        private string[] fileList { get; set; }

        IWindsorContainer container;
        FileWorkerController controller;
        private bool inUse { get; set; }
        public string fileXslt { get; private set; }

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
            controller.txtBoxEventHandler = SetCurrentPath;
            controller.OpenFileDialogConvertHtml(textBox1);
        }

        public void SetCurrentPath(TextBox txtBox, string text)
        {
            txtBox.Text = text;
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
            try
            {
                this.inUse = true;
                this.controller.GenerateHtmlToPdfFile(this.ProgressBarEventHandler);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.inUse = false;
            }       
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
        private void button4_Click(object sender, EventArgs e)
        {
            if (!this.CheckIfOperationPossible())
            {
                return;
            }

            try
            {
                this.inUse = true;
                this.controller.GenerateSplitPdf(this.ProgressBarEventHandler);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.inUse = false;
            }     
        }

        /// <summary>
        /// The button5_Click
        /// </summary>
        /// <param name="sender">The <see cref="object"/></param>
        /// <param name="e">The <see cref="EventArgs"/></param>
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                this.inUse = true;
                controller.txtBoxEventHandler = SetCurrentPath;
                controller.OpenFileDialogSplitPdf(textBox2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            controller.txtBoxEventHandler = SetCurrentPath;
            this.controller.OpenFileDialogXml(textBox3);          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.controller.txtBoxEventHandler = SetCurrentPath;
            this.controller.OpenFileDialogXslt(textBox4);
        }

        private void EnableGenerateButton()
        {
            if (this.textBox3.Text != string.Empty && this.textBox4.Text != string.Empty)
                this.button6.Enabled = true;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            if (!this.CheckIfOperationPossible())
            {
                return;
            }
            try
            {
                this.inUse = true;
                await this.controller.GenerateXmlToPdfFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.inUse = false;
            }
        }
    }
}
