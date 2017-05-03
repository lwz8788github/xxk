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
    public partial class EqkShow : DevExpress.XtraEditors.XtraForm
    {
        private string Sitecode = "";
        public EqkShow()
        {
            InitializeComponent();
        }
        public EqkShow(string _sitecode)
        {
            InitializeComponent();
            this.Sitecode = _sitecode;
        }
        public void LoadEqkData(DataTable dt)
        {
            this.gridControl1.DataSource = null;
            this.gridControl1.DataSource = dt;
            this.gridControl1.Refresh();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            EqkShow eqkfrm = new EqkShow();
            float eqkMlMin = float.Parse(this.textEdit1.Text);
            float eqkMlMax = float.Parse(this.textEdit2.Text);
            if (eqkMlMin > eqkMlMax)
            {
                XtraMessageBox.Show("提示","最大震级应大于最小震级，重新输入！");
                textEdit1.Text = "";
                textEdit2.Text = "";
                return;
            }
            float eqkDepthMin = float.Parse(this.textEdit4.Text);
            float eqkDepthMax = float.Parse(this.textEdit3.Text);
            if (eqkDepthMin > eqkDepthMax)
            {
                XtraMessageBox.Show("提示", "最大深度应大于最小深度，重新输入！");
                textEdit4.Text = "";
                textEdit3.Text = "";
                return;
            }
            //DateTime timeSt = DateTime.ParseExact(this.textEdit6.Text, "yyyyMMdd hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            //DateTime timeEd = DateTime.ParseExact(this.textEdit5.Text, "yyyyMMdd hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            string timeSt = this.textEdit6.Text;
            string timeEd = this.textEdit5.Text;
            float radial = float.Parse(this.textEdit8.Text);

            string lon = xxkUI.BLL.EqkBll.Instance.GetNameByID("LONGTITUDE", "SITECODE", Sitecode);
            string lat = xxkUI.BLL.EqkBll.Instance.GetNameByID("LATITUDE", "SITECODE", Sitecode);
            List<EqkBean> datasource = xxkUI.BLL.EqkBll.Instance.GetList("select longtitude as 经度,latitude as 纬度,eakdate as 时间, magntd as 震级, depth as 深度, place as 地点  from t_eqkcatalog where MAGNTD >= " + eqkMlMin + "MAGNTD <=" + eqkMlMax + "DEPTH >=" + eqkDepthMin + "DEPTH <=" + eqkDepthMax).ToList();

            //GetEqkShowForm();
            //eqkfrm.Show();
        }

        private void GetEqkShowForm()
        {
            throw new NotImplementedException();
        }
    }
}