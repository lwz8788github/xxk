/***********************************************************/
//---模    块：回收站一系列操作
//---功能描述：读取回收站内容，彻底删除，恢复（回收站内文件名格式如下：名称+场地编码+时间）
//---编码时间：2017-5-20           
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using xxkUI.Tool;
using xxkUI.Bll;
using DevExpress.XtraEditors;
using xxkUI.MyCls;

namespace xxkUI.Controls
{
    public partial class RecycleControl : UserControl
    {
     
        public delegate void RefreshTreeHandler(string dbpath);
        public event RefreshTreeHandler RefreshTree;

        private string recyclePath = System.Windows.Forms.Application.StartupPath + "/回收站/";

        public RecycleControl()
        {
            InitializeComponent();
            
            //判断文件夹是否存在
            if (!Directory.Exists(recyclePath))
            {
                //创建文件夹
                Directory.CreateDirectory(recyclePath);
            }
        }





        public void LoadRecycleItems()
        {
            this.gridControlRecycle.DataSource = GetRecycleItems();
        }

        /// <summary>
        /// 清空回收站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(recyclePath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            LoadRecycleItems();
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.gridViewRecycle.GetSelectedRows().Length; i++)
            {
                int rowindex = this.gridViewRecycle.GetSelectedRows()[i];
                DataRow row = gridViewRecycle.GetDataRow(rowindex);

                string datatype = "";
                if (row["RecySite"].ToString() == "远程")
                    datatype = "YC";
                if (row["RecySite"].ToString() == "本地")
                    datatype = "BD";
                if (row["RecySite"].ToString() == "处理")
                    datatype = "CL";

                string filename = row["RecyTime"].ToString()+ datatype + row["RecyName"] + row["RecyFileType"];

                if (File.Exists(recyclePath + filename))
                {
                    File.Delete(recyclePath + filename);
                }
            }

            LoadRecycleItems();
        }

        private void btnRecoverySelected_Click(object sender, EventArgs e)
        {
            string datafile = "";
            try
            {
                for (int i = 0; i < this.gridViewRecycle.GetSelectedRows().Length; i++)
                {
                    int rowindex = this.gridViewRecycle.GetSelectedRows()[i];
                    DataRow row = gridViewRecycle.GetDataRow(rowindex);

                   
                    string datatype = "";
                    if (row["RecySite"].ToString() == "远程")
                    {
                        datafile = DataFromPath.RemoteDbPath;
                        datatype = "YC";
                    }
                    if (row["RecySite"].ToString() == "本地")
                    {
                        datafile = DataFromPath.LocalDbPath;
                        datatype = "BD";
                    }
                    if (row["RecySite"].ToString() == "处理")
                    {
                        datafile = DataFromPath.HandleDataPath;
                        datatype = "CL";
                    }

                    string sourceFilePath = DataFromPath.RecycleDataPath + "//" + row["RecyTime"].ToString() + datatype + row["RecyName"] + row["RecyFileType"];
                    string destFileName = datafile + "//" + row["RecyName"] + row["RecyFileType"];

                    if (File.Exists(destFileName))
                    {
                        if (XtraMessageBox.Show("相同文件已经存在，是否覆盖？如选择不覆盖，请重新选择文件", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            File.Copy(sourceFilePath, destFileName);
                            File.Delete(sourceFilePath);
                        }
                    }
                    else
                    {
                        File.Copy(sourceFilePath, destFileName);
                        File.Delete(sourceFilePath);
                    }
                }

            }
            catch (Exception ex)
            { }

            RefreshTree(datafile);
            LoadRecycleItems();
        }

        private void btnRecoveryRefresh_Click(object sender, EventArgs e)
        {
            LoadRecycleItems();
        }



        /// <summary>
        /// 读取回收站文件内容
        /// </summary>
        /// <returns>数据表</returns>
        public DataTable GetRecycleItems()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RecyName");
            dt.Columns.Add("RecySite");
            dt.Columns.Add("RecyFileType");
            dt.Columns.Add("RecyTime");
            dt.Columns.Add("RecySize");

            DirectoryInfo folder = new DirectoryInfo(recyclePath);

            foreach (FileInfo file in folder.GetFiles("*.*"))
            {
                string filename = Path.GetFileNameWithoutExtension(file.FullName);

                string recyTime = filename.Substring(0, 14);//前14位为时间
                string recyFileType = filename.Substring(14, 2);//时间后面接类型CL、YC、BD（处理、远程、本地）
                if (recyFileType == "YC")
                    recyFileType = "远程";
                if (recyFileType == "BD")
                    recyFileType = "本地";
                if (recyFileType == "CL")
                    recyFileType = "处理";
                string recyName = filename.Substring(16, filename.Length - 16);//类型后面接文件名
                string recySize = (file.Length / 1024).ToString() + "kb";

                DataRow dr = dt.NewRow();
                dr["RecyName"] = recyName;
                dr["RecySite"] = recyFileType;
                dr["RecyFileType"] = file.Extension;
                dr["RecyTime"] = recyTime;
                dr["RecySize"] = recySize;

                dt.Rows.Add(dr);
            }

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
        /// 删除某一项
        /// </summary>
        /// <param name="fileToMove"></param>
        /// <returns></returns>
        public bool DeleteItems(string fileToMove)
        {
            bool isSucceed = false;

            try
            {
                PublicHelper ph = new PublicHelper();
                string fileNewDestination = recyclePath + ph.CreateTimeStr() + "_" + Path.GetFileName(fileToMove);

                if (File.Exists(fileToMove) && !File.Exists(fileNewDestination))
                {
                    File.Move(fileToMove, fileNewDestination);
                    isSucceed = true;
                }
            }
            catch (Exception ex)
            {
                isSucceed = false;
                throw new Exception("删除失败!");
            }


            return isSucceed;
        }

      
    }
}
