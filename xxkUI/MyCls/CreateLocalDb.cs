/***********************************************************/
//---模    块：创建本地数据库
//---功能描述：动态创建本地数据库命令
//---编码时间：2017-5-22
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using Common.Data.MySql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using xxkUI.Tool;

namespace xxkUI.MyCls
{
    public class CreateLocalDb
    {
        public CreateLocalDb()
        {
            MysqlHelper.connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnectWithoutDbname"].ConnectionString;
        }

        public bool CreateDatabase(string dbname)
        {
            bool isok = false;

            try
            {
                string sql = "CREATE DATABASE " + dbname;
                MysqlHelper.ExecuteScalar(sql);
                MysqlHelper.connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
                isok = true;
            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 创建场地信息表
        /// </summary>
        /// <returns></returns>
        public bool CreateSiteinfoTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//2.创建场地信息表.txt");
                MysqlEasy.ExecuteScalar(sql);
                isok = true;
            }
            catch (Exception ex)
            {
                isok = false;
            }



            return isok;
        }


        /// <summary>
        /// 插入场地信息记录
        /// </summary>
        /// <returns></returns>
        public bool InsertSiteinfoLRec(MyBackgroundWorker worker, DoWorkEventArgs e)
        {
            bool isok = false;

            try
            {
                List<string> sqllist = GetTextStrlist(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//3.导入场地信息表记录.txt");

                MysqlEasy.ExecuteSqlTran(sqllist);
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入" + sqllist.Count + "条场地信息记录!");

                //int i = 0;
                //foreach (string sql in sqllist)
                //{
                //    i++;
                //    MysqlEasy.ExecuteScalar(sql);
                //    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入第" + i + "条成功!");
                //}

                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 创建测线信息表
        /// </summary>
        /// <returns></returns>
        public bool CreateLineTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//4.创建测线信息表.txt");

                MysqlEasy.ExecuteScalar(sql);
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 导入测线信息记录
        /// </summary>
        /// <returns></returns>
        public bool InsertLineinfoLRec(MyBackgroundWorker worker, DoWorkEventArgs e)
        {
            bool isok = false;

            try
            {
               
                List<string> sqllist = GetTextStrlist(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//5.导入测线信息表记录.txt");
                MysqlEasy.ExecuteSqlTran(sqllist);
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入" + sqllist.Count + "条测线信息记录!");
                isok = true;
                //int i = 0;
                //foreach (string sql in sqllist)
                //{
                //    i++;
                //    MysqlEasy.ExecuteScalar(sql);
                //    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入第" + i + "条成功!");
                //}
            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }


        /// <summary>
        /// 创建单位信息表
        /// </summary>
        /// <returns></returns>
        public bool CreateUnitTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//6.创建单位信息表.txt");

                MysqlEasy.ExecuteScalar(sql);
                isok = true;
            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }


        /// <summary>
        /// 导入单位信息记录
        /// </summary>
        /// <returns></returns>
        public bool InsertUnitinfoLRec(MyBackgroundWorker worker, DoWorkEventArgs e)
        {
            bool isok = false;

            try
            {
               
                List<string> sqllist = GetTextStrlist(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//7.导入单位信息表记录.txt");
                MysqlEasy.ExecuteSqlTran(sqllist);
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入" + sqllist.Count + "条单位信息记录!");
                //int i = 0;
                //foreach (string sql in sqllist)
                //{
                //    i++;
                //    MysqlEasy.ExecuteScalar(sql);
                //    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入第" + i + "条成功!");
                //}
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 创建地震目录表
        /// </summary>
        /// <returns></returns>
        public bool CreateEqklogTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//8.创建地震目录表.txt");

                MysqlEasy.ExecuteScalar(sql);
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }


        /// <summary>
        /// 导入地震目录记录
        /// </summary>
        /// <returns></returns>
        public bool InsertEqkloginfoLRec(MyBackgroundWorker worker, DoWorkEventArgs e)
        {
            bool isok = false;

            try
            {
              
                List<string> sqllist = GetTextStrlist(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//9.导入地震目录表记录.txt");
                MysqlEasy.ExecuteSqlTran(sqllist);
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入" + sqllist.Count + "条地震目录记录!");
                //int i = 0;
                //foreach (string sql in sqllist)
                //{
                  
                //    MysqlEasy.ExecuteNonQuery(sql);
                   
                //}
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 创建观测信息表
        /// </summary>
        /// <returns></returns>
        public bool CreateObsLineTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//10.创建测线观测表.txt");
                MysqlEasy.ExecuteScalar(sql);
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        /// <summary>
        /// 创建场地布设图表
        /// </summary>
        /// <returns></returns>
        public bool CreateSiteLayoutTb()
        {
            bool isok = false;

            try
            {
                string sql = GetTextStr(System.Windows.Forms.Application.StartupPath + "//创建本地信息库//11.创建场地布设图表.txt");
                MysqlEasy.ExecuteScalar(sql);
                isok = true;

            }
            catch (Exception ex)
            {
                isok = false;
            }

            return isok;
        }

        private string GetTextStr(string file)
        {
            string str = "";
            try
            {
                using (StreamReader sr = new StreamReader(file, GetFileEncodeType(file)))
                {
                    str = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                str = "";
            }

            return str;
        }

        private List<string> GetTextStrlist(string file)
        {
            List<string> strlist = new List<string>();
            try
            {
                string line = string.Empty;

                Encoding ed = Path.GetFileNameWithoutExtension(file) == "9.导入地震目录表记录" ? Encoding.UTF8 : GetFileEncodeType(file);
        
                using (StreamReader sr = new StreamReader(file, ed))
                {

                    //string linestr = sr.ReadLine();

                    //do
                    //{
                    //    strlist.Add(linestr);
                    //    linestr = sr.ReadLine();
                    //}
                    //while (linestr!=string.Empty);

                    line = sr.ReadToEnd();
                    //sr.Close();
                }

                if (line != string.Empty)
                {
                    line = line.Replace("\n", "");
                    string[] checks = line.Split('\r');
                    strlist = checks.ToList();
                }
            }
            catch
            {
               
            }

            return strlist;
        }


        private void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        {
            bool isModified = false;    //记录该连接串是否已经存在 
            if (ConfigurationManager.ConnectionStrings[newName] != null)
            {
                isModified = true;
            }
            //新建一个连接字符串实例 
            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString, newProviderName);

            // 打开可执行的配置文件*.exe.config 
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // 如果连接串已存在，首先删除它 
            if (isModified)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
                // 将新的连接串添加到配置文件中. 
                config.ConnectionStrings.ConnectionStrings.Add(mySettings);
                // 保存对配置文件所作的更改 
                config.Save(ConfigurationSaveMode.Modified);
                // 强制重新载入配置文件的ConnectionStrings配置节  
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }


        private System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }



    }
}
