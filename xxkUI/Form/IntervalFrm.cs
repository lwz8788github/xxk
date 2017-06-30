using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class IntervalFrm : XtraForm
    {
        private bool flagMove = false;
        /// <summary>
        /// 间隔
        /// </summary>
        private int interval = 0;

        public int Interval
        {
            get
            {
                return interval;
            }

            set
            {
                interval = value;
            }
        }

        public IntervalFrm()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Interval = int.Parse(this.spinInterval.Text);
            this.Close();
        }

        private void Canel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void spinInterval_EditValueChanged(object sender, EventArgs e)
        {
            Interval = int.Parse(this.spinInterval.Text);
        }

        private void spinInterval_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null && (e.NewValue.ToString().StartsWith("-")) || (e.NewValue.ToString().StartsWith("0")))
            {
                e.Cancel = true;
            }
        }
        #region 使窗体可以移动的代码
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        #endregion
        private void IntervalFrm_MouseDown(object sender, MouseEventArgs e)
        {
            this.flagMove = true;
            //拖动窗体
            this.Cursor = System.Windows.Forms.Cursors.Hand;//改变鼠标样式
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void IntervalFrm_MouseUp(object sender, MouseEventArgs e)
        {
            this.flagMove = false;
        }

        private void IntervalFrm_MouseMove(object sender, MouseEventArgs e)
        {
            //if (this.flagMove)
            //{
            //    this.Location = new Point(e.X, e.Y);
            //}
        }
    }
}
