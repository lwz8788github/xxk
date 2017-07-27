using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class RestoreDbFrm : XtraForm
    {
        public string DbName = "";
        public string UserName = "";
        public string Psd = "";
        public string Port = "";
        public string SavetoFilename = "";

        private string server = "";
        private string port = "";
        private string database = "";
        private string uid = "";
        private string pwd = "";

        public RestoreDbFrm()
        {
            InitializeComponent();

            try
            {
                string[] localdbfinfo = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString.Split(';');
                server = localdbfinfo[0].Split('=')[1];
                port = localdbfinfo[1].Split('=')[1];
                database = localdbfinfo[2].Split('=')[1];
                uid = localdbfinfo[3].Split('=')[1];
                pwd = localdbfinfo[4].Split('=')[1];

                this.textEditDbName.Text = database;
                this.textEditUserName.Text = uid;
                this.textEditPsd.Text = "";
                this.textEditPort.Text = port;
                this.textEditSaveto.Text = "";
            }
            catch (Exception)
            { }
        }

        private void btnSaveto_Click(object sender, EventArgs e)
        {
           OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.AddExtension = false;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = false;
      
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textEditSaveto.Text = saveFileDialog.FileName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.textEditDbName.Text == "")
            {
                XtraMessageBox.Show("数据库名不能为空", "提示");
                this.textEditDbName.Focus();
                return;
            }
            this.DbName = this.textEditDbName.Text;

            if (this.textEditUserName.Text == "")
            {
                XtraMessageBox.Show("用户名不能为空", "提示");
                this.textEditUserName.Focus();
                return;
            }
            this.UserName = this.textEditUserName.Text;

            if (this.textEditPsd.Text == "")
            {
                XtraMessageBox.Show("密码不能为空", "提示");
                this.textEditPsd.Focus();
                return;
            }
            this.Psd = this.textEditPsd.Text;

            if (this.textEditPort.Text == "")
            {
                XtraMessageBox.Show("端口号不能为空", "提示");
                this.textEditPort.Focus();
                return;
            }
            this.Port = this.textEditPort.Text;
            if (this.textEditSaveto.Text == "")
            {
                XtraMessageBox.Show("保存路径不能为空", "提示");
                this.textEditSaveto.Focus();
                return;
            }
            this.SavetoFilename = this.textEditSaveto.Text;

            if (this.UserName != this.uid || this.Psd != this.pwd || this.Port != this.port)
            {
                XtraMessageBox.Show("数据库信息不一致", "提示");
                return;
            }

            this.DialogResult = DialogResult.OK;
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.textEditDbName.Text = "";
            this.textEditUserName.Text = "";
            this.textEditPsd.Text = "";
            this.textEditPort.Text = "";
            this.textEditSaveto.Text = "";

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
