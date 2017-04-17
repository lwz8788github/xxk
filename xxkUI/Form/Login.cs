using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace xxkUI.Form
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public string Username{get;set;}
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Username = "superadmin";
            if (txtUsername.Text == "" || txtPsd.Text == "")
            {
                XtraMessageBox.Show("用户名和密码不能为空!");
                if (txtUsername.Text == "")
                    txtUsername.Focus();


                return;
            }

            {
                this.Username = "superadmin";
            }


            this.DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}