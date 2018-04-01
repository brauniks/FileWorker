using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFile.Common
{
   public class FileEventArgs : EventArgs
    {
       public FileEventArgs()
        {
            this.currentFile = 0;
            this.totalFiles = 0;
        }
        public string currentFileName { get; set; }
        public int currentFile { get; set; }

        public int totalFiles { get; set; }
    }
}
