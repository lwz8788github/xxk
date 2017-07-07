using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class frm_SplashAhead : XtraForm
    {

        private RibbonForm MainFrm = null;
        //传递状态文本
        private string statusTxt = null;


        public frm_SplashAhead()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }


        //启动分析应用系统
        private void StartDAS()
        {
            try
            {
                statusTxt = "正在初始化系统...";
                backgroundWorker.ReportProgress(10);

                statusTxt = "正在加载系统框架...";
                MainFrm = new RibbonForm();
                backgroundWorker.ReportProgress(20);
                Thread.Sleep(500);
                MainFrm.InitForm();
                backgroundWorker.ReportProgress(25);
                Thread.Sleep(500);
             
                statusTxt = "正在加载信息树...";
                MainFrm.InitTree();
                backgroundWorker.ReportProgress(30);
                Thread.Sleep(800);
             
                statusTxt = "正在加载断层数据...";
                MainFrm.InitFaultCombobox();
                backgroundWorker.ReportProgress(40);
                Thread.Sleep(800);

                statusTxt = "正在加载信息库组件...";
                MainFrm.InitSiteinTab();
                backgroundWorker.ReportProgress(60);
                Thread.Sleep(800);

                statusTxt = "正在加载回收站组件...";
                MainFrm.InitRecycleTab();
                backgroundWorker.ReportProgress(70);
                Thread.Sleep(800);

                statusTxt = "正在加载布设图组件...";
                MainFrm.InitLayoutmapTab();
                backgroundWorker.ReportProgress(75);
                Thread.Sleep(800);
             
                statusTxt = "正在设置界面风格...";
                MainFrm.InitStyle();
                backgroundWorker.ReportProgress(80);
                Thread.Sleep(800);

                statusTxt = "系统初始化成功...";
                backgroundWorker.ReportProgress(100);
                Thread.Sleep(800);
                backgroundWorker_RunWorkerCompleted(null, null);
            }
            catch (Exception excep)
            {

                XtraMessageBox.Show("启动应用程序出错," + excep.Message + ",请与系统管理员联系!", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar_Load.Value = 0;
                updateStatus("准备登录");
            }
        }


        /// <summary>                                
        /// 更新装载状态
        /// </summary>
        /// <param name="strText">状态文本</param>
        public void updateStatus(string strText)
        {
            lbl_Status.Text = "当前状态:" + strText;
            this.Refresh();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            MainFrm.Show(this);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar_Load.Value = e.ProgressPercentage;
            updateStatus(statusTxt);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            StartDAS();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
