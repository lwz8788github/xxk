/***********************************************************/
//---模    块：回收站一系列操作
//---功能描述：读取回收站内容，彻底删除，恢复
//---编码时间：2017-5-19             
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace xxkUI.MyCls
{
    public class RecycledHandler
    {
        private string recyclePath = System.Windows.Forms.Application.StartupPath + "/回收站/";


        /// <summary>
        /// 读取回收站文件内容
        /// </summary>
        /// <returns>数据表</returns>
        public DataTable LoadItems()
        {
            DataTable dt = null;


            return dt;
        }


        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="sitcode">场地编码</param>
        /// <returns>是否恢复成功</returns>
        public bool Recoverty(string sitcode)
        {
            bool isSucceed = false;

            return isSucceed;
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <returns></returns>
        public bool CompleteDelete()
        {
            bool isSucceed = false;

            return isSucceed;
        }

        /// <summary>
        /// 删除某一项
        /// </summary>
        /// <param name="fileToMove"></param>
        /// <returns></returns>
        public bool DeleteItems(string fileToMove)
        {
            bool isSucceed = false;

            try
            {
                string fileNewDestination = recyclePath + CreateTimeStr() + "_" + Path.GetFileName(fileToMove);

                if (File.Exists(fileToMove) && !File.Exists(fileNewDestination))
                {
                    File.Move(fileToMove, fileNewDestination);
                    isSucceed = true;
                }
            }
            catch(Exception ex)
            {
                isSucceed = false;
                throw new Exception("删除失败!");
            }


            return isSucceed;
        }

        /// <summary>
        /// 清空回收站
        /// </summary>
        /// <returns></returns>
        public bool ClearRecycle()
        {
            bool isSucceed = false;

            return isSucceed;
        }

        /// <summary>
        /// 创建当前时间字符串(构建删除文件名用)
        /// </summary>
        /// <returns></returns>
        private string CreateTimeStr()
        {
            string zipname = "";

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            if (month.Length < 2) month = "0" + month;
            if (day.Length < 2) day = "0" + day;
            if (hour.Length < 2) hour = "0" + hour;
            if (minute.Length < 2) minute = "0" + minute;
            if (second.Length < 2) second = "0" + second;

            zipname = year + month + day + hour + minute + second;

            return zipname;
        }
    }
}
