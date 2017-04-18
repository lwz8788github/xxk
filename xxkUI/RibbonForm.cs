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
using xxkUI.BLL;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm()
        {
            InitializeComponent();
          
            InitOriDataTree();
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
            }
            else
            {
                return;
            }
        }


    

       

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



        private void LoadSiteMarker()
        {
            IEnumerable<SiteBean> sblist = SiteBll.Instance.GetAll();

            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapOverlay SiteOverlay = new GMapOverlay("sitemarkers");

            foreach (SiteBean sb in sblist)
            {
                GMapMarker marker = null;

                if (sb.SiteCode.Substring(0,1) == "L")
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.green_pushpin);
                else if (sb.SiteCode.Substring(0, 1) == "D")
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.red_pushpin);

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


        private void InitOriDataTree()
        {
            
            List<TreeBean> treelist = new List<TreeBean>();
            IEnumerable<UnitInfoBean> ubEnumt = UnitInfoBll.Instance.GetAll();
           
            foreach (UnitInfoBean sb in ubEnumt)
            {
                TreeBean tb = new TreeBean();
                tb.KeyFieldName = sb.UnitCode;
                tb.ParentFieldName = "0";
                tb.Caption = sb.UnitName;
                tb.SiteType = "";
                tb.SiteStatus = "";
                treelist.Add(tb);
            }

            IEnumerable<SiteBean> sbEnumt = SiteBll.Instance.GetAll();
            foreach (SiteBean sb in sbEnumt)
            {
                TreeBean tb = new TreeBean();
                tb.KeyFieldName = sb.SiteCode;
                tb.ParentFieldName = sb.UnitCode;
                tb.Caption = sb.SiteName;
                tb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                tb.SiteStatus = sb.SiteStatus=="0"?"正常":(sb.SiteStatus=="1"?"废弃":"改造中");
                treelist.Add(tb);
            }

            this.treeListOriData.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
            this.treeListOriData.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定
   
            this.treeListOriData.DataSource = treelist;　　//绑定数据源
            this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
            this.treeListOriData.OptionsView.ShowCheckBoxes = true;

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
    }
}