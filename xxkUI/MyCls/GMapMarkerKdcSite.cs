using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GsProject;
using xxkUI.GsProject;
using System.Threading;

namespace xxkUI.MyCls
{
    public static class GMapMarkerKdcSite    {
        /// <summary>
        /// 地图中心点
        /// </summary>
        private static readonly PointLatLng chinaCenter = new PointLatLng(35, 107.5);

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <returns></returns>
        public static bool InitMap(GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            bool isOk = false;
            try
            {
                //离线模式
                gMapCtrl.Manager.Mode = AccessMode.CacheOnly;
                //设置控件显示的地图来源  
                gMapCtrl.MapProvider = GMapProviders.GoogleChinaMap;
                //gmdb离线地图数据
                string mapfile = System.Windows.Forms.Application.StartupPath + "\\GisMap\\Data.gmdb";
                //载入离线地图
                MapManagerLoader.Instance.Load(mapfile);

                //不显示中心十字点  
                gMapCtrl.ShowCenter = false;
                //右键拖拽地图  
                gMapCtrl.DragButton = System.Windows.Forms.MouseButtons.Left;

                //设置控件显示的当前中心位置
                gMapCtrl.Position = chinaCenter;
                //设置控件最大的缩放比例
                gMapCtrl.MaxZoom = 50;
                //设置控件最小的缩放比例
                gMapCtrl.MinZoom = 4;
                //设置控件当前的缩放比例
                gMapCtrl.Zoom = 4;


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
        public static void ReloadMap(GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            gMapCtrl.ReloadMap();
        }

        /// <summary>
        /// 地图缩放
        /// </summary>
        /// <param name="scale">缩放级别</param>
        public static void Zoom(int scale, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            gMapCtrl.Zoom += scale;
        }

        /// <summary>
        /// 全图
        /// </summary>
        public static void Full(GMap.NET.WindowsForms.GMapControl gMapCtrl)
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
        public static PointLatLng FromLocalToLatLng(int x, int y, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            return gMapCtrl.FromLocalToLatLng(x, y);
        }

        /// <summary>
        /// 加载场地标记
        /// </summary>
        /// <param name="sblist"></param>
        public static void LoadSiteMarker(IEnumerable<SiteBean> sblist, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
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

                marker.ToolTipText = sb.SiteName;
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                SiteOverlay.Markers.Add(marker);

            }
            gMapCtrl.Overlays.Add(SiteOverlay);

            gMapCtrl.Zoom += 1;
            //gMapCtrl.Refresh();
           
        }

        /// <summary>
        /// 清除所有场地
        /// </summary>
        public static void ClearAllSiteMarker(GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            for (int i = 0; i < gMapCtrl.Overlays.Count; i++)
            {
                if (gMapCtrl.Overlays[i].Id == "sitemarkers")
                {
                    gMapCtrl.Overlays[i].Markers.Clear();
                }
            }
        }

        /// <summary>
        /// 定位到场地
        /// </summary>
        /// <param name="sb"></param>
        public static void ZoomToSite(SiteBean sb, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            PointLatLng sitepoint = new PointLatLng(sb.Latitude, sb.Longtitude);
            gMapCtrl.Position = sitepoint;
            gMapCtrl.Zoom = 10;
        }

        /// <summary>
        /// 清除所有地震标注
        /// </summary>
        /// <param name="gMapCtrl"></param>
        public static void ClearAllEqkMarker(GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            for (int i = 0; i < gMapCtrl.Overlays.Count; i++)
            {
                if (gMapCtrl.Overlays[i].Id == "eqkmarkers")
                {
                    gMapCtrl.Overlays[i].Markers.Clear();
                }
                if (gMapCtrl.Overlays[i].Id == "circleOverlay")
                {
                    gMapCtrl.Overlays[i].Polygons.Clear();
                }
            }
        }

        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="circleCenter">圆心</param>
        /// <param name="r">半径</param>
        public static void CreateCircle(PointLatLng circleCenter,double radius, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {
            radius = radius * 1000;
            GSCoordConvertionClass_Xian80 cc = new GSCoordConvertionClass_Xian80();
            cc.IsBigNumber = true;
            cc.Strip = EnumStrip.Strip3;
            cc.L0 = decimal.Parse(circleCenter.Lng.ToString());
            decimal x = decimal.MinValue, y = decimal.MinValue;
            cc.GetXYFromBL(decimal.Parse(circleCenter.Lat.ToString()), decimal.Parse(circleCenter.Lng.ToString()), ref x, ref y);

            GPoint gp = gMapCtrl.FromLatLngToLocal(circleCenter);
           

            List<PointLatLng> gpollist = new List<PointLatLng>();

            double seg = Math.PI * 2 / 100;

            for (int i = 0; i < 100; i++)
            {
                double theta = seg * i;
                decimal a = decimal.Parse((double.Parse(x.ToString()) + Math.Cos(theta) * radius).ToString());
                decimal b = decimal.Parse((double.Parse(y.ToString()) + Math.Sin(theta) * radius).ToString());

                decimal B = decimal.MinValue, L = decimal.MinValue;
                cc.GetBLFromXY(a, b, ref B, ref L);

                PointLatLng gpoi = new PointLatLng(double.Parse(B.ToString()), double.Parse(L.ToString()));
                gpollist.Add(gpoi);
            }
            GMapPolygon gpol = new GMapPolygon(gpollist, "circlePolygon");

            gpol.Fill = new SolidBrush(Color.FromArgb(50, 0, 155, 255));
            gpol.Stroke = new Pen(Color.FromArgb(50, 0, 155, 255), 0);

            GMapOverlay CircleOverlay = new GMapOverlay("circleOverlay");
            CircleOverlay.Polygons.Add(gpol);
            gMapCtrl.Overlays.Add(CircleOverlay);


        }



        /// <summary>
        /// Map标注地震
        /// </summary>
        public static void AnnotationEqkToMap(List<EqkBean> eqkList, GMap.NET.WindowsForms.GMapControl gMapCtrl)
        {

            GMapOverlay EqkOverlay = null;
            for (int i = 0; i < gMapCtrl.Overlays.Count; i++)
            {
                if (gMapCtrl.Overlays[i].Id == "eqkmarkers")
                {
                    EqkOverlay = gMapCtrl.Overlays[i];
                }
            }

            if (EqkOverlay==null)
            {
                 EqkOverlay = new GMapOverlay("eqkmarkers");
                gMapCtrl.Overlays.Add(EqkOverlay);
            }

            GMapMarker marker = null;
            string picName = "";
            for (int i = 0; i < eqkList.Count; i++)
            {
                switch ((int)eqkList[i].Magntd)
                {
                    case 0: picName = "2.png";
                        break;
                    case 1: picName = "2.png";
                        break;
                    case 2: picName = "2.png";
                        break;
                    case 3: picName = "3.png";
                        break;
                    case 4: picName = "4.png";
                        break;
                    case 5: picName = "5.png";
                        break;
                    case 6: picName = "6.png";
                        break;
                    case 7: picName = "7.png";
                        break;
                    case 8: picName = "8.png";
                        break;
                    case 9: picName = "9.png";
                        break;
                }

                string picPath = System.Windows.Forms.Application.StartupPath + "//地震标注图片//" + picName;
                Bitmap eqkDotPic = new Bitmap(picPath);
                marker = new GMarkerGoogle(new PointLatLng(eqkList[i].Latitude, eqkList[i].Longtitude), eqkDotPic);
                marker.Tag = eqkList[i];

                marker.ToolTipText = "震级："+eqkList[i].Magntd + "\r\n时间："+eqkList[i].EakDate.ToString() + "\r\n地点：" + eqkList[i].Place;
             
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                marker.ToolTip.Format.Alignment = StringAlignment.Near;

                EqkOverlay.Markers.Add(marker);
            }

            gMapCtrl.Position = chinaCenter;
            gMapCtrl.Zoom = 4;
            //gMapCtrl.Refresh();

        }

 



        ///// <summary>
        ///// 创建标注地震圆点集
        ///// </summary>
        //private List<PointLatLng> createPoint(PointF point, double radi, int numP)
        //{
        //    List<PointLatLng> gPolList = new List<PointLatLng>();
        //    double seg = Math.PI * 2.0 / numP;

        //    for (int i=0;i<numP;i++)
        //    {
        //        double theta = seg * i;
        //        double aValue = point.X + Math.Cos(theta) * radi;
        //        double bValue = point.Y + Math.Sin(theta) * radi;

        //        PointLatLng GPOI = new PointLatLng(aValue, bValue);
        //        gPolList.Add(GPOI);
        //    }
        //    return gPolList;
        //} 

    }




    public class MapManagerLoader
    {
        private static readonly MapManagerLoader _instance = new MapManagerLoader();

        public static MapManagerLoader Instance
        {
            get { return _instance; }
        }

        private MapManagerLoader()
        {
        }

        private bool _isLoaded;

        public bool Load(string fileName)
        {
            if (!_isLoaded)
            {
                new Thread(() => GMaps.Instance.ImportFromGMDB(fileName)).Start();
                _isLoaded = true;
            }
            return _isLoaded;
        }
    }
}
