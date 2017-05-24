/***********************************************************/
//---模    块：曲线数据处理类
//---功能描述：加减乘除，消突跳，消台阶，测项合并，测项拆分
//---编码时间：2017-05-24                
//---编码人员：张超
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace xxkUI.Tool
{
    public class PriAlgorithmHelper
    {
        public DataTable PlusMinusMultiplyDivide( DataTable dataIn, double dat, DataProessMethod oper)
        {
            switch (oper)
            {
                case  DataProessMethod.Plus://加
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) + dat;
                    }
                    break;
                case DataProessMethod.Minus://减
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) - dat;
                    }
                    break;
                case DataProessMethod.Multiply://乘
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) * dat;
                    }
                    break;
                case DataProessMethod.Divide://除
                    if (dat == 0) break;
                    foreach (DataRow dr in dataIn.Rows)
                    {
                        dr[1] = double.Parse(dr[1].ToString()) / dat;
                    }
                    break;
                default:
                    
                    break;
            }
            foreach (DataRow dr in dataIn.Rows)
            {

            }

            return dataIn;
        }
    }
}
