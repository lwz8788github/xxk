using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using xxkUI.Bll;
using xxkUI.BLL;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class UserManage : DevExpress.XtraEditors.XtraForm
    {
        public UserManage()
        {
            InitializeComponent();
        }

        private void UserManage_Load(object sender, EventArgs e)
        {
            rICbExamine.Items.Add(new CboItemEntity() { Text = "待审核", Value = 0 });
            rICbExamine.Items.Add(new CboItemEntity() { Text = "通过审核", Value = 1 });
            rICbExamine.Items.Add(new CboItemEntity() { Text = "未通过审核", Value = 2 });

            rICbExamine.SelectedIndexChanged += rICbExamine_SelectedIndexChanged;
            rIBtnEdit.ButtonClick += rIBtnEdit_ButtonClick;
            rIbtnDelte.ButtonClick += rIbtnDelte_ButtonClick;
            InitDataSource();
        }

        void rIbtnDelte_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                string UserName = dr[0].ToString();

                UserInfoBll.Instance.DeleteWhere(new { username = UserName });

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("删除数据失败！", "错误");
            }
            InitDataSource();
           // this.gridView.DeleteSelectedRows();
            
        }

        void rIBtnEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                UserInfoBean UIf = UserInfoBll.Instance.GetUserBy(dr[0].ToString());
                SignUp signupForm = new SignUp(UIf);

                if (signupForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    InitDataSource();
                }
               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("编辑数据失败！", "错误");
            }
            
 
        }

        void rICbExamine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CboItemEntity item = (CboItemEntity)(sender as ComboBoxEdit).SelectedItem;
                string text = item.Text.ToString();
                int value = (int)item.Value;
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                string UserName = dr[0].ToString();

                UserInfoBll.Instance.UpdateWhatWhere(new { status = value.ToString() }, new { username = UserName });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("修改用户状态失败：" + ex.Message, "错误");
            }
            InitDataSource();
        }

        private void InitDataSource()
        {
            try
            {
                List<UserInfoBean> userlist = UserInfoBll.Instance.GetAll().ToList();
                DataTable dt = new DataTable("userinfo");
                dt.Columns.Add("username", System.Type.GetType("System.String"));
                dt.Columns.Add("userunit", System.Type.GetType("System.String"));
                dt.Columns.Add("userathrty", System.Type.GetType("System.String"));
                dt.Columns.Add("status", System.Type.GetType("System.String"));

                for (int i = 0; i < userlist.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["username"] = userlist[i].UserName;
                    dr["userunit"] = UnitInfoBll.Instance.GetUnitNameBy(userlist[i].UserUnit);
                    string userAthstr = "";
                    if (userlist[i].UserAthrty != null)
                    {
                        string[] unitcodes = userlist[i].UserAthrty.Split(';');
                        if (unitcodes.Length > 0)
                            foreach (string uc in unitcodes)
                            {
                                string unitname = UnitInfoBll.Instance.GetUnitNameBy(uc);
                                if (unitname != string.Empty)
                                    userAthstr += unitname + ";";
                            }
                    }
                    dr["userathrty"] = userAthstr;

                    dr["status"] = new PublicHelper().GetUserStatusDiscription(userlist[i].Status);
                 
                    dt.Rows.Add(dr);
                }
                this.gridControl.DataSource = dt;

                //4.解决IConvertible问题
                rICbExamine.ParseEditValue += new ConvertEditValueEventHandler(rICbExamine_ParseEditValue);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("加载用户数据失败:" + ex.Message, "错误");
            }
        }

        void rICbExamine_ParseEditValue(object sender, ConvertEditValueEventArgs e)
        {
            e.Value = e.Value.ToString(); e.Handled = true;
        } 

    }
}