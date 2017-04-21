﻿using System;
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

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        private List<string> userAut = new List<string>();
        public RibbonForm()
        {
            InitializeComponent();
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
                //List<string> siteCodeList = new List<string>();
                //foreach (string u_list in userAhtList)
                //{
                //    ;
                //}
                InitOriDataTree(userAhtList);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 地图加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_Load(object sender, EventArgs e)
        {
            this.gMapCtrl.BackColor = Color.Red;
            //设置控件的管理模式  
            this.gMapCtrl.Manager.Mode = AccessMode.ServerAndCache;
            //设置控件显示的地图来源  
            this.gMapCtrl.MapProvider = GMapProviders.GoogleChinaMap;
            //设置控件显示的当前中心位置  
            //31.7543, 121.6281  
            this.gMapCtrl.Position = new PointLatLng(35, 107.5);
            //设置控件最大的缩放比例  
            this.gMapCtrl.MaxZoom = 50;
            //设置控件最小的缩放比例  
            this.gMapCtrl.MinZoom = 2;
            //设置控件当前的缩放比例  
            this.gMapCtrl.Zoom = 4;

            LoadSiteMarker();

        }


        /// <summary>
        /// 添加场地标记
        /// </summary>
        private void LoadSiteMarker()
        {
            IEnumerable<SiteBean> sblist = SiteBll.Instance.GetAll();

            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapOverlay SiteOverlay = new GMapOverlay("sitemarkers");

            foreach (SiteBean sb in sblist)
            {
                GMapMarker marker = null;

                if (sb.SiteCode.Substring(0,1) == "L")
                { 
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.green_small);
                   
                }
                else if (sb.SiteCode.Substring(0, 1) == "D")
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.red_small);
                marker.Tag = sb;
                SiteOverlay.Markers.Add(marker);

               
            }
           
            gMapCtrl.Overlays.Add(SiteOverlay);

      
        }

        private void btnZoomout_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.gMapCtrl.Zoom += 1;
        }

        private void btnZoomin_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.gMapCtrl.Zoom -= 1;
        }

        private void btnReloadMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.gMapCtrl.ReloadMap();
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
                foreach (SiteBean sb in sbEnumt)
                {
                    if(userAhtList.Contains(sb.UnitCode))
                    { 
                        TreeBean tb = new TreeBean();
                        tb.KeyFieldName = sb.SiteCode;
                        tb.ParentFieldName = sb.UnitCode;
                        tb.Caption = sb.SiteName;
                        tb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                   
                        treelist.Add(tb);
                    }
                }

                //测线列表显示
                IEnumerable<LineBean> olEnumt = LineBll.Instance.GetAll();

                foreach (LineBean ol in olEnumt)
                {
                    if (userAhtList.Contains(ol.SITECODE))
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
                this.treeListOriData.CustomDrawNodeCell += treeListOriData_CustomDrawNodeCell;
                this.treeListOriData.BeforeCheckNode += treeListOriData_BeforeCheckNode_1;

                //工作区树列表显示
                this.treeListWorkSpace.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListWorkSpace.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定

                this.treeListWorkSpace.DataSource = treelist;　　//绑定数据源
                //this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
                this.treeListWorkSpace.OptionsView.ShowCheckBoxes = true;
                //this.treeListWorkSpace.Enabled = false;
                this.treeListWorkSpace.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListWorkSpace.OptionsBehavior.Editable = false;
                this.treeListWorkSpace.CustomDrawNodeCell += treeListWorkSpace_CustomDrawNodeCell;
                this.treeListWorkSpace.BeforeCheckNode += treeListWorkSpace_BeforeCheckNode;

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


        private void gMapCtrl_DoubleClick(object sender, EventArgs e)
        {
            this.gMapCtrl.Zoom += 1;

        }

        private void gMapCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng latLng = this.gMapCtrl.FromLocalToLatLng(e.X, e.Y);
            this.currentLocation.Caption = string.Format("经度：{0}, 纬度：{1} ", latLng.Lng, latLng.Lat);
        }


        private void gMapCtrl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            SiteBean sb = (SiteBean)item.Tag;

            ModelHandler<SiteBean> mh = new ModelHandler<SiteBean>();
            
            this.vGridControlSiteInfo.DataSource = mh.FillDataTable(new List<SiteBean>() { sb });

            SetBaseinfoVGridControl(sb.SiteCode);
        }


        /// <summary>
        /// 设置是VGridControl行列样式
        /// </summary>
        private void SetBaseinfoVGridControl(string sitecode)
        {
            int cHeight = vGridControlSiteInfo.Height;

            DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            memoEdit.LinesCount = 1;
            DevExpress.XtraEditors.Repository.RepositoryItemImageEdit imgEdit = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();

            ModelHandler<SiteBean> hm = new ModelHandler<SiteBean>();


            for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
            {
                vGridControlSiteInfo.Rows[i].Properties.ReadOnly = true;
                vGridControlSiteInfo.Rows[i].Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;

                vGridControlSiteInfo.Rows[i].Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                vGridControlSiteInfo.Rows[i].Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                if (i == 0)
                {
                    MemoryStream ms = new MemoryStream(SiteBll.Instance.GetBlob<SiteBean>("sitecode", sitecode, "SiteMapFile"));
                    Image image = Image.FromStream(ms);
                    imgEdit.ContextImage = image;
                    vGridControlSiteInfo.Rows[i].Properties.RowEdit = imgEdit;

                }
                else
                    vGridControlSiteInfo.Rows[i].Properties.RowEdit = memoEdit;

                vGridControlSiteInfo.Rows[i].Height = (cHeight - 10) / vGridControlSiteInfo.Rows.Count;
            }
            
            vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
            vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 10;
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

        
        private void treeListOriData_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeListColumn1)
            {
                if (e.CellValue.ToString()!="")
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.Options.UseBackColor = true;
                }
            }
        }

        private void treeListWorkSpace_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Column == treeListColumn4)
            {

                if (e.CellValue.ToString() != "")
                {
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.Options.UseBackColor = true;
                }
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
            e.CanCheck = false;
            //if ((bool)sender)
            //{
            //    e.CanCheck = true;
            //}
            
        }

        private void treeListOriData_BeforeCheckNode_1(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = false;
            //if ((bool) sender)
            //{
            //    e.CanCheck = true;
            //}
        }


        public TreeListNode a { get; set; }
    }
}