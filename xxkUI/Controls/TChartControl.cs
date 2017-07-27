using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;
using xxkUI.Form;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Drawing;
using Steema.TeeChart;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using xxkUI.Bll;
using DevExpress.XtraBars;

namespace xxkUI.Controls
{
    public partial class TChartControl : UserControl
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
        private Points RemoveJumpORStepPoints = null;//选中的点

        List<SelectedPointStruct> selectedPtlist = new List<SelectedPointStruct>();

        #endregion

        private Annotation annotation;
        private Annotation annotation_max;
        private Annotation annotation_min;

        #endregion

        private ActionType actiontype = ActionType.NoAction;// 观测数据操作类型

        public TChartControl()
        {
            InitializeComponent();
            this.tChart.Width = int.Parse(Math.Round(this.Width * 0.8, 0).ToString());
            this.gridControlObsdata.Width = this.Width - this.tChart.Width;
       
            SetTitle("");
            SetLegendStyle(this.tChart.Legend, LegendStyles.Series);
            SetAxesBottomStyle(this.tChart.Axes.Bottom, null);
            SetAxesLeftStyle(this.tChart.Axes.Left);

            InitCursorTool();
            InitDragMarks();

            InitAnnotations();

            this.tChart.MouseWheel += TChart_MouseWheel; ;//滚轮事件放大chart
        }


        #region 初始化工具
       
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
        /// 释放拖拽工具
        /// </summary>
        private void DisposeDragPtTool()
        {
            if (this.dragPoints != null)
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
        public bool AddSeries(List<LineBean> obsdatalist, string excelPath)
        {

            bool isok = false;
            this.tChart.Header.Text = "";
            DisposeDragPtTool();//释放拖拽工具
            try
            {
                this.tChart.Series.Clear();

                foreach (LineBean checkedLb in obsdatalist)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable(checkedLb.OBSLINECODE, excelPath);

                    DataView dataViewselec = dt.DefaultView;
                    dataViewselec.Sort = "obvdate asc";
                    dt = dataViewselec.ToTable();
                    //构建哈希表key（文件夹+测项名）

                    string[] dirName = excelPath.Split('\\');
                    string DtKey = dirName[dirName.Length - 1] + "," + checkedLb.OBSLINECODE;

                    if (!ObsdataCls.IsExisted(DtKey))
                        ObsdataCls.ObsdataHash.Add(DtKey, dt);

                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb.OBSLINENAME;
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;

                    //DataTable dtsource = ObsdataCls.ObsdataHash[DtKey] as DataTable;
                    //foreach (DataRow dr in dtsource.Rows)
                    //{
                    //    DateTime datetime = DateTime.Parse(dr[0].ToString());
                    //    double d = double.Parse(dr[1].ToString());
                    //    line.Add(datetime, d);
                    //}

                    line.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;

                    /*只有一条曲线时不显示图例*/
                    line.Legend.Visible = true ? obsdatalist.Count > 1 : obsdatalist.Count <= 1;
                    line.Marks.Visible = false;
                    line.Tag = DtKey;
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.GetSeriesMark += Line_GetSeriesMark;
                    if (this.tChart.Header.Text != "")
                        this.tChart.Header.Text += "/";
                    this.tChart.Header.Text += line.Title;

                    if (obsdatalist.Count == 1)
                    {
                        this.gridControlObsdata.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;
                        this.gridControlObsdata.Refresh();

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

        public bool AddSeries(List<string> obsdatalist, string excelPath)
        {

            bool isok = false;
            this.tChart.Header.Text = "";
            DisposeDragPtTool();//释放拖拽工具
            try
            {
                this.tChart.Series.Clear();

                foreach (string checkedLb in obsdatalist)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable(checkedLb, excelPath);
                    DataView dataViewselec = dt.DefaultView;
                    dataViewselec.Sort = "obvdate asc";
                    dt = dataViewselec.ToTable();
                    //构建哈希表key（文件夹+测项名）

                    string[] dirName = excelPath.Split('\\');
                    string DtKey = dirName[dirName.Length - 1] + "," + checkedLb;

                    if (!ObsdataCls.IsExisted(DtKey))
                        ObsdataCls.ObsdataHash.Add(DtKey, dt);

                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = checkedLb;
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;

                    //foreach (DataRow dr in dtsource.Rows)
                    //{
                    //    DateTime datetime = DateTime.Parse(dr[0].ToString());
                    //    double d = (dr[1].ToString() == "NaN"|| dr[1].ToString() == string.Empty) ? double.NaN : double.Parse(dr[1].ToString());
                    //    line.Add(datetime, d);
                    //}

                    line.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;

                    /*只有一条曲线时不显示图例*/
                    line.Legend.Visible = obsdatalist.Count > 1 ? true : false;
                    line.Marks.Visible = false;
                    line.Tag = DtKey;
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.GetSeriesMark += Line_GetSeriesMark;
                    if (this.tChart.Header.Text != "")
                        this.tChart.Header.Text += "/";
                    this.tChart.Header.Text += line.Title;

                    if (obsdatalist.Count == 1)
                    {
                        this.gridControlObsdata.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;
                        this.gridControlObsdata.Refresh();
                    }
                }

                AddVisibleLineVerticalAxis();

                // 
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
        public bool AddSingleSeries(string linename, string Dtkey)
        {
            bool isok = false;
            DisposeDragPtTool();//释放拖拽工具
            try
            {
                this.tChart.Series.Clear();
                Line line = new Line();
                tChart.Series.Add(line);
                line.Title = linename;

                line.XValues.DataMember = "obvdate";
                line.YValues.DataMember = "obvvalue";
                line.XValues.DateTime = true;

                DataTable dtsource = ObsdataCls.ObsdataHash[Dtkey] as DataTable;
                //foreach (DataRow dr in dtsource.Rows)
                //{
                //    DateTime datetime = DateTime.Parse(dr[0].ToString());
                //    double d = (dr[1].ToString() == "NaN" || dr[1].ToString() == string.Empty) ? double.NaN : double.Parse(dr[1].ToString());
                //    line.Add(datetime, d);

                //}
                line.DataSource = dtsource;
                line.Legend.Visible = false;
                line.Marks.Visible = false;
                line.Tag = Dtkey;
                line.MouseEnter += Line_MouseEnter;
                line.MouseLeave += Line_MouseLeave;
                line.GetSeriesMark += Line_GetSeriesMark;
                this.tChart.Header.Text = linename;
                AddVisibleLineVerticalAxis();

                /*只有一条曲线时要显示数据列表*/
                this.gridControlObsdata.DataSource = ObsdataCls.ObsdataHash[Dtkey] as DataTable;
                this.gridControlObsdata.Refresh();

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



            try
            {
                this.dragMarks.Active = true;
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
                    pts.Color = Color.FromArgb(0, 255, 0);
                    foreach (DataRow dr in ((DataTable)ln.DataSource).Rows)
                    {
                        if (dr["note"].ToString() != "")
                        {
                            pts.Add(DateTime.FromOADate(ln[j].X), ln[j].Y);
                        }
                        j++;
                    }

                    this.tChart.Series[0].Marks.Arrow.Color = pts.Color;

                    this.tChart.Series[0].Marks.Arrow.Width = 1;          //标签与单元之间连线的宽度
                    this.tChart.Series[0].Marks.Arrow.Style = System.Drawing.Drawing2D.DashStyle.DashDot;       //标签与单元之间连线样式
                    this.tChart.Series[0].Marks.Font.Color = Color.Black;

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

        #region 格网标题热线最大小值

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

            try
            {
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
            catch
            { }
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


        #endregion

        #region 数据处理方法

        /// <summary>
        /// 加减乘除
        /// </summary>
        public void PlusMinusMultiplyDivide()
        {
            DisposeDragPtTool();
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;


            Line ln = this.tChart.Series[0] as Line;

            DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;

            DataProgreeFrm dpf = new DataProgreeFrm(dt.Rows.Count);
            if (dpf.ShowDialog() == DialogResult.OK)
            {
                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[ln.Tag.ToString()] = pralg.PlusMinusMultiplyDivide(dt, dpf.progreeValue, dpf.dpm);
                AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
            }
        }
        /// <summary>
        /// 进入消台阶或消突跳操作
        /// </summary>
        public void RemoStepOrJump(TChartEventType tep)
        {
            DisposeDragPtTool();
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            this.tchartEventType = tep;
            start = new Point();//矩形起点
            end = new Point();//矩形终点
            g = this.tChart.CreateGraphics();

            this.tChart.Cursor = Cursors.Cross;

            InitRemoveJumpORStepPoints();
        }

        private void InitRemoveJumpORStepPoints()
        {
            selectedPtlist.Clear();
            if (RemoveJumpORStepPoints != null)
            {
                RemoveJumpORStepPoints.Clear();
                RemoveJumpORStepPoints = null;
            }
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
            DisposeDragPtTool();
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            Line ln = this.tChart.Series[0] as Line;

            ObslineMergeFrm olmf = new ObslineMergeFrm();
            if (olmf.ShowDialog() == DialogResult.OK)
            {

                DataTable dtone = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                DataTable dttwo = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue,note from t_obsrvtntb where obslinecode = '" + olmf.SelectedObsLineCode + "'");

                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[ln.Tag.ToString()] = pralg.Merge(dtone, dttwo, olmf.MoveToAverage);

                AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
            }
        }
        /// <summary>
        /// 测项拆分
        /// </summary>
        public void LinesBreak(TChartEventType tep)
        {
            DisposeDragPtTool();
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
        /// 等间隔采样
        /// 2017.06.27
        /// 张超
        /// </summary>
        public void IntervalPross()
        {
            try
            {
                if (this.tChart == null)
                    return;
                if (this.tChart.Series.Count == 0)
                    return;

                Line ln = this.tChart.Series[0] as Line;
                DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                IntervalFrm interval = new IntervalFrm();
                int inter = 0;
                if (interval.ShowDialog() == DialogResult.OK)
                {
                    inter = interval.Interval;

                    PriAlgorithmHelper test = new PriAlgorithmHelper();
                    ObsdataCls.ObsdataHash[ln.Tag.ToString()] = test.Interval(dt, inter, 1);
                    AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 保存处理数据
        /// </summary>
        public void SaveHandleData()
        {
            try
            {
                Line ln = this.tChart.Series[0] as Line;
                SaveToManipData stmfrm = new SaveToManipData(ln.Title);
                if (stmfrm.ShowDialog() == DialogResult.OK)
                {

                    DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                    NpoiCreator npcreator = new NpoiCreator();
                    npcreator.TemplateFile = DataFromPath.HandleDataPath;
                    npcreator.NpoiExcel(dt, ln.Title + ".xls", DataFromPath.HandleDataPath + "/" + stmfrm.targitFileName + ".xls");
                }

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
                            DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;

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
                                            ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
                                            /*重画选中的点*/
                                            ReLoadRemoveJumpORStepPointsData(dpf.dataoutsel);
                                            tchartEventType = TChartEventType.NoProg;
                                        }
                                        else
                                        {
                                            selectedPtlist.Clear();
                                            RemoveJumpORStepPoints.Clear();
                                            tChart.Refresh();
                                            g = this.tChart.CreateGraphics();
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
                                            ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
                                            /*重画选中的点*/
                                            ReLoadRemoveJumpORStepPointsData(dpf.dataoutsel);
                                            tchartEventType = TChartEventType.NoProg;
                                        }
                                        else
                                        {
                                            selectedPtlist.Clear();
                                            RemoveJumpORStepPoints.Clear();
                                            tChart.Refresh();
                                            g = this.tChart.CreateGraphics();
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
                                        DataTable dtin = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                                        ObsdataCls.ObsdataHash[ln.Tag.ToString()] = pralg.Split(dtin, selectdt, "");

                                        RemoveJumpORStepPoints.Clear();
                                        /*重画曲线*/
                                        AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());

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


        /// <summary>
        /// 重新加载消突跳台阶选中点数据(用于消突跳、台阶完毕后的重画选中点)
        /// </summary>
        /// <param name="dt"></param>
        private void ReLoadRemoveJumpORStepPointsData(DataTable dt)
        {
            try
            {
                RemoveJumpORStepPoints.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    RemoveJumpORStepPoints.Add(DateTime.Parse(dr[0].ToString()), double.Parse(dr[1].ToString()));
                }
                this.tChart.Series.Add(RemoveJumpORStepPoints);
                this.tChart.Refresh();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }
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
            ln.DataSource = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
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


        private void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            Line line1 = series as Line;
            DataTable ds = (DataTable)line1.DataSource;
            e.MarkText = ds.Rows[e.ValueIndex]["note"].ToString();
        }
        private void TChart_MouseWheel(object sender, MouseEventArgs e)
        {

            try
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
            catch { }

        }

        private void tChart_AfterDraw(object sender, Steema.TeeChart.Drawing.Graphics3D g3d)
        {

            if (RemoveJumpORStepPoints != null)
                /*重新计算SelectedPtlist的屏幕XY值*/
                ReCalculateSelectedPtlist();
            if (this.selectedPtlist.Count < 1)
                return;

            if (this.selectedPtlist.Count == 1)
            {
                Point pt1 = this.selectedPtlist[0].PtElement;
                string text = this.selectedPtlist[0].PtValue.ToString();
                g3d.TextOut(pt1.X, pt1.Y, text);
            }

            /*重新绘制台阶线*/
            for (int i = 1; i < this.selectedPtlist.Count; i++)
            {
                Point pt1 = this.selectedPtlist[i - 1].PtElement;
                Point pt2 = new Point(this.selectedPtlist[i - 1].PtElement.X, this.selectedPtlist[i].PtElement.Y);
                Point pt3 = this.selectedPtlist[i].PtElement;

                g3d.Brush.Color = Color.Gray;
                g3d.Line(pt1, pt2);
                g3d.Line(pt2, pt3);

                string text = Math.Round((this.selectedPtlist[i].PtValue - this.selectedPtlist[i - 1].PtValue), 3).ToString();
                g3d.TextOut(pt1.X, pt3.Y - 12, text);
            }
        }

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

        private void tChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                Line ln = s as Line;
                if (this.tChart.Series.Count > 1)
                    AddSingleSeries(ln.Title, ln.Tag.ToString());
            }
            catch (Exception ex)
            {
                // XtraMessageBox.Show("错误", ex.Message);
            }
        }

        private void tChart_ClickLegend(object sender, MouseEventArgs e)
        {

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

        #region 观测数据的显示、增加、删除、修改


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddData_Click(object sender, EventArgs e)
        {
            Line ln = this.tChart.Series[0] as Line;
            DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
            actiontype = ActionType.Add;
            this.gridViewObsdata.OptionsBehavior.Editable = true;//可编辑
            int focusedRow = this.gridViewObsdata.FocusedRowHandle;
            this.gridViewObsdata.AddNewRow();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            actiontype = ActionType.Delete;
            int focusedRow = this.gridViewObsdata.FocusedRowHandle;
            try
            {
                DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(focusedRow);
                DateTime obsdate = new DateTime();
                DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                double obsv = double.NaN;
                double.TryParse(drv["obvvalue"].ToString(), out obsv);
                DeleteChartlineData(obsdate, obsv);
            }
            catch (Exception ex)
            {
                actiontype = ActionType.NoAction;
                //XtraMessageBox.Show("错误", "删除失败:" + ex.Message);
            }

            gridViewObsdata.DeleteRow(focusedRow);
            gridViewObsdata.UpdateCurrentRow();
        }

        /// <summary>
        /// 启动编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditData_Click(object sender, EventArgs e)
        {
            this.gridViewObsdata.OptionsBehavior.Editable = true;//可编辑
            actiontype = ActionType.Modify;
        }
        /// <summary>
        /// 取消编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            this.gridViewObsdata.OptionsBehavior.Editable = false;
            actiontype = ActionType.NoAction;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                LineObsBll lob = new LineObsBll();
                DataTable dt = (this.gridControlObsdata.DataSource as DataTable).GetChanges();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        /*
                         * 新增
                         */
                        if (dr.RowState == DataRowState.Added)
                        {
                            LineObsBean lobean = new LineObsBean();

                        }

                        /*
                         * 修改
                         */
                        else if (dr.RowState == DataRowState.Modified)
                        {

                        }
                    }
                }
                /*
                 * 删除
                 */
                DataView dv = new DataView((this.gridControlObsdata.DataSource as DataTable), string.Empty, string.Empty, DataViewRowState.Deleted);
                if (dv != null)
                {
                    foreach (DataRow dr in dv.ToTable().Rows)
                    { }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败:" + ex.Message, "错误");
            }
        }



        private void gridViewObsdata_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int focusedRow = e.RowHandle;
                DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(focusedRow);

                switch (actiontype)
                {
                    case ActionType.Modify:
                        {
                            gridViewObsdata.UpdateCurrentRow();
                        }
                        break;
                    case ActionType.Add:
                        {
                            if (drv["obvdate"].ToString() == "" || drv["obvvalue"].ToString() == "")
                                return;

                            DateTime obsdate = new DateTime();
                            DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);

                            double obdv = double.NaN;
                            double.TryParse(drv["obvvalue"].ToString(), out obdv);
                            gridViewObsdata.UpdateCurrentRow();

                            AddChartlineData(obsdate, obdv);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }


        private void gridViewObsdata_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = gridViewObsdata.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        
                        DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(hInfo.RowHandle);
                        DateTime obsdate = new DateTime();
                        DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                        double obsv = double.NaN;
                        double.TryParse(drv["obvvalue"].ToString(), out obsv);

                        InitRemoveJumpORStepPoints();

                        Line ln = tChart.Series[0] as Line;
                        for (int i = 0; i < ln.Count; i++)
                        {
                            if (DateTime.FromOADate(ln[i].X) == obsdate && obsv == ln[i].Y)
                            {
                                int screenX = ln.CalcXPosValue(ln[i].X);
                                int screenY = ln.CalcYPosValue(ln[i].Y);
                                RemoveJumpORStepPoints.Add(DateTime.FromOADate(ln[i].X), ln[i].Y);
                            }
                        }
                        this.tChart.Refresh();

                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }

        private void gridViewObsdata_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (actiontype == ActionType.Modify)
                e.Cancel = false;
            else if (actiontype == ActionType.Add)
            {
                /*
                 * 新增状态下只有新增行可以编辑
                 */
                DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(this.gridViewObsdata.FocusedRowHandle);
                if (drv["obvvalue"].ToString() == "" || drv["obvdate"].ToString() == "")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }



        #endregion

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


        public void GetEqkShowForm()
        {
            InitDragPoints();
            InitDrawLines();
            eqkfrm = new EqkShow(this.tChart.Series[0].Tag, this.tChart, this.dragPoints, this.drawLines);
            eqkfrm.ShowDialog();
            eqkfrm.Focus();
        }

    }
}
