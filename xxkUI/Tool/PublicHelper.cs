using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
    public class PublicHelper
    {
        /// <summary>
        /// 数据树结点信息
        /// </summary>
        public struct TreeListItemInfo
        {
            /// <summary>
            /// 结点名称
            /// </summary>
            public string itemName;
            /// <summary>
            /// 结点对应数据的类型,可为：table, comtable,folder等
            /// </summary>
            public string destType;
            /// <summary>
            /// 结点显示名称
            /// </summary>
            public string itemText;
            /// <summary>
            /// item的层次(在第几层结点)
            /// </summary>
            public int itemLevel;
            /// <summary>
            /// 正常图标索引
            /// </summary>
            public int imageIndex;
            /// <summary>
            /// 错误图标索引
            /// </summary>
            public int errorImageIndex;
            /// <summary>
            /// 数据信息表，刘文龙
            /// </summary>
            public string infoTblName;
        }


        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit CreateLookUpEdit(string[] values)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();

            DataTable dtTmp = new DataTable();
            dtTmp.Columns.Add("请选择");

            for (int i = 0; i < values.Length; i++)
            {
                DataRow drTmp1 = dtTmp.NewRow();
                drTmp1[0] = values[i];
                dtTmp.Rows.Add(drTmp1);
            }

            rEdit.DataSource = dtTmp;

            rEdit.ValueMember = "请选择";
            rEdit.DisplayMember = "请选择";
            rEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
            rEdit.ShowFooter = false;
            rEdit.ShowHeader = false;
            return rEdit;
        }


        /// <summary>
        /// 创建当前时间字符串(构建删除文件名用)
        /// </summary>
        /// <returns></returns>
        public string CreateTimeStr()
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

    /// <summary>
    /// 数据操作分类
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 0,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Modify = 2,
        /// <summary>
        /// 无动做
        /// </summary>
        NoAction = 3
    }

    /// <summary>
    /// Series类型分类
    /// </summary>
    public enum SereisType
    {
        /// <summary>
        /// 曲线类型
        /// </summary>
        LineSeries = 0,
        /// <summary>
        /// 散点类型
        /// </summary>
        PointsSeries = 1,
        /// <summary>
        /// 未知类型
        /// </summary>
        UnknownSeris = 3
    }

    /// <summary>
    /// 导出的图片格式分类
    /// </summary>
    public enum ChartToImgType
    {
        jpg = 0,
        bmp = 1,
        png = 2
    }

    public enum LogType
    {
        Common, Right, Warning, Error
    }

    /// <summary>
    /// 数据源分类：原始信息库、本地信息库、处理数据
    /// </summary>
    public enum DataFromType
    {
        OrigDb,LocalDb, HandleData
    }


    /// <summary>
    /// 数据处理方法分类
    /// </summary>
    public enum DataProessMethod
    {
        /// <summary>
        /// 加
        /// </summary>
        Plus,
        /// <summary>
        /// 减
        /// </summary>
        Minus,
        /// <summary>
        /// 乘
        /// </summary>
        Multiply,
        /// <summary>
        /// 除
        /// </summary>
        Divide,
        /// <summary>
        /// 无操作
        /// </summary>
        NoProg
    }
    public static class DataFromPath
    {
        public static string RemoteDbPath = System.Windows.Forms.Application.StartupPath + "//远程信息库缓存";
        public static string LocalDbPath = System.Windows.Forms.Application.StartupPath + "//本地信息库缓存";
        public static string HandleDataPath = System.Windows.Forms.Application.StartupPath + "//处理数据缓存";
      
    }
}
