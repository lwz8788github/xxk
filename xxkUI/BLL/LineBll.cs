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
        public string GetNameByID(string getwhat, string idname, string idvalue)
        {
            return LineDal.Instance.GetByID(getwhat, idname, idvalue).ToString();
        }

        public IEnumerable<LineBean> GetAll()
        {
            return LineDal.Instance.GetAll();
        }

        /// <summary>
        /// ���㳡�ز�����Ŀ
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns>������Ŀ</returns>
        public int GetCountBySiteCode(string sitecode)
        {
            object where = new { SiteCode = sitecode };
            return LineDal.Instance.CountWhere(where);
        }

        /// <summary>
        /// ���ɲ��߱��� ������
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns>���߱���</returns>
        public string GenerateLineCode(string sitecode)
        {
            string linecode = string.Empty;
            try
            {
               int count= GetCountBySiteCode(sitecode);
                if (count < 10)
                    linecode = sitecode + "00" + count.ToString();
                else if (count >= 10 && count < 100)
                    linecode = sitecode + "0" + count.ToString();
                else if (count >= 100)
                    linecode = sitecode + count.ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return linecode;
        }
    }
}
