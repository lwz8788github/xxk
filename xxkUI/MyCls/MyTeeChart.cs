using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxkUI.MyCls
{
    public class MyTeeChart
    {
        private TChart tChart;

        public MyTeeChart(TChart _tchart)
        {
            this.tChart = _tchart;
        }


        /// <summary>
        /// 添加一条曲线
        /// </summary>
        /// <param name="obsdatalist">数据列表</param>
        /// <returns>是否添加成功</returns>
        public bool AddSeries(List<LineObsBean> obsdatalist)
        {
            bool isok = false;
            try
            {
                        
                Line ln = new Line();
                tChart.Series.Add(ln);
                ln.Add(obsdatalist);
                tChart.Refresh();

                isok = true;                
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


    }
}
