using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;

namespace xxkUI.Bll
{
    public class LineObsBll
    {
        public static LineObsBll Instance
        {
            get { return SingletonProvider<LineObsBll>.Instance; }
        }

        public int Add(LineObsBean model)
        {
            model = new LineObsBean();
           
            return LineObsDal.Instance.Insert(model);
        }

        public int Update(LineObsBean model)
        {
            return LineObsDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return LineObsDal.Instance.Delete(keyid);
        }

        public LineObsBean Get(int id)
        {
            return LineObsDal.Instance.Get(id);
        }

    }
}
