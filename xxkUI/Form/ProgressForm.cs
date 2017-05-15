/***********************************************************/
//---模    块：ProgressForm类
//---功能描述：公用的时度控制和显示类
//---编码时间：2008-05-18
//---编码人员：高锡章
//---单    位：LREIS
/***********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using xxkUI.Tool;

namespace xxkUI.Form
{
    /// <summary>
    /// 进度控制和显示对话框
    /// </summary>
    public partial class ProgressForm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 后台线膨程工作类
        /// </summary>
        private MyBackgroundWorker backgroundWorker = null;
        private DateTime sTime;
        /// <summary>
        /// 多线程工作类
        /// </summary>
        public MyBackgroundWorker progressWorker
        {
            get
            {
                return backgroundWorker;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
            backgroundWorker = new MyBackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            //backgroudWorker.DoWork += new DoWorkEventHandler(backgroundCalculator_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.Disposed += new EventHandler(backgroundWorker_Disposed);            
            sTime = DateTime.Now;
            updateStatus("开始时间:" + sTime.ToString());
            updateStatus("*****************************************************************************");
            updateStatus("开始执行");
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker_Disposed(object sender, EventArgs e)
        { 
            this.Close();//pyl 此行被提前了两行,原来行执行不到.
            throw new Exception("当前操作未完成");         
        }

        /// <summary>
        /// 进度发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                progressBar.Value = e.ProgressPercentage;                
                updateStatus(backgroundWorker.StatusText);
                if (backgroundWorker.logType != LogType.Common)
                {
                    backgroundWorker.logType = LogType.Common;
                }
            }
            else
            {
                backgroundWorker.Dispose();
            }
        }
        //操作完成
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                updateStatus("当前操作已终止.");                
            }
            else if (e.Error != null)
            {
                updateStatus("执行过程出现错误，请检查参数或数据是否正确!");
            }
            else
            {
                updateStatus("当前操作已执行完成!");
                if (check_AutoClose.Checked)
                {
                    this.Close();
                }
            }
            updateStatus("*****************************************************************************");
            this.progressBar.Style = ProgressBarStyle.Continuous;
            this.progressBar.Value = 100;
            DateTime eTime = DateTime.Now;
            updateStatus("结束时间:" + eTime.ToString());
            TimeSpan sSpan = eTime.Subtract(sTime);
            updateStatus("使用时间:" + sSpan.Days.ToString() + "天" + sSpan.Hours.ToString() + "小时" +
                sSpan.Minutes.ToString() + "分" + sSpan.Seconds.ToString() + "秒");
            this.Text = "当前操作已执行完成(" + this.Text + ")";
            this.Cancel.Enabled = true;
            this.Cancel.Text = "关闭";
            this.Export.Enabled = true;
        }
        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="strText">状态信息</param>
        public void updateStatus(string strText)
        {
            try
            {
                if (!backgroundWorker.IsExitWithException)
                {
                    if (richTextBox != null && strText!="")
                    {
                        int p1 = richTextBox.TextLength;  //取出未添加时的字符串长度。                       
                       
                        richTextBox.AppendText("\n" + strText);
                        int p2 = strText.Length;  //取出要添加的文本的长度    
                        richTextBox.Select(p1, p2+1);        //选中要添加的文本 
                        if (backgroundWorker.logType == LogType.Error)
                        {
                            richTextBox.SelectionColor = Color.Red;
                        }
                        else if (backgroundWorker.logType == LogType.Warning)
                        {
                            richTextBox.SelectionColor = Color.Orange;
                        }
                        else if (backgroundWorker.logType == LogType.Right)
                        {
                            richTextBox.SelectionColor = Color.Green;
                        }
                        else
                        {
                            richTextBox.SelectionColor = Color.Black;
                        }
                        richTextBox.ScrollToCaret();
                    }
                }                
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }
        /// <summary>
        /// 开始运行多线程
        /// </summary>
        public void beginWorking()
        {
            this.Export.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                updateStatus("正在取消当前操作，请稍候...");
                backgroundWorker.CancelAsync();
                this.Cancel.Text = "关闭";
                this.Cancel.Enabled = false;
            }
            else if (this.Cancel.Text == "关闭")
            {
                //this.Dispose();
                this.Close();
            }
        }
       
        //导出执行日志
        private void Export_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Show(this.Export, this.Export.Width-10, this.Export.Height-10);
        }
       

        private void SaveTxt(string fileName)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

                foreach (string strText in richTextBox.Lines)
                {
                    sw.WriteLine(strText);
                }
                sw.Close();
                fs.Close();

                if (XtraMessageBox.Show("日志导出完成，是否现在打开日志文件并查看？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("写文本文件出错," + ex.Message);
            } 
        }
        //对话框关闭时的操作
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {                
                backgroundWorker.CancelAsync();                
            }
            backgroundWorker.IsExitWithException = true;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "menu_txt")
            {
                this.contextMenuStrip1.Close();
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "文本文件(*.txt)|*.txt";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        string fileName = fileDialog.FileName;                        
                        SaveTxt(fileName);                       
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        XtraMessageBox.Show("写日志文件出错," + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Cursor = Cursors.Default;
                }
            }
            else if (e.ClickedItem.Name == "menu_word")
            {
                this.contextMenuStrip1.Close();
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Word文件(*.doc)|*.doc";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        string fileName = fileDialog.FileName;                        
                        //SaveDoc(fileName);                       
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        XtraMessageBox.Show("写日志文件出错," + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Cursor = Cursors.Default;
                }
            }
        }
    }
}