using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using xxkUI.BLL;

namespace xxkUI.Bll
{
    public class SiteBll
    {
        public static SiteBll Instance
        {
            get { return SingletonProvider<SiteBll>.Instance; }
        }

        public int Add(SiteBean model)
        {
            model = new SiteBean();
           
            return SiteDal.Instance.Insert(model);
        }

        public int Update(SiteBean model)
        {
            return SiteDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return SiteDal.Instance.Delete(keyid);
        }

        public SiteBean Get(int id)
        {
            return SiteDal.Instance.Get(id);
        }

        public IEnumerable<SiteBean> GetAll()
        {
            return SiteDal.Instance.GetAll();
        }

        public byte[] GetBlob<SiteBean>(string idname, string idvalue, string blobfield)
        {
            return SiteDal.Instance.GetBlob(idname, idvalue, blobfield);
        }

        public IEnumerable<SiteBean> GetWhere(object where)
        {
            return SiteDal.Instance.GetWhere(where);
        }
        public string GetNameByID(string name, string ID, string Value)
        {
            string nameStr = SiteDal.Instance.GetByID(name, ID, Value).ToString();
            return nameStr;
        }

        public IEnumerable<SiteBean> GetSitesByAuth(string auths)
        {
            return SiteDal.Instance.GetList("select * from t_siteinfodb where UNITCODE in " + auths);
        }


     
    }
}
