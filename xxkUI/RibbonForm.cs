using System;
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
using Steema.TeeChart.Styles;
using System.Configuration;
using Common.Data.MySql;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private eqkList eqklist = null;

        private XTreeList xtl;
      
        private List<string> userAut = new List<string>();
        private TreeBean currentClickNodeInfo;//当前点击的树节点信息
        private SiteAttri siteAttriFrm = new SiteAttri();
        private List<string> importDataFiles = new List<string>();//导入数据的文件路径集
        /// <summary>
        /// 观测数据操作类型
        /// </summary>
        private ActionType actiontype = ActionType.NoAction;
        private MyTeeChart mtc = null;
        private EqkShow eqkShow;
        public RibbonForm()
        {
            InitializeComponent();
            defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";//蓝色风格
            this.WindowState = FormWindowState.Maximized;//默认最大化窗体
            this.chartTabPage.PageVisible = false;//曲线图页面不可见
            this.siteInfoTabPage.PageVisible = false;//文档页面不可见
            this.recycleTabPage.PageVisible = false;

            mtc = new MyTeeChart(this.chartGroupBox, this.gridControlObsdata);
            xtl = new XTreeList(this.treeListData, this.treeListManipData);

            MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;
            InitFaultCombobox();
            xtl.bSignDbTree(DataFromPath.RemoteDbPath);
            xtl.bSignInitManipdbTree();

          
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
                    currentUserBar.Caption = currentUserBar.Caption + lg.Username;

                    //获取用户权限，放入userAut
                    List<string> userAhtList = UserInfoBll.Instance.GetAthrByUser<UserInfoBean>(lg.Username);
                    //xtl.InitOriDataTree(userAhtList, this.gmmkks);
                   
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
                GMapMarkerKdcSite.ClearAllSiteMarker(this.gMapCtrl);
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
            GMapMarkerKdcSite.InitMap(this.gMapCtrl);
        }
             
        private void gMapCtrl_DoubleClick(object sender, EventArgs e)
        {
            GMapMarkerKdcSite.Zoom(1, this.gMapCtrl);

        }

        private void gMapCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng latLng = GMapMarkerKdcSite.FromLocalToLatLng(e.X, e.Y, this.gMapCtrl);
            this.currentLocation.Caption = string.Format("经度：{0}, 纬度：{1} ", latLng.Lng, latLng.Lat);
        }


        private void gMapCtrl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            try
            {
              
                /*点击场地标注弹出测项下拉列表*/
                SiteBean sb = (SiteBean)item.Tag;
                sb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                /*点击地震标注弹出地震详细说明*/
            }
            catch 
            {

            }
            //GetSiteAttriForm();
            //this.siteAttriFrm.SetDataSource(new List<SiteBean>() { sb });
        }

        private void btnFull_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMapMarkerKdcSite.Full(this.gMapCtrl);
        }

        private void btnZoomout_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMapMarkerKdcSite.Zoom(1, this.gMapCtrl);
        }

        private void btnZoomin_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMapMarkerKdcSite.Zoom(-1, this.gMapCtrl);

        }

        private void btnReloadMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            GMapMarkerKdcSite.ReloadMap(this.gMapCtrl);
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
                        popRemoteSiteTree.ShowPopup(p);
                    }
                    else if (hitInfo.Node.Level == 2)
                    {
                        popRemoteLineTree.ShowPopup(p);
                    }
                }
            }
        }

        /// <summary>
        /// 本地库树点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListLocalData_MouseUp(object sender, MouseEventArgs e)
        {
            TreeList tree = sender as TreeList;

            if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None) && (tree.State == TreeListState.Regular))
            {
                Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                if (tree.Nodes.Count > 0)
                {
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
                            popRemoteSiteTree.ShowPopup(p);
                           
                        }
                        else if (hitInfo.Node.Level == 2)
                        {
                            //popLineTree.ShowPopup(p);
                            popLineTreeWork.ShowPopup(p);
                        }
                    }
                }
                else
                {
                  
                    popRemoteSiteTree.ShowPopup(p);
                }
            }
        }

        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popMenuRemote_ItemClick(object sender, ItemClickEventArgs e)
        {

            string filePath = "";
            switch (e.Item.Name)
            {
                case "btnSaveToManip"://保存到处理数据处理缓存
                    {
                        if (this.dockPanelDb.Text == "本地信息库")
                        {
                            filePath = DataFromPath.LocalDbPath;
                        }
                        else if (this.dockPanelDb.Text == "远程信息库")
                        {
                            filePath = DataFromPath.RemoteDbPath;
                        }
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            List<LineBean> checkedNodes = xtl.GetCheckedLine(this.treeListData.Name);
                            foreach (LineBean checkedLb in checkedNodes)
                            {
                                string sourceFilenanme = filePath + "//" + checkedLb.OBSLINECODE + ".xls";
                                string targetFilenanme = DataFromPath.HandleDataPath + "//" + checkedLb.OBSLINENAME + ".xls";
                                string messageStr = "";
                                FileOperateProxy.CopyFile(sourceFilenanme, targetFilenanme, true, false, true, ref messageStr);
                            }
                            xtl.bSignInitManipdbTree();
                        }
                    }
                    break;
                case "btnChart"://趋势图
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            if (this.dockPanelDb.Text == "本地信息库")
                            {
                                filePath = DataFromPath.LocalDbPath;
                            }
                            else if (this.dockPanelDb.Text == "远程信息库")
                            {
                                filePath = DataFromPath.RemoteDbPath;
                            }
                            this.chartTabPage.PageVisible = true;//曲线图页面可见
                            this.xtraTabControl1.SelectedTabPage = this.chartTabPage;
                            mtc.AddSeries(xtl.GetCheckedLine(this.treeListData.Name), filePath);
                        }
                      }
                    break;
                case "btnSiteLocation"://定位到地图
                    this.xtraTabControl1.SelectedTabPage = this.mapTabPage;
                    GMapMarkerKdcSite.ZoomToSite((SiteBean)currentClickNodeInfo.Tag, this.gMapCtrl);
                    break;
                case "btnSiteInfo"://信息库
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;
                           
                            this.siteInfoDocCtrl.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
                            this.siteInfoDocCtrl.FillBookMarkText(sb);
                            this.siteInfoTabPage.PageVisible = true;
                            this.xtraTabControl1.SelectedTabPage = this.siteInfoTabPage;
                        }
                    }
                    break;
                case "btnImportObsline"://导入观测数据
                    {
                        string userName = this.currentUserBar.Caption.Split('：')[1];

                        if (userName == string.Empty && this.dockPanelDb.Text == "远程信息库")
                        {
                            XtraMessageBox.Show("没有登录！", "警告");
                            return;
                        }
                        else
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
                    }
                    break;
                case "btnDownLoad"://下载数据
                    {
                        string userName = this.currentUserBar.Caption.Split('：')[1];

                        if (userName == string.Empty && this.dockPanelDb.Text == "远程信息库")
                        {
                            XtraMessageBox.Show("没有登录！", "警告");
                            return;
                        }
                        else
                        {
                            if (userName == string.Empty) userName = "superadmin";
                            List<string> userAhtyList = UserInfoBll.Instance.GetAthrByUser<UserInfoBean>(userName);

                            using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                            {
                                List<SiteBean> checkedNodes = xtl.GetCheckedSite(this.treeListData.Name);

                                foreach (SiteBean checkedSb in checkedNodes)
                                {
                                    if (!userAhtyList.Contains(checkedSb.UnitCode))
                                    {
                                        string unitname = UnitInfoBll.Instance.GetUnitNameBy(checkedSb.UnitCode);
                                        XtraMessageBox.Show("没有下载" + unitname + "数据的权限！", "警告");
                                        continue;
                                    }
                                    DataTable linecode = LineObsBll.Instance.GetDataTable("select obslinecode,obslinename from t_obslinetb where SITECODE = '" + checkedSb.SiteCode + "'");

                                    foreach (DataRow row in linecode.Rows)
                                    {
                                        string lCode = row[0].ToString();
                                        string lName = row[1].ToString();
                                        DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + lCode + "'");
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (this.dockPanelDb.Text == "本地信息库")
                                            { 
                                                filePath = DataFromPath.LocalDbPath; 
                                            }
                                            else if (this.dockPanelDb.Text == "远程信息库")
                                            {
                                                filePath = DataFromPath.RemoteDbPath;
                                            }
                                            NpoiCreator npcreator = new NpoiCreator();
                                            npcreator.TemplateFile = filePath;
                                            npcreator.NpoiExcel(dt, lCode + ".xls", filePath + "/" + lCode + ".xls");

                                            TreeBean tb = new TreeBean();

                                            tb.KeyFieldName = lCode;
                                            tb.ParentFieldName = checkedSb.SiteCode;
                                            tb.Caption = lName;
                                        }
                                    }
                                }
                                if (this.dockPanelDb.Text == "本地信息库")
                                {
                                    filePath = DataFromPath.LocalDbPath;
                                }
                                else if (this.dockPanelDb.Text == "远程信息库")
                                {
                                    filePath = DataFromPath.RemoteDbPath;
                                }
                                xtl.RefreshWorkspace(filePath);
                                
                            }
                        }
                    }
                    break;
              
            }
        }


        //private void popMenuLocal_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
        //    MysqlHelper.connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
        //    switch (e.Item.Name)
        //    {
        //        case "btnSaveToManip"://保存到处理数据处理缓存
        //            {
        //                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
        //                {
        //                    List<LineBean> checkedNodes = xtl.GetCheckedLine(this.treeListData.Name);
        //                    foreach (LineBean checkedLb in checkedNodes)
        //                    {
        //                        string sourceFilenanme = DataFromPath.LocalDbPath + "//" + checkedLb.OBSLINECODE + ".xls";
        //                        string targetFilenanme = DataFromPath.HandleDataPath + "//" + checkedLb.OBSLINENAME + ".xls";
        //                        string messageStr = "";
        //                        FileOperateProxy.CopyFile(sourceFilenanme, targetFilenanme, true, false, true, ref messageStr);
        //                    }
        //                    xtl.bSignInitManipdbTree();
        //                }
        //            }
        //            break;
        //        case "btnChart"://趋势图
        //            {
        //                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
        //                {
        //                    this.chartTabPage.PageVisible = true;//曲线图页面可见
        //                    this.xtraTabControl1.SelectedTabPage = this.chartTabPage;
        //                    mtc.AddSeries(xtl.GetCheckedLine(this.treeListData.Name), DataFromPath.LocalDbPath);
        //                }
        //            }
        //            break;
        //        case "btnSiteLocation"://定位到地图
        //            this.xtraTabControl1.SelectedTabPage = this.mapTabPage;
        //            GMapMarkerKdcSite.ZoomToSite((SiteBean)currentClickNodeInfo.Tag,this.gMapCtrl);
        //            break;
        //        case "btnSiteInfo"://信息库
        //            {
        //                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
        //                {
        //                    SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;
        //                    this.siteInfoDocCtrl.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
        //                    this.siteInfoDocCtrl.FillBookMarkText(sb);
        //                    this.siteInfoTabPage.PageVisible = true;
        //                    this.xtraTabControl1.SelectedTabPage = this.siteInfoTabPage;
        //                }
        //            }
        //            break;
        //        case "btnImportObsline"://导入观测数据
        //            {
        //                try
        //                {
        //                    OpenFileDialog ofd = new OpenFileDialog();
        //                    ofd.Multiselect = true;//可多选
        //                    ofd.Filter = "Excel文件|*.xls;*.xlsx;";
        //                    if (ofd.ShowDialog() == DialogResult.OK)
        //                    {
        //                        importDataFiles = ofd.FileNames.ToList();
        //                        ProgressForm ptPro = new ProgressForm();
        //                        ptPro.Show(this);
        //                        ptPro.progressWorker.DoWork += ImportData_DoWork;
        //                        ptPro.beginWorking();
        //                        ptPro.progressWorker.RunWorkerCompleted += ImportData_RunWorkerCompleted;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    XtraMessageBox.Show("导入失败:" + ex.Message, "错误");
        //                }
        //            }
        //            break;
        //        case "btnDownLoad"://下载数据
        //            {
        //                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
        //                {
        //                    List<SiteBean> checkedNodes = xtl.GetCheckedSite(this.treeListData.Name);
        //                    foreach (SiteBean checkedSb in checkedNodes)
        //                    {
        //                        DataTable linecode = LineObsBll.Instance.GetDataTable("select obslinecode,obslinename from t_obslinetb where SITECODE = '" + checkedSb.SiteCode + "'");
        //                        foreach (DataRow row in linecode.Rows)
        //                        {
        //                            string lCode = row[0].ToString();
        //                            string lName = row[1].ToString();
        //                            DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue,note,from t_obsrvtntb where OBSLINECODE = '" + lCode + "'");
        //                            if (dt.Rows.Count > 0)
        //                            {
        //                                NpoiCreator npcreator = new NpoiCreator();
        //                                npcreator.TemplateFile = DataFromPath.LocalDbPath;
        //                                npcreator.NpoiExcel(dt, lCode + ".xls", DataFromPath.LocalDbPath + "/" + lCode + ".xls");

        //                                TreeBean tb = new TreeBean();

        //                                tb.KeyFieldName = lCode;
        //                                tb.ParentFieldName = checkedSb.SiteCode;
        //                                tb.Caption = lName;
        //                            }
        //                        }
        //                    }
        //                    xtl.RefreshWorkspace(DataFromPath.LocalDbPath);

        //                }

        //            }
        //            break;
              
        //    }

        //}

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
         
        }

        #endregion


        #region 创建数据库

        private void CreateLocalDb_DoWork(object sender, DoWorkEventArgs e)
        {
            MyBackgroundWorker worker = (MyBackgroundWorker)sender;
            e.Cancel = false;

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                CreateLocalDb cldb = new CreateLocalDb();

                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【创建数据库开始提示】开始创建本地数据库...");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 1. 正在创建数据库...");
                if (cldb.CreateDatabase("localinfo"))
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    创建数据库成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "    创建数据库失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 2. 正在创建数据库表...");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.1 正在创建场地表...");
                if (cldb.CreateSiteinfoTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建场地表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建场地表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.2 正在导入场地表记录...");
                if (cldb.InsertSiteinfoLRec(worker, e))
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入场地表记录成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     导入场地表记录失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.3 正在创建测线信息表...");
                if (cldb.CreateLineTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建测线信息表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建测线信息表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.4 正在导入测线信息表记录...");
                if (cldb.InsertLineinfoLRec(worker, e))
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入测线信息表记录成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     导入测线信息表记录失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.5 正在创建单位信息表...");
                if (cldb.CreateUnitTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建单位信息表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建单位信息表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.6 正在导入单位信息表记录...");
                if (cldb.InsertUnitinfoLRec(worker, e))
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入单位信息表记录成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     导入单位信息表记录失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.7 正在创建地震目录表...");
                if (cldb.CreateEqklogTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建地震目录表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建地震目录表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.8 正在导入地震目录表记录...");
                if (cldb.InsertEqkloginfoLRec(worker, e))
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     导入地震目录表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     导入地震目录表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.9 正在创建观测信息表...");
                if (cldb.CreateObsLineTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建观测信息表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建观测信息表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "  2.10 正在创建场地布设图表...");
                if (cldb.CreateSiteLayoutTb())
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     创建场地布设图表成功!");
                else
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "     创建场地布设图表失败!");
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    创建数据库表成功！");

            }
            catch (Exception ex)
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "创建过程中发生错误:" + ex.Message);
            }
            

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【创建数据库完成提示】完成本地信息库的创建！");
        }

        private void CreateLocalDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            SwapDb(DataFromType.LocalDb);
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

        private void btnRecycled_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.recycleTabPage.PageVisible = true;
            this.xtraTabControl1.SelectedTabPage = this.recycleTabPage;
            this.recycleControl1.LoadRecycleItems();
        }
        /// <summary>
        /// 查询地震
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEqkQuery_ItemClick(object sender, ItemClickEventArgs e)
        {
            float eqkMlMin = float.Parse(this.beiEqkMinMtd.EditValue.ToString());
            float eqkMlMax = float.Parse(this.beiEqkMaxMtd.EditValue.ToString());
            if (eqkMlMin > eqkMlMax)
            {
                XtraMessageBox.Show("最大震级应大于最小震级，重新输入！", "提示");
                this.beiEqkMinMtd.EditValue = "";
                this.beiEqkMaxMtd.EditValue = "";
                return;
            }
            float eqkDepthMin = float.Parse(this.beiEqkMinDepth.EditValue.ToString());
            float eqkDepthMax = float.Parse(this.beiEqkMaxDepth.EditValue.ToString());
            if (eqkDepthMin > eqkDepthMax)
            {
                XtraMessageBox.Show("最大深度应大于最小深度，重新输入！", "提示");
                this.beiEqkMinDepth.EditValue = "";
                this.beiEqkMinDepth.EditValue = "";
                return;
            }
            string timeStStr = this.beiEqkStartTime.EditValue.ToString();
            DateTime timeStc = Convert.ToDateTime(timeStStr);
            DateTime timeSt = Convert.ToDateTime(timeStc).Date;
            string timeEdStr = this.beiEqkEndTime.EditValue.ToString();
            DateTime timeEdc = Convert.ToDateTime(timeEdStr);
            DateTime timeEd = Convert.ToDateTime(timeEdc).Date;
            if (DateTime.Compare(timeSt, timeEd) > 0)
            {
                XtraMessageBox.Show("结束时间应在开始时间之后！", "提示");
                this.beiEqkStartTime.EditValue = "";
                this.beiEqkEndTime.EditValue = "";
                return;
            }
            //string sql0 = "select longtitude as 'u经度',latitude as 'u纬度',eakdate as 'u时间', magntd as 'u震级', depth as 'u深度', place as 'u地点'";
            string sql0 = "select longtitude,latitude,eakdate, magntd, depth, place";
            string sql1 = "  from t_eqkcatalog where MAGNTD >= " + eqkMlMin + " and MAGNTD <=" + eqkMlMax;
            if (eqkMlMin == eqkMlMax) sql1 = "  from t_eqkcatalog where MAGNTD = " + eqkMlMin;
            string sql2 = " and DEPTH >=" + eqkDepthMin + " and DEPTH <=" + eqkDepthMax;
            if (eqkDepthMin == eqkDepthMax) sql2 = " and DEPTH =" + eqkDepthMin;
            string sql3 = " and EAKDATE >=" + "\'" + timeSt.ToString() + "\'" + " and EAKDATE <=" + "\'" + timeEd.ToString() + "\'";
            if (DateTime.Compare(timeSt, timeEd) == 0) sql3 = " and EAKDATE =" + "\'" + timeSt.ToString() + "\'";
            string sql = sql0 + sql1 + sql2 + sql3;
            List<EqkBean> eqkDataList = xxkUI.BLL.EqkBll.Instance.GetList(sql).ToList();

            if (eqkDataList.Count() > 0)
            {
                this.xtraTabControl1.SelectedTabPage = this.mapTabPage;
                MapEqkShowForm(eqkDataList);
                GMap.NET.WindowsForms.GMapControl gmapcontrol = Application.OpenForms["RibbonForm"].Controls.Find("gMapCtrl", true)[0] as GMap.NET.WindowsForms.GMapControl;
                GMapMarkerKdcSite.ClearAllEqkMarker(gmapcontrol);
                GMapMarkerKdcSite.AnnotationEqkToMap(eqkDataList, gmapcontrol);

            }
            else
            {
                XtraMessageBox.Show("没有相应震例！", "提示");
            }

        }
        /// <summary>
        /// 地震列表
        /// </summary>
        public void MapEqkShowForm(List<EqkBean> eqkShowList)
        {
            if (eqklist != null)
            {
                if (eqklist.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    eqklist = new eqkList(eqkShowList);

                    eqklist.Show();
                    eqklist.Focus();
                }
                else
                {
                    eqklist.Show();
                    eqklist.Focus();
                }
            }
            else
            {
                eqklist = new eqkList(eqkShowList);
                eqklist.Show();
                eqklist.Focus();
            }
        }


        /// <summary>
        /// 数据处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataProgress_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnFourCal"://加减乘除
                    mtc.PlusMinusMultiplyDivide();
                    break;
                case "btnRemoveStep"://消台阶
                    mtc.RemoStepOrJump(TChartEventType.RemoveStep);
                    break;
                case "btnRemoveJump"://消突跳
                    mtc.RemoStepOrJump(TChartEventType.RemoveJump);
                    break;
                case "btnLinesUnion"://测线合并

                    break;
                case "btnLinesBreak"://测线拆分

                    break;

            }

        }

        /// <summary>
        /// Tchart操作工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChartTool_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnShowTitle"://标题
                    mtc.btnShowTitle();
                    break;
                case "btnGrid"://网格
                    mtc.btnGrid();
                    break;
                case "btnShowNote"://备注
                    mtc.ShowNotes();
                    break;
                case "btnMouseCur"://鼠标热线
                    mtc.btnMouseCur();
                    break;
                case "btnMaxMinValue"://最大最小值
                    mtc.btnMaxMinValue();
                    break;
                case "btnHistoryEqk"://历史地震
                    mtc.GetEqkShowForm();
                    break;
                case "btnExportChart"://导出曲线图
                    mtc.ExportChart();
                    break;
            }
        }

      
        #region 观测数据的显示、增加、删除、修改

       
        private void barbtnObsData_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "barbtnInsertRec"://插入
                    {
                        DataTable dt = ObsdataCls.ObsdataHash["东焦裴村d1d2水准01_11_6"] as DataTable;
                        actiontype = ActionType.Add;
                        this.gridViewObsdata.OptionsBehavior.Editable = true;//可编辑
                        int focusedRow = this.gridViewObsdata.FocusedRowHandle;
                        this.gridViewObsdata.AddNewRow();
                    }
                    break;
                case "barbtnDeleteRec"://删除
                    {
                        actiontype = ActionType.Delete;
                        int focusedRow = this.gridViewObsdata.FocusedRowHandle;
                        try
                        {
                            DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(focusedRow);
                            DateTime obsdate = new DateTime();
                            DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                            double obsv = double.NaN;
                            double.TryParse(drv["obvvalue"].ToString(), out obsv);
                            mtc.DeleteChartlineData(obsdate, obsv);
                        }
                        catch (Exception ex)
                        {
                            actiontype = ActionType.NoAction;
                            //XtraMessageBox.Show("错误", "删除失败:" + ex.Message);
                        }

                        gridViewObsdata.DeleteRow(focusedRow);
                        gridViewObsdata.UpdateCurrentRow();
                    }
                    break;
                case "barbtnEditRec"://编辑
                    {
                        if (barbtnEditRec.Caption == "取消编辑")
                        {
                            this.gridViewObsdata.OptionsBehavior.Editable = false;
                            barbtnEditRec.Caption = "编辑数据";
                            actiontype = ActionType.NoAction;
                        }
                        else if (barbtnEditRec.Caption == "编辑数据")
                        {
                            this.gridViewObsdata.OptionsBehavior.Editable = true;//可编辑
                            barbtnEditRec.Caption = "取消编辑";
                            actiontype = ActionType.Modify;
                        }
                    }
                    break;
                case "barbtnSaveRec"://保存
                    {
                        try
                        {
                            LineObsBll lob = new LineObsBll();
                            DataTable dt = (this.gridControlObsdata.DataSource as DataTable).GetChanges();
                            if (dt != null)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    /*
                                     * 新增
                                     */
                                    if (dr.RowState == DataRowState.Added)
                                    {
                                        LineObsBean lobean = new LineObsBean();

                                    }

                                    /*
                                     * 修改
                                     */
                                    else if (dr.RowState == DataRowState.Modified)
                                    {

                                    }
                                }
                            }
                            /*
                             * 删除
                             */
                            DataView dv = new DataView((this.gridControlObsdata.DataSource as DataTable), string.Empty, string.Empty, DataViewRowState.Deleted);
                            if (dv != null)
                            {
                                foreach (DataRow dr in dv.ToTable().Rows)
                                { }
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("保存失败:" + ex.Message, "错误");
                        }
                    }
                    break;
              
            }
        }

        private void gridViewObsdata_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None))
            {
                Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                GridHitInfo hitInfo = gridViewObsdata.CalcHitInfo(e.Location);
                popObsdata.ShowPopup(p);
            }
        }

        private void gridViewObsdata_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int focusedRow = e.RowHandle;
                DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(focusedRow);

                switch (actiontype)
                {
                    case ActionType.Modify:
                        {
                            gridViewObsdata.UpdateCurrentRow();
                        }
                        break;
                    case ActionType.Add:
                        {
                            if (drv["obvdate"].ToString() == "" || drv["obvvalue"].ToString() == "")
                                return;

                          
                            DateTime obsdate = new DateTime();
                            DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);

                            double obdv = double.NaN;
                            double.TryParse(drv["obvvalue"].ToString(), out obdv);
                            gridViewObsdata.UpdateCurrentRow();
                           
                            mtc.AddChartlineData(obsdate, obdv);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }


        private void gridViewObsdata_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = gridViewObsdata.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(hInfo.RowHandle);
                        DateTime obsdate = new DateTime();
                        DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                        double obsv = double.NaN;
                        double.TryParse(drv["obvvalue"].ToString(), out obsv);
                        mtc.GoTodata(obsdate, obsv);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }

        private void gridViewObsdata_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (actiontype == ActionType.Modify)
                e.Cancel = false;
            else if (actiontype == ActionType.Add)
            {
                /*
                 * 新增状态下只有新增行可以编辑
                 */
                DataRowView drv = (DataRowView)this.gridViewObsdata.GetRow(this.gridViewObsdata.FocusedRowHandle);
                if (drv["obvvalue"].ToString() == "" || drv["obvdate"].ToString() == "")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }



        #endregion

        #region 数据库操作（创建、切换、备份）
        private void btnDb_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnCreateLocaDb"://创建本地库
                    {
                        CreateLocalDb cld = new CreateLocalDb();
                        if (!cld.IsLocaldbExist())
                        {
                            ProgressForm ptPro = new ProgressForm();
                            ptPro.Show(this);
                            ptPro.progressWorker.DoWork += CreateLocalDb_DoWork;
                            ptPro.beginWorking();
                            ptPro.progressWorker.RunWorkerCompleted += CreateLocalDb_RunWorkerCompleted;
                        }
                        else
                        {
                            XtraMessageBox.Show("本地库已存在！","提示");
                        }

                    }
                    break;
                case "btnSwitchDb"://数据库切换
                    {
                        if (this.dockPanelDb.Text.Contains("远程"))
                            SwapDb(DataFromType.LocalDb);
                        else if (this.dockPanelDb.Text.Contains("本地"))
                            SwapDb(DataFromType.RemoteDb);

                    }
                    break;
                case "btnCopyDb"://数据库备份
                    { }
                    break;
            }
        }

        /// <summary>
        /// 切换数据库和列表
        /// </summary>
        private void SwapDb(DataFromType dft)
        {
            if (dft == DataFromType.RemoteDb)
            {
                MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;
                xtl.bSignDbTree(DataFromPath.RemoteDbPath);
                dockPanelDb.Text = "远程信息库";
            }
            else if (dft == DataFromType.LocalDb)
            {
                MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
                xtl.bSignDbTree(DataFromPath.RemoteDbPath);
                dockPanelDb.Text = "本地信息库";
            }
        }
        #endregion

  
    }
}