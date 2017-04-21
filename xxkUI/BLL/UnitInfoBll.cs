using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xxkUI.Dal;
using Common.Provider;

namespace xxkUI.BLL
{
    class UnitInfoBll
    {
        public static UnitInfoBll Instance
        {
            get { return SingletonProvider<UnitInfoBll>.Instance; }
        }

        public int Add(UnitInfoBean model)
        {
            model = new UnitInfoBean();

            return UnitInfoDal.Instance.Insert(model);
        }

        public int Update(UnitInfoBean model)
        {
            return UnitInfoDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return UnitInfoDal.Instance.Delete(keyid);
        }

        public UnitInfoBean Get(int id)
        {
            return UnitInfoDal.Instance.Get(id);
        }


        public IEnumerable<UnitInfoBean> GetAll()
        {
            return UnitInfoDal.Instance.GetAll();
        }

        //public List<string> GeSiteByUnit<UnitInfoBean>(string UnitCode)
        //{
        //    string authstr = UnitInfoDal.Instance.GetByID("SITECODE", "USERNAME", UnitCode).ToString();
        //    string[] auths = authstr.Split(';');
        //    return auths.ToList();
        //}

        public string GetUnitNameBy(string _unitcode)
        {
            IEnumerable<UnitInfoBean> uibEnum = UnitInfoDal.Instance.GetList("select unitname from t_unittb where unitcode=@Unitcode", new { Unitcode = _unitcode });
            return uibEnum.ToList()[0].UnitName;
        }



    }
}
