using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xxkUI.Dal;
using xxkUI.Bll;
using Common.Data.MySql;
using System.Configuration;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public string CurrentDb { get; set; }
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPsd.Text == "")
            {
                XtraMessageBox.Show("用户名和密码不能为空!", "提示");
                if (txtUsername.Text == "")
                    txtUsername.Focus();

                if (txtUsername.Text != "" && txtPsd.Text == "")
                    txtPsd.Focus();

                return;
            }
            try
            {
                MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;

                var userName = txtUsername.Text;
                var password = txtPsd.Text;
              
                UserInfoBean u = UserInfoBll.Instance.GetUserBy(userName);
                if (u != null)
                {
                    if (UserInfoBll.Instance.GetLogin(u))
                    {
                        CurrentUSerInfo.UIB = u;
                                         }
                    else
                    {
                        XtraMessageBox.Show("用户名或密码错误!", "提示");
                        return;
                    }
                }
                else
                {
                    XtraMessageBox.Show("用户名不存在!", "提示");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message,"错误");
            }
            this.DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}