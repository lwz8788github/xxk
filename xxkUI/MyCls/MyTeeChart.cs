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
using System.Runtime.InteropServices;
using xxkUI.Tool;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;

namespace xxkUI.MyCls
{

    public class LineTag
    {
        public string Sitecode { get; set; }
        public string Linecode { get; set; }
    }
    public class MyTeeChart
    {
        /// <summary>
        /// 选中点的结构体（点要素和值）,用于消突跳和消台阶
        /// </summary>
        struct SelectedPointStruct
        {
            public Point PtElement;
            public double PtValue;
        }

        #region 变量
        private TChart tChart;
        private EqkShow eqkfrm = null;
        private CursorTool cursorTool;
        private DragMarks dragMarks;//可拖拽标签工具
        private DragPoint dragPoints;//可拖拽节点工具
        private DrawLine drawLines;//绘制线

        #region 消突跳和台阶用到的变量

        /*记录鼠标操作类型(鼠标热线、消突跳、消台阶)，在Tchart事件交互事件中作为区分*/
        public TChartEventType tchartEventType = TChartEventType.NoProg;
        private Point start = new Point();//矩形起点
        private Point end = new Point();//矩形终点
        private Graphics g;
        private bool isDrawing = false;
        private Points RemoveJumpORStepPoints = null;
        private Points LineBreakPoints = null;
        List<SelectedPointStruct> selectedPtlist = new List<SelectedPointStruct>();

        #endregion

        private Annotation annotation;
        private Annotation annotation_max;
        private Annotation annotation_min;
        private GridControl ObsDatacontrol;//观测数据列表控件
        #endregion

        #region 初始化（MyTeeChart、CursorTool、Annotation）

        /// <summary>
        /// myteechart构造
        /// </summary>
        /// <param name="gb">承载chart控件的groupbox</param>
        /// <param name="obsdatacontrol">观测数据列表控件</param>
        public MyTeeChart(GroupBox _gb, GridControl _obsdatactrl)
        {
            if (_gb == null && _obsdatactrl == null)
                return;

            this.tChart = new TChart();
            this.tChart.Aspect.View3D = false;
            this.tChart.Series.Clear();
            this.tChart.Dock = DockStyle.Fill;
            this.tChart.Zoom.MouseButton = MouseButtons.None;//禁用放大按钮

            SetTitle("");
            SetLegendStyle(this.tChart.Legend, LegendStyles.Series);
            SetAxesBottomStyle(this.tChart.Axes.Bottom, null);
            SetAxesLeftStyle(this.tChart.Axes.Left);
            _gb.Controls.Add(this.tChart);

            ObsDatacontrol = _obsdatactrl;

            InitCursorTool();
            InitDragMarks();
            //InitDragPoints();

            InitAnnotations();

            this.tChart.ClickSeries += TChart_ClickSeries;
            this.tChart.ClickLegend += TChart_ClickLegend;
            this.tChart.MouseDown += tChart_MouseDown;
            this.tChart.MouseMove += tChart_MouseMove;
            this.tChart.MouseUp += tChart_MouseUp;
            this.tChart.AfterDraw += TChart_AfterDraw;
            this.tChart.MouseWheel += TChart_MouseWheel;//滚轮事件放大chart
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

        /// <summary>
        /// 初始化RemoveJumpOrStepPoint
        /// </summary>
        private void InitRemoveJumpOrStepPoint()
        {
           
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
        public bool AddSeries(List<LineBean> obsdatalist, string excelPath)
        {

            bool isok = false;
            this.tChart.Header.Text = "";
            try
            {
                this.tChart.Series.Clear();
                foreach (LineBean checkedLb in obsdatalist)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable(checkedLb.OBSLINECODE, excelPath);
                    if (!ObsdataCls.IsExisted(checkedLb.OBSLINENAME))
                        ObsdataCls.ObsdataHash.Add(checkedLb.OBSLINENAME, dt);


                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb.OBSLINENAME;
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;

                    line.DataSource = ObsdataCls.ObsdataHash[checkedLb.OBSLINENAME] as DataTable;

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

                    if (obsdatalist.Count == 1)
                    {
                        this.ObsDatacontrol.DataSource = ObsdataCls.ObsdataHash[checkedLb.OBSLINENAME] as DataTable;
                        this.ObsDatacontrol.Refresh();
                    }
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
        public bool AddSingleSeries(string linename)
        {
            bool isok = false;
            try
            {
                this.tChart.Series.Clear();
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = linename;

                line.XValues.DataMember = "obvdate";
                line.YValues.DataMember = "obvvalue";
                line.XValues.DateTime = true;
                line.DataSource = ObsdataCls.ObsdataHash[linename] as DataTable;
                line.Legend.Visible = false;
                line.Marks.Visible = false;

                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.GetSeriesMark += Line_GetSeriesMark;
                this.tChart.Header.Text = linename;
                AddVisibleLineVerticalAxis();

                /*只有一条曲线时要显示数据列表*/
                this.ObsDatacontrol.DataSource = ObsdataCls.ObsdataHash[linename] as DataTable;
                this.ObsDatacontrol.Refresh();

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
        /// 获取曲线Series数量
        /// </summary>
        /// <returns></returns>
        private int GetLineSeriesCount()
        {
            int seriesCount = 0;
            for (int i = 0; i < this.tChart.Series.Count; i++)
            {
                try
                {
                    Line ln = this.tChart.Series[i] as Line;
                    if (ln != null)
                        seriesCount++;
                }
                catch
                {
                    continue;
                }
            }
            return seriesCount;
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

                    eqkfrm.ShowDialog();
                    eqkfrm.Focus();
                }
                else
                {
                    eqkfrm.ShowDialog();
                    eqkfrm.Focus();
                }
            }
            else
            {
                eqkfrm = new EqkShow(this.tChart.Series[0].Tag as LineTag, this.tChart, this.dragPoints, this.drawLines);

                eqkfrm.ShowDialog();
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
        /// 鼠标按下事件（消突跳、消台阶拉选选择的开始）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (tchartEventType)
                {
                    case TChartEventType.RemoveJump://消突跳
                        {
                            selectedPtlist.Clear();
                            RemoveJumpORStepPoints.Clear();
                            tChart.Refresh();

                            g = this.tChart.CreateGraphics();
                            start.X = e.X;
                            start.Y = e.Y;
                            end.X = e.X;
                            end.Y = e.Y;
                            isDrawing = true;
                        }
                        break;
                    case TChartEventType.RemoveStep://消台阶
                        {
                            selectedPtlist.Clear();
                            RemoveJumpORStepPoints.Clear();
                            tChart.Refresh();

                            g = this.tChart.CreateGraphics();
                            start.X = e.X;
                            start.Y = e.Y;
                            end.X = e.X;
                            end.Y = e.Y;
                            isDrawing = true;
                        }
                        break;
                    case TChartEventType.LineBreak://测项拆分
                        {
                            selectedPtlist.Clear();
                            RemoveJumpORStepPoints.Clear();
                            tChart.Refresh();

                            g = this.tChart.CreateGraphics();
                            start.X = e.X;
                            start.Y = e.Y;
                            end.X = e.X;
                            end.Y = e.Y;
                            isDrawing = true;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 标注随鼠标移动显示事件（鼠标热线、消突跳消台阶拉框选择）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tChart_MouseMove(object sender, MouseEventArgs e)
        {

            switch (tchartEventType)
            {
                case TChartEventType.Hotline://鼠标热线
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
                    break;

                case TChartEventType.RemoveJump://消突跳
                    if (e.Button == MouseButtons.Left)
                    {
                        if (isDrawing)
                        {
                            this.tChart.Cursor = Cursors.Cross;
                            //先擦除
                            g.DrawRectangle(new Pen(Color.White), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                            end.X = e.X;
                            end.Y = e.Y;
                            //再画
                            g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                        }
                    }
                    break;
                case TChartEventType.RemoveStep://消台阶
                    if (e.Button == MouseButtons.Left)
                    {

                        if (isDrawing)
                        {
                            this.tChart.Cursor = Cursors.Cross;
                            //先擦除
                            g.DrawRectangle(new Pen(Color.White), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                            end.X = e.X;
                            end.Y = e.Y;
                            //再画
                            g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                        }
                    }
                    break;
                case TChartEventType.LineBreak://测项拆分
                    if (e.Button == MouseButtons.Left)
                    {

                        if (isDrawing)
                        {
                            this.tChart.Cursor = Cursors.Cross;
                            //先擦除
                            g.DrawRectangle(new Pen(Color.White), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                            end.X = e.X;
                            end.Y = e.Y;
                            //再画
                            g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                        }
                    }
                    break;
            }


        }

        /// <summary>
        /// 鼠标点击抬起事件（消突跳消台阶）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                switch (tchartEventType)
                {
                    case TChartEventType.RemoveJump://消突跳
                        {
                            this.tChart.Cursor = Cursors.Arrow;
                            DrawJumOrStepPoints(e);
                        }

                        break;
                    case TChartEventType.RemoveStep://消台阶
                        {
                            this.tChart.Cursor = Cursors.Arrow;
                            DrawJumOrStepPoints(e);
                        }
                        break;
                    case TChartEventType.LineBreak://测项拆分
                        {
                            this.tChart.Cursor = Cursors.Arrow;
                            DrawJumOrStepPoints(e);
                        }
                        break;
                }
            }

           
        }

        /// <summary>
        /// 滚轮事件（放大缩小）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (GetLineSeriesCount() != 1)
                return;
            if (e.Delta > 0)
            {
                tChart.Zoom.ZoomPercent(110);
            }
            else
            {
                tChart.Zoom.ZoomPercent(90);
            }
        }

        /// <summary>
        /// 重新加载消突跳台阶选中点数据(用于消突跳、台阶完毕后的重画选中点)
        /// </summary>
        /// <param name="dt"></param>
        private void ReLoadRemoveJumpORStepPointsData(DataTable dt)
        {
            RemoveJumpORStepPoints.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                RemoveJumpORStepPoints.Add(DateTime.Parse(dr[0].ToString()), double.Parse(dr[1].ToString()));
            }
            this.tChart.Series.Add(RemoveJumpORStepPoints);
            this.tChart.Refresh();
        }

        /// <summary>
        /// 重新计算SelectedPtlist的屏幕XY值
        /// </summary>
        private void ReCalculateSelectedPtlist()
        {
            selectedPtlist.Clear();
            for (int i = 0; i < RemoveJumpORStepPoints.Count; i++)
            {
                int screenX = RemoveJumpORStepPoints.CalcXPosValue(RemoveJumpORStepPoints[i].X);
                int screenY = RemoveJumpORStepPoints.CalcYPosValue(RemoveJumpORStepPoints[i].Y);
                selectedPtlist.Add(new SelectedPointStruct() { PtElement = new Point(screenX, screenY), PtValue = RemoveJumpORStepPoints[i].Y });
            }
        }

        /// <summary>
        /// tchartAfterDraw事件（重新绘制台阶线）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="g"></param>
        private void TChart_AfterDraw(object sender, Graphics3D g)
        {
            if (RemoveJumpORStepPoints != null)
                /*重新计算SelectedPtlist的屏幕XY值*/
                ReCalculateSelectedPtlist();
            if (this.selectedPtlist.Count <= 1)
                return;

            /*重新绘制台阶线*/
            for (int i = 1; i < this.selectedPtlist.Count; i++)
            {
                Point pt1 = this.selectedPtlist[i - 1].PtElement;
                Point pt2 = new Point(this.selectedPtlist[i - 1].PtElement.X, this.selectedPtlist[i].PtElement.Y);
                Point pt3 = this.selectedPtlist[i].PtElement;

                g.Brush.Color = Color.Gray;

                g.Line(pt1, pt2);
                g.Line(pt2, pt3);

                string text = Math.Round((this.selectedPtlist[i].PtValue - this.selectedPtlist[i - 1].PtValue), 3).ToString();
                g.TextOut(pt1.X, pt3.Y - 12, text);
            }

        }

        /// <summary>
        /// 曲线点击事件，弹出数据框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="s"></param>
        /// <param name="valueIndex"></param>
        /// <param name="e"></param>
        private void TChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                Line ln = s as Line;
                if (this.tChart.Series.Count > 1)
                    AddSingleSeries(ln.Title);
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
            if (this.tChart.Series.Count != 1) return;

            this.cursorTool.Active = !this.cursorTool.Active;
            annotation.Active = this.cursorTool.Active;

            if (annotation.Active && this.cursorTool.Active)
            {
                this.tchartEventType = TChartEventType.Hotline;
            }
            else
            {
                this.tchartEventType = TChartEventType.NoProg;
            }
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

        #region 数据处理方法

        /// <summary>
        /// 加减乘除
        /// </summary>
        public void PlusMinusMultiplyDivide()
        {
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;


            Line ln = this.tChart.Series[0] as Line;

            DataTable dt = ObsdataCls.ObsdataHash[ln.Title] as DataTable;

            DataProgreeFrm dpf = new DataProgreeFrm(dt.Rows.Count);
            if (dpf.ShowDialog() == DialogResult.OK)
            {
                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[ln.Title] = pralg.PlusMinusMultiplyDivide(dt, dpf.progreeValue, dpf.dpm);
                AddSingleSeries(this.tChart.Header.Text);
            }
        }
        /// <summary>
        /// 进入消台阶或消突跳操作
        /// </summary>
        public void RemoStepOrJump(TChartEventType tep)
        {
          
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;
           
            this.tchartEventType = tep;
            start = new Point();//矩形起点
            end = new Point();//矩形终点
            g = this.tChart.CreateGraphics();

            this.tChart.Cursor = Cursors.Cross;
            this.tChart.Series.Remove(this.RemoveJumpORStepPoints);
            RemoveJumpORStepPoints = new Points(this.tChart.Chart);
            RemoveJumpORStepPoints.Color = Color.Red;
            RemoveJumpORStepPoints.Legend.Visible = false;
            RemoveJumpORStepPoints.Marks.Visible = false;
            RemoveJumpORStepPoints.Pointer.Style = PointerStyles.PolishedSphere;
            RemoveJumpORStepPoints.Pointer.SizeUnits = PointerSizeUnits.Axis;
            RemoveJumpORStepPoints.Pointer.SizeDouble = 20;
        }

        /// <summary>
        /// 测项合并
        /// </summary>
        public void LinesUnion()
        {
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            Line ln = this.tChart.Series[0] as Line;
  
            ObslineMergeFrm olmf = new ObslineMergeFrm();
            if (olmf.ShowDialog() == DialogResult.OK)
            {

                DataTable dtone = ObsdataCls.ObsdataHash[ln.Title] as DataTable;
                DataTable dttwo = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue,note from t_obsrvtntb where obslinecode = '" + olmf.SelectedObsLineCode + "'");

                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[ln.Title] = pralg.Merge(dtone, dttwo, olmf.MoveToAverage);

                AddSingleSeries(this.tChart.Header.Text);
            }
        }
        /// <summary>
        /// 测项拆分
        /// </summary>
        public void LinesBreak(TChartEventType tep)
        {

            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            //this.tChart.Cursor = Cursors.Cross;

            this.tchartEventType = tep;
            start = new Point();//矩形起点
            end = new Point();//矩形终点
            g = this.tChart.CreateGraphics();

            this.tChart.Cursor = Cursors.Cross;

            this.tChart.Series.Remove(this.RemoveJumpORStepPoints);
            RemoveJumpORStepPoints = new Points(this.tChart.Chart);
            RemoveJumpORStepPoints.Color = Color.Red;
            RemoveJumpORStepPoints.Legend.Visible = false;
            RemoveJumpORStepPoints.Marks.Visible = false;
            RemoveJumpORStepPoints.Pointer.Style = PointerStyles.PolishedSphere;
            RemoveJumpORStepPoints.Pointer.SizeUnits = PointerSizeUnits.Axis;
            RemoveJumpORStepPoints.Pointer.SizeDouble = 20;
           
        }
        /// <summary>
        /// 保存处理数据
        /// </summary>
        public void SaveHandleData()
        {
            try
            {
                Line ln = this.tChart.Series[0] as Line;
                DataTable dt = ObsdataCls.ObsdataHash[ln.Title] as DataTable;
                NpoiCreator npcreator = new NpoiCreator();
                npcreator.TemplateFile = DataFromPath.HandleDataPath;
                npcreator.NpoiExcel(dt, ln.Title + ".xls", DataFromPath.HandleDataPath + "/" + ln.Title + ".xls");

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败！：" + ex.Message, "错误");
            }
        }
        /// <summary>
        /// 画消突跳或者台阶的标注点
        /// </summary>
        private void DrawJumOrStepPoints(MouseEventArgs e)
        {
            if (isDrawing)
            {
                //清空选中点数组的内容
                selectedPtlist.Clear();

                g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, e.X - start.X, e.Y - start.Y);
                int minX = Math.Min(start.X, e.X);
                int minY = Math.Min(start.Y, e.Y);
                int maxX = Math.Max(start.X, e.X);
                int maxY = Math.Max(start.Y, e.Y);

                try
                {
                    if (tChart != null)
                    {
                        if (tChart.Series.Count > 0)
                        {
                            Series series = tChart.Series[0];
                            Line ln = series as Line;
                            DataTable dt = ObsdataCls.ObsdataHash[ln.Title] as DataTable;

                            //this.tChart.Refresh();
                            for (int i = 0; i < ln.Count; i++)
                            {
                                int screenX = series.CalcXPosValue(ln[i].X);
                                int screenY = series.CalcYPosValue(ln[i].Y);
                                if (screenX >= minX && screenX <= maxX && screenY >= minY && screenY <= maxY)
                                {
                                    RemoveJumpORStepPoints.Add(ln[i].X, ln[i].Y);
                                    selectedPtlist.Add(new SelectedPointStruct() { PtElement = new Point(screenX, screenY), PtValue = ln[i].Y });
                                }
                            }
                            //this.tChart.Refresh();

                            switch (tchartEventType)
                            {
                                case TChartEventType.RemoveJump://消突跳
                                    {

                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < RemoveJumpORStepPoints.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(RemoveJumpORStepPoints[i].X);
                                            newdr[1] = RemoveJumpORStepPoints[i].Y;
                                            selectdt.Rows.Add(newdr);
                                        }
                                        //this.tChart.Cursor = Cursors.Default;
                                        RemoveJumpFrm dpf = new RemoveJumpFrm(dt, selectdt);
                                        if (dpf.ShowDialog() == DialogResult.OK)
                                        {
                                            ObsdataCls.ObsdataHash[ln.Title] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSingleSeries(this.tChart.Header.Text);
                                            /*重画选中的点*/
                                            ReLoadRemoveJumpORStepPointsData(dpf.dataoutsel);
                                            tchartEventType = TChartEventType.NoProg;
                                        }
                                    }
                                    break;
                                case TChartEventType.RemoveStep://消台阶
                                    {
                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < RemoveJumpORStepPoints.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(RemoveJumpORStepPoints[i].X);
                                            newdr[1] = RemoveJumpORStepPoints[i].Y;
                                            selectdt.Rows.Add(newdr);
                                        }

                                        RemoveStepFrm dpf = new RemoveStepFrm(dt, selectdt);
                                        if (dpf.ShowDialog() == DialogResult.OK)
                                        {
                                            ObsdataCls.ObsdataHash[ln.Title] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSingleSeries(this.tChart.Header.Text);
                                            /*重画选中的点*/
                                            ReLoadRemoveJumpORStepPointsData(dpf.dataoutsel);
                                            tchartEventType = TChartEventType.NoProg;
                                        }
                                    }
                                    break;

                                case TChartEventType.LineBreak://测项拆分
                                    {
                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < RemoveJumpORStepPoints.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(RemoveJumpORStepPoints[i].X);
                                            newdr[1] = RemoveJumpORStepPoints[i].Y;
                                            selectdt.Rows.Add(newdr);
                                        }

                                        PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                                        DataTable dtin = ObsdataCls.ObsdataHash[ln.Title] as DataTable;
                                        ObsdataCls.ObsdataHash[ln.Title] = pralg.Split(dtin, selectdt,"" );

                                        RemoveJumpORStepPoints.Clear();
                                        /*重画曲线*/
                                        AddSingleSeries(this.tChart.Header.Text);

                                        tchartEventType = TChartEventType.NoProg;
                                    }
                                    break;
                            }

                        }

                    }
                }
                catch
                {
                }
                isDrawing = false;
            }

        }

        #endregion

        #region 数据增删改引起的曲线动态变化

        public void DeleteChartlineData(DateTime obsdate, double obsv)
        {
            Line ln = this.tChart.Series[0] as Line;
            for (int i = 0; i < this.tChart.Series[0].Count; i++)
            {
                if (DateTime.FromOADate(ln[i].X) == obsdate && obsv == ln[i].Y)
                    ln.Delete(i);
            }
            this.tChart.Refresh();
        }

        public void ModifyChartlineData(int focusedRow, DateTime obsdate, double obsv)
        {
            this.tChart.Series[0].XValues[focusedRow] = obsdate.ToOADate();
            this.tChart.Series[0].YValues[focusedRow] = obsv;
            this.tChart.Refresh();
        }

        public void AddChartlineData(DateTime obsdate, double obdv)
        {
            if (!IsExisted(this.tChart.Series[0], obsdate, obdv))
            {
                this.tChart.Series[0].Add(obsdate, obdv);
            }
            else
            {
                XtraMessageBox.Show("已存在相同数据", "提示");
                //drv["观测时间"] = new DateTime();
                //drv["观测值"] = double.NaN;
            }

            this.tChart.Refresh();
        }

        public void RefreshLineDatasource()
        {
            Line ln = this.tChart.Series[0] as Line;
            ln.DataSource = ObsdataCls.ObsdataHash[ln.Title] as DataTable;
            this.tChart.Refresh();
        }

        /// <summary>
        /// 定位到曲线上该数据位置
        /// </summary>
        /// <param name="obsdate"></param>
        /// <param name="obsv"></param>
        public void GoTodata(DateTime obsdate, double obsv)
        {
            /*
             * * 保留第一个Series，其他删除
             */
            int sc = this.tChart.Series.Count;
            if (sc > 1)
            {
                for (int i = 1; i < sc; i++)
                    this.tChart.Series.RemoveAt(i);
            }

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


        #endregion
    }
}
