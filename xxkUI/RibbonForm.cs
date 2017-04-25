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
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraEditors.Controls;
using xxkUI.MyCls;
using DevExpress.XtraTab;
using Steema.TeeChart;
using DevExpress.XtraGrid;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        private GMapMarkerKdcSite gmmkks;
        private List<string> userAut = new List<string>();
        private TreeBean currentClickNodeInfo;//当前点击的树节点信息
        private SiteAttri siteAttriFrm = new SiteAttri();
        public RibbonForm()
        {
            InitializeComponent();
            this.chartTabPage.PageVisible = false;//曲线图页面不可见
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
                xtl.InitOriDataTree(userAhtList,this.gmmkks);
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
            gmmkks.InitMap();
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

            GetSiteAttriForm();
            this.siteAttriFrm.SetDataSource(new List<SiteBean>() { sb });
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

        /// <summary>
        /// 树列表右击菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tree_MouseUp(object sender, MouseEventArgs e)
        {
            TreeList tree = sender as TreeList;
            if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None)&& (tree.State == TreeListState.Regular))
            {
                Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                TreeListHitInfo hitInfo = tree.CalcHitInfo(e.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    tree.SetFocusedNode(hitInfo.Node);

                    currentClickNodeInfo = tree.GetDataRecordByNode(hitInfo.Node) as TreeBean;
                    if (currentClickNodeInfo == null)
                    {
                        return;
                    }
                    if (hitInfo.Node.Level == 1)
                    {
                        popSiteTree.ShowPopup(p);
                    }
                    else if (hitInfo.Node.Level == 2)
                    {
                        popLineTree.ShowPopup(p);
                    }
                }
            }
        }

        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnSaveToWorkspace"://保存到工作区
                    break;
                case "btnChart"://趋势图
                    {

                        this.chartTabPage.PageVisible = true;//曲线图页面可见
                        this.xtraTabControl1.SelectedTabPage = this.chartTabPage;

                        tChart.Series.Clear();
                        tChart.Header.Text = "";

                        List<LineBean> checkedNodes = xtl.GetCheckedLine(this.treeListOriData.Name);

                        foreach (LineBean checkedLb in checkedNodes)
                        {
                            Steema.TeeChart.Styles.Line ln = new Steema.TeeChart.Styles.Line();
                            ln.Title = checkedLb.OBSLINENAME;
                            ln.YValues.DataMember = "obvvalue";
                            ln.LabelMember = "obvdate";
                            DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "'");
                            ln.DataSource = dt;
                            tChart.Series.Add(ln);
                        }
                      }
                    break;
                case "btnLineAttri"://测线属性
                    break;
                case "btnSiteLocation"://定位到地图
                    this.xtraTabControl1.SelectedTabPage = this.mapTabPage;
                    gmmkks.ZoomToSite((SiteBean)currentClickNodeInfo.Tag);
                    break;
                case "btnSiteAttri"://场地属性
                    {
                        GetSiteAttriForm();
                        this.siteAttriFrm.SetDataSource(new List<SiteBean>() { (SiteBean)currentClickNodeInfo.Tag });
                    }
                    break;

            }
        }


        /// <summary>
        /// 打开SiteAttri窗体
        /// </summary>
        private void GetSiteAttriForm()
        {
            if (siteAttriFrm != null)
            {
                if (siteAttriFrm.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    siteAttriFrm = new  SiteAttri();//此为你双击打开的FORM
                    siteAttriFrm.Show();
                    siteAttriFrm.Focus();
                }
                else
                {
                    siteAttriFrm.Show();
                    siteAttriFrm.Focus();
                }
            }
            else
            {
                siteAttriFrm = new SiteAttri();
                siteAttriFrm.Show();
                siteAttriFrm.Focus();
            }
           
        }

        #region Teechart鼠标交互操作

        //private Point start = new Point();//矩形起点
        //private Point end = new Point();//矩形终点
        //private bool blnDraw = false;//是否开始画矩形
        //Graphics g;

        //private void chartControl1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    g = this.tChart1.CreateGraphics();
        //    start.X = e.X;
        //    start.Y = e.Y;
        //    end.X = e.X;
        //    end.Y = e.Y;
        //    blnDraw = true;


        //}

        //private void chartControl1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (blnDraw)
        //    {
        //        //先擦除
        //        g.DrawRectangle(new Pen(Color.White), start.X, start.Y, end.X - start.X, end.Y - start.Y);
        //        end.X = e.X;
        //        end.Y = e.Y;
        //        //再画
        //        g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, end.X - start.X, end.Y - start.Y);
        //    }
        //}
        //private void chartControl1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, e.X - start.X, e.Y - start.Y);
        //    blnDraw = false;


        //    int minX = Math.Min(start.X, e.X);
        //    int minY = Math.Min(start.Y, e.Y);
        //    int maxX = Math.Max(start.X, e.X);
        //    int maxY = Math.Max(start.Y, e.Y);

        //    try
        //    {
        //        if (tChart1 != null)
        //        {
        //            if (tChart1.Series.Count > 0)
        //            {

        //                Steema.TeeChart.Styles.Series series = tChart1.Series[0];
        //               Steema.TeeChart.Styles.Line  ln = series as Steema.TeeChart.Styles.Line;
        //                this.tChart1.Refresh();
        //                for (int i = 0; i < ln.Count; i++)
        //                {
        //                    int screenX = series.CalcXPosValue(ln[i].X);
        //                    int screenY = series.CalcYPosValue(ln[i].Y);
        //                    if (screenX >= minX && screenX <= maxX && screenY >= minY && screenY <= maxY)
        //                    {
        //                        Rectangle r = new Rectangle(screenX - 4, screenY - 4, 10, 10);//标识圆的大小
        //                        g.DrawEllipse(new Pen(Color.Red), r);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        #endregion


        private void treeListOriData_AfterCheckNode(object sender, NodeEventArgs e)
        {
           
        }

        private void tChart_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {

        }

        private void chartTabPage_SizeChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Control cl in chartTabPage.Controls)
            {
                cl.Width = chartTabPage.Width;
                cl.Height = chartTabPage.Height / chartTabPage.Controls.Count;
                cl.Location = new Point(0, cl.Height * i);
                i++;
            }
            chartTabPage.Refresh();
        }
    }
}