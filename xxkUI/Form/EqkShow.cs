using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xxkUI.MyCls;
using System.Reflection;
using System.Collections;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;

namespace xxkUI.Form
{
    public partial class EqkShow :XtraForm
    {
       private List<EqkBean> eqkDataList = new List<EqkBean>();
       private LineTag lineTag = new LineTag();
       private TChart tChart;
       private DragPoint DragPtTool;
      
        public EqkShow(LineTag _lineTag,TChart _tChart,DragPoint _dragptTool)
        {
            InitializeComponent();
            this.lineTag = _lineTag;
            tChart = _tChart;
            DragPtTool = _dragptTool;
        }
        public void LoadEqkData(DataTable dt)
        {
            dt.Columns.Add("check", Type.GetType("System.Boolean"));
            //this.gridView.Columns.Clear();
            this.gridControl1.DataSource = null;
            this.gridControl1.DataSource = dt;
            this.gridControl1.Refresh();
            //this.gridView.RefreshData();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            float eqkMlMin = float.Parse(this.textEdit1.Text);
            float eqkMlMax = float.Parse(this.textEdit2.Text);
            if (eqkMlMin > eqkMlMax)
            {
                XtraMessageBox.Show("最大震级应大于最小震级，重新输入！","提示");
                textEdit1.Text = "";
                textEdit2.Text = "";
                return;
            }
            float eqkDepthMin = float.Parse(this.textEdit4.Text);
            float eqkDepthMax = float.Parse(this.textEdit3.Text);
            if (eqkDepthMin > eqkDepthMax)
            {
                XtraMessageBox.Show( "最大深度应大于最小深度，重新输入！","提示");
                textEdit4.Text = "";
                textEdit3.Text = "";
                return;
            }
            string timeStStr = this.textEdit6.Text;
            DateTime timeStc = Convert.ToDateTime(timeStStr);
            DateTime timeSt = Convert.ToDateTime(timeStc).Date; 
            string timeEdStr = this.textEdit5.Text;
            DateTime timeEdc = Convert.ToDateTime(timeEdStr);
            DateTime timeEd = Convert.ToDateTime(timeEdc).Date; 
            if(DateTime.Compare(timeSt,timeEd)>0)
            {
                XtraMessageBox.Show( "结束时间应在开始时间之后！","提示");
                textEdit6.Text = "";
                textEdit5.Text = "";
                return;
            }
            float radial = float.Parse(this.textEdit8.Text);
            if (radial <= 0)
            {
                XtraMessageBox.Show("提示", "缓冲半径应大于0！");
                textEdit8.Text = "";
                return;
            }
            double lon = double.Parse(xxkUI.Bll.SiteBll.Instance.GetNameByID("LONGTITUDE", "SITECODE", lineTag.Sitecode));
            double lat = double.Parse(xxkUI.Bll.SiteBll.Instance.GetNameByID("LATITUDE", "SITECODE", lineTag.Sitecode));

            string sql0 = "select longtitude,latitude,eakdate, magntd, depth, place";
            string sql1 = "  from t_eqkcatalog where MAGNTD >= " + eqkMlMin + " and MAGNTD <=" + eqkMlMax; 
            string sql2 = " and DEPTH >=" + eqkDepthMin + " and DEPTH <=" + eqkDepthMax;
            string sql3 = " and EAKDATE >=" + "\'" + timeSt.ToString() + "\'" + " and EAKDATE <=" + "\'" + timeEd.ToString() + "\'";
            string sql = sql0 + sql1 + sql2 + sql3;
            List<EqkBean> datasource = xxkUI.BLL.EqkBll.Instance.GetList(sql).ToList();
            eqkDataList.Clear();
            foreach (EqkBean eqkData in datasource)
            {
                double dist = LantitudeLongitudeDist(lon, lat, eqkData.Longtitude, eqkData.Latitude);
                if (dist <= radial)
                {
                    eqkData.Dist = Math.Round(dist);
                    eqkDataList.Add(eqkData);
                }
            }
            DataTable eqkObsData = ToDataTable<EqkBean>(eqkDataList);
            LoadEqkData(eqkObsData);
        }

        ///<summary>
        ///转化为弧度(rad) 
        ///</summary>
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /** 
         * 基于余弦定理求两经纬度距离 
         * @param lon1 第一点的经度 
         * @param lat1 第一点的纬度 
         * @param lon2 第二点的经度 
         * @param lat3 第二点的纬度 
         * @return 返回的距离，单位km 
         * */
        private double LantitudeLongitudeDist(double lon1, double lat1, double lon2, double lat2)
        {
            double EARTH_RADIUS = 6378.137;
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);

            double radLon1 = rad(lon1);
            double radLon2 = rad(lon2);

            if (radLat1 < 0)
                radLat1 = Math.PI / 2 + Math.Abs(radLat1);// south  
            if (radLat1 > 0)
                radLat1 = Math.PI / 2 - Math.Abs(radLat1);// north  
            if (radLon1 < 0)
                radLon1 = Math.PI * 2 - Math.Abs(radLon1);// west  
            if (radLat2 < 0)
                radLat2 = Math.PI / 2 + Math.Abs(radLat2);// south  
            if (radLat2 > 0)
                radLat2 = Math.PI / 2 - Math.Abs(radLat2);// north  
            if (radLon2 < 0)
                radLon2 = Math.PI * 2 - Math.Abs(radLon2);// west  
            double x1 = EARTH_RADIUS * Math.Cos(radLon1) * Math.Sin(radLat1);
            double y1 = EARTH_RADIUS * Math.Sin(radLon1) * Math.Sin(radLat1);
            double z1 = EARTH_RADIUS * Math.Cos(radLat1);

            double x2 = EARTH_RADIUS * Math.Cos(radLon2) * Math.Sin(radLat2);
            double y2 = EARTH_RADIUS * Math.Sin(radLon2) * Math.Sin(radLat2);
            double z2 = EARTH_RADIUS * Math.Cos(radLat2);

            double d = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2));
            //余弦定理求夹角  
            double theta = Math.Acos((EARTH_RADIUS * EARTH_RADIUS + EARTH_RADIUS * EARTH_RADIUS - d * d) / (2 * EARTH_RADIUS * EARTH_RADIUS));
            double dist = theta * EARTH_RADIUS;
            return dist;
        }
        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
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

        private void btnEqkAnnotation_Click(object sender, EventArgs e)
        {
            string eakText = "";
            string value = "";
            string eqkTimeStr = "";
            int eqkSelectNum = 0;
            double scale = 1.0;
            Boolean isEqkInTimeSpan = false;
            this.tChart.Tools.Clear();
            for (int i = 0; i < this.gridView.RowCount; i++)
            {
                value = this.gridView.GetDataRow(i)["check"].ToString();
                if (value == "True")
                {
                    eqkTimeStr = this.gridView.GetRowCellValue(i, "EakDate").ToString();
                    DateTime eqkTime = DateTime.Parse(eqkTimeStr);
                    double maxX = tChart.Series[0].XValues.Maximum;
                    double minX = tChart.Series[0].XValues.Minimum;
                    DateTime maxEqkT = DateTime.FromOADate(maxX);
                    DateTime minEqkT = DateTime.FromOADate(minX);
                    TimeSpan spanT = eqkTime.Subtract(minEqkT);
                    double eqkT = spanT.Days + minX;

                    double maxY = tChart.Chart.Series[0].MaxYValue();
                    double minY = tChart.Chart.Series[0].MinYValue();
                    scale = maxY - minY;

                    int index0 = tChart.Chart.Series[0].XValues.IndexOf(maxX);
                    int index1 = tChart.Chart.Series[0].XValues.IndexOf(minX);
                    int index2 = tChart.Chart.Series[0].XValues.IndexOf((minX+maxX)/2.0);

                    //观测时间距离地震时间最近索引
                    int minIndex = 0;
                    double deltX = Math.Abs(tChart.Chart.Series[0].XValues[0] - eqkT), deltX1;

                    for (int j = 1; j < tChart.Chart.Series[0].XValues.Count; j++)
                    {
                        deltX1 = Math.Abs(tChart.Chart.Series[0].XValues[j] - eqkT);
                        if (deltX > deltX1)
                        {
                            minIndex = j;
                            deltX = deltX1;
                        }
                        else break;
                    }
                    
                    //标注地震事件
                    if (maxEqkT.CompareTo(eqkTime) > 0 && minEqkT.CompareTo(eqkTime) < 0)
                    {
                        eakText = this.gridView.GetRowCellValue(i, "Place").ToString() + "\r\n" +"ML="+ this.gridView.GetRowCellValue(i, "Magntd").ToString();
                        eqkAnnotation(scale,eqkTime, tChart.Chart.Series[0].YValues[minIndex], eakText);
                        isEqkInTimeSpan = true;
                    }
                    eqkSelectNum++;
                }
            }
            if (eqkSelectNum == 0) XtraMessageBox.Show("未选中任何震例！","提示");
            if (!isEqkInTimeSpan) XtraMessageBox.Show("所选地震未在观测时间段内！","提示" );
        }
        /// <summary>
        /// 地震事件标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eqkAnnotation(double scale,DateTime date,double value, string eakText)//
        {
            //this.tChart.Chart.Series[0].Legend.Visible = false;
            Arrow arw = new Arrow(this.tChart.Chart);
            //arw.Active = true;
            arw.Visible = true;
            arw.Legend.Visible = false;

            arw.Add(date, value);
            arw.Color = Color.Red;

            arw.Tag = eakText;
            arw.Visible = true;

            arw.Marks.Visible = true;
            arw.Marks.TextAlign = StringAlignment.Center;
            arw.Marks.TextFormat = Steema.TeeChart.Drawing.TextFormat.Normal;
            arw.GetSeriesMark += arw_GetSeriesMark;

            arw.StartYValues.Value[0] = value + scale*0.15;
            arw.EndYValues.Value[0] = value + scale * 0.1;
            arw.StartXValues.Value[0] = arw.XValues.First;
            arw.EndXValues.Value[0] = arw.XValues.Last;


            //this.DragPtTool.Series = arw;

        }

        void arw_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            //throw new NotImplementedException();
            e.MarkText = series.Tag.ToString();
        }
    }
}