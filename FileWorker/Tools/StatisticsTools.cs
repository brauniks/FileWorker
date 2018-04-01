using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfFile.Tools
{
    public static class StatisticsTools
    {
        public static Stopwatch TimerFactory()
        {
            var timer = new Stopwatch();
            timer.Start();
            return timer;
        }
        public static void ShowTaskCompleted(Stopwatch timer)
        {
            MessageBox.Show($"Task Completed: {timer.ElapsedMilliseconds} ms elapsed");
        }
    }
}
