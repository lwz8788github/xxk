using DevExpress.XtraEditors;
using Steema.TeeChart.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class eqkList : DevExpress.XtraEditors.XtraForm
    {
        public eqkList(List<EqkBean> eqkShowList)
        {
            InitializeComponent();
            LoadEqkData(eqkShowList);
        }
        private void LoadEqkData(List<EqkBean> eqkShowList)
        {
            DataTable eqkShowData = ToDataTable<EqkBean>(eqkShowList);
            eqkShowData.Columns.Add("check", Type.GetType("System.Boolean"));
            //this.gridView.Columns.Clear();
            this.gridControl1.DataSource = null;
            this.gridControl1.DataSource = eqkShowData;
            this.gridControl1.Refresh();
            //this.gridView.RefreshData();
        }
        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        private static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 执行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        //int sc = this.tChart.Series.Count;
                        //if (sc > 1)
                        //{
                        //    for (int i = 1; i < sc; i++)
                        //        this.tChart.Series.RemoveAt(i);
                        //}

                        //DataRowView drv = (DataRowView)gridView1.GetRow(hInfo.RowHandle);
                        //DateTime obsdate = new DateTime();
                        //DateTime.TryParse(drv["观测时间"].ToString(), out obsdate);
                        //double obsv = double.NaN;
                        //double.TryParse(drv["观测值"].ToString(), out obsv);

                        //Line ln = tChart.Series[0] as Line;

                        //for (int i = 0; i < ln.Count; i++)
                        //{
                        //    if (DateTime.FromOADate(ln[i].X) == obsdate && obsv == ln[i].Y)
                        //    {
                        //        Points pts = new Points(this.tChart.Chart);
                        //        pts.Add(DateTime.FromOADate(ln[i].X), ln[i].Y);

                        //        pts.Pointer.Style = PointerStyles.Circle;

                        //        pts.Legend.Visible = false;
                        //        pts.Color = Color.DeepSkyBlue;
                        //    }
                        //}
                        //this.tChart.Refresh();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }
    }
}
