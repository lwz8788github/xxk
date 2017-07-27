using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using System.Web.Security;

namespace xxkUI.Bll
{
    public class LayoutmapBll
    {
        public static LayoutmapBll Instance
        {
            get { return SingletonProvider<LayoutmapBll>.Instance; }
        }

        public int Add(LayoutmapBean model)
        {
            return LayoutmapDal.Instance.Insert(model);
        }

        public int Update(LayoutmapBean model)
        {
            return LayoutmapDal.Instance.Update(model);
        }

        public int UpdateWhatWhere(object what, object where)
        {
            return LayoutmapDal.Instance.UpdateWhatWhere(what, where);
        }


        public int Delete(int keyid)
        {
            return UserInfoDal.Instance.Delete(keyid);
        }

        public LayoutmapBean Get(int id)
        {
            return LayoutmapDal.Instance.Get(id);
        }

        public IEnumerable<LayoutmapBean> GetAll()
        {
            return LayoutmapDal.Instance.GetAll();
        }

        public List<LayoutmapBean> GetLayoutmapBy(string sitecode)
        {
            return LayoutmapDal.Instance.GetAll().ToList().FindAll(n => n.sitecode == sitecode);
        }


        public string CreateLayoutmapCode()
        {
            string layoutmapcodestr = "";

            try
            {
                IEnumerable<LayoutmapBean> sblist = GetAll();

                int maxcode = sblist.Count() + 1;

                if (maxcode.ToString().Length == 1)
                {
                    layoutmapcodestr = "PIC"+"00" + (maxcode).ToString();
                }
                if (maxcode.ToString().Length == 2)
                {
                    layoutmapcodestr = "PIC" + "0" + (maxcode).ToString();
                }
                if (maxcode.ToString().Length == 3)
                {
                    layoutmapcodestr = "PIC" + (maxcode).ToString();
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return layoutmapcodestr;
        }





    }
}
