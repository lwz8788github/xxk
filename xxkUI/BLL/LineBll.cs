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

        /// <summary>
        /// 根据测线编码获取测线信息
        /// </summary>
        /// <param name="idvalue">测线编码</param>
        /// <returns></returns>
        public LineBean GetInfoByID(string idvalue)
        {
            return LineDal.Instance.GetList("select * from t_obslinetb where OBSLINECODE = '" + idvalue + "'").ToList()[0];
        }

        /// <summary>
        /// 根据场地编码获取测线集合
        /// </summary>
        /// <param name="sitecode">场地编码</param>
        /// <returns></returns>
        public IEnumerable<LineBean> GetBySitecode(string sitecode)
        {
            return LineDal.Instance.GetList("select * from t_obslinetb where SITECODE = '" + sitecode + "'").ToList();
        }

        public IEnumerable<LineBean> GetAll()
        {
            return LineDal.Instance.GetAll();
        }

        /// <summary>
        /// 测线是否存在
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsExist(string id)
        {
            bool existed = false;

            if (LineDal.Instance.CountWhere(new { OBSLINENAME = id }) > 0)
                existed = true;
            else
                existed = false;

            return existed;
        }
        /// <summary>
        /// 计算场地测线数目
        /// </summary>
        /// <param name="sitecode">场地编码</param>
        /// <returns>测线数目</returns>
        public int GetCountBySiteCode(string sitecode)
        {
            object where = new { SiteCode = sitecode };
            return LineDal.Instance.CountWhere(where);
        }

        /// <summary>
        /// 通过测线名称获取测线编码
        /// </summary>
        /// <param name="linename">测线名称</param>
        /// <returns>测线编码</returns>
        public string GetIdByName(string linename)
        {
            return LineDal.Instance.GetWhere(new { OBSLINENAME = linename }).ToList()[0].OBSLINECODE;
        }

        /// <summary>
        /// 生成测线编码 刘文龙
        /// </summary>
        /// <param name="sitecode">场地编码</param>
        /// <returns>测线编码</returns>
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
