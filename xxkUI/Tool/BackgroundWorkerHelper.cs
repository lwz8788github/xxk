using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
    public class BackgroundWorkerHelper
    {

        /// <summary>
        /// 输出批处理的日志
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="loginfo"></param>
        public static void outputWorkerLog(MyBackgroundWorker worker, string loginfo)
        {
            worker.StatusText = loginfo;
            worker.ReportProgress(20);
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// 输出批处理的日志
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="loginfo"></param>
        public static void outputWorkerLog(MyBackgroundWorker worker, LogType lType, string loginfo)
        {
            worker.logType = lType;
            worker.StatusText = loginfo;
            worker.ReportProgress(20);
            System.Threading.Thread.Sleep(100);
        }
    }
}
