using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xxkUI.BLL;
using System.Web.Security;
using xxkUI.Tool;
using xxkUI.Bll;

namespace xxkUI.Form
{
    public partial class SignUp : DevExpress.XtraEditors.XtraForm
    {
        public string Username { get; set; }

        List<UnitInfoBean> UnitDt = null;
        public SignUp()
        {
            InitializeComponent();
            UnitDt = UnitInfoBll.Instance.GetAll().ToList();
            LoadComboBoxEdit(UnitDt);
        }

        private void LoadComboBoxEdit(List<UnitInfoBean> dt)
        {
            this.cbeAuth.Properties.NullText = "请选择...";
          
            for (int i = 0; i < dt.Count; i++) 
            {
                cbeAuth.Properties.Items.Add(dt[i].UnitCode, dt[i].UnitName, CheckState.Unchecked, true);
                cbeUnit.Properties.Items.Add(dt[i].UnitName);
            }
         
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPsd1.Text == "" || txtPsd2.Text == "")
            {
                XtraMessageBox.Show("用户名和密码不能为空!", "提示");
                if (txtUsername.Text == "")
                    txtUsername.Focus();

                if (txtUsername.Text != "" && txtPsd1.Text == "")
                    txtPsd1.Focus();

                if (txtUsername.Text != "" && txtPsd1.Text == "" && txtPsd2.Text == "")
                    txtPsd2.Focus();

                return;
            }
            if (txtPsd1.Text != txtPsd2.Text)
            {
                XtraMessageBox.Show("两次输入密码不一致!", "提示");
                return;
            }
            string userAuthStr = "";
            for (int i = 0; i < cbeUnit.Properties.Items.Count; i++)
            {
                if (cbeAuth.Properties.Items[i].CheckState == CheckState.Checked)
                {
                    userAuthStr += cbeAuth.Properties.Items[i].Value.ToString() + ";";
                }

            }
            if (userAuthStr == string.Empty)
            {
                XtraMessageBox.Show("申请权限不能为空！", "提示");
                cbeAuth.Focus();
                return;
            }

            if (cbeUnit.Text == "")
            {
                XtraMessageBox.Show("用户单位不能为空!", "提示");
                if (cbeUnit.Text == "")
                    cbeUnit.Focus();
                return;
            }
            try
            {
                UserInfoBean usermodel = new UserInfoBean();
                usermodel.UserName = txtUsername.Text;
                usermodel.Password = UserInfoBll.Instance.encryptPWD(txtPsd1.Text);
                usermodel.UserAthrty = userAuthStr;
                usermodel.UserUnit = UnitDt.Find(n => n.UnitName == cbeUnit.Text).UnitCode;
                usermodel.Status = UserDicCls.UserDictionary[UserStatus.Examining];

                if (UserInfoBll.Instance.GetUserBy(usermodel.UserName) != null)
                {
                    XtraMessageBox.Show("该用户已被使用", "提示");
                    return;
                }
                else
                {
                    UserInfoBll.Instance.Add(usermodel);
                    XtraMessageBox.Show("用户注册成功，请等待审核", "提示");
                    this.Close();
                }
                        
             
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("用户注册失败，请联系管理员", "错误");
            }
        }

        private void btnSignCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}