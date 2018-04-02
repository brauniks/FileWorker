using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace FileWorker.Interfaces
{
    public interface IXmlTools
    {
        Task TransformXML(string[] inputXml, string inputXslt, string outputPathDirectory);
        Dictionary<string, XslCompiledTransform> cachedTransforms { get; set; }

        XslCompiledTransform GetAndCacheTransform(string xslt);
    }
}