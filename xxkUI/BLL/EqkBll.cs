using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.DAL;

namespace xxkUI.BLL
{
   public class EqkBll
    {
        public static EqkBll Instance
        {
            get { return SingletonProvider<EqkBll>.Instance; }
        }

        public int Add(EqkBean model)
        {
            return EqkDal.Instance.Insert(model);
        }

        public int Update(EqkBean model)
        {
            return EqkDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return EqkDal.Instance.Delete(keyid);
        }

        public EqkBean Get(int id)
        {
            return EqkDal.Instance.Get(id);
        }


    }
}
