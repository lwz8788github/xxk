using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using System.Web.Security;
using xxkUI.Tool;

namespace xxkUI.Bll
{
    public class UserInfoBll
    {
        public static UserInfoBll Instance
        {
            get { return SingletonProvider<UserInfoBll>.Instance; }
        }

        public int Add(UserInfoBean model)
        {
            return UserInfoDal.Instance.Insert(model);
        }

        public int Update(UserInfoBean model)
        {
            return UserInfoDal.Instance.Update(model);
        }

        public int UpdateWhatWhere(object what, object where)
        {
            return UserInfoDal.Instance.UpdateWhatWhere(what, where);
        }
        public int Delete(int keyid)
        {
            return UserInfoDal.Instance.Delete(keyid);
        }

        public int DeleteWhere(object where)
        {
            return UserInfoDal.Instance.DeleteWhere(where);
        }

        public IEnumerable<UserInfoBean> GetAll()
        {
            return UserInfoDal.Instance.GetAll();
        }
        public UserInfoBean Get(int id)
        {
            return UserInfoDal.Instance.Get(id);
        }


        public UserInfoBean GetUserBy(string userName)
        {
            return UserInfoDal.Instance.GetAll().ToList().Find(n => n.UserName == userName);
        }

        public bool GetLogin(UserInfoBean uif)
        {
            UserInfoBean obj = UserInfoDal.Instance.GetAll().ToList().Find(n => (n.UserName == uif.UserName && n.Password == uif.Password));

            if (obj != null)
                return true;
            else
                return false;
        }


        public List<string> GetAthrByUser<UserInfoBean>(string username)
        {
            string authstr = UserInfoDal.Instance.GetByID("USERATHRTY", "USERNAME", username).ToString();
            string[] auths = authstr.Split(';');
            return auths.ToList();
        }

        /// <summary>
        /// 给密码加密
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public string encryptPWD(string pwd)
        {
            //加密算法不可逆
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            return password;
        }

        /// <summary>
        /// 用户状态转换
        /// </summary>
        /// <param name="us"></param>
        /// <returns></returns>
        public string GetUserStatusValueByKey(UserStatus us)
        {
            string usvalue = "";

           

            return usvalue;
        }

    }
}
