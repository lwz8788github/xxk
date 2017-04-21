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
    public class UserInfoBll
    {
        public static UserInfoBll Instance
        {
            get { return SingletonProvider<UserInfoBll>.Instance; }
        }

        public int Add(UserInfoBean model)
        {
            model = new UserInfoBean();
           
            return UserInfoDal.Instance.Insert(model);
        }

        public int Update(UserInfoBean model)
        {
            return UserInfoDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return UserInfoDal.Instance.Delete(keyid);
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
        private string encryptPWD(string pwd)
        {
            //加密算法不可逆
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            return password;
        }

    }
}
