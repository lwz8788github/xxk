﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using xxkUI.Form;
using GMap.NET;
using GMap.NET.WindowsForms;
using xxkUI.Bll;
using xxkUI.Model;
using xxkUI.Tool;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using xxkUI.BLL;
using DevExpress.XtraEditors.Controls;
using xxkUI.MyCls;
using DevExpress.XtraTab;
using Steema.TeeChart;
using DevExpress.XtraGrid;
using System.Configuration;
using Common.Data.MySql;


namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        private GMapMarkerKdcSite gmmkks;
        private List<string> userAut = new List<string>();
        private TreeBean currentClickNodeInfo;//当前点击的树节点信息
        private SiteAttri siteAttriFrm = new SiteAttri();
        private List<string> importDataFiles = new List<string>();//导入数据的文件路径集

        private MyTeeChart mtc = null;
        public RibbonForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;//默认最大化窗体
            this.chartTabPage.PageVisible = false;//曲线图页面不可见
            this.siteInfoTabPage.PageVisible = false;//文档页面不可见
            mtc = new MyTeeChart(this.chartGroupBox);
            xtl = new XTreeList(this.treeListRemoteData, this.treeListLocalData);
            gmmkks = new GMapMarkerKdcSite(this.gMapCtrl);
            InitFaultCombobox();
            MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["OrigInfoConnnect"].ConnectionString;
            xtl.bSignInitOriDataTree(this.gmmkks);

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
                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                {
                    currentUserBar.Caption = currentUserBar.Caption.Split(':')[0] + lg.Username;

                    //获取用户权限，放入userAut
                    List<string> userAhtList = UserInfoBll.Instance.GetAthrByUser<UserInfoBean>(lg.Username);
                    xtl.InitOriDataTree(userAhtList, this.gmmkks);
                   
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                xtl.ClearTreelistNodes();
                gmmkks.ClearAllSiteMarker();
                currentUserBar.Caption = "当前用户:";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("注销登录过程发生错误：" + ex.Message, "错误");
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
            //sb.SiteMapFile = SiteBll.Instance.GetBlob<SiteBean>("sitecode", sb.SiteCode, "SiteMapFile");
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
                case "btnSaveToWorkspace"://保存到处理数据缓存
                    {
                        string folderName = "处理数据缓存";
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            List<LineBean> checkedNodes = xtl.GetCheckedLine(this.treeListRemoteData.Name);
                            foreach (LineBean checkedLb in checkedNodes)
                            {
                                DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "'");

                                NpoiCreator npcreator = new NpoiCreator();
                                string savefile = Application.StartupPath + "/" + folderName;
                                npcreator.TemplateFile = savefile;
                                npcreator.NpoiExcel(dt, checkedLb.OBSLINECODE + ".xls", savefile + "/" + checkedLb.OBSLINECODE + ".xls");

                                TreeBean tb = new TreeBean();

                                tb.KeyFieldName = checkedLb.OBSLINECODE;
                                tb.ParentFieldName = checkedLb.SITECODE;
                                tb.Caption = checkedLb.OBSLINENAME;
                            }
                            xtl.RefreshWorkspace(folderName);
                            if (DataManipulations.SaveToWorkspace(xtl.GetCheckedLine(this.treeListRemoteData.Name)))
                                xtl.RefreshWorkspace(folderName);
                        }

                    }
                    break;
                case "btnChart"://趋势图
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            this.chartTabPage.PageVisible = true;//曲线图页面可见
                            this.xtraTabControl1.SelectedTabPage = this.chartTabPage;

                            mtc.AddSeries(xtl.GetCheckedLine(this.treeListRemoteData.Name));
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
                case "btnSiteInfo"://信息库
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;
                           
                            this.siteInfoDocCtrl.LoadDocument(Application.StartupPath + "/tempDoc/信息库模板.doc");
                            this.siteInfoDocCtrl.FillBookMarkText(sb);
                        
                            this.siteInfoTabPage.PageVisible = true;
                            this.xtraTabControl1.SelectedTabPage = this.siteInfoTabPage;
                        }

                    }
                    break;
                case "btnImportObsline"://导入观测数据
                    {
                        try
                        {
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Multiselect = true;//可多选
                            ofd.Filter = "Excel文件|*.xls;*.xlsx;";
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                importDataFiles = ofd.FileNames.ToList();
                                ProgressForm ptPro = new ProgressForm();
                                ptPro.Show(this);
                                ptPro.progressWorker.DoWork += ImportData_DoWork;
                                ptPro.beginWorking();
                                ptPro.progressWorker.RunWorkerCompleted += ImportData_RunWorkerCompleted;
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("导入失败:" + ex.Message, "错误");
                        }
                    }
                    break;
                case "btnDownLoad"://下载数据
                    {
                        string folderName = "远程信息库缓存";
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            List<SiteBean> checkedNodes = xtl.GetCheckedSite(this.treeListRemoteData.Name);
                            foreach (SiteBean checkedSb in checkedNodes)
                            {
                                DataTable linecode = LineObsBll.Instance.GetDataTable("select obslinecode,obslinename from t_obslinetb where SITECODE = '" + checkedSb.SiteCode + "'");

                                foreach (DataRow row in linecode.Rows)
                                {
                                    string lCode = row[0].ToString();
                                    string lName = row[0].ToString();
                                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + lCode + "'");
                                    NpoiCreator npcreator = new NpoiCreator();
                                    string savefile = Application.StartupPath + "/" + folderName;
                                    npcreator.TemplateFile = savefile;
                                    npcreator.NpoiExcel(dt, lCode + ".xls", savefile + "/" + lCode + ".xls");

                                    TreeBean tb = new TreeBean();

                                    tb.KeyFieldName = lCode;
                                    tb.ParentFieldName = checkedSb.SiteCode;
                                    tb.Caption = lName;
                                }
                            }
                            xtl.RefreshWorkspace(folderName);
                            if (DataManipulations.SaveToWorkspace(xtl.GetCheckedLine(this.treeListRemoteData.Name)))
                                xtl.RefreshWorkspace(folderName);
                        }
                    }
                    break;

            }
        }


        #region 导入观测数据
        
        private void ImportData_DoWork(object sender, DoWorkEventArgs e)
        {
            string sitecode = ((SiteBean)currentClickNodeInfo.Tag).SiteCode;

            if (importDataFiles.Count == 0 || sitecode == string.Empty)
                return;

            MyBackgroundWorker worker = (MyBackgroundWorker)sender;
            e.Cancel = false;

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            NpoiCreator npcreator = new NpoiCreator();
            ModelHandler<LineObsBean> mhd = new ModelHandler<LineObsBean>();

            int succedCount = 0;//入库的数量
            int faildCount = 0;//失败的数量
            foreach (string file in importDataFiles)
            {
                try
                {
                    string linename = Path.GetFileNameWithoutExtension(file);
                    string linecode = string.Empty;

                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【导入开始提示】正在处理" + linename + "数据...");

                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 1.正在从数据库中获取测线信息...");
                    if (LineBll.Instance.IsExist(linename))
                    {
                        linecode = LineBll.Instance.GetIdByName(linename);
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    测线已存在！");
                    }
                    else
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    测线不存在,正在添加测线信息...");
                        /*提取测线信息入库*/
                        LineBean lb = new LineBean();
                        lb.SITECODE = sitecode;
                        lb.OBSLINENAME = linename;
                        lb.OBSLINECODE = LineBll.Instance.GenerateLineCode(sitecode);
                        LineBll.Instance.Add(lb);
                        linecode = lb.OBSLINECODE;
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    测线信息入库成功！");
                    }

                    if (linecode != string.Empty)
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 2.正在提取测线观测数据...");
                        /*提取测线观测信息入库*/
                        List<LineObsBean> lineobslist = mhd.FillObsLineModel(npcreator.ExcelToDataTable_LineObs(file, true));
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    测线观测数据提取成功！");
                        foreach (LineObsBean lob in lineobslist)
                        {
                            LineObsBll.Instance.Add(new LineObsBean() { obslinecode = linecode, obvdate = lob.obvdate, obvvalue = lob.obvvalue });
                            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     观测时间："+lob.obvdate + "  观测值："+ lob.obvvalue+" 已入库！");
                            succedCount++;
                        }
                    }
                    else
                    {
                        /*获取测线编码失败*/
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "   获取测线编码失败！");
                        faildCount++;
                    }

                }
                catch (Exception ex)
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "处理中发生错误:" + ex.Message);
                }
            }

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【导入完成提示】此任务处理了" + (succedCount + faildCount).ToString() + "条观测记录，其中成功入库" + succedCount.ToString() + "条，失败" + faildCount.ToString() + "条！");
        }

        private void ImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*原始树刷新，方法未写*/
            xtl.RefreshOrigData();
        }

        #endregion


        /////<summary>
        /////数据下载
        /////</summary>
        /////
        //private string Download(DownLoadInfoBean dlb)
        //{
        //    try
        //    {
        //        string targetPath = dlb.DownloadPath;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return downLoadName;
        //}

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
        private void treeListWorkSpace_AfterCheckNode(object sender, NodeEventArgs e)
        {

        }

        private void tChart_ClickLegend(object sender, MouseEventArgs e)
        {
            mtc.AddVisibleLineVerticalAxis();
        }

         private void btnShowNote_Click(object sender, EventArgs e)
        {
            mtc.ShowNotes();
        }

        private void btnShowTitle_Click(object sender, EventArgs e)
        {
            mtc.btnShowTitle();

        }

        private void btnMouseCur_Click(object sender, EventArgs e)
        {
            mtc.btnMouseCur();
        }


        private void btnMaxMinValue_Click(object sender, EventArgs e)
        {
            mtc.btnMaxMinValue();
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            mtc.btnGrid();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            mtc.GetEqkShowForm();
        }

        /// <summary>
        /// 导出曲线图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportChart_Click(object sender, EventArgs e)
        {
            mtc.ExportChart();
        }

        private void dockPanelOriData_Click(object sender, EventArgs e)
        {

        }

        private void treeListOriData_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            //try
            //{
            //    if (e.Node.Nodes.Count > 0)
            //    {

                    //if (e.Node.Level == 1)
                    //{
                    //    TreeBean tb = e.Node.TreeList.GetDataRecordByNode(e.Node) as TreeBean;
                    //    if (tb != null)
                    //    {
                    //        SiteBean sb = tb.Tag as SiteBean;
                    //        if (sb.SiteCode.Substring(0, 1) == "L")
                    //        {
                    //            e.Node.StateImageIndex = 1;
                    //            e.Node.ImageIndex = 1;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            e.Node.StateImageIndex = 0;
                    //            e.Node.ImageIndex = 0;
                    //            return;
                    //        }

                    //    }
                    //    else
                    //    {
                    //        e.Node.StateImageIndex = -1;
                    //        e.Node.ImageIndex = -1;
                    //    }
                    //}
                    //else
                    //{
                    //    e.Node.StateImageIndex = -1;
                    //    e.Node.ImageIndex = -1;
                    //    return;
                    //}
            //    }
            //    else
            //    {
            //        e.StateImageIndex = -1;
            //        e.SelectImageIndex = -1;
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "错误");
            //}

        }

        private void btnSilveryStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            defaultLookAndFeel.LookAndFeel.SkinName = "DevExpress Style";
        }

        private void btnBlueStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
        }

    }
}