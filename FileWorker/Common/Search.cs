using PdfFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWorker.Common
{
   public static class Search
    {
        public static string GetSearchPatternForDialog(KindOfFileEnum pattern)
        {
            return Search.GetSearchPattern(pattern).Replace("*", $"Files (.{pattern.ToString()})|*.");
        }
        public static string GetSearchPattern(this KindOfFileEnum fileType)
        {
            string searchPattern;
            switch (fileType)
            {
                case KindOfFileEnum.Any:
                    searchPattern = "*";
                    break;
                case KindOfFileEnum.Pdf:
                    searchPattern = "*pdf";
                    break;
                case KindOfFileEnum.Doc:
                    searchPattern = "*doc";
                    break;
                case KindOfFileEnum.Xml:
                    searchPattern = "*xml";
                    break;
                case KindOfFileEnum.Xslt:
                    searchPattern = "*xslt";
                    break;
                case KindOfFileEnum.Html:
                    searchPattern = "*html";
                    break;
                default:
                    searchPattern = "*";
                    break;
            }

            return searchPattern;
        }
    }
}
