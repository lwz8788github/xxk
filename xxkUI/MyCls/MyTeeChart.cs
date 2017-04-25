using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using xxkUI.Bll;

namespace xxkUI.MyCls
{
    public class MyTeeChart
    {
        private TChart tChart;
        private int space = 3;
        public MyTeeChart(TChart _tchart)
        {
            this.tChart = _tchart;

            this.tChart.Dock = DockStyle.Fill;
            this.tChart.Aspect.View3D = false;
            this.tChart.Series.Clear();
            this.tChart.Header.Text = "";
            this.tChart.Axes.Bottom.Labels.Angle = 45;
            this.tChart.Legend.LegendStyle = LegendStyles.Series;
            this.tChart.Axes.Bottom.Labels.DateTimeFormat = "yyyy-MM-dd";
            this.tChart.Axes.Bottom.Labels.ExactDateTime = true;
 
            this.tChart.Axes.Bottom.Minimum = 12 * Utils.GetDateTimeStep(DateTimeSteps.OneSecond);
            this.tChart.Axes.Bottom.Minimum = 60 * Utils.GetDateTimeStep(DateTimeSteps.OneSecond);
        }


        /// <summary>
        /// 添加一条曲线
        /// </summary>
        /// <param name="obsdatalist">数据列表</param>
        /// <returns>是否添加成功</returns>
        public bool AddSeries(List<LineBean> obsdatalist)
        {


            bool isok = false;
            try
            {
                foreach (LineBean checkedLb in obsdatalist)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate as 观测时间,obvvalue as 观测值,note as 备注 from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "'");

                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb.OBSLINENAME;
                    line.XValues.DataMember = "观测时间";
                    line.YValues.DataMember = "观测值";
                    line.XValues.DateTime = true;
                    line.DataSource = dt;
                }

                AddCustomAxis(obsdatalist.Count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isok;
        }

        
        public TChart CreateTChartCtrol(int width,int height, System.Drawing.Point location)
        {
            this.tChart.Width = width;
            this.tChart.Height = height;
            this.tChart.Location = location;
            return this.tChart;
        }

        /// <summary>
        /// 添加若干个自定义坐标轴
        /// </summary>
        /// <param name="count"></param>
        public void AddCustomAxis(int count)
        {
            List<BaseLine> listBaseLine = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                if (tChart.Series[i].Visible)
                {
                    listBaseLine.Add((BaseLine)tChart.Series[i]);
                }
            }

            double single = (100 - space * (count + 2)) / (count + 1);//单个坐标轴的百分比
            tChart.Axes.Left.StartPosition = space;
            tChart.Axes.Left.EndPosition = tChart.Axes.Left.EndPosition = tChart.Axes.Left.StartPosition + single;
            tChart.Axes.Left.StartEndPositionUnits = PositionUnits.Percent;
            listBaseLine[0].CustomVertAxis = tChart.Axes.Left;

            double startPosition = tChart.Axes.Left.StartPosition;
            double endPosition = tChart.Axes.Left.EndPosition;
            Axis axis;
            for (int i = 0; i < count; i++)
            {
                axis = new Axis();
                startPosition = endPosition + space;
                endPosition = startPosition + single;
                axis.StartPosition = startPosition;
                axis.EndPosition = endPosition;
                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }




    }
}
