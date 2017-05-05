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
    public enum ActionType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 0,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Modify = 2,
        /// <summary>
        /// 无动做
        /// </summary>
        NoAction = 3
    }

    public partial class ObsData : XtraForm
    {
       
        private string linecode = string.Empty;
        private DataTable datasource = null;
        private TChart tChart;
        private ActionType actiontype = ActionType.NoAction;
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
            actiontype = ActionType.Add;
            this.gridView.OptionsBehavior.Editable = true;//可编辑
            int focusedRow = this.gridView.FocusedRowHandle;
            this.gridView.AddNewRow();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            actiontype = ActionType.Delete;
            int focusedRow = this.gridView.FocusedRowHandle;

            try
            {
                DataRowView drv = (DataRowView)this.gridView.GetRow(focusedRow);
                DateTime obsdate = new DateTime();
                DateTime.TryParse(drv["观测时间"].ToString(), out obsdate);
                double obsv = double.NaN;
                double.TryParse(drv["观测值"].ToString(), out obsv);

                Line ln = this.tChart.Series[0] as Line;
                for (int i = 0; i < this.tChart.Series[0].Count; i++)
                {
                    if (DateTime.FromOADate(ln[i].X) == obsdate && obsv == ln[i].Y)
                        ln.Delete(i);
                }
                this.tChart.Refresh();

            }
            catch (Exception ex)
            {
                actiontype = ActionType.NoAction;
                //XtraMessageBox.Show("错误", "删除失败:" + ex.Message);
            }


            gridView.DeleteRow(focusedRow);
            gridView.UpdateCurrentRow();
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditData_Click(object sender, EventArgs e)
        {
            this.gridView.OptionsBehavior.Editable = true;//可编辑
            actiontype = ActionType.Modify;
        }
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int focusedRow = e.RowHandle;
                DataRowView drv = (DataRowView)this.gridView.GetRow(focusedRow);


                switch (actiontype)
                {
                    case ActionType.Modify:
                        {
                            DateTime obsdate = DateTime.Parse(drv["观测时间"].ToString());
                            double obdv = double.Parse(drv["观测值"].ToString());
                            this.tChart.Series[0].XValues[focusedRow] = obsdate.ToOADate();
                            this.tChart.Series[0].YValues[focusedRow] = obdv;
                            this.tChart.Refresh();
                            gridView.UpdateCurrentRow();
                        }
                        break;
                    case ActionType.Add:
                        {

                            if (drv["观测时间"].ToString() == "" || drv["观测值"].ToString() == "")
                                return;

                            DateTime obsdate = new DateTime();
                            DateTime.TryParse(drv["观测时间"].ToString(), out obsdate);

                            double obdv = double.NaN;
                            double.TryParse(drv["观测值"].ToString(), out obdv);

                            this.tChart.Series[0].Add(obsdate, obdv);

                            this.tChart.Refresh();
                            gridView.UpdateCurrentRow();
                        }
                        break;




                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("错误", ex.Message);
            }
           
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
                        /*
                         * 新增
                         */
                        if (dr.RowState == DataRowState.Added)
                        {
                            LineObsBean lobean = new LineObsBean();

                        }
                      
                        /*
                         * 修改
                         */
                        else if (dr.RowState == DataRowState.Modified)
                        {

                        }
                    }
                }
                /*
                 * 删除
                 */
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
                    try
                    {
                        Line ln = this.tChart.Series[0] as Line;

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
                    catch (Exception ex)
                    { }
                }
            }
        }

   
        

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (actiontype == ActionType.Modify)
                e.Cancel = false;
            else if (actiontype == ActionType.Add)
            {
              
                DataRowView drv = (DataRowView)this.gridView.GetRow(this.gridView.FocusedRowHandle);

                if (drv["观测值"].ToString() == "" || drv["观测时间"].ToString() == "")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
                  
            }
        }
    }
}