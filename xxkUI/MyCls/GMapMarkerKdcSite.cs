using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace xxkUI.MyCls
{
    public class GMapMarkerKdcSite    {
        /// <summary>
        /// 地图中心点
        /// </summary>
        private readonly PointLatLng chinaCenter = new PointLatLng(35, 107.5);
        private GMap.NET.WindowsForms.GMapControl gMapCtrl;

        public GMapMarkerKdcSite(GMap.NET.WindowsForms.GMapControl _gmapctrl)
        {
            gMapCtrl = _gmapctrl;
        }

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <returns></returns>
        public bool InitMap()
        {
            bool isOk = false;
            try
            {
                this.gMapCtrl.BackColor = Color.Red;
                //设置控件的管理模式  
                this.gMapCtrl.Manager.Mode = AccessMode.ServerAndCache;
                //设置控件显示的地图来源  
                this.gMapCtrl.MapProvider = GMapProviders.GoogleChinaMap;
                //设置控件显示的当前中心位置  
                //31.7543, 121.6281  
                this.gMapCtrl.Position = chinaCenter;
                //设置控件最大的缩放比例  
                this.gMapCtrl.MaxZoom = 50;
                //设置控件最小的缩放比例  
                this.gMapCtrl.MinZoom = 2;
                //设置控件当前的缩放比例  
                this.gMapCtrl.Zoom = 4;

                isOk = true;
            }

            catch (Exception ex)
            {
                isOk = false;
                throw;
            }

            return isOk;
        }
            
        /// <summary>
        /// 重载地图
        /// </summary>
        public  void ReloadMap()
        {
            gMapCtrl.ReloadMap();
        }

        /// <summary>
        /// 地图缩放
        /// </summary>
        /// <param name="scale">缩放级别</param>
        public void Zoom(int scale)
        {
            gMapCtrl.Zoom += scale;
        }

        /// <summary>
        /// 全图
        /// </summary>
        public void Full()
        {
            gMapCtrl.Position = chinaCenter;
            //设置控件当前的缩放比例  
            gMapCtrl.Zoom = 4;
        }

        /// <summary>
        /// 坐标转经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PointLatLng FromLocalToLatLng(int x, int y)
        {
            return this.gMapCtrl.FromLocalToLatLng(x, y);
        }

        /// <summary>
        /// 加载场地标记
        /// </summary>
        /// <param name="sblist"></param>
        public void LoadSiteMarker(IEnumerable<SiteBean> sblist)
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapOverlay SiteOverlay = new GMapOverlay("sitemarkers");

            foreach (SiteBean sb in sblist)
            {
                GMapMarker marker = null;
                if (sb.SiteCode.Substring(0, 1) == "L")
                {
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.green_small);
                }
                else
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), GMarkerGoogleType.red_small);

                marker.Tag = sb;
                SiteOverlay.Markers.Add(marker);

            }
            gMapCtrl.Overlays.Add(SiteOverlay);

            gMapCtrl.Zoom += 1;
            gMapCtrl.Refresh();
           
        }

        /// <summary>
        /// 定位到场地
        /// </summary>
        /// <param name="sb"></param>
        public void ZoomToSite(SiteBean sb)
        {
            PointLatLng sitepoint = new PointLatLng(sb.Latitude, sb.Longtitude);
            gMapCtrl.Position = sitepoint;
            gMapCtrl.Zoom = 10;
        }
    }
}
