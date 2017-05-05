using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using xxkUI.Bll;
using Steema.TeeChart.Drawing;
using System.Drawing;
using Steema.TeeChart.Tools;
using xxkUI.Form;

namespace xxkUI.MyCls
{

    public class LineTag
    {
        public string Sitecode { get; set; }
        public string Linecode { get; set; }
    }
    public class MyTeeChart 
    {
        #region 变量
        private TChart tChart;
        private ObsData obsfrm = new ObsData();
        private EqkShow eqkfrm = new EqkShow();
        private CursorTool cursorTool;
        private Annotation annotation;
        private Annotation annotation_max;
        private Annotation annotation_min;
    
        /// <summary>
        /// 是否显示备注
        /// </summary>
        public bool IsShowNote{get;set;}
        #endregion

        #region 初始化（MyTeeChart、CursorTool、Annotation）

        public MyTeeChart(GroupBox gb)
        {
            this.tChart = new TChart();
            this.tChart.Aspect.View3D = false;
            this.tChart.Series.Clear();
            this.tChart.Dock = DockStyle.Fill;
			
            SetTitle("");
            SetLegendStyle(this.tChart.Legend, LegendStyles.Series);
            SetAxesBottomStyle(this.tChart.Axes.Bottom,null);
            SetAxesLeftStyle(this.tChart.Axes.Left);
            gb.Controls.Add(this.tChart);
           
		   InitCursorTool();
		   InitAnnotations();
		   
            this.tChart.ClickSeries += TChart_ClickSeries;
            this.tChart.ClickLegend += TChart_ClickLegend;
            this.tChart.MouseDown += TChart_MouseDown;
            this.tChart.MouseMove += tChart_MouseMove;
            this.tChart.MouseUp += TChart_MouseUp;
          
            IsShowNote = false;
        }

    

        /// <summary>
        /// 初始化CursorTool
        /// </summary>
        private void InitCursorTool()
        {
            this.cursorTool = new CursorTool();
            this.cursorTool.Chart = this.tChart.Chart;
            this.cursorTool.Active = false;
            this.cursorTool.FollowMouse = true;
            Points pointSeries = new Points(tChart.Chart);
            this.cursorTool.Series = pointSeries;
            this.cursorTool.Style = CursorToolStyles.Vertical;
            this.cursorTool.UseChartRect = true;
        }
		
		/// <summary>
        /// 初始化Annotations
        /// </summary>
		private void InitAnnotations()
		{
            annotation_min = new Annotation(tChart.Chart);
            annotation_min.Active = false;
            annotation_max = new Annotation(tChart.Chart);
            annotation_max.Active = false;
            annotation = new Annotation(tChart.Chart);
            annotation.Active = false;
            annotation.Shape.CustomPosition = true;
            annotation.Shape.Gradient.Visible = true;
            annotation.Shape.Transparency = 30;
		}

        #endregion

        #region 图表样式（Title、Legend、Axes）

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
        private void SetAxesBottomStyle(Axis ax,Series ss)
        {
            ax.Labels.Angle = 90;
            ax.Labels.DateTimeFormat = "yyyy-MM-dd";
            ax.Labels.ExactDateTime = true;
            ax.Labels.Font.Brush.Color = Color.Black;
            ax.Grid.Visible = true;
           
            //ax.Increment = Utils.AnimationTypesCount;//(DateTimeSteps.OneMonth);
            if (ss != null)
            {
                //if (ss.Count < 20)
                //    ax.Increment = (ax.MaxXValue - ax.MinXValue) / ss.Count;
                //else
                    ax.Increment = (ax.MaxXValue - ax.MinXValue) / 20;
            }
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

        #endregion

        #region 方法（添加数据、显示备注、获取可见Series、添加多个坐标轴）
       
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
                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate as 观测时间,obvvalue as 观测值,note as 备注 from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "' order by 观测时间");
                    string currentSitecode = LineBll.Instance.GetNameByID("SITECODE", "OBSLINECODE", checkedLb.OBSLINECODE);

                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb.OBSLINENAME;
                    line.XValues.DataMember = "观测时间";
                    line.YValues.DataMember = "观测值";
                    line.XValues.DateTime = true;
                    line.DataSource = dt;
                    line.Legend.Visible = true;
                    line.Marks.Visible = false;
                    line.Tag = new LineTag() { Sitecode = currentSitecode, Linecode = checkedLb.OBSLINECODE };
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.GetSeriesMark += Line_GetSeriesMark;
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

      

        /// <summary>
        /// 添加单个Series
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="linename"></param>
        /// <returns></returns>
        public bool AddSingleSeries(DataTable dt,string linename)
        {
            bool isok = false;
            try
            {
                this.tChart.Series.Clear();
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = linename;

                line.XValues.DataMember = "观测时间";
                line.YValues.DataMember = "观测值";
                line.XValues.DateTime = true;
                line.DataSource = dt;
                line.Legend.Visible = true;
                line.Marks.Visible = false;
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.GetSeriesMark += Line_GetSeriesMark;
                this.tChart.Header.Text = linename;
                AddVisibleLineVerticalAxis();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isok;
        }

        /// <summary>
        /// 显示备注
        /// </summary>
        public void ShowNotes()
        {

            for (int i = 0; i < this.tChart.Series.Count; i++)
            {
                this.tChart.Series[i].Marks.Visible = !this.tChart.Series[i].Marks.Visible;
            }
            //try
            //{
            //    Graphics3D g = this.tChart.Graphics3D;
             
            //    if (IsShowNote)
            //    {
            //        for (int i = 0; i < this.tChart.Series.Count; i++)
            //        {
            //            Line ln = this.tChart.Series[i] as Line;
            //            int j = 0;
            //            foreach (DataRow dr in ((DataTable)ln.DataSource).Rows)
            //            {
            //                if (dr["备注"].ToString() != "")
            //                {
            //                    Points pts = new Points(this.tChart.Chart);
            //                    pts.Marks.Visible = true;
                             
            //                    pts.Add(DateTime.FromOADate(ln[j].X), ln[j].Y);

            //                    pts.Pointer.Style = PointerStyles.Circle;

            //                    pts.Legend.Visible = false;
            //                    pts.Color = Color.Orange;

            //                    Annotation att = new Annotation(this.tChart.Chart);
                               
            //                    att.Shape.CustomPosition = true;
            //                    att.Shape.Gradient.Visible = true;
            //                    att.Shape.Transparency = 15;
            //                    Point poToScrMin = ln.ValuePointToScreenPoint(ln.XValues[j], ln.YValues[j]);
            //                    att.Top = int.Parse(poToScrMin.Y.ToString());
            //                    att.Left = int.Parse(poToScrMin.X.ToString());
            //                    att.Text = dr["备注"].ToString();



            //                }
            //                j++;
            //            }
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{ } 

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
                SetAxesBottomStyle(tChart.Axes.Bottom, s);
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

        #endregion

        #region 激活窗体

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
        /// 打开历史震例窗体
        /// </summary>
        public void GetEqkShowForm()
        {
            if (eqkfrm != null)
            {
                if (eqkfrm.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    eqkfrm = new EqkShow();
                    eqkfrm.Show();
                    eqkfrm.Focus();
                }
                else
                {
                    eqkfrm.Show();
                    eqkfrm.Focus();
                }
            }
            else
            {
                eqkfrm = new EqkShow();
                eqkfrm.Show();
                eqkfrm.Focus();
            }
        }

        #endregion

        #region 事件

        private void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
        //    Line line1 = series as Line;
        //    DataTable ds = (DataTable)line1.DataSource;
        //    if (ds.Rows[e.ValueIndex]["备注"].ToString() == "")
           
        //        return;

        //    e.MarkText = ds.Rows[e.ValueIndex]["备注"].ToString();
        //    //foreach (DataRow dr in ().Rows)
            //{

            //        if (DateTime.FromOADate([i].X) == obsdate && obsv == s[i].Y)

            //    if (dr["备注"].ToString() != "")
            //    {

            //        e.MarkText = dr["备注"].ToString();
            //    }

        //}



            

            //if (e.ValueIndex > 0)
            //{
            //    if (line1.YValues[e.ValueIndex] > line1.YValues[e.ValueIndex - 1])
            //    {
            //        e.MarkText = e.MarkText + " (Up)";
            //    }
            //    else if (line1.YValues[e.ValueIndex] < line1.YValues[e.ValueIndex - 1])
            //    {
            //        e.MarkText = e.MarkText + " (Down)";
            //    }
            //    else
            //    {
            //        e.MarkText = e.MarkText + " (No Change)";dscore
            //    }
            //}
        }

        private void TChart_MouseUp(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 标注随鼠标移动显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tChart_MouseMove(object sender, MouseEventArgs e)
        {
            int maxX = tChart.Chart.ChartRect.X + tChart.Chart.ChartRect.Width;
            int minX = tChart.Chart.ChartRect.X;
            int maxY = tChart.Chart.ChartRect.Y + tChart.Chart.ChartRect.Height;
            int minY = tChart.Chart.ChartRect.Y;
            List<BaseLine> visibleSeries = GetVisibleLine();
            PointDouble scrToVa = visibleSeries[0].ScreenPointToValuePoint(e.X, e.Y);

            if (e.X < maxX && e.X > minX && e.Y < maxY && e.Y > minY)
            {
                if (!this.cursorTool.Active)
                {
                    return;
                }
                else
                {

                    ValueList listXValue = visibleSeries[0].XValues;
                    ValueList listYValue = visibleSeries[0].YValues;

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
                    Point poToScr = visibleSeries[0].ValuePointToScreenPoint(listXValue[minIndex], listYValue[minIndex]);
                    DateTime showTime = DateTime.FromOADate(listXValue[minIndex]);
                    string showTxt = "观测时间:" + showTime.ToShortDateString() + "\r\n" + "观测值:" + listYValue[minIndex].ToString();

                    annotation.Top = int.Parse(poToScr.Y.ToString());
                    annotation.Left = int.Parse(poToScr.X.ToString());
                    annotation.Text = showTxt;

                }
            }
        }

        private void TChart_MouseDown(object sender, MouseEventArgs e)
        {

        }

       

        private void TChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                Line ln = s as Line;

                DataTable obsdata = ln.DataSource as DataTable;
                if (this.tChart.Series.Count > 1)
                    AddSingleSeries(obsdata, ln.Title);

                GetObsDataForm();
                obsfrm.LoadDataSource(obsdata, this.tChart);
                obsfrm.Show();
            }
            catch (Exception ex)
            {
                // XtraMessageBox.Show("错误", ex.Message);
            }
        }

        private void TChart_ClickLegend(object sender, MouseEventArgs e)
        {
            AddVisibleLineVerticalAxis();
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
            if (this.tChart.Series.Count > 1) return;
            this.cursorTool.Active = !this.cursorTool.Active;
            annotation.Active = this.cursorTool.Active;
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
            if (this.tChart.Series.Count > 1) return;
            annotation_max.Active = !annotation_max.Active;
            annotation_min.Active = !annotation_min.Active;
            if (!(annotation_max.Active && annotation_min.Active)) return;
            List<BaseLine> visibleSeries = GetVisibleLine();
            foreach (BaseLine vSeri in visibleSeries)
            {
                ValueList listXValue = vSeri.XValues;
                ValueList listYValue = vSeri.YValues;

                double maxY = vSeri.YValues.Maximum;
                double minY = vSeri.YValues.Minimum;
                int indexMax = vSeri.YValues.IndexOf(maxY);
                int indexMin = vSeri.YValues.IndexOf(minY);

                annotation_max.Shape.CustomPosition = true;
                annotation_max.Shape.Gradient.Visible = true;
                annotation_max.Shape.Transparency = 15;
                Point poToScrMax = vSeri.ValuePointToScreenPoint(vSeri.XValues[indexMax], maxY);
                string showTxtMax = maxY.ToString();
                annotation_max.Top = int.Parse(poToScrMax.Y.ToString());
                annotation_max.Left = int.Parse(poToScrMax.X.ToString());
                annotation_max.Text = showTxtMax;

                annotation_min.Shape.CustomPosition = true;
                annotation_min.Shape.Gradient.Visible = true;
                annotation_min.Shape.Transparency = 15;
                Point poToScrMin = vSeri.ValuePointToScreenPoint(vSeri.XValues[indexMin], minY);
                string showTxtMin = minY.ToString();
                annotation_min.Top = int.Parse(poToScrMin.Y.ToString());
                annotation_min.Left = int.Parse(poToScrMin.X.ToString());
                annotation_min.Text = showTxtMin;
            }
        }


        /// <summary>
        /// 鼠标离开测线，变窄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_MouseLeave(object sender, EventArgs e)
        {
            Line ln = sender as Line;
            ln.LinePen.Width--;

        }

        /// <summary>
        /// 鼠标进入测线，变宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_MouseEnter(object sender, EventArgs e)
        {
            Line ln = sender as Line;
            ln.LinePen.Width++;
        }

        #endregion
    }
}
