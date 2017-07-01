using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace xxkUI.Model
{
   public class TreeBean
    {
        [Description("树名称")]
        private string _TreeName;
        public string TreeName
        {
            get { return _TreeName; }
            set { _TreeName = value; }
        }

        private string _KeyFieldName;
        public string KeyFieldName 
        {
            get { return _KeyFieldName; }
            set { _KeyFieldName = value; }
        }

        private string _ParentFieldName;
        public string ParentFieldName 
        {
            get { return _ParentFieldName; }
            set { _ParentFieldName = value; }
        }

       [Description("描述")]
        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        [Description("场地类型")]
        private string _SiteType;
        public string SiteType
        {
            get { return _SiteType; }
            set { _SiteType = value; }
        }
        [Description("运行状况")]
        private string _LineStatus;
        public string LineStatus
        {
            get { return _LineStatus; }
            set { _LineStatus = value; }
        }

        private object tag;
        /// <summary>
        /// 用于装节点对象
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}
