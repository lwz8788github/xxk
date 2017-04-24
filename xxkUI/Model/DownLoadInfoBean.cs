using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace xxkUI.Model
{
    public class DownLoadInfoBean
    {

       [Description("下载内容")]
        private string _DownloadStr;
       public string DownloadStr
       {
           get { return _DownloadStr; }
           set { _DownloadStr = value; }
       }

        [Description("下载路径")]
       private System.IO.DirectoryInfo _DownloadPath;
        public System.IO.DirectoryInfo DownloadPath
        {
            get { return _DownloadPath; }
            set { _DownloadPath = value; }
        }
        [Description("测线编码")]
        private string _Obslinecode;
        public string Obslinecode
        {
            get { return _Obslinecode; }
            set { _Obslinecode = value; }
        }

        private object tag;
        /// <summary>
        /// 用于装节点对象
        /// </summary>
        /// 
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}
