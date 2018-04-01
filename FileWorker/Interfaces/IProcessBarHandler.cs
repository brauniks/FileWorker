using PdfFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWorker.Interfaces
{
   public interface IProcessBarHandler
    {
        /// <summary>
        /// Gets or sets the eventHandler
        /// </summary>
        Action<FileEventArgs> eventHandler { get; set; }

        /// <summary>
        /// Gets or sets the args
        /// </summary>

        FileEventArgs args { get; set; }
    }
}
