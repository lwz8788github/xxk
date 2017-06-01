using DevExpress.XtraEditors;
using GMap.NET;
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
            //eqkShowData.Columns.Add("check", Type.GetType("System.Boolean"));
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
                        Control xtraTabControl1 = Application.OpenForms["RibbonForm"].Controls.Find("xtraTabControl1", true)[0];
                        Control mapTabPage = Application.OpenForms["RibbonForm"].Controls.Find("mapTabPage", true)[0];
                        ((DevExpress.XtraTab.XtraTabControl)xtraTabControl1).SelectedTabPage = (DevExpress.XtraTab.XtraTabPage)mapTabPage;
                        GMap.NET.WindowsForms.GMapControl gmapcontrol = Application.OpenForms["RibbonForm"].Controls.Find("gMapCtrl", true)[0] as GMap.NET.WindowsForms.GMapControl;

                        DataRowView drv = (DataRowView)this.gridView1.GetRow(hInfo.RowHandle);

                        gmapcontrol.Position = new PointLatLng(double.Parse(drv["Latitude"].ToString()), double.Parse(drv["Longtitude"].ToString()));
                        //PointLatLng sitepoint = new PointLatLng(sb.Latitude, sb.Longtitude);
                        //gmapcontrol.Position = sitepoint;
                        gmapcontrol.Zoom = 6;
                        //gmapcontrol.Refresh();
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
