/***********************************************************/
//---模    块：NumberHelper静态类
//---功能描述：数字的一些操作（取有效数字、取指定最大小数位数，检验顺序百分位等）
//---编码时间：2008-04-09                
//---编码人员：肖如林
//---单    位：LREIS
/***********************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace xxkUI.Tool
{
    /// <summary>
    /// 保存制定有效数字类
    /// </summary>
    public class NumberHelper
    {
        /// <summary>
        /// 将一个数字转换为自定制定有效位数的数字
        /// </summary>
        /// <param name="dInputValue">输入的数字</param>
        /// <param name="iValidPrecision">要求的有效数字位数</param>
        /// <returns>成功返回对应的有效位数的数字，否则返回原值</returns>
        public static double GetSpecifiedPrecisionValue(double dInputValue, int iValidPrecision)
        {
            double dResult = dInputValue;
            try
            {
                if (iValidPrecision > 0)
                {
                    bool bNegative = false;
                    if (dInputValue < 0)
                    {
                        bNegative = true;
                    }

                    double dTemp = Math.Abs(dInputValue);

                    string sValue = dTemp.ToString("0.000000000000000");
                    int iPos = sValue.IndexOf(".");
                    //有效位计算在小数点后
                    if (iPos < iValidPrecision)
                    {
                        string sTempValue = sValue.Replace(".", "");
                        int iCount = 0;
                        bool bStarted = false;
                        int i = 0;
                        for (i = 0; i < sTempValue.Length && iCount < iValidPrecision; i++)
                        {
                            if (bStarted == false)
                            {
                                if (sTempValue[i] != '0')
                                {
                                    bStarted = true;
                                    iCount++;
                                }
                            }
                            else
                            {
                                iCount++;
                            }
                        }
                        if (i == sTempValue.Length)
                        {
                            string sResultValue = sTempValue.Substring(0, i);
                            sResultValue = sResultValue.Insert(iPos, ".");
                            dResult = double.Parse(sResultValue);
                        }
                        else
                        {

                            string sResultValue = sTempValue.Substring(0, i);
                            char sRight = sTempValue[i];

                            sResultValue = sResultValue.Insert(iPos, ".");
                            dResult = double.Parse(sResultValue);
                            //四舍五入
                            if (sRight > '4')
                            {
                                int iNumNegative = i - iPos;
                                double dRemains = Math.Pow(0.1, iNumNegative);

                                dResult = dResult + dRemains;

                            }

                            if (bNegative)
                            {
                                dResult = -dResult;
                            }
                        }
                    }
                    //有效位在小数点前或包含小数点
                    else
                    {
                        if (iPos == iValidPrecision)
                        {
                            char cRight = sValue[iPos + 1];

                            sValue = sValue.Substring(0, iPos);
                            dResult = double.Parse(sValue);

                            //四舍五入
                            if (cRight > '4')
                            {
                                dResult = dResult + 1;
                            }
                        }
                        else
                        {
                            sValue = sValue.Substring(0, iPos);
                            dResult = double.Parse(sValue);
                        }

                        if (bNegative)
                        {
                            dResult = -dResult;
                        }
                    }

                }
            }
            catch //(Exception excp)//被pyl注释掉,因为没有用到(Exception excp)
            {
            }
            return dResult;
        }

        /// <summary>
        /// 将一个数字转换为自定制定有效位数的字符
        /// </summary>
        /// <param name="dInputValue">输入的数字</param>
        /// <param name="iValidPrecision">要求的有效数字位数</param>
        /// <returns>成功返回对应的有效位数的数字，否则返回原值</returns>
        public static string GetSpecifiedPrecisionStringValue(double dInputValue, int iValidPrecision)
        {
            double dResultValue = dInputValue;
            string sResultValue = dInputValue.ToString();
            try
            {
                if (iValidPrecision > 0)
                {
                    bool bNegative = false;
                    if (dInputValue < 0)
                    {
                        bNegative = true;
                    }

                    double dTemp = Math.Abs(dInputValue);

                    string sValue = dTemp.ToString("0.000000000000000");
                    int iPos = sValue.IndexOf(".");
                    //有效位计算在小数点后
                    if (iPos < iValidPrecision)
                    {
                        string sTempValue = sValue.Replace(".", "");
                        int iCount = 0;
                        bool bStarted = false;
                        int i = 0;
                        for (i = 0; i < sTempValue.Length && iCount < iValidPrecision; i++)
                        {
                            if (bStarted == false)
                            {
                                if (sTempValue[i] != '0')
                                {
                                    bStarted = true;
                                    iCount++;
                                }
                            }
                            else
                            {
                                iCount++;
                            }
                        }
                        if (i == sTempValue.Length)
                        {
                            sResultValue = sTempValue.Substring(0, i);
                            sResultValue = sResultValue.Insert(iPos, ".");
                        }
                        else
                        {

                            sResultValue = sTempValue.Substring(0, i);
                            char sRight = sTempValue[i];

                            sResultValue = sResultValue.Insert(iPos, ".");
                            dResultValue = double.Parse(sResultValue);
                            //四舍五入
                            if (sRight > '4')
                            {
                                int iNumNegative = i - iPos;
                                double dRemains = Math.Pow(0.1, iNumNegative);
                                dResultValue = dResultValue + dRemains;
                                sResultValue = dResultValue.ToString("0.000000000000000000000000");
                                //去掉字符末尾的0
                                while (sResultValue.EndsWith("0"))
                                {
                                    sResultValue = sResultValue.Substring(0, sResultValue.Length - 1);
                                }
                            }

                            if (bNegative)
                            {
                                sResultValue = "-" + sResultValue;
                            }
                        }
                    }
                    //有效位在小数点前或包含小数点
                    else
                    {
                        if (iPos == iValidPrecision)
                        {
                            char cRight = sValue[iPos + 1];

                            sResultValue = sValue.Substring(0, iPos);
                            dResultValue = double.Parse(sResultValue);

                            //四舍五入
                            if (cRight > '4')
                            {
                                dResultValue = dResultValue + 1;
                                sResultValue = dResultValue.ToString();
                            }
                        }
                        else
                        {
                            sResultValue = sValue.Substring(0, iPos);

                        }

                        if (bNegative)
                        {
                            sResultValue = "-" + sResultValue;
                        }
                    }

                }
            }
            catch
            {
            }
            return sResultValue;
        }

        /// <summary>
        /// 得到指定最大小数位数的数字
        /// </summary>
        /// <param name="dSourceNumberValue">待转换的小数</param>
        /// <param name="iMaxDecimalDig">指定的最大小数位数</param>
        /// <returns>返回满足条件的小数</returns>
        public static double getSpecDecimalDigNumber(double dSourceNumberValue, int iMaxDecimalDig)
        {
            string strSourceNumberValue = Convert.ToString(dSourceNumberValue);
            string strTargetNumberValue = "";
            string strDecimalDigValue = strSourceNumberValue.Substring(strSourceNumberValue.IndexOf('.') + 1, strSourceNumberValue.Length - strSourceNumberValue.IndexOf('.') - 1);

            if (strDecimalDigValue.Length > iMaxDecimalDig)
            {
                string fromatter = "0.";
                for (int i = 0; i < iMaxDecimalDig; i++)
                {
                    fromatter += "0";
                }
                strTargetNumberValue = dSourceNumberValue.ToString(fromatter);
            }
            else
            {
                strTargetNumberValue = strSourceNumberValue;
            }

            return Convert.ToDouble(strTargetNumberValue);
        }


        /// <summary>
        /// 按指定小数位数获取将浮点型数值(小位位数多不处理，少了补零)，加入了对无小数位数的处理。ck
        /// </summary>
        /// <param name="dSourceNumberValue">待转换的小数</param>
        /// <param name="iMaxDecimalDig">指定的返回的小数位数</param>
        /// <returns>返回满足条件的浮点型数值</returns>
        public static string getSpecDecimalDigNumber1(double dSourceNumberValue, int iMaxDecimalDig)
        {
            if (dSourceNumberValue == 0)
            {
                return "0";
            }
            string strSourceNumberValue = Convert.ToString(dSourceNumberValue);
            string strTargetNumberValue = "";

            string[] array = strSourceNumberValue.Split('.');

            int decimalLen;
            if (array.Length < 2)
            {
                decimalLen = 0;
            }
            else
            {
                decimalLen = array[1].Length;
            }


            if (decimalLen < iMaxDecimalDig)
            {
                string fromatter = "0.";
                for (int i = 0; i < iMaxDecimalDig; i++)
                {
                    fromatter += "0";
                }
                strTargetNumberValue = dSourceNumberValue.ToString(fromatter);
            }
            else
            {
                strTargetNumberValue = strSourceNumberValue;
            }

            return strTargetNumberValue;
        }

        /// <summary>
        /// 按指定小数位数获取将浮点型数值(小位位数四舍五入，少了补零)，加入了对无小数位数的处理。ck
        /// </summary>
        /// <param name="dSourceNumberValue">待转换的小数</param>
        /// <param name="iMaxDecimalDig">指定的返回的小数位数</param>
        /// <returns>返回满足条件的浮点型数值</returns>
        public static string getSpecDecimalDigNumber2(double dSourceNumberValue, int iMaxDecimalDig)
        {
            if (dSourceNumberValue == 0)
            {
                return "0";
            }

            string strSourceNumberValue = Convert.ToString(dSourceNumberValue);
            string strTargetNumberValue = "";

            string[] array = strSourceNumberValue.Split('.');

            int decimalLen;
            if (array.Length < 2)
            {
                decimalLen = 0;
            }
            else
            {
                decimalLen = array[1].Length;
            }


            if (decimalLen < iMaxDecimalDig)//小数位数小于要求的补零
            {
                string fromatter = "0.";
                for (int i = 0; i < iMaxDecimalDig; i++)
                {
                    fromatter += "0";
                }
                strTargetNumberValue = dSourceNumberValue.ToString(fromatter);
            }
            else//小数位数大于等于要求的补零
            {
                if (iMaxDecimalDig >= 0)
                {
                    strTargetNumberValue = double.Parse(strSourceNumberValue).ToString("f" + iMaxDecimalDig);
                }
            }

            return strTargetNumberValue;
        }


        /// <summary>
        /// 按指定小数位数获取将浮点型数值(小位位数去尾，少了补零)，加入了对无小数位数的处理。ck
        /// 修改了getSpecDecimalDigNumber2的一个bug（只能保留两位小数）
        /// </summary>
        /// <param name="dSourceNumberValue">待转换的小数</param>
        /// <param name="iMaxDecimalDig">指定的返回的小数位数</param>
        /// <returns>返回满足条件的浮点型数值</returns>
        public static string getSpecDecimalDigNumber3(double dSourceNumberValue, int iMaxDecimalDig)
        {
            if (dSourceNumberValue == 0)
            {
                return "0";
            }

            string strSourceNumberValue = Convert.ToString(dSourceNumberValue);
            string strTargetNumberValue = "";

            string[] array = strSourceNumberValue.Split('.');

            int decimalLen;
            if (array.Length < 2)
            {
                decimalLen = 0;
            }
            else
            {
                decimalLen = array[1].Length;
            }


            if (decimalLen < iMaxDecimalDig)//小数位数小于要求的补零
            {
                string fromatter = "0.";
                for (int i = 0; i < iMaxDecimalDig; i++)
                {
                    fromatter += "0";
                }
                strTargetNumberValue = dSourceNumberValue.ToString(fromatter);
            }
            else//小数位数大于等于要求的去尾
            {
                if (iMaxDecimalDig >= 0)
                {
                    if (iMaxDecimalDig == 0)
                    {
                        return array[0];
                    }

                    string intPart = array[0];
                    string pointPart = array[1].Substring(0, iMaxDecimalDig);

                    strTargetNumberValue = intPart + "." + pointPart;
                }
            }

            return strTargetNumberValue;
        }

        /// <summary>
        /// 按指定小数位数获取将浮点型数值(小位位数四舍五入，少了补零)，加入了对无小数位数的处理。ck
        /// </summary>
        /// <param name="dSourceNumberValue">待转换的小数</param>
        /// <param name="iMaxDecimalDig">指定的返回的小数位数</param>
        /// <returns>返回满足条件的浮点型数值</returns>
        public static double getSpecDecimalDigNumber4(double dSourceNumberValue, int iMaxDecimalDig)
        {
            return double.Parse(getSpecDecimalDigNumber2(dSourceNumberValue, iMaxDecimalDig));
        }
        /// <summary>
        /// 得到double类型的正确字符型
        /// </summary>
        /// <param name="dblValue"></param>
        /// <returns></returns>
        public static string getValidStringFromDouble(double dblValue)
        {
            //得到按照指定格式化的字符
            string strValue = dblValue.ToString("0.000000000000");
            //去掉字符末尾的0
            while (strValue.EndsWith("0"))
            {
                strValue = strValue.Substring(0, strValue.Length - 1);
            }
            return strValue;

        }

        /// <summary>
        /// 检查顺序百分位值是否有效：为递增序列，值域范围在0-1，不包含0和1
        /// </summary>
        /// <param name="dPercentTileValues">进行检验的百分位小数值</param>
        /// <returns>成功返回valid，失败返回具体无效原因</returns>
        public static string checkTheValidOfPercentValueList(double[] dPercentTileValues)
        {
            string result = "valid";
            //检验是否在0-1之间
            for (int i = 1; i < dPercentTileValues.Length; i++)
            {
                if (dPercentTileValues[i] <= 0)
                {
                    result = "百分位必须为正数";
                    break;
                }
                if (dPercentTileValues[i] > 1)
                {
                    result = "百分位不能超过100";
                    break;
                }
            }

            if (result == "valid")
            {
                //检验是否递增
                double preValue = dPercentTileValues[0];
                for (int i = 1; i < dPercentTileValues.Length; i++)
                {
                    if (dPercentTileValues[i] <= preValue)
                    {
                        result = "顺序百分位必须为递增序列";
                        break;
                    }
                    else
                    {
                        preValue = dPercentTileValues[i];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 得到数字的中文写法,如2返回二.
        /// </summary>
        /// <param name="numberStr"></param>
        /// <returns></returns>
        public static string GetTheCaptionOfNumber(string numberStr)
        {
            numberStr = numberStr.Replace("0", "零").Replace("1", "一").Replace("2", "二").Replace("3", "三").Replace("4", "四").Replace("5", "五").Replace("6", "六").Replace("7", "七").Replace("8", "八").Replace("9", "九");
            return numberStr;
        }
        /// <summary>
        /// 得到指定小数位数的0
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string getSpecialZero(int length)
        {
            string result = "0.";
            for (int i = 0; i < length; i++)
            {
                result += "0";
            }
            return result;
        }


        public static bool isFloat(string str)
        {

            try
            {
                float f = float.Parse(str);
            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool isDouble(string str)
        {

            try
            {
                double f = double.Parse(str);

                if (double.IsNaN(f))
                {
                    return false;
                }
            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断是否为浮点型，并且小数位数是否正确
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pointNum">为-1，不验证小数位数</param>
        /// <returns></returns>
        public static bool isDouble(string str, int pointNum)
        {
            try
            {
                double f = double.Parse(str);

                if (double.IsNaN(f))
                {
                    return false;
                }

                if (pointNum == -1 || f == 0)
                {
                    return true;
                }

                if (str.Split('.').Length == 2)
                {
                    if (str.Split('.')[1].Length == pointNum)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool isInt(string str)
        {

            try
            {
                int i = int.Parse(str);

            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

    }
}
