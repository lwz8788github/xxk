using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;

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

    }
}
