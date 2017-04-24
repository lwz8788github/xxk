using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Charts.Model;
using DevExpress.Utils;
using DevExpress.XtraCharts;

namespace xxkUI.MyCls
{
    class DevChart
    {
        private ChartControl chartControl;
        private List<Color> colorList;
        
        public DevChart(ChartControl _chartcontrol)
        {
            chartControl = _chartcontrol;
            colorList= new List<Color> { Color.Red, Color.Yellow, Color.Tomato, Color.Blue, Color.Green };// 可以添加个数组
        }
        /// <summary>
        /// 准备数据内容
        /// </summary>
        /// <returns></returns>
        public DataTable CreateData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("类型"));
            dt.Columns.Add(new DataColumn("2005-1月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-2月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-3月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-4月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-5月", typeof(decimal)));
            dt.Columns.Add(new DataColumn("2005-6月", typeof(decimal)));

            dt.Rows.Add(new object[] { "员工人数", 437, 437, 414, 397, 387, 378 });
        

            return dt;
        }


        public void CreateChart(DataTable dt)
        {
            #region Series
            //创建几个图形的对象
            Series series1 = CreateSeries("员工人数", ViewType.Line, dt, 0);
       
            #endregion

            List<Series> list = new List<Series>() { series1};
            chartControl.Series.AddRange(list.ToArray());
         

            for (int i = 0; i < list.Count; i++)
            {
                list[i].View.Color = colorList[i];

                CreateAxisY(list[i]);
            }
        }


        /// <summary>
        /// 根据数据创建一个图形展现
        /// </summary>
        /// <param name="caption">图形标题</param>
        /// <param name="viewType">图形类型</param>
        /// <param name="dt">数据DataTable</param>
        /// <param name="rowIndex">图形数据的行序号</param>
        /// <returns></returns>
        private Series CreateSeries(string caption, ViewType viewType, DataTable dt, int rowIndex)
        {
            Series series = new Series(caption, viewType);
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                string argument = dt.Columns[i].ColumnName;//参数名称
                decimal value = (decimal)dt.Rows[rowIndex][i];//参数值
                series.Points.Add(new SeriesPoint(argument, value));
            }

            //必须设置ArgumentScaleType的类型，否则显示会转换为日期格式，导致不是希望的格式显示
            //也就是说，显示字符串的参数，必须设置类型为ScaleType.Qualitative
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;//显示标注标签

            return series;
        }



        /// <summary>
        /// 创建图表的第二坐标系
        /// </summary>
        /// <param name="series">Series对象</param>
        /// <returns></returns>
        private SecondaryAxisY CreateAxisY(Series series)
        {
            SecondaryAxisY myAxis = new SecondaryAxisY(series.Name);
            ((XYDiagram)chartControl.Diagram).SecondaryAxesY.Add(myAxis);
            ((LineSeriesView)series.View).AxisY = myAxis;
            myAxis.Title.Text = series.Name;
            myAxis.Title.Alignment = StringAlignment.Far; //顶部对齐
            myAxis.Title.Visible = true; //显示标题
            myAxis.Title.Font = new Font("宋体", 9.0f);

            Color color = series.View.Color;//设置坐标的颜色和图表线条颜色一致

            myAxis.Title.TextColor = color;
            myAxis.Label.TextColor = color;
            myAxis.Color = color;

            return myAxis;
        }


    }
}
