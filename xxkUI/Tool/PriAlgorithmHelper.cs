/***********************************************************/
//---模    块：曲线处理方法类
//---功能描述：加减乘除、消突跳、消台阶
//---编码时间：2017-05-24
//---编码人员：张超
//---单    位：一测中心
/***********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using xxkUI.Model;

namespace xxkUI.Tool
{
public class PriAlgorithmHelper
    {
      
        public DataTable PlusMinusMultiplyDivide(DataTable dataIn, double dat, DataProcessMethod oper)
        {
            switch (oper)
            {
                case DataProcessMethod.Plus://加
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) + dat;
                    }
                    break;
                case DataProcessMethod.Minus://减
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) - dat;
                    }
                    break;
                case DataProcessMethod.Multiply://乘
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) * dat;
                    }
                    break;
                case DataProcessMethod.Divide://除
                    if (dat == 0) break;
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) / dat;
                    }
                    break;
                default:

                    break;
            }
            return dataIn;
        }

        /// <summary>
        /// 设置全局变量
        /// </summary>
        public int n_left = 0;//左侧总数(消突跳）
        public int n_right = 0;//右侧总数(消突跳） 
        public int index_left=0 ,index_right = 0;//(消突跳左右索引）

        public double ave_left = 0;//左侧平均值        
        public double ave_right = 0;//右侧平均值

        public double m_offset;//偏移量;

        public double left_ave = 0;//移动台阶左侧(偏移%f)
        public double right_ave = 0;//移动台阶右侧(偏移%f)
        
        //消台阶
        public int nLeftStep = 0;
        public int nRightStep = 0;

        private int TipLeftIndex = 0;
        private int TipRightIndex = 0;
        private string Tipstr = "";

        Left_Right change;

        /// <summary>
        ///为未初始化消突跳或台阶做预处理
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="datasel"></param>
        /// <param name="oper"></param>
        /// <param name="Num_Sample">采样数</param>
        /// <returns></returns>
        public RemoveTipBean RemoveStepJumpTip(DataTable dataIn, DataTable datasel, DataProcessMethod oper, int Num_Sample, Left_Right LRB)
        {
            RemoveTipBean rt = new RemoveTipBean();
           // Num_Sample = 5;
            try
            {
                DateTime startSel = DateTime.Parse(datasel.Rows[0][0].ToString());
                DateTime endSel = DateTime.Parse(datasel.Rows[datasel.Rows.Count - 1][0].ToString());
                double val_left = double.Parse(datasel.Rows[0][1].ToString());
                double val_right = double.Parse(datasel.Rows[datasel.Rows.Count - 1][1].ToString());
                int tatal = dataIn.Rows.Count;
                int select = datasel.Rows.Count;//选择数据总数 
                                                /*获取相邻观测值差的绝对值最大的两个值对应的序列号*/
                int index_stepL = 0, index_stepR = 0;
                for (int i = 0; i < select - 1; i++)
                {
                    double biggest = 0;
                    double step = System.Math.Abs(double.Parse(datasel.Rows[i][1].ToString()) - double.Parse(datasel.Rows[i + 1][1].ToString()));
                    if (step > biggest)
                    {
                        index_stepL = i;
                        index_stepR = i + 1;
                        biggest = step;
                    }                    
                   
                }                
                int index = 0;
                foreach(DataRow dr in dataIn.Rows)
                {
                    if (DateTime.Parse(dr[0].ToString()).CompareTo(startSel) == 0)
                    {
                        index_left = index;
                    }
                    if (DateTime.Parse(dr[0].ToString()).CompareTo(endSel) == 0)
                    {
                        index_right = index;
                        break;
                    }
                    index++;
                }                
                //消台阶或消突跳
                switch (oper)
                {
                    case DataProcessMethod.RemoveJump: //消突跳
                        {
                            foreach (DataRow dr in dataIn.Rows)
                            {
                                DateTime t = DateTime.Parse(dr[0].ToString());
                                if (t.CompareTo(startSel) < 0)
                                {
                                    n_left++;
                                }                              
                                if ((t.CompareTo(endSel) > 0) && (n_right < Num_Sample))
                                {
                                    n_right++;
                                    ave_right += double.Parse(dr[1].ToString());
                                }                               
                            }
                            ave_left = double.Parse(dataIn.Rows[n_left][1].ToString());
                            double cal = 0;
                            int n_cal = 0;
                            int Sample = Num_Sample;
                            if (n_left > 0)
                            {
                                for (int i = n_left - 1; i >= 0; i--)
                                {
                                    if (Sample == 0)
                                    {
                                        break;
                                    }
                                    cal += double.Parse(dataIn.Rows[i][1].ToString());
                                    n_cal++;
                                    Sample--;
                                }
                            }                            
                            if (n_cal > 0) ave_left = cal / n_cal;
                            
                            ave_right = ave_right / n_right;

                            //移动台阶左侧(偏移%f)
                            double left_ave = ave_left;
                            //移动台阶右侧(偏移%f)
                            double right_ave = ave_right;

                            //偏移量选择
                            double var = 0;
                            switch (LRB)
                            {
                                case Left_Right.left:
                                    var = ave_left - val_left;
                                    break;
                                case Left_Right.right:
                                    var = ave_right - val_right;
                                    break;
                                case Left_Right.both:
                                    var = (ave_left + ave_right) / 2 - (val_left + val_right) / 2;
                                    break;
                                default:
                                    break;
                            }

                            m_offset = var;

                            //实例化提示信息
                            rt.LeftAve = "左侧采样平均值（" + Convert.ToString(ave_left) + "）";
                            rt.RightAve = "右侧采样平均值（" + Convert.ToString(ave_right) + "）";
                            rt.BothAve = "两侧采样平均值（" + Convert.ToString((ave_left + ave_right) / 2) + "）";
                            rt.TargetNum = "待处理目标数量：" + Convert.ToString(select) + "）";
                            rt.Offset = m_offset;//偏移量

                            rt.Tip = "[突跳消除@" + DateTime.Now.ToShortDateString() + "]\r\n偏移值：" + m_offset.ToString() + "\r\n偏移区间：" + startSel.ToShortDateString() + "-" + endSel.ToShortDateString();
                            TipLeftIndex = n_left;

                            TipRightIndex = n_left + datasel.Rows.Count-1;
                            Tipstr = rt.Tip;
                        }
                        break;
                    case DataProcessMethod.RemoveStep:
                        DateTime dateL = DateTime.Parse(datasel.Rows[index_stepL][0].ToString());
                        DateTime dateR = DateTime.Parse(datasel.Rows[index_stepR][0].ToString());
                        //计算消除左台阶起算索引
                        nLeftStep = index_stepL + index_left;//左
                        nRightStep = nLeftStep + 1;//消除右侧台阶起算索引

                        //计算左侧平均偏移量
                        int nL = 0;
                        double totalright = 0;
                        foreach (DataRow dr in dataIn.Rows)
                        {
                            if (DateTime.Parse(dr[0].ToString()).CompareTo(dateR) >= 0 && nL < Num_Sample)
                            {
                                nL++;
                                totalright += double.Parse(dr[1].ToString());
                            }
                        }
                        ave_left = totalright / nL;
                        //计算右侧平均偏移量
                        int nR = 0;
                        double totalleft = 0;
                        int RSample = Num_Sample;
                        for (int i = nLeftStep; i >= 0; i--)
                        {
                            if (RSample == 0)
                            {
                                break;
                            }
                            totalleft += double.Parse(dataIn.Rows[i][1].ToString());
                            nR++;
                            RSample--;
                        }

                        ave_right = totalleft / nR;
                                              

                        //偏移量
                        m_offset = (LRB == Left_Right.left) ? (ave_left - ave_right) : (ave_right - ave_left);
                      
                        //实例化提示语句
                        rt.LeftAve = "消除左侧(平均值" + Convert.ToString(ave_left) + ")";
                        rt.RightAve = "消除右侧(平均值" + Convert.ToString(ave_right) + ")";
                        rt.Totalright = dataIn.Rows.Count - (index_stepL + index_left+1+1);
                        rt.Totaleft = index_stepL + index_left;
                        rt.Offset = m_offset;
                        rt.Tip = "[台阶消除@" + DateTime.Now.ToShortDateString() + "]\r\n偏移值：" + m_offset.ToString();

                        TipLeftIndex = nLeftStep;
                        TipRightIndex = nLeftStep + datasel.Rows.Count-1;
                        Tipstr = rt.Tip;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                rt = null;
                throw new Exception(ex.Message);
            }
            return rt;
        }
        /// <summary>
        /// 根据用户选择进行数据处理（消突跳点或消台阶）
        /// 调用此功能前必须先调用此函数：
        /// RemoveStepJumpTip(DataTable dataIn, DataTable datasel, DataProcessMethod oper)
        /// </summary>
        /// <param name="dataIn"></param>
        /// <param name="datasel"></param>
        /// <param name="oper"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public DataTable RemoveStepJump(DataTable dataIn, DataTable datasel, DataProcessMethod oper, Left_Right select,bool  AddNote,out DataTable Points)
        {

            DataView dataView = dataIn.DefaultView;
            dataView.Sort = "obvdate asc";
            dataIn = dataView.ToTable();

            DataView dataViewselec = datasel.DefaultView;
            dataViewselec.Sort = "obvdate asc";
            datasel = dataViewselec.ToTable();

            Points = dataIn.Clone();

            switch (oper)
            {
                case DataProcessMethod.RemoveStep://消台阶
                    if (select == Left_Right.left)//消左边台阶
                    {
                        for (int i = dataIn.Rows.Count - 1; i > nLeftStep; i--)
                        {
                            double d = double.Parse(dataIn.Rows[i][1].ToString());
                            d = d + m_offset;
                            dataIn.Rows[i][1] = Math.Round(d, 2);
                        }
                    }
                    else if (select == Left_Right.right)//消右边台阶
                    {
                        for (int i = nRightStep ; i < dataIn.Rows.Count; i++)
                        {
                            double d = double.Parse(dataIn.Rows[i][1].ToString());
                            d = d + m_offset;
                            dataIn.Rows[i][1] = Math.Round(d, 2);
                        }
                    }
                    for (int i = index_left; i <= index_right; i++)
                    {
                        DataRow dr = Points.NewRow();
                        dr[0] = dataIn.Rows[i][0];
                        dr[1] = dataIn.Rows[i][1];
                        Points.Rows.Add(dr);
                    }
                    break;
                case DataProcessMethod.RemoveJump://消突跳
                    for (int i = index_left; i <= index_right; i++)
                    {
                        double d = double.Parse(dataIn.Rows[i][1].ToString());
                        d = d + m_offset;
                        dataIn.Rows[i][1] = Math.Round(d, 2);

                        DataRow dr = Points.NewRow();
                        dr[0] = dataIn.Rows[i][0];
                        dr[1] = Math.Round(d, 2);
                        Points.Rows.Add(dr);
                    }
                    break;
                default:
                    break;
            }


            if (AddNote)
            {
                if (select == Left_Right.left || select == Left_Right.both)
                {
                    dataIn.Rows[TipLeftIndex][2] = Tipstr + "\r\n" + dataIn.Rows[TipLeftIndex][2].ToString();
                }
                else if (select == Left_Right.right)
                {
                    dataIn.Rows[TipRightIndex][2] = Tipstr + "\r\n" + dataIn.Rows[TipRightIndex][2].ToString();
                }
               
            }

            DataView dvover = dataIn.DefaultView;
            dvover.Sort = "obvdate asc";
            dataIn = dvover.ToTable();
            return dataIn;
        }


        /// <summary>
        /// 测项合并
        /// </summary>
        /// <param name="dataOne">显示的数据</param>
        /// <param name="dataTwo">合并的数据</param>
        /// <param name="choose">是否移动到平均值处</param>
        /// <param name="field">排序字段,一般为时间</param>
        /// <returns>处理结果</returns>
        public DataTable Merge(DataTable dataOne,DataTable dataTwo,bool choose)
        {
            try
            {
                if (dataOne.Rows.Count == 0 || dataTwo.Rows.Count == 0)
                {
                    return null;
                }
                //起止时间
                DateTime SartOne = DateTime.Parse(dataOne.Rows[0][0].ToString());
                DateTime EndOne = DateTime.Parse(dataOne.Rows[0][0].ToString());
                DateTime SartTwo = DateTime.Parse(dataTwo.Rows[0][0].ToString());
                DateTime EndTwo = DateTime.Parse(dataTwo.Rows[0][0].ToString());

                double ave_One = 0, ave_Two = 0;
                double n_One = 0, n_Two = 0;
                double offset = 0;
                if (choose)
                {
                    foreach (DataRow dr in dataOne.Rows)
                    {
                        ave_One += double.Parse(dr[1].ToString());
                        n_One += 1;
                    }
                    ave_One = ave_One / n_One;
                    foreach (DataRow dr in dataTwo.Rows)
                    {
                        ave_Two += double.Parse(dr[1].ToString());
                        n_Two += 1;
                    }
                    ave_Two = ave_Two / n_Two;
                    offset = ave_One - ave_Two;
                }

                if (System.Math.Abs(EndOne.Year - SartOne.Year) >= 1)
                {
                    DataRow dr = dataOne.NewRow();
                    dr[0]=dr[1]=DBNull.Value;
                    dataOne.Rows.Add(dr);
                }

                for (int i = 0; i < dataTwo.Rows.Count; i++)
                {                    
                    double d = double.Parse(dataTwo.Rows[i][1].ToString());
                    d = d + offset;
                    dataTwo.Rows[i][1] = d;

                    DataRow dr = dataOne.NewRow();
                    for (int j = 0; j < dataOne.Columns.Count; j++)
                    {
                        dr[j] = dataTwo.Rows[i][j];
                    }
                    dataOne.Rows.Add(dr);
                }


                DataView dataView = dataOne.DefaultView;
                dataView.Sort = "obvdate asc";
                dataOne = dataView.ToTable();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           

            return dataOne;
        }
        /// <summary>
        /// 测项拆分
        /// </summary>
        /// <param name="dataIn"><图上显示数据/param>
        /// <param name="datasel"><图上框选数据/param>
        /// <param name="SiteName"><测站名称，后期可更换为图上显示数据产地信息类或表/param>
        /// <returns></returns>
        public DataTable Split(DataTable dataIn, DataTable datasel, string SiteName)
        {
            try
            {
                //DataTable rt = new DataTable();
                if (datasel.Rows.Count == 0)
                {
                    MessageBox.Show("请选择要拆分的数据！");
                    return null;
                }
                if (SiteName == null)
                {
                    MessageBox.Show("找不到隶属场地！");
                    return null;
                }
                DialogResult res = MessageBox.Show("是否将数据从测项中剔除？", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                bool divide = false;
                if (res == DialogResult.Yes)
                {
                    divide = true;
                }
                if (divide)
                {
                    //按时间排序
                    dataIn.DefaultView.Sort = "obvdate ASC";
                    dataIn = dataIn.DefaultView.ToTable();

                    //计算观测周期
                    DateTime InStart = DateTime.Parse(dataIn.Rows[0][0].ToString());
                    DateTime InEnd = DateTime.Parse(dataIn.Rows[dataIn.Rows.Count - 1][0].ToString());
                    TimeSpan ts = InEnd - InStart;
                    int days = ts.Days;
                    double obsRecycle = double.Parse((days / dataIn.Rows.Count).ToString());

                    DateTime SelStart = DateTime.Parse(datasel.Rows[0][0].ToString());
                    DateTime SelEnd = DateTime.Parse(datasel.Rows[datasel.Rows.Count - 1][0].ToString());
                    TimeSpan selectts = SelEnd - SelStart;
                    int selectdays = selectts.Days;

                    int lenSel = datasel.Rows.Count;
                    int StartSplit = 0;
                    int EndSplit = 0;
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        DateTime d = DateTime.Parse(dr[0].ToString());
                        if (d.CompareTo(SelStart) == 0)
                        {
                            break;
                        }
                        StartSplit++;
                    }
                    EndSplit = StartSplit + datasel.Rows.Count;
                    for (int i = StartSplit; i < EndSplit; i++)
                    {

                        if (selectdays >= obsRecycle * 2)
                        {
                            dataIn.Rows[i][1] = double.NaN;
                        }
                        else
                        {
                            dataIn.Rows[i].Delete();
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dataIn;
        }
        /// <summary>
        /// 等间隔处理
        /// 2017.6.27
        /// 张超
        /// </summary>
        /// <param name="dataIn">输入数据</param>
        /// <param name="dataOut">返回结果</param>
        /// <param name="num">等间隔数（单位：天）</param>
        /// <param name="pro_method">数据处理方法（后续添加）</param>
        /// <returns></returns>
        public DataTable Interval(DataTable dataIn,int num,int pro_method)
        {
            if ( num <= 0 || num > 31)
            {
                MessageBox.Show("请输入正确的间隔天数！");
                return null;
            }

            DataTable res = null;
            DataTable dtPro = null;
            int method = pro_method;
            method = 1;//默认为1

            //观测数据排序
            DataView dataViewselec = dataIn.DefaultView;
            dataViewselec.Sort = "obvdate asc";
            dtPro = dataViewselec.ToTable();

            //转换输入数据
            //根据等间隔值计算需要输出数据的时间节点（即，需要计算的插值点）        
            int n_In = dtPro.Rows.Count;
            ObsPoint[] points = new ObsPoint[n_In];
            for (int i = 0; i < n_In; i++)
            {
                points[i] = new ObsPoint();
                points[i].x = DateTime.Parse(dtPro.Rows[i][0].ToString()).ToOADate();
                points[i].y = double.Parse(dtPro.Rows[i][1].ToString());
            }

            //根据等间隔值计算需要输出数据的时间节点（即，需要计算的插值点）
            List<ObsPoint> rt = new List<ObsPoint>();
            DateTime Last = DateTime.Parse(dtPro.Rows[n_In-1][0].ToString());

            DateTime r = DateTime.Parse(dtPro.Rows[0][0].ToString());
            ObsPoint t1 = new ObsPoint();
            DateTime s = new DateTime();
            t1.x = r.ToOADate();//添加第一个节点
            rt.Add(t1);       
            while(DateTime.Compare(Last, r) > 0)//添加其他节点
            {
                ObsPoint point = new ObsPoint();
                s = r.AddDays(num);
                if (DateTime.Compare(Last, s) < 0)
                {
                    break;
                }
                else
                {
                    point.x = s.ToOADate();
                    rt.Add(point);
                    r = s;
                }               
                
            }
            //将等间隔数据转换为数组模式
            ObsPoint[] rt_points = rt.ToArray();

            switch (method)
            {
                case 1:
                    MathHelper Res = new MathHelper();                    
                    Res.SplineInsertPoint(points,ref rt_points,1);
                    //将返回数据实例化为表
                    res = new DataTable();
                    DataColumn col1 = new DataColumn("obvdate", typeof(DateTime));
                    res.Columns.Add(col1);
                    DataColumn col2 = new DataColumn("obvvalue", typeof(Double));
                    res.Columns.Add(col2);
                    DataColumn col3 = new DataColumn("note", typeof(String));
                    res.Columns.Add(col3);                    
                    int total = rt_points.Length;
                    for(int i = 0; i < total; i++)
                    {
                        DataRow dr = res.NewRow();
                        dr[0] = DateTime.FromOADate(rt_points[i].x);
                        dr[1] = rt_points[i].y;
                        dr[2] = "";
                        res.Rows.Add(dr);
                    }                    
                    break;
                case 2:
                    break;
                default:
                    break;

            }
            return res;
        }

        
    }
}
