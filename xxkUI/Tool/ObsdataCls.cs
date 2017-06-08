/***********************************************************/
//---模    块：观测数据静态类
//---功能描述：数据缓存及数据操作
//---编码时间：2017-06-06
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
    /// <summary>
    /// 存储，一直放在缓存中
    /// </summary>
    public static class ObsdataCls
    {
        public static Hashtable ObsdataHash = new Hashtable();

        /// <summary>
        /// 记录是否已经存在
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public static bool IsExisted(string linecode)
        {
            bool isexisted = false;
            foreach (string key in ObsdataCls.ObsdataHash.Keys)
            {
                if (key == linecode)
                    isexisted = true;
            }
            return isexisted;
        }

        /// <summary>
        /// 从缓存中获取观测数据
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public static DataTable GetObsdataFromHash(string linecode)
        {
            return ObsdataHash[linecode] as DataTable;
        }


    }
}
