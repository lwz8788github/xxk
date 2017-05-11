using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xxkUI.Tool
{
    
    public class LogManager
    {
     
        private static string logLevel = string.Empty;
        /// <summary>
        /// 自定义打印级别  Warning,DeBug,Error,Run
        /// </summary>
        public static string LogLevel
        {
            get
            {
                if (string.IsNullOrEmpty(logLevel))
                {
                    logLevel = LogFile.debug.ToString();
                }

                return logLevel;
            }
            set { logLevel = value; }
        }

        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = AppDomain.CurrentDomain.BaseDirectory+"\\logs\\";//这个没有尝试过，应该没有什么错误
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFielPrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFielPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(logFielPrefix))
                {
                    logFielPrefix = "Lev_";
                }
                return logFielPrefix;
            }
            set { logFielPrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logFile"></param>
        /// <param name="msg"></param>
        public static void WriteLog(LogFile logFile, string username,string msg)
        {
            WriteLog(logFile.ToString(),username, msg);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string logFile, string username,string msg)
        {
            try
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(
                        LogPath + LogFielPrefix + logFile + " " +
                        DateTime.Now.ToString("yyyyMMdd") + ".Log"
                    );
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:  ") + username + "  " + msg);
                sw.Close();
            }
            catch (Exception ee)
            {
                //System.Windows.Forms.MessageBox.Show("在LogManager类中操作WriteLog方法时异常：" + ee.Message);   
            }
        }
        /// <summary>
        /// 清除所有日志
        /// </summary>
        public static void ClearHistoryLog()
        {
            File.Delete(LogPath);
        }
        /// <summary>
        /// 删除指定日期以前的所有日志
        /// </summary>
        /// <param name="dt"></param>
        public static void DeleteLogBeforeDate(DateTime dt)
        {
            foreach (string fileName in Directory.GetFiles(LogPath))
            {
                FileInfo f = new FileInfo(fileName);
                if (f.LastWriteTime.CompareTo(dt) < 0)
                {
                    System.IO.File.Delete(fileName);
                }

            }
        }
        /// <summary>
        /// 删除某个级别的的所有日志
        /// </summary>
        /// <param name="level"></param>
        public static void DeleteOneLevelLogs(string level)
        {
            string[] filenames = Directory.GetFiles(LogPath);
            foreach (string name in filenames)
            {
                if (name.IndexOf(level.ToLower()) > 0)
                {
                    System.IO.File.Delete(name);
                }
            }
        }
    }
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        warning,
        debug,
        error,
        info
    }


       
}
