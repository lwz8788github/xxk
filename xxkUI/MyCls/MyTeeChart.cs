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

using Steema.TeeChart.Drawing;

using System.Drawing;
using Steema.TeeChart.Tools;
using xxkUI.Form;

namespace xxkUI.MyCls
{
    public class MyTeeChart : TChart
    {
        private TChart tChart;
        private ObsData obsfrm = new ObsData();
        private CursorTool cursorTool;
        Steema.TeeChart.Tools.Annotation annotation;
        Steema.TeeChart.Tools.Annotation annotation_max;
        Steema.TeeChart.Tools.Annotation annotation_min;

        /// <summary>
        /// 是否显示备注
        /// </summary>
        public bool IsShowNote { get; set; }


        public MyTeeChart(GroupBox gb)
        {
            this.tChart = new TChart();

			this.tChart.Aspect.View3D = false;
            this.tChart.Series.Clear();
            this.tChart.Dock = DockStyle.Fill;

            SetTitle("");
            SetLegendStyle(this.tChart.Legend, LegendStyles.Series);
            SetAxesBottomStyle(this.tChart.Axes.Bottom);
            SetAxesLeftStyle(this.tChart.Axes.Left);

            gb.Controls.Add(this.tChart);

            this.cursorTool = new CursorTool();
            this.cursorTool.Chart = this.tChart.Chart;
            this.cursorTool.Active = false;
            this.cursorTool.FollowMouse = true;
            Points pointSeries = new Points(tChart.Chart);
            this.cursorTool.Series = pointSeries;
            this.cursorTool.Style = CursorToolStyles.Vertical;
            this.cursorTool.UseChartRect = true;

            this.tChart.ClickSeries += TChart_ClickSeries;
            this.tChart.ClickLegend += TChart_ClickLegend;
            this.tChart.CursorChanged += tChart_CursorChanged;
            this.tChart.MouseMove += tChart_MouseMove;

            annotation = new Steema.TeeChart.Tools.Annotation(tChart.Chart);
            annotation.Active = false;
            annotation.Shape.CustomPosition = true;
            annotation.Shape.Gradient.Visible = true;
            annotation.Shape.Transparency = 30;

            IsShowNote = false;
       
        }
        /// <summary>
        /// 标签随光标移动显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tChart_CursorChanged(object sender, EventArgs e)
        {
            //double x = cursorTool.XValue;
            List<BaseLine> visibleSeries = GetVisibleLine();
            Steema.TeeChart.Styles.ValueList listXValue = visibleSeries[0].XValues;
            Steema.TeeChart.Styles.ValueList listYValue = visibleSeries[0].YValues;

            Steema.TeeChart.Drawing.PointDouble scrToVa = visibleSeries[0].ScreenPointToValuePoint(int.Parse(cursorTool.XValue.ToString()), int.Parse(cursorTool.YValue.ToString()));
            if (this.cursorTool.Active)
            {
                int minIndex = 0;
                double deltX = Math.Abs(listXValue[0] - scrToVa.X), deltX1;

                for (int i = 1; i < listXValue.Count; i++)
                {
                    deltX1 = Math.Abs(listXValue[i] - scrToVa.X);
                    if (deltX > deltX1)
                    {
                        minIndex = i;
                        deltX = deltX1;
                    }
                    else break;
                }
                System.Drawing.Point poToScr = visibleSeries[0].ValuePointToScreenPoint(listXValue[minIndex], listYValue[minIndex]);
                string showTxt = listYValue[minIndex].ToString();
                annotation.Top = int.Parse(poToScr.Y.ToString());
                annotation.Left = int.Parse(poToScr.X.ToString());
                annotation.Text = showTxt;
                annotation.Active = this.cursorTool.Active;
            }
        }
        /// <summary>
        /// 标注随鼠标移动显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tChart_MouseMove(object sender, MouseEventArgs e)
        {
            List<BaseLine> visibleSeries = GetVisibleLine();
            Steema.TeeChart.Styles.ValueList listXValue = visibleSeries[0].XValues;
            Steema.TeeChart.Styles.ValueList listYValue = visibleSeries[0].YValues;

            Steema.TeeChart.Drawing.PointDouble scrToVa = visibleSeries[0].ScreenPointToValuePoint(e.X, e.Y);
            if (this.cursorTool.Active)
            {
                int minIndex = 0;
                double deltX = Math.Abs(listXValue[0] - scrToVa.X), deltX1;

                for (int i = 1; i < listXValue.Count; i++)
                {
                    deltX1 = Math.Abs(listXValue[i] - scrToVa.X);
                    if (deltX > deltX1)
                    {
                        minIndex = i;
                        deltX = deltX1;
                    }
                    else break;
                }
                System.Drawing.Point poToScr = visibleSeries[0].ValuePointToScreenPoint(listXValue[minIndex], listYValue[minIndex]);
                string showTxt = listYValue[minIndex].ToString();
                annotation.Top = int.Parse(poToScr.Y.ToString());
                annotation.Left = int.Parse(poToScr.X.ToString());
                annotation.Text = showTxt;
                annotation.Active = this.cursorTool.Active;
            }
        }

        private void TChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            DataTable obsdata = s.DataSource as DataTable;

            if (this.tChart.Series.Count > 1)
                AddSeries(obsdata);

            GetObsDataForm();
            obsfrm.LoadDataSource(obsdata);
            obsfrm.Show();
        }
   
        private void TChart_ClickLegend(object sender, MouseEventArgs e)
        {
      
            AddVisibleLineVerticalAxis();
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="titlename">标题名</param>
        private void SetTitle(string titlename)
        {
            this.tChart.Header.Text = titlename;
        }
        /// <summary>
        /// 设置图例样式
        /// </summary>
        /// <param name="lg">图例</param>
        /// <param name="ls">样式</param>
        private void SetLegendStyle(Legend lg, LegendStyles ls)
        {
            lg.LegendStyle = ls;
            lg.CheckBoxes = false;

        }
        /// <summary>
        /// 设置AxesBottom样式
        /// </summary>
        /// <param name="ax"></param>
        private void SetAxesBottomStyle(Axis ax)
        {
            ax.Labels.Angle = 90;
            
            ax.Labels.DateTimeFormat = "yyyy-MM-dd";
            ax.Labels.ExactDateTime = true;
            ax.Labels.Font.Brush.Color = Color.Black;
            ax.Grid.Visible = true;
            ax.Increment = Utils.GetDateTimeStep(DateTimeSteps.OneMonth);
        }
        /// <summary>
        /// 设置AxesLeft样式
        /// </summary>
        /// <param name="ax"></param>
        private void SetAxesLeftStyle(Axis ax)
        {
            ax.AxisPen.Visible = true;
            ax.Grid.DrawEvery = 1;
            ax.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            ax.Grid.Transparency = 0;
            ax.Grid.Visible = true;
            ax.Labels.Font.Brush.Color = Color.Black;
            ax.Labels.Font.Size = 8;
            ax.Labels.Font.SizeFloat = 8F;
            ax.MinorTickCount = 4;
            ax.MinorTicks.Visible = true;
            ax.Ticks.Visible = true;
            ax.TicksInner.Length = 1;
            ax.TicksInner.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            ax.TicksInner.Visible = true;
        }

        /// <summary>
        /// 添加一条曲线
        /// </summary>
        /// <param name="obsdatalist">数据列表</param>
        /// <returns>是否添加成功</returns>
        public bool AddSeries(List<LineBean> obsdatalist)
        {

            bool isok = false;
            this.tChart.Header.Text = "";
            try
            {
                this.tChart.Series.Clear();
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

                    if (this.tChart.Header.Text != "") this.tChart.Header.Text += "/";
                    this.tChart.Header.Text += line.Title;
                }
                AddVisibleLineVerticalAxis();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isok;
        }


        public bool AddSeries(DataTable dt)
        {

            bool isok = false;
            try
            {
                this.tChart.Series.Clear();
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = dt.TableName;
                line.XValues.DataMember = "观测时间";
                line.YValues.DataMember = "观测值";
                line.XValues.DateTime = true;
                line.DataSource = dt;

                AddVisibleLineVerticalAxis();
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


        public void ShowNotes()
        {
            Graphics3D g = this.tChart.Graphics3D;
            if (IsShowNote)
            {
                for (int i = 0; i < this.tChart.Series.Count; i++)
                {
                    Line ln = this.tChart.Series[i] as Line;
                    int j = 0;
                    foreach (DataRow dr in ((DataTable)ln.DataSource).Rows)
                    {
                        if (dr["备注"].ToString() != "")
                        {
                            int screenX = ln.CalcXPosValue(ln[j].X);
                            int screenY = ln.CalcYPosValue(ln[j].Y);
                            Rectangle r = new Rectangle(screenX - 4, screenY - 4, 5, 5);//标识圆的大小
                            g.Cube(r, 0, 20, true);
                        }
                        j++;
                    }
                }
            }



        }

        /// <summary>
        /// 获取可见series
        /// </summary>
        /// <returns></returns>
        private List<BaseLine> GetVisibleLine()
        {
            List<BaseLine> visibleSeries = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                if (tChart.Series[i].Visible)
                {
                    visibleSeries.Add((BaseLine)tChart.Series[i]);
                }
            }
            return visibleSeries;

        }

        /// <summary>
        /// 添加多个纵坐标轴
        /// </summary>
        public void AddVisibleLineVerticalAxis()
        {

            int verticalAxisSpace = 3;

            List<BaseLine> visibleSeries = GetVisibleLine();
            //tChart.Axes.Custom.Clear(); //清除所有自定义的坐标轴
            double singleAxisLengthPercent;//单个纵轴占据的百分比

            //计算每个坐标轴占据的百分比
            if (visibleSeries.Count < 1)
            {
                return;
            }
            else
            {
                singleAxisLengthPercent = Convert.ToDouble(100 - verticalAxisSpace * (visibleSeries.Count + 1)) / (visibleSeries.Count);
            }

            //给可见的曲线加上纵轴
            for (int i = 0; i < visibleSeries.Count; i++)
            {
                Series s = visibleSeries[i];

                Axis axis;
                //设置纵轴的起始位置
                if (i == 0)
                {
                    axis = tChart.Axes.Left; ;
                    axis.StartPosition = verticalAxisSpace;
                    axis.Automatic = true;

                    axis.EndPosition = singleAxisLengthPercent;

                }
                else
                {
                    axis = new Axis(false, false, tChart.Chart);
                    if (i == 1)
                    {
                        axis.StartPosition = tChart.Axes.Left.EndPosition + verticalAxisSpace;
                    }
                    else
                    {
                        axis.StartPosition = visibleSeries[i - 1].CustomVertAxis.EndPosition + verticalAxisSpace;
                    }
                }
                //设置纵轴的结束位置
                axis.EndPosition = axis.StartPosition + singleAxisLengthPercent;

                SetAxesLeftStyle(axis);
                if (i == 0)
                {
                    //曲线本身的纵轴，无需额外处理
                    //tChart.Axes.Custom.Add(axis);
                    ////将纵轴和对应的曲线关联
                    //s.CustomVertAxis = axis;
                }
                else
                {
                    //将自定义纵轴加入图表
                    tChart.Axes.Custom.Add(axis);
                    //将纵轴和对应的曲线关联
                    s.CustomVertAxis = axis;
                }
            }
        }


        /// <summary>
        /// 光标移动事件
        /// </summary>
        /// <param name="CursorTool"></param>
        //public void CursorChange()
        //{
        //}

    

        /// <summary>
        /// 打开数据窗体
        /// </summary>
        private void GetObsDataForm()
        {
            if (obsfrm != null)
            {
                if (obsfrm.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    obsfrm = new ObsData();
                    obsfrm.Show();
                    obsfrm.Focus();
                }
                else
                {
                    obsfrm.Show();
                    obsfrm.Focus();
                }
            }
            else
            {
                obsfrm = new ObsData();
                obsfrm.Show();
                obsfrm.Focus();
            }

        }
        /// <summary>
        /// 标题
        /// </summary>
        public void btnShowTitle()
        {
            this.tChart.Header.Visible = !this.tChart.Header.Visible;
        }
        /// <summary>
        /// 鼠标热线
        /// </summary>
        public void btnMouseCur()
        {
            this.cursorTool.Active = !this.cursorTool.Active;
        }
        /// <summary>
        /// 格网
        /// </summary>
        public void btnGrid()
        {
            this.tChart.Axes.Left.Grid.Visible = !this.tChart.Axes.Left.Grid.Visible;
            this.tChart.Axes.Bottom.Grid.Visible = !this.tChart.Axes.Bottom.Grid.Visible;
        }
        /// <summary>
        /// 最大最小值
        /// </summary>
        public void btnMaxMinValue()
        {
            List<BaseLine> visibleSeries = GetVisibleLine();
            foreach (BaseLine vSeri in visibleSeries)
            {
                Steema.TeeChart.Styles.ValueList listXValue = vSeri.XValues;
                Steema.TeeChart.Styles.ValueList listYValue = vSeri.YValues;

                double maxY = vSeri.YValues.Maximum;
                double minY = vSeri.YValues.Minimum;
                int indexMax = vSeri.YValues.IndexOf(maxY);
                int indexMin = vSeri.YValues.IndexOf(minY);

                annotation_max = new Steema.TeeChart.Tools.Annotation(tChart.Chart);
                annotation_max.Shape.CustomPosition = true;
                annotation_max.Shape.Gradient.Visible = true;
                annotation_max.Shape.Transparency = 15;
                System.Drawing.Point poToScrMax = vSeri.ValuePointToScreenPoint(vSeri.XValues[indexMax], maxY);
                string showTxtMax = maxY.ToString();
                annotation_max.Top = int.Parse(poToScrMax.Y.ToString());
                annotation_max.Left = int.Parse(poToScrMax.X.ToString());
                annotation_max.Text = showTxtMax;

                annotation_min = new Steema.TeeChart.Tools.Annotation(tChart.Chart);
                annotation_min.Shape.CustomPosition = true;
                annotation_min.Shape.Gradient.Visible = true;
                annotation_min.Shape.Transparency = 15;
                System.Drawing.Point poToScrMin = vSeri.ValuePointToScreenPoint(vSeri.XValues[indexMin], minY);
                string showTxtMin = minY.ToString();
                annotation_min.Top = int.Parse(poToScrMin.Y.ToString());
                annotation_min.Left = int.Parse(poToScrMin.X.ToString());
                annotation_min.Text = showTxtMin;
            }
        }
    }
}
