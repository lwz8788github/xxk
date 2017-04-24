using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Data.Filter;

namespace Common.Data
{
    public class BaseRepository<T> :IRepository<T> where T:new ()
    {

        public T Get(int id)
        {
            return DbUtils.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbUtils.GetAll<T>();
        }

        /// <summary>
        /// 执行SQL语句并返回指定类型的列表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="whereParam">条件参数</param>
        /// <returns></returns>
        public  IEnumerable<T> GetList(string sql,object whereParam)
        {
            IEnumerable<T> list;
            try
            {
                list=DbUtils.GetList<T>(sql, whereParam);
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }

        public IEnumerable<T> GetList(string sql)
        {
            IEnumerable<T> list;
            try
            {
                list = DbUtils.GetList<T>(sql);
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
        public virtual int Insert(T o)
        {
            return DbUtils.Insert(o);
        }

        public virtual int Update(T o)
        {
            return DbUtils.Update(o);
        }

        public virtual int UpdateWhatWhere(object what, object where)
        {
            return DbUtils.UpdateWhatWhere<T>(what, where);
        }

        public virtual int InsertNoIdentity(T o)
        {
            return DbUtils.InsertNoIdentity(o);
        }

        public  IEnumerable<T> GetPage(int page, int pageSize)
        {
            return DbUtils.GetPage<T>(page, pageSize);
        }

        public int Count()
        {
            return DbUtils.Count<T>();
        }

        public IPageable<T> GetPageable(int page, int pageSize)
        {
            return new Pageable<T>
            {
                Rows = GetPage(page, pageSize),
                PageCount = DbUtils.GetPageCount(pageSize, Count()),
                PageIndex = page,
            };
        }

        public IEnumerable<T> GetWhere(object where)
        {
            return DbUtils.GetWhere<T>(where);
        }

        public int Delete(int id)
        {
            return DbUtils.Delete<T>(id);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">格式如： 1,2,4,6,8....</param>
        /// <returns></returns>
        public int Delete(string ids)
        {
            return DbUtils.Delete<T>(ids);
        }

        public int CountWhere(object where)
        {
            return DbUtils.CountWhere<T>(where);
        }

        public DataTable GetPageWithSp(ProcCustomPage pcp,out int recordCount)
        {
            return DbUtils.GetPageWithSp(pcp, out recordCount);
        }

        public byte[] GetBlob(string idname,string idvalue,string blobfield)
        {
            return DbUtils.GetBlobByID<T>(idname, idvalue, blobfield);
        }

        public object GetByID(string getwhat, string idname, string idvalue)
        {
            return DbUtils.GetByID<T>(getwhat, idname, idvalue);
        }
    }
}
