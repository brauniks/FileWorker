namespace FileWorker.Tools
{
    using PdfFile.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Linq;
    using Interfaces;

    /// <summary>
    /// Defines the <see cref="HtmlTools" />
    /// </summary>
    public class HtmlTools 
    {
        /// <summary>
        /// The CreateHtmlFromString
        /// </summary>
        /// <param name="filePath">The <see cref="string"/></param>
        /// <param name="content">The <see cref="string[]"/></param>
        public void CreateHtmlFromString(string[] content)
        {
            //var fileName = Path.GetFileName(content[0]).Replace(".xml", ".html");
            //var childDirectory = DirectoryDataFactory.CreateChildDirectory(content.First().Key.ToString(), $"htmle{ DateTime.Now.ToString("ddHHmm")}");

            //Parallel.For(0, content.Length, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, (i) =>
            //{
            //    File.WriteAllText($"{childDirectory}\\{fileName}", content[0]);
            //});
        }
    }
}
