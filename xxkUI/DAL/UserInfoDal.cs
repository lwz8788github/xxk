using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Security;
using Common.Data;
using Common.Data.MySql;
using Common.Provider;


namespace xxkUI.Dal
{
    public class UserInfoDal : BaseRepository<UserInfoBean>
    {
        public static UserInfoDal Instance
        {
            get { return SingletonProvider<UserInfoDal>.Instance; }
        }

        public UserInfoBean GetUserBy(string userName)
        {
            return GetAll().ToList().Find(n => n.UserName == userName);
        }

        public bool GetLogin(UserInfoBean uif)
        {
            UserInfoBean obj = GetAll().ToList().Find(n => (n.UserName == uif.UserName&& n.Password == uif.Password));

            if (obj != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="pwd">����</param>
        /// <returns></returns>
        private string encryptPWD(string pwd)
        {
            //�����㷨������
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            return password;
        }

    }
}