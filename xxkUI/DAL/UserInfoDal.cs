using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
             
    }
}