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
        GMapMarkerKdcSite gmmkks;
        private List<string> userAut = new List<string>();
        public RibbonForm()
        {
            InitializeComponent();
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
                InitOriDataTree(userAhtList);
            }
            else
            {
                return;
            }
        }





        private void InitOriDataTree(List<string> userAhtList)
        {
            try
            {
                List<TreeBean> treelist = new List<TreeBean>();
                IEnumerable<UnitInfoBean> ubEnumt = UnitInfoBll.Instance.GetAll();

                foreach (UnitInfoBean sb in ubEnumt)
                {
                    TreeBean tb = new TreeBean();
                    if (sb.UnitCode == "152002" || sb.UnitCode == "152003"
                        || sb.UnitCode == "152006" || sb.UnitCode == "152008"
                        || sb.UnitCode == "152009" || sb.UnitCode == "152010"
                        || sb.UnitCode == "152012" || sb.UnitCode == "152015"
                        || sb.UnitCode == "152022" || sb.UnitCode == "152023"
                        || sb.UnitCode == "152026" || sb.UnitCode == "152029"
                        || sb.UnitCode == "152032" || sb.UnitCode == "152034"
                        || sb.UnitCode == "152035" || sb.UnitCode == "152036"
                        || sb.UnitCode == "152039" || sb.UnitCode == "152040"
                        || sb.UnitCode == "152041" || sb.UnitCode == "152042"
                        || sb.UnitCode == "152043" || sb.UnitCode == "152044"
                        || sb.UnitCode == "152045" || sb.UnitCode == "152046"
                        || sb.UnitCode == "152001" || sb.UnitCode == "152047") { continue; }
                    if (userAhtList.Contains(sb.UnitCode))
                    {
                        tb.KeyFieldName = sb.UnitCode;
                        tb.ParentFieldName = "0";
                        tb.Caption = sb.UnitName;
                        tb.SiteType = "";
                        tb.LineStatus = "";
                        treelist.Add(tb);
                    }
                }

                IEnumerable<SiteBean> sbEnumt = SiteBll.Instance.GetAll();
                List<string> siteCodeList = new List<string>();
                foreach (SiteBean sb in sbEnumt)
                {

                    if (userAhtList.Contains(sb.UnitCode))
                    {

                        if (userAhtList.Contains(sb.UnitCode))
                        {
                            siteCodeList.Add(sb.SiteCode);

                            TreeBean tb = new TreeBean();
                            tb.KeyFieldName = sb.SiteCode;
                            tb.ParentFieldName = sb.UnitCode;
                            tb.Caption = sb.SiteName;
                            tb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";

                            treelist.Add(tb);
                        }
                    }
                }
                //测线列表显示
                IEnumerable<LineBean> olEnumt = LineBll.Instance.GetAll();

                foreach (LineBean ol in olEnumt)
                {
                    if (siteCodeList.Contains(ol.SITECODE))
                    {
                        TreeBean tb = new TreeBean();
                        tb.KeyFieldName = ol.OBSLINECODE;
                        tb.ParentFieldName = ol.SITECODE;
                        tb.Caption = ol.OBSLINENAME;
                        tb.LineStatus = ol.LineStatus == "0" ? "正常" : (ol.LineStatus == "1" ? "停测" : "改造中");
                        treelist.Add(tb);
                    }
                }

                //原始数据树列表显示

                this.treeListOriData.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListOriData.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定

                this.treeListOriData.DataSource = treelist;　　//绑定数据源
                //this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
                this.treeListOriData.OptionsView.ShowCheckBoxes = true;
                this.treeListOriData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListOriData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListOriData.OptionsBehavior.Editable = false;
                //this.treeListOriData.CustomDrawNodeCell += treeListOriData_CustomDrawNodeCell;
                //this.treeListOriData.BeforeCheckNode += treeListOriData_BeforeCheckNode_1;

                //工作区树列表显示
                this.treeListWorkSpace.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListWorkSpace.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定

                this.treeListWorkSpace.DataSource = treelist;　　//绑定数据源
                //this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
                this.treeListWorkSpace.OptionsView.ShowCheckBoxes = true;
                //this.treeListWorkSpace.Enabled = false;
                this.treeListWorkSpace.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListWorkSpace.OptionsBehavior.Editable = false;
                //this.treeListWorkSpace.CustomDrawNodeCell += treeListWorkSpace_CustomDrawNodeCell;
                //this.treeListWorkSpace.BeforeCheckNode += treeListWorkSpace_BeforeCheckNode;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }
        private void GetObsDataByUser(string username)
        {
            //1.根据username查询权限，并放入list中
            List<string> userlist = new List<string>();
            // userlist = ;
            //2.遍历list下载数据
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
        }


        

        
        /// <summary>
        /// 禁止操作节点CheckBox
        /// 说明
        /// 在BeforeCheckNode事件中使用
        /// </summary>
        /// <param name="tree">TreeListNode</param>
        /// <param name="conditionHanlder">委托</param>
        /// <param name="e">CheckNodeEventArgs</param>
        private void treeListWorkSpace_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = true;
            //if ((bool)sender)
            //{
            //    e.CanCheck = true;
            //}

        }

        private void treeListOriData_BeforeCheckNode_1(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = true;
            //if ((bool) sender)
            //{
            //    e.CanCheck = true;
            //}
        }


        public TreeListNode a { get; set; }
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