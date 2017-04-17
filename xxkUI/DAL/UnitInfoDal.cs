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
    public class UnitInfoDal : BaseRepository<UnitInfoBean>
    {
        public static UnitInfoDal Instance
        {
            get { return SingletonProvider<UnitInfoDal>.Instance; }
        }
             
    }
}