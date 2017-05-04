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
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Drawing;

namespace xxkUI.Form
{
    public partial class ObsData : XtraForm
    {
        private string linecode = string.Empty;
        private DataTable datasource = null;
        private TChart tChart;

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
        public void LoadDataSource(DataTable _dt, TChart _tChart)
        {
            tChart = _tChart;
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
            datasource.Rows.InsertAt(dr, focusedRow + 1);
            this.gridControl.DataSource = datasource;


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            try
            {
                int focusedRow = this.gridView.FocusedRowHandle;
                gridView.DeleteRow(focusedRow);
                gridView.UpdateCurrentRow();
                this.tChart.Series[0].Delete(focusedRow);
                this.tChart.Refresh();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("错误", "删除失败:" + ex.Message);
            }
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
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int focusedRow = e.RowHandle;
            DataRowView drv = (DataRowView)this.gridView.GetRow(focusedRow);
            DateTime obsdate = DateTime.Parse(drv["观测时间"].ToString());
            double obdv = double.Parse(drv["观测值"].ToString());
          
            this.tChart.Series[0].XValues[focusedRow] = obsdate.ToOADate();
            this.tChart.Series[0].YValues[focusedRow] = obdv;
            this.tChart.Refresh();

            gridView.UpdateCurrentRow();
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
                XtraMessageBox.Show("错误", "保存失败:"+ex.Message);
            }
        }

        private void gridView_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gridView.CalcHitInfo(new Point(e.X, e.Y));

            /*
             * 行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    Line ln = this.tChart.Series[0] as Line;
                    //int screenX = ln.CalcXPosValue(ln[hInfo.RowHandle].X);
                    //int screenY = ln.CalcYPosValue(ln[hInfo.RowHandle].Y);
                    //Rectangle r = new Rectangle(screenX - 4, screenY - 4, 7, 7);//标识圆的大小
                    //Graphics3D grafics = tChart.Graphics3D;
                    //grafics.Ellipse(r);
                    if (this.tChart.Series.Count > 1)
                    {
                        this.tChart.Series.RemoveAt(1);
                    }
                   

                    Points pts = new Points(this.tChart.Chart);
                    pts.Add(DateTime.FromOADate(ln[hInfo.RowHandle].X), ln[hInfo.RowHandle].Y);
                    pts.Marks.ShapeStyle = TextShapeStyle.RoundRectangle;
                    pts.Legend.Visible = false;
                    pts.Color = Color.Red;
                }
            }
        }

      
    }
}