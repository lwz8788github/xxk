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
using xxkUI.Form;

namespace xxkUI.MyCls
{
    public class MyTeeChart:TChart
    {
        private TChart tChart;
        private ObsData obsfrm = new ObsData();
        /// <summary>
        /// 是否显示备注
        /// </summary>
        public bool IsShowNote{get;set;}
    

        public MyTeeChart(GroupBox gb)
        {
            this.tChart = new TChart();

  this.tChart.Aspect.View3D = false;
            this.tChart.Series.Clear();


            SetTitle("");
            SetLegendStyle(this.tChart.Legend, LegendStyles.Series);
            SetAxesBottomStyle(this.tChart.Axes.Bottom);
            SetAxesLeftStyle(this.tChart.Axes.Left);

            gb.Controls.Add(this.tChart);

            this.tChart.ClickSeries += TChart_ClickSeries;
            this.tChart.ClickLegend += TChart_ClickLegend;
         
            IsShowNote = false;
       

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


                    this.tChart.Header.Text = line.Title;
                }


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
        /// 添加备注图形
        /// </summary>
        public void ShowNoteGraphic()
        {
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                if (tChart.Series[i].Visible)
                {
                    DataTable dtsourec = tChart.Series[i].DataSource as DataTable;


                    Steema.TeeChart.Styles.Line ln = tChart.Series[i] as Steema.TeeChart.Styles.Line;
                    DataTable datasource = ln.DataSource as DataTable;



                    for (int j = 0; j < ln.Count; j++)
                    {
                        int screenX = tChart.Series[i].CalcXPosValue(ln[j].X);
                        int screenY = tChart.Series[i].CalcYPosValue(ln[j].Y);

                        if (datasource.Rows[j]["备注"] != "")
                        {
                          
                        
                            Graphics g = tChart.CreateGraphics();
                            Brush bs = new SolidBrush(Color.Green);
                            Rectangle r = new Rectangle(screenX - 4, screenY - 4, 5, 5);//标识圆的大
                            g.DrawEllipse(new Pen(Color.Red), r);
                            g.FillRectangle(bs, r);
                        }
                    }


                }
            }
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



            double single = (100 - space * (count + 2)) / (count+1);//单个坐标轴的百分比
      
            tChart.Axes.Left.StartPosition = space;
            tChart.Axes.Left.EndPosition = tChart.Axes.Left.StartPosition + single;
            tChart.Axes.Left.StartEndPositionUnits = PositionUnits.Percent;
            listBaseLine[0].CustomVertAxis = tChart.Axes.Left;


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
             
                axis.EndPosition = axis.StartPosition + singleAxisLengthPercent;

                //设置纵轴刻度的颜色
                axis.Labels.Font.Color = Color.Red;

                //设置网格的可见性以及颜色
                axis.Grid.Visible = true;// VisibleSettings.Default.Grid;
                axis.Grid.Color = Color.Red;


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



    }



}
