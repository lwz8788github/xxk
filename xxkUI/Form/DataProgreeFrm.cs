using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class DataProgreeFrm : XtraForm
    {
        public DataProessMethod dpm = DataProessMethod.NoProg;
        public double progreeValue = double.NaN;
        public DataProgreeFrm(int datacount)
        {
            InitializeComponent();
            lbDataCount.Text = "待处理目标数量：" + datacount.ToString() + "个";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.radioGroup.SelectedIndex == -1 || this.textValue.Text == "")
                return;

            try
            {
                progreeValue = double.Parse(this.textValue.Text);
            }
            catch
            {
                progreeValue = double.NaN;
                XtraMessageBox.Show("不是有效的数字", "错误");
                this.textValue.Focus();
                return;

            }
            switch (this.radioGroup.Properties.Items[this.radioGroup.SelectedIndex].ToString())
            {
                case "＋":
                    this.dpm = DataProessMethod.Plus;
                    break;
                case "－":
                    this.dpm = DataProessMethod.Minus;
                    break;
                case "×":
                    this.dpm = DataProessMethod.Multiply;
                    break;
                case "÷":
                    this.dpm = DataProessMethod.Divide;
                    break;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.radioGroup.Properties.Items[this.radioGroup.SelectedIndex].Description)
            {
                case "＋":
                    lbProgressExplain.Text = "操作说明：处理数据 = 原始数据 + 操作值";
                    break;
                case "－":
                    lbProgressExplain.Text = "操作说明：处理数据 = 原始数据 － 操作值";
                    break;
                case "×":
                    lbProgressExplain.Text = "操作说明：处理数据 = 原始数据 × 操作值";
                    break;
                case "÷":
                    lbProgressExplain.Text = "操作说明：处理数据 = 原始数据 ÷ 操作值";
                    break;

            }
        }
    }
}
