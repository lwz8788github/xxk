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
    public class LineObsDal : BaseRepository<LineObsBean>
    {
        public static LineObsDal Instance
        {
            get { return SingletonProvider<LineObsDal>.Instance; }
        }
             
    }
}