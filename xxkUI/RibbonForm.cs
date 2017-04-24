using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using xxkUI.Form;
using System.Configuration;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Collections;
using xxkUI.Bll;
using DevExpress.XtraTreeList.Nodes;
using xxkUI.Model;
using System.Reflection;
using xxkUI.Tool;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using xxkUI.BLL;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraEditors.Controls;
using xxkUI.MyGMap;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        GMapMarkerKdcSite gmmkks;
        private List<string> userAut = new List<string>();
        public RibbonForm()
        {
            InitializeComponent();
            xtl = new XTreeList(this.treeListOriData, this.treeListWorkSpace);
            gmmkks = new GMapMarkerKdcSite(this.gMapCtrl);
            InitFaultCombobox();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
        {

            Login lg = new Login();

            if (lg.ShowDialog() == DialogResult.OK)
            {
                currentUserBar.Caption = currentUserBar.Caption.Split(':')[0] + lg.Username;

                //获取用户权限，放入userAut
                List<string> userAhtList = UserInfoBll.Instance.GetAthrByUser<UserInfoBean>(lg.Username);
   
                xtl.InitOriDataTree(userAhtList);
            }
            else
            {
                return;
            }
        }

        #region 地图事件 刘文龙

        /// <summary>
        /// 地图加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_Load(object sender, EventArgs e)
        {
            if (gmmkks.InitMap())
                gmmkks.LoadSiteMarker(SiteBll.Instance.GetAll());
                   }
        private void gMapCtrl_DoubleClick(object sender, EventArgs e)
        {
            gmmkks.Zoom(1);

        }

        private void gMapCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng latLng = gmmkks.FromLocalToLatLng(e.X, e.Y);
            this.currentLocation.Caption = string.Format("经度：{0}, 纬度：{1} ", latLng.Lng, latLng.Lat);
        }


        private void gMapCtrl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            SiteBean sb = (SiteBean)item.Tag;
            sb.SiteMapFile = SiteBll.Instance.GetBlob<SiteBean>("sitecode", sb.SiteCode, "SiteMapFile");
            sb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";

            this.vGridControlSiteInfo.DataSource = new List<SiteBean>() { sb };
            SetBaseinfoVGridControl();
        }

        private void btnFull_ItemClick(object sender, ItemClickEventArgs e)
        {
            gmmkks.Full();
        }

        private void btnZoomout_ItemClick(object sender, ItemClickEventArgs e)
        {
            gmmkks.Zoom(1);
        }

        private void btnZoomin_ItemClick(object sender, ItemClickEventArgs e)
        {
            gmmkks.Zoom(-1);

        }

        private void btnReloadMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            gmmkks.ReloadMap();
        }


        #endregion

        /// <summary>
        /// 设置是VGridControl行列样式
        /// </summary>
        private void SetBaseinfoVGridControl()
        {
            try
            {
                int cHeight = vGridControlSiteInfo.Height;

                DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
                memoEdit.LinesCount = 1;
                DevExpress.XtraEditors.Repository.RepositoryItemImageEdit imgEdit = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
                imgEdit.ShowIcon = true;


                for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                {
                    vGridControlSiteInfo.Rows[i].Properties.ReadOnly = true;
                    vGridControlSiteInfo.Rows[i].Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;

                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (i != 0)
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = memoEdit;
                    else
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = imgEdit;

                    vGridControlSiteInfo.Rows[i].Height = (cHeight) / vGridControlSiteInfo.Rows.Count;
                }

                vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
                vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 10;
                //vGridControlSiteInfo.Rows[0].Height = vGridControlSiteInfo.Width / 3 * 2 - 10;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

         
            vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
            vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 10;

        }
        private void dockPanel2_SizeChanged(object sender, EventArgs e)
        {
            SetBaseinfoVGridControl();

        }

        private void vGridControlSiteInfo_CustomDrawRowValueCell(object sender, DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventArgs e)
        {
            if (e.Row.Properties.FieldName == "UnitCode")
            {
                if (e.CellText != "")
                {
                    string unitname = UnitInfoBll.Instance.GetUnitNameBy(e.CellText);
                    e.CellText = unitname;
                }
            }
        }

        /// <summary>
        /// 初始化断层数据下拉列表
        /// </summary>
        private void InitFaultCombobox()
        {
            try
            {

                List<string> districtlist = new List<string>() {
                    "前第四纪活动断裂(隐伏)",
                    "全新世活动断裂(非隐伏)",
                    "全新世活动断裂(隐伏)",
                    "晚更新世活动断裂(非隐伏)",
                    "晚更新世活动断裂(隐伏)",
                    "早第四纪活动断裂(Q12)(非隐伏)",
                    "早第四纪活动断裂(Q12)(隐伏)",
                    "早第四纪活动断裂(Q1)(非隐伏)",
                    "早第四纪活动断裂(Q1)(隐伏)",
                    "早第四纪活动断裂(Q2)(非隐伏)",
                    "早第四纪活动断裂(Q2)(隐伏)"};

                this.faultChckCbbxEdit.Items.Clear();

                CheckedListBoxItem[] itemListQuery = new CheckedListBoxItem[districtlist.Count];
                int check = 0;
                foreach (string det in districtlist)
                {
                    itemListQuery[check] = new CheckedListBoxItem(det);
                    check++;
                }
                this.faultChckCbbxEdit.Items.AddRange(itemListQuery);
                this.faultChckCbbxEdit.AllowMultiSelect = true;
                this.faultChckCbbxEdit.SelectAllItemVisible = true;
                this.faultChckCbbxEdit.SelectAllItemCaption = "全选";
                for (int i = 0; i < this.faultChckCbbxEdit.Items.Count; i++)
                {
                    this.faultChckCbbxEdit.Items[i].CheckState = CheckState.Checked;
                }
                barFault.EditValue = faultChckCbbxEdit.GetCheckedItems();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show("初始化断层数据发生错误：" + ex.Message, "错误");
            }
        }

        private void dockPanelWorkSpace_Click(object sender, EventArgs e)
        {

        }
    }
}