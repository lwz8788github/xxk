using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace xxkUI.Tool
{
  public  class PublicHelper
    {
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
