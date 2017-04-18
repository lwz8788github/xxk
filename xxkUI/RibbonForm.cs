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

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public RibbonForm()
        {
            InitializeComponent();
            GmapInit();
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
                currentUserBar.Caption = currentUserBar.Caption + lg.Username;
            }
            else
            {
                return;
            }
        }


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

            LoadSiteMarker();


        }


        private void LoadSiteMarker()
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapOverlay SiteOverlay = new GMapOverlay("sitemarkers");
            GMapMarker marker = new GMarkerGoogle(new PointLatLng(48.8617774, 2.349272),GMarkerGoogleType.blue_pushpin);

            SiteOverlay.Markers.Add(marker);
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
            IEnumerable<SiteBean> sblist = null;


            this.treeListOriData.KeyFieldName = "SiteCode";　　　　      //这里绑定的ID的值必须是独一无二的
            this.treeListOriData.ParentFieldName = "UnitCode";　　//表示使用parentID进行树形绑定
            this.treeListOriData.DataSource = sblist;　　//绑定数据源
            this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
        }
    }
}