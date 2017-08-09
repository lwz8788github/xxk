using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.BLL;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class DataCommit : XtraForm
    {
        List<SiteBean> SiteDt = null;
        List<LineBean> LineDt = null;

        public DataCommit()
        {
            InitializeComponent();
            SiteDt = SiteBll.Instance.GetWhere(new { unitcode = CurrentUSerInfo.UIB.UserUnit }).ToList();
            LoadComboBoxEdit(SiteDt);
        }
        private void LoadComboBoxEdit(List<SiteBean> SiteDt)
        {
            this.cbeSite.Properties.NullText = "请选择...";
            cbeSite.Properties.Items.Clear();
            for (int i = 0; i < SiteDt.Count; i++)
            {
                cbeSite.Properties.Items.Add(SiteDt[i].SiteName);
            }
        }

        private void LoadComboBoxEditLine(List<LineBean> LineDt)
        {
            this.cbeLine.Properties.NullText = "请选择...";
            cbeLine.Properties.Items.Clear();
            for (int i = 0; i < LineDt.Count; i++)
            {
                cbeLine.Properties.Items.Add(LineDt[i].OBSLINENAME);
            }
        }
        private void cbeSite_SelectedValueChanged(object sender, EventArgs e)
        {
            string SiteCode = SiteBll.Instance.GetSiteCodeByName(cbeSite.Text);
            LineDt = LineBll.Instance.GetBySitecode(SiteCode).ToList();
            LoadComboBoxEditLine(LineDt);
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            if (cbeSite.Text == "")
            {
                XtraMessageBox.Show("场地不能为空!", "提示");
                cbeSite.Focus();
                return;
            }
            if (cbeLine.Text == "")
            {
                XtraMessageBox.Show("测线不能为空!", "提示");
                cbeLine.Focus();
                return;
            }
            if (dateEdit.Text == "")
            {
                XtraMessageBox.Show("观测时间不能为空!", "提示");
                dateEdit.Focus();
                return;
            }
            if (txtObsvalue.Text == "")
            {
                XtraMessageBox.Show("观测值不能为空!", "提示");
                txtObsvalue.Focus();
                return;
            }
            try
            {
                DataCommitBean dataCmit = new DataCommitBean();
                dataCmit.username = CurrentUSerInfo.UIB.UserName;
                dataCmit.unitcode = CurrentUSerInfo.UIB.UserUnit;
                dataCmit.sitecode = SiteBll.Instance.GetSiteCodeByName(cbeSite.Text);
                dataCmit.obslinecode = LineBll.Instance.GetNameByID("OBSLINECODE", "OBSLINENAME", cbeLine.Text);
                dataCmit.obvdate = Convert.ToDateTime(dateEdit.Text);
                dataCmit.cmitdate = DateTime.Now;
                dataCmit.obvvalue = double.Parse(txtObsvalue.Text);
                DataCommitBll.Instance.Add(dataCmit);
                XtraMessageBox.Show("数据提交成功！", "提示");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("观测值提交失败，请检查数据后重新提交！", "错误");
                //this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
