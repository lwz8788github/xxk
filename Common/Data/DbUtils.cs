using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Omu.ValueInjecter;
using System.Configuration;
using Common.Data.MySql;
using MySql.Data.MySqlClient;
using System.IO;

namespace Common.Data
{

    public static class DbUtils
    {
        static string cs = MysqlEasy.ConnectionString; //数据库连接字符串
        public static IEnumerable<T> GetWhere<T>(object where) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T)) + " where "
                        .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                    }
                }
            }


        }



        public static int CountWhere<T>(object where) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select count(*) from " + TableConvention.Resolve(typeof(T)) + " where "
                        .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where);
                    cmd.InjectFrom<SetParamsValues>(where);
                    conn.Open();

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static int Delete<T>(int id)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from " + TableConvention.Resolve(typeof(T)) + " where KeyID=@KeyID";

                cmd.InjectFrom<SetParamsValues>(new { KeyID = id });
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int DeleteWhere<T>(object where)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from " + TableConvention.Resolve(typeof(T)) + " where "
                    .InjectFrom(new FieldsBy()
                        .SetFormat("{0}=@{0}")
                        .SetNullFormat("{0} is null")
                        .SetGlue("and"),
                        where);

                cmd.InjectFrom<SetParamsValues>(where);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static int Delete<T>(string ids)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from " + TableConvention.Resolve(typeof(T)) + " where charindex(',' + cast(keyid AS varchar(50)) + ',',','  + @KeyID + ',') > 0";

                cmd.InjectFrom<SetParamsValues>(new { KeyID = ids });
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        ///<returns> the id of the inserted object </returns>
        public static int Insert(object o)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                    .InjectFrom(new FieldsBy().IgnoreFields("keyid"), o) + ") values("
                    .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("@{0}"), o)
                    + ") select @@identity";

                cmd.InjectFrom(new SetParamsValues().IgnoreFields("keyid"), o);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int Insert(object o, string IgnoreFields)
        {
            string[] strarr = { };
            if (!string.IsNullOrEmpty(IgnoreFields))
                strarr = IgnoreFields.Split(',');
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                    .InjectFrom(new FieldsBy().IgnoreFields(strarr), o) + ") values("
                    .InjectFrom(new FieldsBy().IgnoreFields(strarr).SetFormat("@{0}"), o)
                    + ") ";

                cmd.InjectFrom(new SetParamsValues().IgnoreFields(strarr), o);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
        }


        public static int Update(object o)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update " + TableConvention.Resolve(o) + " set "
                    .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("{0}=@{0}"), o)
                    + " where KeyID = @KeyID";

                cmd.InjectFrom<SetParamsValues>(o);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
        }

        public static int Update(object o, params string[] fields)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update " + TableConvention.Resolve(o) + " set "
                    .InjectFrom(new FieldsBy().IgnoreFields(fields).SetFormat("{0}=@{0}"), o)
                    + " where KeyID = @KeyID";

                cmd.InjectFrom<SetParamsValues>(o);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteNonQuery());
            }
        }

        public static int UpdateWhatWhere<T>(object what, object where)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update " + TableConvention.Resolve(typeof(T)) + " set "
                    .InjectFrom(new FieldsBy().SetFormat("{0}=@{0}"), what)
                    + " where "
                    .InjectFrom(new FieldsBy()
                    .SetFormat("{0}=@wp{0}")
                    .SetNullFormat("{0} is null")
                    .SetGlue("and"),
                    where);

                cmd.InjectFrom<SetParamsValues>(what);
                cmd.InjectFrom(new SetParamsValues().Prefix("wp"), where);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }


        public static int InsertNoIdentity(object o)
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert " + TableConvention.Resolve(o) + " ("
                    .InjectFrom(new FieldsBy().IgnoreFields("keyid"), o) + ") values("
                    .InjectFrom(new FieldsBy().IgnoreFields("keyid").SetFormat("@{0}"), o) + ")";

                cmd.InjectFrom<SetParamsValues>(o);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <returns>rows affected</returns>
        public static int ExecuteNonQuerySp(string sp, object parameters)
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static int ExecuteNonQuery(string commendText, object parameters)
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = commendText;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static IEnumerable<T> ExecuteReader<T>(string sql, object parameters) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                }
            }
        }


        public static IEnumerable<T> ExecuteReaderSp<T>(string sp, object parameters) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                }
            }
        }

        public static IEnumerable<T> ExecuteReaderSpValueType<T>(string sp, object parameters)
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = sp;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();
                    using (var dr = cmd.ExecuteReader())
                        while (dr.Read())
                        {
                            yield return (T)dr.GetValue(0);
                        }
                }
            }
        }

        public static int Count<T>()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select count(*) from " + TableConvention.Resolve(typeof(T));
                    conn.Open();

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static int GetPageCount(int pageSize, int count)
        {
            var pages = count / pageSize;
            if (count % pageSize > 0) pages++;
            return pages;
        }

        public static IEnumerable<T> GetAll<T>() where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T));
                    conn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> GetList<T>(string sql, object parameters) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.InjectFrom<SetParamsValues>(parameters);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> GetList<T>(string sql) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    conn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                    }
                }
            }
        }

        public static DataTable GetPageWithSp(ProcCustomPage pcp, out int recordCount)
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = pcp.Sp_PagerName;
                    cmd.InjectFrom(new SetParamsValues().IgnoreFields("sp_pagername"), pcp);

                    SqlParameter outputPara = new SqlParameter("@RecordCount", SqlDbType.Int);
                    outputPara.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputPara);

                    conn.Open();

                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        recordCount = PublicMethod.GetInt(outputPara.Value);
                        conn.Close();
                        return ds.Tables[0];
                    }
                }
            }
        }


        public static IEnumerable<T> GetPage<T>(int page, int pageSize) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    var name = TableConvention.Resolve(typeof(T));

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = string.Format(@"with result as(select *, ROW_NUMBER() over(order by keyid desc) nr
                            from {0}
                    )
                    select  * 
                    from    result
                    where   nr  between (({1} - 1) * {2} + 1)
                            and ({1} * {2}) ", name, page, pageSize);
                    conn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var o = new T();
                            o.InjectFrom<ReaderInjection>(dr);
                            yield return o;
                        }
                    }
                }
            }
        }

        public static T Get<T>(long keyid) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T)) + " where keyid = " + keyid;
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        var o = new T();
                        o.InjectFrom<ReaderInjection>(dr);
                        return o;
                    }
            }
            return default(T);
        }

        public static IEnumerable<T> Get<T>(string keyid) where T : new()
        {
            using (var conn = new MySqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from " + TableConvention.Resolve(typeof(T)) + " where keyid = " + keyid;
                conn.Open();

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var o = new T();
                        o.InjectFrom<ReaderInjection>(dr);
                        yield return o;
                    }
                }
            }

        }


        /// <summary>
        /// 读取流字段
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="idname">主键字段名</param>
        /// <param name="idvalue">主键字段值</param>
        /// <param name="blobfieldname">流字段名</param>
        /// <returns></returns>
        public static byte[] GetBlobByID<T>(string idname, string idvalue, string blobfieldname)
        {
            byte[] ms = null;
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select " + blobfieldname + " from " + TableConvention.Resolve(typeof(T))
                        + " where " + idname + "='" + idvalue + "'";
                    conn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr[0] != DBNull.Value)
                        {
                            ms = (byte[])dr[0];
                        }
                        else
                            ms = new byte[0];

                    }
                }
            }
            return ms;
        }


        /// <summary>
        /// 根据id获取字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getwhat">查询字段</param>
        /// <param name="idname">id字段名</param>
        /// <param name="idvalue">id字段值</param>
        /// <returns></returns>
        public static object GetByID<T>(string getwhat, string idname, string idvalue)
        {
            object o;
            using (var conn = new MySqlConnection(cs))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select " + getwhat + " from " + TableConvention.Resolve(typeof(T))
                        + " where " + idname + "='" + idvalue + "'";
                    conn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr[0] != DBNull.Value)
                        {
                            o = dr[0];
                        }
                        else
                            o = null;
                    }
                }
            }
            return o;
        }



        //public static DataTable GetDataTable(string sql)
        //{
        //    using (var conn = new MySqlConnection(cs))
        //    {
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = sql;
        //            conn.Open();


        //            using (var da = new MySqlDataAdapter(cmd))
        //            {
        //                DataSet ds = new DataSet();
        //                da.Fill(ds);
        //                cmd.Parameters.Clear();
        //                conn.Close();
        //                return ds.Tables[0];
        //            }
        //        }
        //    }
        //}


        public static DataTable GetDataTable(string sql)
        {
            MySqlCommand cmd = new MySqlCommand();
            DataTable dt = new DataTable();
            MySqlConnection conn = new MySqlConnection(cs);//connectionString))
            {
                MySqlDataAdapter SqlDA = new MySqlDataAdapter();
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    SqlDA.SelectCommand = cmd;
                    SqlDA.Fill(dt);
                    conn.Close();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                    cmd = null;
                    SqlDA.Dispose();
                    SqlDA = null;
                }
                return dt;
            }


        }

        /// <summary>   
        /// 执行SQL语句，返回影响的记录数   
        /// </summary>   
        /// <param name="SQLString">SQL语句</param>   
        /// <returns>影响的记录数</returns>   
        public static int WriteBlob(string SQLString, string blobname, object blob)
        {
            using (MySqlConnection connection = new MySqlConnection(cs))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        MySqlParameter param = new MySqlParameter("@" + blobname, MySqlDbType.Blob);

                        param.Value = blob;
                        cmd.Parameters.Add(param);
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

    }
}
