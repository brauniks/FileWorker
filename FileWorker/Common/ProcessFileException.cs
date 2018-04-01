using PdfFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWorker.Common
{
    public class ProcessFileException :Exception
    {
        public FileEventArgs args { get; set; }
        public ProcessFileException(FileEventArgs _args)
        {
            this.args = _args;
        }
    }
}
