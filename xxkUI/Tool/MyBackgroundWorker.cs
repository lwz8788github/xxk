/***********************************************************/
//---模    块：MyBackgroundWorker类
//---功能描述：多线程管理类
//---编码时间：2008-05-18
//---编码人员：高锡章
//---单    位：LREIS
/***********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;

namespace xxkUI.Tool
{
    public partial class MyBackgroundWorker : BackgroundWorker
    {
        private string strStatusText = null;
        private bool exitWithException = false;
        public LogType logType = LogType.Common;
        /// <summary>
        /// 状态信息
        /// </summary>
        public string StatusText
        {
            set
            {
                strStatusText = value;
            }
            get
            {
                return strStatusText;
            }
        }
        /// <summary>
        /// 是否异常退出
        /// </summary>
        public bool IsExitWithException
        {
            set
            {
                exitWithException = value;
            }
            get
            {
                return exitWithException;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyBackgroundWorker()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ck add,2012-6-1.
        /// 当调用ReportProgress太频繁时，会来不及调用ProgressForm的updateStatus，而显示错误的提示。
        /// 所以写此方法，以方便改变状态文本。
        /// </summary>
        /// <param name="sStatusText"></param>
        public void SetStatusText(string sStatusText)
        {
            this.StatusText = sStatusText;
            this.ReportProgress(50);
            System.Threading.Thread.Sleep(50);//休眠一会，让backgroundWorker_ProgressChanged有时间执行
        }

        /// <summary>
        /// ck add,2012-6-1.
        /// 当只想改变进度，而不想改变状态文本时，可以调用这个函数。
        /// </summary>
        public void ChangeProgressNoStatusText()
        {
            this.StatusText = "";
            this.ReportProgress(50);
            System.Threading.Thread.Sleep(50);
        }
    }
}
