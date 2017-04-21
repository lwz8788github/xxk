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
    public class SiteDal : BaseRepository<SiteBean>
    {
        public static SiteDal Instance
        {
            get { return SingletonProvider<SiteDal>.Instance; }
        }


        internal IEnumerable<SiteBean> GetList(string p)
        {
            throw new NotImplementedException();
        }
    }
}