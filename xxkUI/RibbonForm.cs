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

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm()
        {
            InitializeComponent();
            GmapInit();
        }


        private void btnLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            Login lg = new Login();
        
            if (lg.ShowDialog() == DialogResult.OK)
            {
                currentUser.Caption = currentUser.Caption + lg.Username;
            }
            else
            {
                return;
            }
        }


<<<<<<< HEAD
=======
        private void GmapInit()
        {
     
        }

        private void gMapCtrl_DoubleClick(object sender, EventArgs e)
        {
            this.gMapCtrl.Zoom += 1;

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
            this.gMapCtrl.Position = new PointLatLng(45.7543, 126.6281);
            //设置控件最大的缩放比例  
            this.gMapCtrl.MaxZoom = 50;
            //设置控件最小的缩放比例  
            this.gMapCtrl.MinZoom = 1;
            //设置控件当前的缩放比例  
            this.gMapCtrl.Zoom = 13;
            //创建一个新图层  
           GMapOverlay routes = new GMapOverlay("routes");
            GMapOverlay MyMark = new GMapOverlay("MyMark");
            routes.IsVisibile = true;//可以显示  
            MyMark.IsVisibile = true;
            this.gMapCtrl.Overlays.Add(routes);//添加到图层列表中  
            this.gMapCtrl.Overlays.Add(MyMark);
        

           // GMapMarker gmm =new G


           
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

     
>>>>>>> 23097dd35e7a0327aff6aebe80f332bc171f71b2
    }
}