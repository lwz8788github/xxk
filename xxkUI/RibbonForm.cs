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

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        private List<string> userAut = new List<string>();
        public RibbonForm()
        {
            InitializeComponent();
            xtl = new XTreeList(this.treeListOriData, this.treeListWorkSpace);
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
                //XTreeList.InitOriDataTree(userAhtList);
                xtl.InitOriDataTree(userAhtList);
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

    }
}