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
using xxkUI.Tool;

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
        private EqkShow eqkfrm = null;
        private CursorTool cursorTool;
        private DragMarks dragMarks;//可拖拽标签工具
        private DragPoint dragPoints;//可拖拽节点工具
        private DrawLine drawLines;//绘制线

       
        //Bar bar1 = new Bar(this.tChart.Chart); 
        //DrawLine drawLine1 = new DrawLine(this.tChart.Chart);  
        //bar1.FillSampleValues(20);
        //drawLine1.Series = bar1;
        //drawLine1.Button = MouseButtons.Left; 
        //drawLine1.EnableDraw = true; 
        //drawLine1.EnableSelect = true; 
        //drawLine1.Pen.Color = Color.AliceBlue;

        private Annotation annotation;
        private Annotation annotation_max;
        private Annotation annotation_min;
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
            SetAxesBottomStyle(this.tChart.Axes.Bottom, null);
            SetAxesLeftStyle(this.tChart.Axes.Left);
            gb.Controls.Add(this.tChart);

            InitCursorTool();
            InitDragMarks();
            //InitDragPoints();

            InitAnnotations();

            this.tChart.ClickSeries += TChart_ClickSeries;
            this.tChart.ClickLegend += TChart_ClickLegend;
            this.tChart.MouseMove += tChart_MouseMove;

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
        /// 初始化DragMarks
        /// </summary>
        private void InitDragMarks()
        {
            this.dragMarks = new DragMarks();
            this.tChart.Tools.Add(this.dragMarks);
            this.dragMarks.Active = false;
        }

        /// <summary>
        /// 初始化DragPoints
        /// </summary>
        private void InitDragPoints()
        {
            this.dragPoints = new DragPoint();
            this.tChart.Tools.Add(this.dragPoints);
            this.dragPoints.Active = false;
        }

        /// <summary>
        /// 初始化DragPoint
        /// </summary>
        private void InitDrawLines()
        {
            this.drawLines = new DrawLine();
            this.tChart.Tools.Add(this.drawLines);
            this.drawLines.Active = false;
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
        private void SetAxesBottomStyle(Axis ax, Series ss)
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
        public bool AddSeries(List<LineBean> obsdatalist,string excelPath)
        {

            bool isok = false;
            this.tChart.Header.Text = "";
            try
            {
                this.tChart.Series.Clear();
                foreach (LineBean checkedLb in obsdatalist)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable(checkedLb.OBSLINECODE, excelPath);
                    dt.Columns[0].ColumnName = "观测时间";
                    dt.Columns[1].ColumnName = "观测值";

                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb.OBSLINENAME;
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;
                   
                    line.DataSource = dt;

                    /*只有一条曲线时不显示图例*/
                    line.Legend.Visible = true ? obsdatalist.Count > 1 : obsdatalist.Count <= 1;

                    line.Marks.Visible = false;
                    line.Tag = new LineTag() { Sitecode = checkedLb.SITECODE, Linecode = checkedLb.OBSLINECODE };
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.GetSeriesMark += Line_GetSeriesMark;
                    if (this.tChart.Header.Text != "")
                        this.tChart.Header.Text += "/";
                    this.tChart.Header.Text += line.Title;
                }
                AddVisibleLineVerticalAxis();

            }
            catch (Exception ex)
            {
             //   throw new Exception(ex.Message);
            }
            return isok;
        }


        /// <summary>
        /// 添加单个Series
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="linename"></param>
        /// <returns></returns>
        public bool AddSingleSeries(DataTable dt, string linename)
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
                line.Legend.Visible = false;
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
            if (this.tChart.Series.Count == 0)
                return;

            this.dragMarks.Active = !this.dragMarks.Active;

            try
            {
                if (this.dragMarks.Active)
                {
                    Line ln = this.tChart.Series[0] as Line;
                    int j = 0;

                    if (this.tChart.Series.Count > 1)
                    {
                        this.tChart.Series.RemoveAt(1);
                    }
                    Points pts = new Points(this.tChart.Chart);
                    pts.Pointer.Style = PointerStyles.Circle;
                    pts.Legend.Visible = false;
                    pts.Color = Color.Orange;
                    foreach (DataRow dr in ((DataTable)ln.DataSource).Rows)
                    {
                        if (dr["备注"].ToString() != "")
                        {
                            pts.Add(DateTime.FromOADate(ln[j].X), ln[j].Y);
                        }
                        j++;
                    }

                    this.tChart.Series[0].Marks.Arrow.Color = pts.Color;


                    this.tChart.Series[0].Marks.Arrow.Width = 1;          //标签与单元之间连线的宽度
                    this.tChart.Series[0].Marks.Arrow.Style = System.Drawing.Drawing2D.DashStyle.DashDot;       //标签与单元之间连线样式
                    //this.tChart.Series[0].Marks.Transparent = false;          //标签是否透明
                    //this.tChart.Series[0].Marks.Font.Color = vbBlue;             //'标签文字色
                    //this.tChart.Series[0].Marks.BackColor = pts.Color;            //标签背景色
                   //this.tChart.Series[0].Marks.Gradient.Visible = True;          //是否起用标签渐变色
                     //this.tChart.Series[0].Marks.Bevel = bvNone;                   //标签样式(凹,凸,平面)
                    //this.tChart.Series[0].Marks.ShadowSize = 0;                   //标签阴影大小
                    this.tChart.Series[0].Marks.MultiLine = true;               //是否允许标签多行显示(当标签太长时)

                    this.tChart.Series[0].Marks.TailStyle = MarksTail.None;
                    this.tChart.Series[0].Marks.ShapeStyle = TextShapeStyle.Rectangle;
                    this.tChart.Series[0].Marks.Visible = this.dragMarks.Active;
                    this.dragMarks.Series = this.tChart.Series[0];
                    //this.dragPoints.Series = this.tChart.Series[0];
                }
                else
                {
                    this.tChart.Series[0].Marks.Visible = this.dragMarks.Active;
                    if (this.tChart.Series.Count > 1)
                    {
                        this.tChart.Series.RemoveAt(1);
                    }
                }

            }

            catch (Exception ex)
            { }

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
        /// <param name="isOneLine">是否合并纵轴</param>
        public void AddVisibleLineVerticalAxis()
        {

            int verticalAxisSpace = 3;
            List<BaseLine> visibleSeries = GetVisibleLine();

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

        /// <summary>
        /// 导出曲线图
        /// </summary>
        public void ExportChart()
        {
            this.tChart.Export.ShowExportDialog();
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
            InitDragPoints();
            InitDrawLines();
            if (eqkfrm != null)
            {
                if (eqkfrm.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    eqkfrm = new EqkShow(this.tChart.Series[0].Tag as LineTag, this.tChart, this.dragPoints, this.drawLines);
                   
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
                eqkfrm = new EqkShow(this.tChart.Series[0].Tag as LineTag, this.tChart, this.dragPoints, this.drawLines);
               
                eqkfrm.Show();
                eqkfrm.Focus();
            }
        }

        void eqkfrm_FocousToMapPage(List<EqkBean> eblist)
        {
           
        }

        #endregion

        #region 事件

        private void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            Line line1 = series as Line;
            DataTable ds = (DataTable)line1.DataSource;
            e.MarkText = ds.Rows[e.ValueIndex]["备注"].ToString();
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
            if (visibleSeries.Count == 0)
                return;
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

        private void TChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                Line ln = s as Line;

                DataTable obsdata = (ln.DataSource as DataSet).Tables[0];
                obsdata.Columns[0].ColumnName = "观测时间";
                obsdata.Columns[1].ColumnName = "观测值";
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
