using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common.Data;
using xxkUI.Bll;

namespace xxkUI.Form
{
    public partial class ObsData :XtraForm
    {
        private string linecode = string.Empty;
        private DataTable datasource = null;

        public ObsData()
        {
            InitializeComponent();
            this.gridView.OptionsBehavior.Editable = false;//不可编辑
        }

        public void LoadDataSource(List<LineObsBean> obslist)
        {
            this.gridControl.DataSource = obslist;
          
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="_dt"></param>
        public void LoadDataSource(DataTable _dt)
        {
            datasource = _dt;
            this.gridControl.DataSource = datasource;
            this.gridControl.Refresh();
        }


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertData_Click(object sender, EventArgs e)
        {
            this.gridView.OptionsBehavior.Editable = true;//可编辑
            int focusedRow = this.gridView.FocusedRowHandle;

            DataRow dr = datasource.NewRow();
            dr["观测时间"] = DateTime.Now;
            dr["观测值"] = double.NaN;
            dr["备注"] = "";
            datasource.Rows.InsertAt(dr, focusedRow);
            this.gridControl.DataSource = datasource;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            int focusedRow = this.gridView.FocusedRowHandle;
            datasource.Rows[focusedRow].Delete();//.RemoveAt(rowindex);
            this.gridControl.DataSource = datasource;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditData_Click(object sender, EventArgs e)
        {
            this.gridView.OptionsBehavior.Editable = true;//可编辑
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LineObsBll lob = new LineObsBll();

                DataTable dt = datasource.GetChanges();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //新增
                        if (dr.RowState == DataRowState.Added)
                        {

                            LineObsBean lobean = new LineObsBean();
                            
                        }
                        //修改
                        else if (dr.RowState == DataRowState.Modified)
                        {
                        }
                    }
                }

                //删除
                DataView dv = new DataView(this.datasource, string.Empty, string.Empty, DataViewRowState.Deleted);
                if (dv != null)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    { }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("错误", ex.Message);
            }
        }



    }


}