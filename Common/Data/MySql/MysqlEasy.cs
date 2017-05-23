using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data;
using Common.Data.MySql;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Common.Data.MySql
{
    public class MysqlEasy
    {
        private static string connString;
        public static string ConnectionString
        {
            get { return connString; }
            set
            {
                connString = value;
            }
        }

        public static int ExecuteNonQuery(string SQLString)
        {
            return MysqlHelper.ExecuteNonQuery(SQLString);
        }
        /// <summary>   
        /// 执行SQL语句，返回影响的记录数   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <returns>影响的记录数</returns>   
        public static int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            return MysqlHelper.ExecuteNonQuery(SQLString,cmdParms);
        }
        //执行多条SQL语句，实现数据库事务。   
        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="SQLStringList">多条SQL语句</param>   
        public static bool ExecuteNoQueryTran(List<String> SQLStringList)
        {
            return MysqlHelper.ExecuteNoQueryTran(SQLStringList);
        }

        #region ExecuteScalar   
        /// <summary>   
        /// 执行一条计算查询结果语句，返回查询结果（object）。   
        /// </summary>   
        /// <param name="SQLString">计算查询结果语句</param>   
        /// <returns>查询结果（object）</returns>   
        public static object ExecuteScalar(string SQLString)
        {
            return MysqlHelper.ExecuteScalar(SQLString);
        }
        /// <summary>   
        /// 执行一条计算查询结果语句，返回查询结果（object）。   
        /// </summary>   
        /// <param name="SQLString">计算查询结果语句</param>   
        /// <returns>查询结果（object）</returns>   
        public static object ExecuteScalar(string SQLString, params MySqlParameter[] cmdParms)
        {
            return MysqlHelper.ExecuteScalar(SQLString,cmdParms);
        }
        #endregion
        #region ExecuteReader   
        /// <summary>   
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )   
        /// </summary>   
        /// <param name="strSQL">查询语句</param>   
        /// <returns>MySqlDataReader</returns>   
        public static MySqlDataReader ExecuteReader(string strSQL)
        {
            return MysqlHelper.ExecuteReader(strSQL);
        }
        /// <summary>   
        /// 执行查询语句，返回MySqlDataReader ( 注意：调用该方法后，一定要对MySqlDataReader进行Close )   
        /// </summary>   
        /// <param name="strSQL">查询语句</param>   
        /// <returns>MySqlDataReader</returns>   
        public static MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            return MysqlHelper.ExecuteReader(SQLString,cmdParms);
        }
        #endregion
        #region ExecuteDataTable   
        /// <summary>   
        /// 执行查询语句，返回DataTable   
        /// </summary>   
        /// <param name="SQLString">查询语句</param>   
        /// <returns>DataTable</returns>   
        public static DataTable ExecuteDataTable(string SQLString)
        {
            return MysqlHelper.ExecuteDataTable(SQLString);
        }
        /// <summary>   
        /// 执行查询语句，返回DataSet   
        /// </summary>   
        /// <param name="SQLString">查询语句</param>   
        /// <returns>DataTable</returns>   
        public static DataTable ExecuteDataTable(string SQLString, params MySqlParameter[] cmdParms)
        {
            return MysqlHelper.ExecuteDataTable(SQLString,cmdParms);
        }
        //获取起始页码和结束页码   
        public static DataTable ExecuteDataTable(string cmdText, int startResord, int maxRecord)
        {
            return MysqlHelper.ExecuteDataTable(cmdText,startResord,maxRecord);
        }
        #endregion
        /// <summary>   
        /// 获取分页数据 在不用存储过程情况下   
        /// </summary>   
        /// <param name="recordCount">总记录条数</param>   
        /// <param name="selectList">选择的列逗号隔开,支持top num</param>   
        /// <param name="tableName">表名字</param>   
        /// <param name="whereStr">条件字符 必须前加 and</param>   
        /// <param name="orderExpression">排序 例如 ID</param>   
        /// <param name="pageIdex">当前索引页</param>   
        /// <param name="pageSize">每页记录数</param>   
        /// <returns></returns>   
        public static DataTable getPager(out int recordCount, string selectList, string tableName, string whereStr, string orderExpression, int pageIdex, int pageSize)
        {
            return MysqlHelper.getPager(out recordCount, selectList, tableName, whereStr, orderExpression, pageIdex, pageSize);
        }
     
    }
}
