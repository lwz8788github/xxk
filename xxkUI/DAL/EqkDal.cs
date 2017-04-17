using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Provider;

namespace xxkUI.DAL
{
    public class EqkDal:BaseRepository<EqkBean>
    {
        public static EqkDal Instance
        {
            get { return SingletonProvider<EqkDal>.Instance; }
        }

    }
}
