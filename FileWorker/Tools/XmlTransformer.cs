namespace FileWorker.Tools
{
    using Interfaces;
    using PdfFile.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Xsl;

    /// <summary>
    /// Defines the <see cref="XmlTransformer" />
    /// </summary>
    public class XmlTransformer : IProcessBarHandler
    {
        public XmlTransformer()
        {
            this.cachedTransforms = new Dictionary<string, XslCompiledTransform>();
            this.args = new FileEventArgs();
        }

        /// <summary>
        /// Gets or sets the eventHandler
        /// </summary>
        public Action<FileEventArgs> eventHandler { get; set; }

        /// <summary>
        /// Gets or sets the args
        /// </summary>
        public FileEventArgs args { get; set; }

        /// <summary>
        /// Defines the cachedTransforms
        /// </summary>
        private Dictionary<String, XslCompiledTransform> cachedTransforms = new Dictionary<string, XslCompiledTransform>();
                
        /// <summary>
        /// The TransformXMLToHTML
        /// </summary>
        /// <param name="inputXml">The <see cref="string[]"/></param>
        /// <param name="inputXslt">The <see cref="string"/></param>
        /// <param name="outputPathDirectory">The <see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task TransformXMLToHTML(string[] inputXml, string inputXslt, string outputPathDirectory)
        {
            await Task.Factory.StartNew(() =>
            {
                var xsltString = File.ReadAllText(inputXslt, Encoding.UTF8);
                XslCompiledTransform transform = this.GetAndCacheTransform(xsltString);
                this.args.totalFiles = inputXml.Length;
                Parallel.For(0, inputXml.Length, (i) =>
                {
                    var fileName = Path.GetFileName(inputXml[i]);
                    this.args.currentFileName = fileName;
                    var xmlString = File.ReadAllText(inputXml[i], Encoding.UTF8);

                    StringWriter results = new StringWriter();
                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
                    {
                        transform.Transform(reader, null, results);
                    }

                    File.WriteAllText($"{outputPathDirectory}\\{fileName.Replace(".xml", ".html")}", results.ToString());

                    this.args.currentFile++;
                    this.eventHandler?.Invoke(this.args);
                });
            });
        }

        /// <summary>
        /// The GetAndCacheTransform
        /// </summary>
        /// <param name="xslt">The <see cref="string"/></param>
        /// <returns>The <see cref="XslCompiledTransform"/></returns>
        private XslCompiledTransform GetAndCacheTransform(string xslt)
        {
            XslCompiledTransform transform;
            if (!cachedTransforms.TryGetValue(xslt, out transform))
            {
                transform = new XslCompiledTransform();
                using (XmlReader reader = XmlReader.Create(new StringReader(xslt)))
                {
                    transform.Load(reader);
                }
                cachedTransforms.Add(xslt, transform);
            }
            return transform;
        }
    }
}
