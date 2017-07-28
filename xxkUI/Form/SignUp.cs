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
        private bool IsPsdChanged = false;
        List<UnitInfoBean> UnitDt = null;

      private  UserInfoBean UserInfoBean = null;
        public SignUp()
        {
            InitializeComponent();
            UnitDt = UnitInfoBll.Instance.GetAll().ToList();
            LoadComboBoxEdit(UnitDt);
        }
        public SignUp(UserInfoBean UIf)
        {
            InitializeComponent();
            UnitDt = UnitInfoBll.Instance.GetAll().ToList();
            LoadComboBoxEdit(UnitDt);
            this.btnSignUp.Text = "提交";
            this.Text = "用户信息编辑";
            this.txtUsername.Text = UIf.UserName;
            this.txtPsd1.Text = UIf.Password;
            this.txtPsd2.Text = UIf.Password;
            AthParse(UIf.UserAthrty);
            UnitParse(UIf.UserUnit);

            UserInfoBean = UIf;
        }
        private void AthParse(string athStr)
        {
            string[] items = athStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                for (int i = 0; i < cbeUnit.Properties.Items.Count; i++)
                {
                    if (item == cbeAuth.Properties.Items[i].Value.ToString())
                    {
                        cbeAuth.Properties.Items[i].CheckState = CheckState.Checked;
                    }
                }
            }
        }
        private void UnitParse(string unitStr)
        {
            string unitName = UnitDt.Find(n => n.UnitCode == unitStr).UnitName;
            for (int i = 0; i < cbeUnit.Properties.Items.Count; i++)
            {
                if (unitName == cbeUnit.Properties.Items[i].ToString())
                {
                    //cbeUnit.Properties = CheckState.Checked;
                    cbeUnit.Text = unitName;
                    return;
                }
            }

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
             
                if (this.btnSignUp.Text == "注册")
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
                else
                {
                    UserInfoBean usermodel = new UserInfoBean();
                    usermodel.UserName = txtUsername.Text;
                    if (txtPsd1.Text!=UserInfoBean.Password)
                        usermodel.Password = UserInfoBll.Instance.encryptPWD(txtPsd1.Text);
                    else
                        usermodel.Password = txtPsd1.Text;
                    usermodel.UserAthrty = userAuthStr;
                    usermodel.UserUnit = UnitDt.Find(n => n.UnitName == cbeUnit.Text).UnitCode;
                    usermodel.Status = UserDicCls.UserDictionary[UserStatus.Examining];
                    UserInfoBll.Instance.UpdateWhatWhere(new { password = usermodel.Password, userunit = usermodel.UserUnit, userathrty = usermodel.UserAthrty, status = usermodel.Status }, new {username = usermodel.UserName });
                    XtraMessageBox.Show("用户信息修改成功！", "提示");
                    this.Close();
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                if (this.btnSignUp.Text == "注册")
                {
                    XtraMessageBox.Show("用户注册失败，请联系管理员", "错误");
                }
                else
                {
                    XtraMessageBox.Show("用户信息修改失败，请联系管理员", "错误");
                }
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        private void btnSignCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPsd1_EditValueChanged(object sender, EventArgs e)
        {
            IsPsdChanged = true;

            txtPsd2.Text = "";
        }


    }
}