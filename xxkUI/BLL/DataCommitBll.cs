using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.DAL;
using xxkUI.Dal;

namespace xxkUI.BLL
{
   public class DataCommitBll
    {
        public static DataCommitBll Instance
        {
            get { return SingletonProvider<DataCommitBll>.Instance; }
        }

        public int Add(DataCommitBean model)
        {
            return DataCommitDal.Instance.Insert(model);
        }

        public int Update(DataCommitBean model)
        {
            return DataCommitDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return DataCommitDal.Instance.Delete(keyid);
        }

        public int DeleteWhere(object where)
        {
            return DataCommitDal.Instance.DeleteWhere(where);
        }

        public DataCommitBean Get(int id)
        {
            return DataCommitDal.Instance.Get(id);
        }
        public IEnumerable<DataCommitBean> GetAll()
        {
            return DataCommitDal.Instance.GetAll();
        }
    }
}
