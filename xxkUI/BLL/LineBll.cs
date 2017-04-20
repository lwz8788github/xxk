using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;

namespace xxkUI.Bll
{
    public class LineBll
    {
        public static LineBll Instance
        {
            get { return SingletonProvider<LineBll>.Instance; }
        }

        public int Add(LineBean model)
        {
            model = new LineBean();
           
            return LineDal.Instance.Insert(model);
        }

        public int Update(LineBean model)
        {
            return LineDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return LineDal.Instance.Delete(keyid);
        }

        public LineBean Get(int id)
        {
            return LineDal.Instance.Get(id);
        }
        public IEnumerable<LineBean> GetAll()
        {
            return LineDal.Instance.GetAll();
        }

    }
}
