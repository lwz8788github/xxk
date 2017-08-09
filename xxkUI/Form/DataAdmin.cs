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

namespace xxkUI.Form
{
    public partial class DataAdmin : XtraForm
    {
        public DataAdmin()
        {
            InitializeComponent();
        }
        private void DataAdmin_Load(object sender, EventArgs e)
        {
            this.btnAgreeInsertToDb.Click += BtnAgreeInsertToDb_Click;
            this.btnReject.Click += BtnReject_Click;

            InitDataSource();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定删除该上报数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                LineObsBean lob = new LineObsBean();
                lob.obslinecode = LineBll.Instance.GetIdByName(dr["obslinename"].ToString());
                lob.obvdate = DateTime.Parse(dr["obsdate"].ToString());

                DataCommitBll.Instance.DeleteWhere(new { OBSLINECODE = lob.obslinecode, OBVDATE = lob.obvdate });

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("删除数据失败:" + ex.Message, "错误");
            }
            InitDataSource();
        }

        private void BtnAgreeInsertToDb_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("确定接收该数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                LineObsBean lob = new LineObsBean();
                lob.obslinecode = LineBll.Instance.GetIdByName(dr["obslinename"].ToString());
                lob.obvdate = DateTime.Parse(dr["obsdate"].ToString());
                lob.obvvalue = double.Parse(dr["obsvalue"].ToString());
                lob.note = dr["note"].ToString();
                LineObsBll.Instance.Add(lob);

                DataCommitBll.Instance.DeleteWhere(new { OBSLINECODE = lob.obslinecode, OBVDATE = lob.obvdate });

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("审核数据失败:"+ex.Message, "错误");
            }
            InitDataSource();
        }

        private void InitDataSource()
        {
            try
            {
                List<DataCommitBean> dataCmtlist = DataCommitBll.Instance.GetAll().ToList();
                DataTable dt = new DataTable("datacommit");
                dt.Columns.Add("username", System.Type.GetType("System.String"));
                dt.Columns.Add("userunit", System.Type.GetType("System.String"));
                dt.Columns.Add("sitename", System.Type.GetType("System.String"));
                dt.Columns.Add("obslinename", System.Type.GetType("System.String"));
                dt.Columns.Add("obsvalue", System.Type.GetType("System.Double"));
                dt.Columns.Add("obsdate", System.Type.GetType("System.DateTime"));
                dt.Columns.Add("cmitdate", System.Type.GetType("System.DateTime"));
                dt.Columns.Add("note", System.Type.GetType("System.String"));

                for (int i = 0; i < dataCmtlist.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["username"] = dataCmtlist[i].username;
                    dr["userunit"] = UnitInfoBll.Instance.GetUnitNameBy(dataCmtlist[i].unitcode);
                    dr["sitename"] = SiteBll.Instance.GetSitenameByID(dataCmtlist[i].sitecode);
                    dr["obslinename"] = LineBll.Instance.GetNameByID("OBSLINENAME", "OBSLINECODE", dataCmtlist[i].obslinecode);
                    dr["obsvalue"] = dataCmtlist[i].obvvalue;
                    dr["obsdate"] = dataCmtlist[i].obvdate;
                    dr["cmitdate"] = dataCmtlist[i].cmitdate;
                    dr["note"] = dataCmtlist[i].note;

                    dt.Rows.Add(dr);
                }
                this.gridControl.DataSource = dt;

                //4.解决IConvertible问题
                //rICbExamine.ParseEditValue += new ConvertEditValueEventHandler(rICbExamine_ParseEditValue);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("加载用户数据失败:" + ex.Message, "错误");
            }
        }

        //void rICbExamine_ParseEditValue(object sender, ConvertEditValueEventArgs e)
        //{
        //    e.Value = e.Value.ToString(); e.Handled = true;
        //}
    }
}
