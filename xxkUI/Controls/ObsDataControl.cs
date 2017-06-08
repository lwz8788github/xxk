using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using xxkUI.Tool;
using xxkUI.Bll;

namespace xxkUI.Controls
{
    public partial class ObsDataControl : DevExpress.XtraEditors.XtraUserControl
    {
        private string linecode = string.Empty;
        private DataTable datasource = null;
        private TChart tChart;
        private ActionType actiontype = ActionType.NoAction;

        public ObsDataControl()
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
            if (btnEditData.Text == "取消编辑")
            {
                this.gridView.OptionsBehavior.Editable = false;
                btnEditData.Text = "编辑数据";
                actiontype = ActionType.NoAction;
            }
            else if (btnEditData.Text == "编辑数据")
            {
                this.gridView.OptionsBehavior.Editable = true;//可编辑
                btnEditData.Text = "取消编辑";
                actiontype = ActionType.Modify;
            }
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

                            if (!IsExisted(this.tChart.Series[0], obsdate, obdv))
                                this.tChart.Series[0].Add(obsdate, obdv);
                            else
                            {
                                XtraMessageBox.Show("已存在相同数据", "提示");
                                //drv["观测时间"] = new DateTime();
                                //drv["观测值"] = double.NaN;

                                this.gridView.SetRowCellValue(focusedRow, gridView.Columns["观测时间"], null);
                                this.gridView.SetRowCellValue(focusedRow, gridView.Columns["观测值"], null);
                            }

                            this.tChart.Refresh();
                            gridView.UpdateCurrentRow();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }


        /// <summary>
        /// 是否存在相同的记录
        /// </summary>
        /// <param name="obsdate"></param>
        /// <param name="obsv"></param>
        /// <returns></returns>
        private bool IsExisted(Series s, DateTime obsdate, double obsv)
        {
            bool isExist = false;

            for (int i = 0; i < s.Count; i++)
                if (DateTime.FromOADate(s[i].X) == obsdate && obsv == s[i].Y)
                    isExist = true;

            return isExist;
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
                XtraMessageBox.Show("保存失败:" + ex.Message, "错误");
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
                        /*
                         * 保留第一个Series，其他删除
                         */
                        int sc = this.tChart.Series.Count;
                        if (sc > 1)
                        {
                            for (int i = 1; i < sc; i++)
                                this.tChart.Series.RemoveAt(i);
                        }

                        DataRowView drv = (DataRowView)this.gridView.GetRow(hInfo.RowHandle);
                        DateTime obsdate = new DateTime();
                        DateTime.TryParse(drv["观测时间"].ToString(), out obsdate);
                        double obsv = double.NaN;
                        double.TryParse(drv["观测值"].ToString(), out obsv);

                        Line ln = tChart.Series[0] as Line;

                        for (int i = 0; i < ln.Count; i++)
                        {
                            if (DateTime.FromOADate(ln[i].X) == obsdate && obsv == ln[i].Y)
                            {
                                Points pts = new Points(this.tChart.Chart);
                                pts.Add(DateTime.FromOADate(ln[i].X), ln[i].Y);

                                pts.Pointer.Style = PointerStyles.Circle;

                                pts.Legend.Visible = false;
                                pts.Color = Color.DeepSkyBlue;
                            }
                        }
                        this.tChart.Refresh();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }

        /// <summary>
        /// 获取Series类型
        /// </summary>
        /// <param name="s">Series</param>
        /// <returns>SereisType</returns>
        private SereisType GetSeriesType(Steema.TeeChart.Styles.Series s)
        {

            SereisType st = SereisType.UnknownSeris;
            try
            {
                Line ln = s as Line;
                if (ln != null)
                {
                    st = SereisType.LineSeries;
                }
                else
                {
                    Points pt = s as Points;
                    if (pt != null)
                    {
                        st = SereisType.PointsSeries;
                    }
                }
            }
            catch (Exception ex)
            {
                Points pt = s as Points;
                if (pt != null)
                {
                    st = SereisType.PointsSeries;
                }
            }

            return st;
        }

        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (actiontype == ActionType.Modify)
                e.Cancel = false;
            else if (actiontype == ActionType.Add)
            {
                /*
                 * 新增状态下只有新增行可以编辑
                 */
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
