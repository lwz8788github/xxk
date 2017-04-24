using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
  public  class PublicHelper
    {
        /// <summary>
        /// 数据树结点信息
        /// </summary>
        public struct TreeListItemInfo
        {
            /// <summary>
            /// 结点名称
            /// </summary>
            public string itemName;
            /// <summary>
            /// 结点对应数据的类型,可为：table, comtable,folder等
            /// </summary>
            public string destType;
            /// <summary>
            /// 结点显示名称
            /// </summary>
            public string itemText;
            /// <summary>
            /// item的层次(在第几层结点)
            /// </summary>
            public int itemLevel;
            /// <summary>
            /// 正常图标索引
            /// </summary>
            public int imageIndex;
            /// <summary>
            /// 错误图标索引
            /// </summary>
            public int errorImageIndex;
            /// <summary>
            /// 数据信息表，刘文龙
            /// </summary>
            public string infoTblName;
        }


        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit CreateLookUpEdit(string[] values)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();

            DataTable dtTmp = new DataTable();
            dtTmp.Columns.Add("请选择");

            for (int i = 0; i < values.Length; i++)
            {
                DataRow drTmp1 = dtTmp.NewRow();
                drTmp1[0] = values[i];
                dtTmp.Rows.Add(drTmp1);
            }

            rEdit.DataSource = dtTmp;

            rEdit.ValueMember = "请选择";
            rEdit.DisplayMember = "请选择";
            rEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
            rEdit.ShowFooter = false;
            rEdit.ShowHeader = false;
            return rEdit;
        }

    }
}
