using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using xxkUI.BLL;
using System.IO;

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
            return SiteDal.Instance.GetList(@"select UNITCODE,SITECODE,SITENAME,TYPE,HISTORYSITE,LONGTITUDE,
                                                LATITUDE, ALTITUDE,PLACE,FAULTNAME,FAULTPROPERTY,FAULTSTRIKE,
                                                FAULTTENDENCY,FAULTDIP,UPROCK,BOTTOMROCK,STARTDATE,XZCODE
                                                 from t_siteinfodb where UNITCODE in " + auths);
        }

        /// <summary>
        /// �����ĵ�
        /// </summary>
        /// <param name="idname"></param>
        /// <param name="idvalue"></param>
        /// <param name="blobfield"></param>
        /// <returns></returns>
        public string DownloadDoc(string idname, string idvalue, string blobfield)
        {
            string filename = string.Empty;
            try
            {
                filename = System.Windows.Forms.Application.StartupPath + "/tempDoc/" + idvalue + ".doc";
                if (!File.Exists(filename))
                {
                    byte[] blob = GetBlob<SiteBean>(idname, idvalue, blobfield);
                    if (blob == null)
                        return string.Empty;
                    if (blob.Length == 0)
                        return string.Empty;

                    File.WriteAllBytes(filename, blob);
                }

            }
            catch (Exception ex)
            {
                filename = string.Empty;
                throw new Exception(ex.Message);
            }

            return filename;
        }

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <param name="filename">�ĵ�·��</param>
        /// <returns>Ӱ������</returns>
        public int UpdateBASEINFO(string sitecode, string filename)
        {
            byte[] blobData = File.ReadAllBytes(filename);
            string sql = "update t_siteinfodb set BASEINFO = @blobData where SiteCode ='" + sitecode + "'";
           
            return SiteDal.Instance.Writeblob(sql, "blobData", blobData);
        }
    }
}
