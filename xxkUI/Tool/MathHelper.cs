﻿/***********************************************************/
//---模    块：数学算法
//---功能描述：①三次样条插值
//---编码时间：2017-06-27
//---编码人员：张超
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
    /// <summary>
    /// 数学方法
    /// </summary>
    class MathHelper
    {
        /// <summary>  
        /// 三次样条插值  
        /// </summary>  
        /// <param name="points">排序好的数</param>  
        /// <param name="xs">需要计算的插值点</param>  
        /// <param name="chf">写1</param>  
        /// <returns>返回计算好的数值</returns> 
        public void SplineInsertPoint(ObsPoint[] points, ref ObsPoint[] rt, int chf)
        {
            int plength = points.Length;
            double[] h = new double[plength];
            double[] f = new double[plength];
            double[] l = new double[plength];
            double[] v = new double[plength];
            double[] g = new double[plength];

            //获取需要计算的插值点
            int l_rt = rt.Length;
            double[] xs = new double[l_rt];
            for(int i = 0; i < l_rt; i++)
            {
                xs[i] = rt[i].x;
            }
            
            //插值
            for (int i = 0; i < plength - 1; i++)
            {
                h[i] = points[i + 1].x - points[i].x;
                f[i] = (points[i + 1].y - points[i].y) / h[i];
            }

            for (int i = 1; i < plength - 1; i++)
            {
                l[i] = h[i] / (h[i - 1] + h[i]);
                v[i] = h[i - 1] / (h[i - 1] + h[i]);
                g[i] = 3 * (l[i] * f[i - 1] + v[i] * f[i]);
            }

            double[] b = new double[plength];
            double[] tem = new double[plength];
            double[] m = new double[plength];
            double f0 = (points[0].y - points[1].y) / (points[0].x - points[1].x);
            double fn = (points[plength - 1].y - points[plength - 2].y) / (points[plength - 1].x - points[plength - 2].x);

            b[1] = v[1] / 2;
            for (int i = 2; i < plength - 2; i++)
            {
                b[i] = v[i] / (2 - b[i - 1] * l[i]);
            }
            tem[1] = g[1] / 2;
            for (int i = 2; i < plength - 1; i++)
            {
                tem[i] = (g[i] - l[i] * tem[i - 1]) / (2 - l[i] * b[i - 1]);
            }
            m[plength - 2] = tem[plength - 2];
            for (int i = plength - 3; i > 0; i--)
            {
                m[i] = tem[i] - b[i] * m[i + 1];
            }
            m[0] = 3 * f[0] / 2.0;
            m[plength - 1] = fn;
            int xlength = xs.Length;
            double[] insertRes = new double[xlength];
            for (int i = 0; i < xlength; i++)
            {
                int j = 0;
                for (j = 0; j < plength; j++)
                {
                    if (xs[i] < points[j].x)
                        break;
                }
                j = j - 1;
                Console.WriteLine(j);
                if (j == -1 || j == points.Length - 1)
                {
                    if (j == -1)
                        throw new Exception("插值下边界超出");
                    if (j == points.Length - 1 && xs[i] == points[j].x)
                    {
                        insertRes[i] = points[j].y;
                        rt[i].y = points[j].y;
                    }                        
                    else
                        throw new Exception("插值下边界超出");
                }
                else
                {
                    double p1;
                    p1 = (xs[i] - points[j + 1].x) / (points[j].x - points[j + 1].x);
                    p1 = p1 * p1;
                    double p2; p2 = (xs[i] - points[j].x) / (points[j + 1].x - points[j].x);
                    p2 = p2 * p2;
                    double p3; p3 = p1 * (1 + 2 * (xs[i] - points[j].x) / (points[j + 1].x - points[j].x)) * points[j].y + p2 * (1 + 2 * (xs[i] - points[j + 1].x) / (points[j].x - points[j + 1].x)) * points[j + 1].y;

                    double p4; p4 = p1 * (xs[i] - points[j].x) * m[j] + p2 * (xs[i] - points[j + 1].x) * m[j + 1];
                    p4 = p4 + p3;
                    insertRes[i] = p4;
                    rt[i].y = p4;
                    //rt[i].y = points[j].y;
                }

            }
        }
    }
}
