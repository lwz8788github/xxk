using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Data;

namespace xxkUI.Tool
{
    public class ModelHandler<T> where T:new()
    {
        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }


    

        /// <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    foreach (System.Reflection.PropertyInfo field1 in model.GetType().GetProperties())
                    {
                        object[] objAttrs = field1.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (objAttrs.Length > 0)
                        {
                            DescriptionAttribute attr = objAttrs[0] as DescriptionAttribute;
                            if (attr != null)
                            {
                                if (attr.Description == dr.Table.Columns[i].ColumnName)
                                    if(dr[i] != DBNull.Value)
                                    {
                                        if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("DateTime"))
                                        {
                                            DateTime v = DateTime.Parse(dr[i].ToString());
                                               field1.SetValue(model, v, null);
                                        }
                                        if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("String"))
                                        {
                                            field1.SetValue(model, dr[i].ToString(), null);
                                        }
                                        if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("Double"))
                                        {
                                            double v = double.Parse(dr[i].ToString());
                                            field1.SetValue(model, v, null);
                                        }
                                        if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("Int"))
                                        {
                                            int v = int.Parse(dr[i].ToString());
                                            field1.SetValue(model, v, null);
                                        }

                                    }
                            }
                        }

                    }
                    
                }

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// 填充观测数据对象列表 刘文龙
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> FillObsLineModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    foreach (System.Reflection.PropertyInfo field1 in model.GetType().GetProperties())
                    {
                        object[] objAttrs = field1.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (objAttrs.Length > 0)
                        {
                            DescriptionAttribute attr = objAttrs[0] as DescriptionAttribute;
                            if (attr != null)
                            {
                                if (attr.Description == dr.Table.Columns[i].ColumnName)
                                    if (dr[i] != DBNull.Value)
                                    {
                                        if (i == 0)
                                        {
                                            if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("DateTime"))
                                            {
                                                DateTime v = DateTime.Parse(dr[i].ToString());
                                                field1.SetValue(model, v, null);
                                            }

                                            if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("String"))
                                            {
                                                DateTime v = DateTime.Parse(dr[i].ToString());
                                                field1.SetValue(model, dr[i].ToString(), null);
                                            }
                                        }
                                        if (i == 1)
                                        {
                                            if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("Double"))
                                            {
                                                double v = double.Parse(dr[i].ToString());
                                                field1.SetValue(model, v, null);
                                            }
                                            if (field1.PropertyType.IsValueType && field1.PropertyType.Name.StartsWith("Int"))
                                            {
                                                int v = int.Parse(dr[i].ToString());
                                                field1.SetValue(model, v, null);
                                            }
                                        }
                                    }
                            }
                        }

                    }

                }

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                 
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable FillDataTable(List<T> modelList)
        {
          
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData(T model)
        {
            Type t = typeof(T);
            DataTable dataTable = new DataTable(t.Name);
          
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo item in properties)
            {
                dataTable.Columns.Add(new DataColumn(item.Name, item.PropertyType));
                //string des = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute))).Description;// 属性值
            }
            return dataTable;
        }



        #endregion


    

    }
}
