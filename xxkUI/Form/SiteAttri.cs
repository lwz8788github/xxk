using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

namespace xxkUI.Form
{
    public partial class SiteAttri : DevExpress.XtraEditors.XtraForm
    {
        public SiteAttri()
        {
           InitializeComponent();
        }
        public void SetDataSource(List<SiteBean> dataSource)
        {
            this.vGridControlSiteInfo.DataSource = dataSource;
            SetBaseinfoVGridControl();
        }

        /// 设置是VGridControl行列样式
        /// </summary>
        private void SetBaseinfoVGridControl()
        {
            try
            {
                int cHeight = vGridControlSiteInfo.Height;

                DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
               
                //DevExpress.XtraEditors.Repository.RepositoryItemImageEdit imgEdit = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
                //imgEdit.ShowIcon = true;




                RepositoryItemPictureEdit pictureEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
                pictureEdit.SizeMode = PictureSizeMode.Zoom;
                pictureEdit.NullText = " ";




                for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                {
                    vGridControlSiteInfo.Rows[i].Properties.ReadOnly = true;
                    vGridControlSiteInfo.Rows[i].Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;

                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (i != 0)
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = memoEdit;
                    else
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = pictureEdit;

                    vGridControlSiteInfo.Rows[i].Height = (cHeight) / vGridControlSiteInfo.Rows.Count;
                }

                //vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
                //vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 20;
                //vGridControlSiteInfo.Rows[0].Height = vGridControlSiteInfo.Width / 3 * 2 - 10;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }


            vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
            vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 20;

        }

        private void SiteAttri_SizeChanged(object sender, EventArgs e)
        {
            SetBaseinfoVGridControl();
        }
    }
}