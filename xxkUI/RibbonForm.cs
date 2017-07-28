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
using System.Text.RegularExpressions;
using DevExpress.XtraTab;
using DevExpress.XtraGrid.Views.Card;

namespace xxkUI
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        private List<string> userAut = new List<string>();
        private TreeBean currentClickNodeInfo;// 当前点击的树节点信息
        private TreeList currentTree;//当前树
        private SiteAttri siteAttriFrm = new SiteAttri();
        private List<string> importDataFiles = new List<string>();// 导入数据的文件路径集
     
    
        private bool IsEqkShow = false;// 是否显示地震目录列表
        private int pagesize = 50;// 页行数
        private int pageIndex = 1;// 当前页
        private int pageCount;// 总页数

        public RibbonForm()
        {
            InitializeComponent();
            InitForm();
            InitTree();
            InitFaultCombobox();
          
            InitSiteinTab();
            InitRecycleTab();
            InitLayoutmapTab();
            InitStyle();

         
        }

        /// <summary>
        /// 初始化主框架
        /// </summary>
        public void InitForm()
        {
            this.WindowState = FormWindowState.Maximized;//默认最大化窗体
        }

        /// <summary>
        /// 初始化信息树
        /// </summary>
        public void InitTree()
        {
            xtl = new XTreeList(this.treeListData, this.treeListManipData);

            if (this.dockPanelDb.Text.Contains("本地"))
                SwapDb();

            xtl.bSignDbTree(DataFromPath.RemoteDbPath);
            xtl.bSignInitManipdbTree();
        }


        /// <summary>
        /// 初始化断层数据列表
        /// </summary>
        public void InitFaultCombobox()
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
        /// 初始化Chart页面及工具
        /// </summary>
     
        /// <summary>
        /// 初始化信息库页面
        /// </summary>
        public void InitSiteinTab()
        {
            this.siteInfoTabPage.PageVisible = false;//文档页面不可见
            this.addXxkTabPage.PageVisible = false;
        }

        /// <summary>
        /// 初始化回收站页面
        /// </summary>
        public void InitRecycleTab()
        {
            this.recycleTabPage.PageVisible = false;
        }

        /// <summary>
        /// 初始化布设图页面
        /// </summary>
        public void InitLayoutmapTab()
        {
            this.layoutmapTabpage.PageVisible = false;
        }
        /// <summary>
        /// 设置界面风格
        /// </summary>
        public void InitStyle()
        {
            defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";//蓝色风格
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
                    currentUserBar.Caption = currentUserBar.Caption + CurrentUSerInfo.UIB.UserName;
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
                CurrentUSerInfo.UIB = new UserInfoBean();
                currentUserBar.Caption = "当前用户:" + CurrentUSerInfo.UIB.UserName;
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
            if (GMapMarkerKdcSite.InitMap(this.gMapCtrl))
            {
                IEnumerable<SiteBean> sbEnumt = SiteBll.Instance.GetAll();
                //加载场地标记(lwl)
                GMapMarkerKdcSite.LoadSiteMarker(sbEnumt, gMapCtrl);
            }
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
                //SiteBean sb = (SiteBean)item.Tag;
                //sb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                /*点击地震标注弹出地震详细说明*/

                if (item is GMapMarker)
                {
                    if (item.Overlay.Id == "sitemarkers")//场地标签图层
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            SiteBean sb = (SiteBean)item.Tag;

                            this.siteInfoDocCtrl1.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
                            this.siteInfoDocCtrl1.FillBookMarkText(sb);
                            this.siteInfoTabPage.PageVisible = true;
                            this.TabControl.SelectedTabPage = this.siteInfoTabPage;
                        }
                    }

                }
            }
            catch
            {

            }
            //GetSiteAttriForm();
            //this.siteAttriFrm.SetDataSource(new List<SiteBean>() { sb });
        }

        private void btnEventOnMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnZoomout":
                    { GMapMarkerKdcSite.Zoom(1, this.gMapCtrl); }
                    break;
                case "btnZoomin":
                    { GMapMarkerKdcSite.Zoom(-1, this.gMapCtrl); }
                    break;
                case "btnFull":
                    { GMapMarkerKdcSite.Full(this.gMapCtrl); }
                    break;
                case "btnReloadMap":
                    { GMapMarkerKdcSite.ReloadMap(this.gMapCtrl); }
                    break;
                case "btnEqkSearch":
                    {
                        try
                        {
                            string sqlwhere = GetSqlWhere();
                            if (sqlwhere != string.Empty)
                                BindPageGridList(sqlwhere);
                            else
                                throw new Exception("不是有效的查询语句");
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("查询失败：" + ex.Message, "错误");
                        }
                    }
                    break;
                case "btnClearEqk":
                    {
                        this.gridControlEqklist.DataSource = null;
                        GMapMarkerKdcSite.ClearAllEqkMarker(this.gMapCtrl);
                    }
                    break;

            }

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
        /// 控制dockPanel的显示
        /// </summary>
        /// <param name="controlname"></param>
        private void ChangePanelContainerItemVisible()
        {
            try
            {
                if (this.TabControl.SelectedTabPage.Name == "chartTabPage")
                {
                    this.ribbon.SelectedPage = ribbonPageTchartTool;
                }
                else if (this.TabControl.SelectedTabPage.Name == "mapTabPage")
                {
                    if (IsEqkShow)
                    {
                        if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Visible)
                            this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    }
                    else
                    {
                        
                        if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                            this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    }

                    this.ribbon.SelectedPage = ribbonPageMapTool;
                }
                else if(this.TabControl.SelectedTabPage.Name== "recycleTabPage")
                {
                    if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                        this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    this.ribbon.SelectedPage = ribbonPageStart;
                }
                else if (this.TabControl.SelectedTabPage.Name == "layoutmapTabpage")
                {
                   
                    if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                        this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    this.ribbon.SelectedPage = ribbonPageStart;
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }


        /// <summary>
        /// 树列表右击菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tree_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeList tree = sender as TreeList;
                currentTree = tree;
                if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None) && (tree.State == TreeListState.Regular))
                {
                    Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                    TreeListHitInfo hitInfo = tree.CalcHitInfo(e.Location);
                    if (hitInfo.HitInfoType == HitInfoType.Cell)
                    {
                        tree.SetFocusedNode(hitInfo.Node);

                        if (tree.Name == "treeListData")//信息库树
                        {
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
                        if (tree.Name == "treeListManipData")//处理数据
                        {
                            popRemoteLineTree.ShowPopup(p);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");

            }

        }

        /// <summary>
        /// treelist双击事件，显示曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeList tree = sender as TreeList;
                MouseEventArgs me = e as MouseEventArgs;

                TreeListHitInfo hitInfo = tree.CalcHitInfo(me.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    tree.SetFocusedNode(hitInfo.Node);

                    if (tree.Name == "treeListData")//信息库树
                    {
                        currentClickNodeInfo = tree.GetDataRecordByNode(hitInfo.Node) as TreeBean;
                        if (currentClickNodeInfo == null)
                        {
                            return;
                        }
                        if (hitInfo.Node.Level == 1)//场地
                        {

                        }
                        if (hitInfo.Node.Level == 2)//测线
                        {
                            LineBean tag = currentClickNodeInfo.Tag as LineBean;
                            string filePath = (this.dockPanelDb.Text == "本地信息库") ? DataFromPath.LocalDbPath : DataFromPath.RemoteDbPath;
                            AddSeriesToChart(new List<LineBean>() { tag }, filePath);
                        }

                    }
                    if (tree.Name == "treeListManipData")//处理数据
                    {
                        AddSeriesToChart(new List<string>() { hitInfo.Node.GetDisplayText(0) }, DataFromPath.HandleDataPath);
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

            ChangePanelContainerItemVisible();



        }

        private void AddSeriesToChart <T>(List<T> checkednode, string dfp)
        {

            try
            {
                Type t = typeof(T);
                if (t.Name == "LineBean")
                {
                    this.tChartControl.AddSeries(checkednode as List<LineBean>, dfp);
                }
                else if (t.Name == "String")
                {
                    this.tChartControl.AddSeries(checkednode as List<string>, dfp);
                }

                this.chartTabPage.PageVisible = true;//曲线图页面可见
                this.TabControl.SelectedTabPage = this.chartTabPage;
                //跳转至菜单栏
                this.Ribbon.SelectedPage = ribbonPageTchartTool;
            }
            catch (Exception ex)
            { }
        }


        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popMenuRemote_ItemClick(object sender, ItemClickEventArgs e)
        {

            switch (e.Item.Name)
            {
                case "btnChart"://趋势图
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            if (currentTree.Name == this.treeListData.Name)
                            {
                                string filePath = (this.dockPanelDb.Text == "本地信息库") ? DataFromPath.LocalDbPath : DataFromPath.RemoteDbPath;
                                AddSeriesToChart(xtl.GetCheckedLine(currentTree.Name), filePath);

                            }
                            else if (currentTree.Name == this.treeListManipData.Name)
                            {
                                AddSeriesToChart(xtl.GetCheckedLineOnMuniTree(currentTree.Name), DataFromPath.HandleDataPath);
                            }
                        }
                    }
                    break;
                case "btnSiteLocation"://定位到地图
                    this.TabControl.SelectedTabPage = this.mapTabPage;
                    this.Ribbon.SelectedPage = ribbonPageMapTool;
                    GMapMarkerKdcSite.ZoomToSite((SiteBean)currentClickNodeInfo.Tag, this.gMapCtrl);
                    break;
                case "btnSiteInfo"://信息库
                    {
                        using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                        {
                            SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;

                            this.siteInfoDocCtrl1.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
                            this.siteInfoDocCtrl1.FillBookMarkText(sb);
                            this.siteInfoTabPage.PageVisible = true;
                            this.TabControl.SelectedTabPage = this.siteInfoTabPage;
                        }
                    }
                    break;
                case "btnAddSiteInfo"://新增信息库
                    {
                        this.addXxkTabPage.PageVisible = true;
                        this.TabControl.SelectedTabPage = this.addXxkTabPage;

                        this.addXxkTabPage.Text = "新增信息库";
                        this.groupControl1.Text = "新增信息库表单";
                        this.btnXxkAdd.Text = "上传至数据库";
                        this.btnXxkAdd.Enabled = true;

                        SetBaseinfoVGridControl();
                        SetSiteValueVGridControl(null, true);
                        //SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;

                    }
                    break;
                case "btnUpdateSiteInfo"://更新信息库
                    {
                        this.addXxkTabPage.PageVisible = true;
                        this.TabControl.SelectedTabPage = this.addXxkTabPage;
                        SetBaseinfoVGridControl();
                        SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;
                        this.addXxkTabPage.Text = "更新信息库";
                        this.groupControl1.Text = "更新信息库表单";
                        this.btnXxkAdd.Text = "更新至数据库";
                        this.btnXxkAdd.Enabled = true;
                        SetSiteValueVGridControl(sb, false);
                    }
                    break;
                case "btnImportObsline"://导入观测数据
                    {
                        if (currentTree.Name == this.treeListData.Name)//远程库
                        {
                            if (CurrentUSerInfo.UIB.UserName == null|| CurrentUSerInfo.UIB.UserName == string.Empty)
                            {
                                //先登录再下载
                                Login lg = new Login();
                                if (lg.ShowDialog() == DialogResult.OK)
                                {
                                    using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                                    {
                                        currentUserBar.Caption = CurrentUSerInfo.UIB.UserName;
                                        string sitecode = currentClickNodeInfo.ParentFieldName;
                                        if (!CurrentUSerInfo.UIB.UserAthrty.Split(';').ToList().Contains(sitecode))
                                        {
                                            XtraMessageBox.Show("当前用户无该场地权限，请向管理部门申请相应权限再执行该操作！", "提示");
                                            return;
                                        }

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
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                                {
                                    currentUserBar.Caption = CurrentUSerInfo.UIB.UserName;
                                    string sitecode = currentClickNodeInfo.ParentFieldName;
                                    if (!CurrentUSerInfo.UIB.UserAthrty.Split(';').ToList().Contains(sitecode))
                                    {
                                        XtraMessageBox.Show("当前用户无该场地权限，请向管理部门申请相应权限再执行该操作！", "提示");
                                        return;
                                    }
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
                        }
                        else if (currentTree.Name == this.treeListManipData.Name)//本地库
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
                        if (this.dockPanelDb.Text.Contains("远程"))
                        {
                            if (CurrentUSerInfo.UIB.UserName == null|| CurrentUSerInfo.UIB.UserName == string.Empty)
                            {
                                Login lg = new Login();

                                if (lg.ShowDialog() == DialogResult.OK)
                                {
                                    using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                                    {
                                        currentUserBar.Caption = "当前用户：" + CurrentUSerInfo.UIB.UserName;
                                    }
                                }
                                else
                                {
                                    return;
                                }

                            }
                        }
                       
                        DownloadData();
                    }
                    break;
                case "btnDeleteObsline"://删除测项至回收站
                    {
                        try
                        {
                            PublicHelper php = new PublicHelper();
                            DataFromType dft = (this.dockPanelDb.Text == "本地信息库") ? DataFromType.LocalDb : DataFromType.RemoteDb;
                            string filePath = (this.dockPanelDb.Text == "本地信息库") ? DataFromPath.LocalDbPath : DataFromPath.RemoteDbPath;

                            if (currentTree.Name == this.treeListData.Name)
                            {
                              
                                List<LineBean> lblist = xtl.GetCheckedLine(currentTree.Name);

                                foreach (LineBean lb in lblist)
                                {
                                    string sourceFilePath = filePath + "\\" + lb.OBSLINECODE + ".xls";

                                    string dbtype = (dft == DataFromType.RemoteDb) ? "YC" : "BD";//远程或本地
                                    string deletetime = php.CreateTimeStr();
                                    string excelname = lb.OBSLINECODE;

                                    string destFileName = DataFromPath.RecycleDataPath + "\\" + deletetime + dbtype + excelname + ".xls";
                                    File.Copy(sourceFilePath, destFileName);
                                    File.Delete(sourceFilePath);
                                }

                                xtl.bSignDbTree(filePath);
                            }
                            else if (currentTree.Name == this.treeListManipData.Name)
                            {
                                filePath = DataFromPath.HandleDataPath;
                                dft = DataFromType.HandleData;
                                List<string> checklines = xtl.GetCheckedLineOnMuniTree(currentTree.Name);

                                foreach (string lb in checklines)
                                {
                                    string sourceFilePath = filePath + "\\" + lb + ".xls";

                                    string deletetime = php.CreateTimeStr();
                                    string dbtype = "CL";
                                    string excelname = lb;
                                    string destFileName = DataFromPath.RecycleDataPath + "\\" + deletetime + dbtype + excelname + ".xls";

                                    File.Copy(sourceFilePath, destFileName);
                                    File.Delete(sourceFilePath);
                                }

                                xtl.bSignInitManipdbTree();
                            }

                            recycleControl.LoadRecycleItems();
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "错误");
                        }
                    }

                    break;

                case "btnLayoutmap"://场地布设图
                    {
                        SiteBean sb = (SiteBean)currentClickNodeInfo.Tag;

                        List<LayoutmapBean> lymplist = LayoutmapBll.Instance.GetLayoutmapBy(sb.SiteCode);
                        if (lymplist.Count == 0)
                        {
                            if (XtraMessageBox.Show("没有找到与该场地对应的布设图，是否增加布设图？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                FolderBrowserDialog fbd = new FolderBrowserDialog();
                                fbd.SelectedPath = Application.StartupPath;
                                if (fbd.ShowDialog() == DialogResult.OK)
                                {
                                    using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                                    {
                                        FileInfo[] fifs = new DirectoryInfo(fbd.SelectedPath).GetFiles();
                                        foreach (FileInfo fi in fifs)
                                        {
                                            LayoutmapBean lymapbean = new LayoutmapBean();
                                            lymapbean.layoutmapcode = LayoutmapBll.Instance.CreateLayoutmapCode();
                                            lymapbean.sitecode = sb.SiteCode;
                                            lymapbean.layoutmapname = Regex.Split(fi.Name, fi.Extension, RegexOptions.IgnoreCase)[0];
                                            lymapbean.layoutmap = File.ReadAllBytes(fi.FullName);
                                            LayoutmapBll.Instance.Add(lymapbean);
                                            lymplist.Add(lymapbean);
                                        }
                                    }
                                    SetDatasourceLympGridControl(lymplist, sb.SiteCode);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            SetDatasourceLympGridControl(lymplist, sb.SiteCode);
                        }

                    }
                    break;

            }
        }


        private void SetDatasourceLympGridControl(List<LayoutmapBean> lymplist, string sitecode)
        {
            try
            {
                this.gridControlLymp.DataSource = null;
                //自定义card长宽
                int w = this.gridControlLymp.Width;
                int h = this.gridControlLymp.Height;
                this.cardViewLymp.CardWidth = (w - 50) / 4;
                this.lyotmapPicEdit.CustomHeight = (h -20)/ 2;

                List<LineBean> lblist = LineBll.Instance.GetBySitecode(sitecode).ToList();
                this.lyotmapCheckedCmb.Items.Clear();
                CheckedListBoxItem[] itemListQuery = new CheckedListBoxItem[lblist.Count];
                int check = 0;
                foreach (LineBean det in lblist)
                {
                    itemListQuery[check] = new CheckedListBoxItem(det.OBSLINECODE, det.OBSLINENAME, CheckState.Unchecked);
                    check++;
                }
                this.lyotmapCheckedCmb.Items.AddRange(itemListQuery);
                this.lyotmapCheckedCmb.AllowMultiSelect = true;
                this.lyotmapCheckedCmb.SelectAllItemVisible = true;
                this.lyotmapCheckedCmb.SelectAllItemCaption = "全选";
                this.lyotmapCheckedCmb.EditValueChanged += LyotmapCheckedCmb_EditValueChanged;
                this.gridControlLymp.DataSource = lymplist;
                this.layoutmapTabpage.PageVisible = true;
                this.TabControl.SelectedTabPage = this.layoutmapTabpage;

                ChangePanelContainerItemVisible();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }
        }

        private void LyotmapCheckedCmb_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string bingdinglinesStr = "";
                for (int i = 0; i < this.lyotmapCheckedCmb.Items.Count; i++)
                {
                    if (this.lyotmapCheckedCmb.Items[i].CheckState == CheckState.Checked)
                        bingdinglinesStr += this.lyotmapCheckedCmb.Items[i].Value.ToString() + ",";
                }

                if (bingdinglinesStr != string.Empty)
                    bingdinglinesStr = bingdinglinesStr.Substring(0, bingdinglinesStr.Length - 1);

                CardView myView = (gridControlLymp.MainView as CardView);
                List<LayoutmapBean> datasource = this.gridControlLymp.DataSource as List<LayoutmapBean>;
                LayoutmapBean focusedRow = datasource[myView.FocusedRowHandle];
                string lyoutmapcode = focusedRow.layoutmapcode;
                //更新数据库
                LayoutmapBll.Instance.UpdateWhatWhere(new { BINDINGLINES = bingdinglinesStr }, new { LAYOUTMAPCODE = lyoutmapcode });
                datasource[myView.FocusedRowHandle].Bindinglines = bingdinglinesStr;
                this.gridControlLymp.DataSource = datasource;
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="username">用户名</param>
        private void DownloadData()
        {
            try
            {
                string datafilepath = this.dockPanelDb.Text.Contains("远程") ? DataFromPath.RemoteDbPath : DataFromPath.LocalDbPath;

                using (new DevExpress.Utils.WaitDialogForm("请稍后……", "正在加载", new Size(250, 50)))
                {
                    List<SiteBean> checkedNodes = xtl.GetCheckedSite(this.treeListData.Name);

                    foreach (SiteBean checkedSb in checkedNodes)
                    {
                        if (this.dockPanelDb.Text.Contains("远程"))
                        {
                            if (CurrentUSerInfo.UIB.UserAthrty.Split(';').ToList() != null)
                                if (!CurrentUSerInfo.UIB.UserAthrty.Split(';').ToList().Contains(checkedSb.UnitCode))
                                {
                                    string unitname = UnitInfoBll.Instance.GetUnitNameBy(checkedSb.UnitCode);
                                    XtraMessageBox.Show("没有下载" + unitname + "数据的权限！", "警告");
                                    continue;
                                }
                        }

                       DataTable linecode = LineObsBll.Instance.GetDataTable("select obslinecode,obslinename from t_obslinetb where SITECODE = '" + checkedSb.SiteCode + "'");

                        foreach (DataRow row in linecode.Rows)
                        {
                            string lCode = row[0].ToString();
                            string lName = row[1].ToString();
                            DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue,note from t_obsrvtntb where OBSLINECODE = '" + lCode + "'");
                            if (dt.Rows.Count > 0)
                            {

                                NpoiCreator npcreator = new NpoiCreator();
                                npcreator.TemplateFile = datafilepath;
                                npcreator.NpoiExcel(dt, lCode + ".xls", datafilepath + "/" + lCode + ".xls");
                            }
                        }
                    }
                 
                   // xtl.bSignDbTree(datafilepath);
                    xtl.RefreshWorkspace(datafilepath);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("下载数据失败：" + ex.Message, "错误");
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
                            if (!LineObsBll.Instance.IsExist(lob.obvdate.ToShortDateString(), linecode))
                            {
                                LineObsBll.Instance.Add(new LineObsBean() { obslinecode = linecode, obvdate = lob.obvdate, obvvalue = lob.obvvalue });
                                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     观测时间：" + lob.obvdate.ToShortDateString() + "  观测值：" + lob.obvvalue + " 已入库！");
                            }
                            else
                            {
                                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "     观测时间：" + lob.obvdate.ToShortDateString() + "  观测值：" + lob.obvvalue + " 已存在！");
                            }
                          
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
            //xtl.bSignDbTree(DataFromPath.LocalDbPath);
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

                if (!cldb.IsLocaldbExist())
                {
                    if (cldb.CreateDatabase("localinfo"))
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    创建数据库成功!");
                    else
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "    创建数据库失败!");
                }
                else
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    本地数据库已存在!");
                }

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
            MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
            xtl.bSignDbTree(DataFromPath.RemoteDbPath);
            dockPanelDb.Text = "本地信息库";
        }

        #endregion


        private void treeListOriData_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
           
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
            this.TabControl.SelectedTabPage = this.recycleTabPage;
            this.recycleControl.LoadRecycleItems();
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
                    {
                        this.tChartControl.PlusMinusMultiplyDivide();
                    }
                    break;
                case "btnRemoveStep"://消台阶
                    {
                        this.tChartControl.RemoStepOrJump(TChartEventType.RemoveStep);
                    }
                    break;
                case "btnRemoveJump"://消突跳
                    {
                        this.tChartControl.RemoStepOrJump(TChartEventType.RemoveJump);
                    }
                    break;
                case "btnLinesUnion"://测线合并
                    {
                        this.tChartControl.LinesUnion();
                    }
                    break;
                case "btnLinesBreak"://测线拆分
                    {
                        this.tChartControl.LinesBreak(TChartEventType.LineBreak);
                    }
                    break;
                case "barSaveToChuLi"://保存处理数据
                    {
                        this.tChartControl.SaveHandleData();
                        xtl.bSignInitManipdbTree();
                    }
                    break;

                case "btnInterval"://等间隔处理
                    {
                        this.tChartControl.IntervalPross();
                    }
                    break;
                case "btnExportToExcel"://输出为Excel
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel文件(*.xls)|*.xls";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            //this.gridControlObsdata.ExportToXls(sfd.FileName);
                        }
                    }
                    break;
                case "btnExportToTXT"://输出为TXT
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "文本文件(*.txt)|*.txt";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            //this.gridControlObsdata.ExportToText(sfd.FileName);
                        }
                      
                    }
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
                    this.tChartControl.btnShowTitle();
                    break;
                case "btnGrid"://网格
                    this.tChartControl.btnGrid();
                    break;
                case "btnShowNote"://备注
                    this.tChartControl.ShowNotes();
                    break;
                case "btnMouseCur"://鼠标热线
                    this.tChartControl.btnMouseCur();
                    break;
                case "btnMaxMinValue"://最大最小值
                    this.tChartControl.btnMaxMinValue();
                    break;
                case "btnHistoryEqk"://历史地震
                    this.tChartControl.GetEqkShowForm();
                    break;
                case "btnExportChart"://导出曲线图
                    this.tChartControl.ExportChart();
                    break;
            }
        }


        

        #region 数据库操作（创建、切换、备份）
        private void btnDb_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnCreateLocaDb"://创建本地库
                    {
                        ProgressForm ptPro = new ProgressForm();
                        ptPro.Show(this);
                        ptPro.progressWorker.DoWork += CreateLocalDb_DoWork;
                        ptPro.beginWorking();
                        ptPro.progressWorker.RunWorkerCompleted += CreateLocalDb_RunWorkerCompleted;

                    }
                    break;
                case "btnSwitchDb"://数据库切换
                    {
                        if (this.dockPanelDb.Text.Contains("远程"))
                        {
                            SwapDb();
                        }
                        else if (this.dockPanelDb.Text.Contains("本地"))
                        {
                            SwapDb();
                        }

                    }
                    break;
                case "btnCopyDb"://数据库备份
                    {
                        try
                        {
                            //String command = "mysqldump --quick --host=localhost --default-character-set=gb2312 --lock-tables --verbose  --force --port=端口号 --user=用户名 --password=密码 数据库名 -r 备份到的地址";

                            if (!System.IO.File.Exists(Application.StartupPath + "\\mysqldump.exe"))
                            {
                                XtraMessageBox.Show("无法完成备份，请检查 mysqldump.exe 是否存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            BakupDbFrm bdf = new BakupDbFrm();
                            if (bdf.ShowDialog() == DialogResult.OK)
                            {
                                //构建执行的命令
                                StringBuilder sbcommand = new StringBuilder();

                                string portnum = bdf.Port;
                                string username = bdf.UserName;
                                string psd = bdf.Psd;
                                string dbname = bdf.DbName;
                                string directory = bdf.SavetoFilename;

                                sbcommand.AppendFormat("mysqldump --quick --host=localhost --default-character-set=gbk --lock-tables --verbose  --force --port=" + portnum +
                                    " --user=" + username + " --password=" + psd + " " + dbname + " -r \"{0}\"", directory);
                                String command = sbcommand.ToString();

                                //获取mysqldump.exe所在路径
                                String appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";
                                Cmd.StartCmd(appDirecroty, command);
                                XtraMessageBox.Show(@"数据库已成功备份到 " + directory + " 文件中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("数据库备份失败！","错误");

                        }
                    }
                    break;
                case "btnDbExeute"://数据库还原
                    {
                        if (!System.IO.File.Exists(Application.StartupPath + "\\mysql.exe"))
                        {
                            XtraMessageBox.Show("无法完成数据库恢复，请检查 mysql.exe 是否存在。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }


                        try
                        {

                            RestoreDbFrm bdf = new RestoreDbFrm();
                            if (bdf.ShowDialog() == DialogResult.OK)
                            {
                                //构建执行的命令
                                StringBuilder sbcommand = new StringBuilder();

                                string portnum = bdf.Port;
                                string username = bdf.UserName;
                                string psd = bdf.Psd;
                                string dbname = bdf.DbName;
                                string directory = bdf.SavetoFilename;

                                //在文件路径后面加上""避免空格出现异常
                                sbcommand.AppendFormat("mysql --host=localhost --default-character-set=gbk --port=" + portnum
                                    + " --user=" + username + " --password=" + psd + " " + dbname + "<\"{0}\"", directory);
                                String command = sbcommand.ToString();

                                //获取mysql.exe所在路径
                                String appDirecroty = System.Windows.Forms.Application.StartupPath + "\\";

                                DialogResult result = XtraMessageBox.Show("您是否真的想覆盖以前的数据库吗？那么以前的数据库数据将丢失！！！", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    Cmd.StartCmd(appDirecroty, command);
                                    XtraMessageBox.Show("数据库还原成功！");
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("数据库还原失败！");
                        }



                    }
                    break;
            }
        }

        /// <summary>
        /// 切换数据库和列表
        /// </summary>
        private void SwapDb()
        {

            if (this.dockPanelDb.Text.Contains("本地"))
            {
                MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;
                xtl.bSignDbTree(DataFromPath.RemoteDbPath);
                dockPanelDb.Text = "远程信息库";

            }
            else if (this.dockPanelDb.Text.Contains("远程"))
            {
                MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;

                CreateLocalDb Createlocaldb = new CreateLocalDb();

                if (Createlocaldb.IsLocaldbExist())
                {
                    if (Createlocaldb.FullTableValidate())
                    {
                        xtl.bSignDbTree(DataFromPath.LocalDbPath);
                        dockPanelDb.Text = "本地信息库";
                    }
                    else
                    {
                        if (XtraMessageBox.Show("本地库数据表不完整，是否重新创建？", "提示") == System.Windows.Forms.DialogResult.OK)
                        {
                            ProgressForm ptPro = new ProgressForm();
                            ptPro.Show(this);
                            ptPro.progressWorker.DoWork += CreateLocalDb_DoWork;
                            ptPro.beginWorking();
                            ptPro.progressWorker.RunWorkerCompleted += CreateLocalDb_RunWorkerCompleted;
                        }
                    }

                }
                else
                {
                    if (XtraMessageBox.Show("本地库不存在，是否创建？", "提示") == System.Windows.Forms.DialogResult.OK)
                    {
                        ProgressForm ptPro = new ProgressForm();
                        ptPro.Show(this);
                        ptPro.progressWorker.DoWork += CreateLocalDb_DoWork;
                        ptPro.beginWorking();
                        ptPro.progressWorker.RunWorkerCompleted += CreateLocalDb_RunWorkerCompleted;
                    }
                }

            }
        }

        #endregion

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            ChangePanelContainerItemVisible();
        }

        /// <summary>
        /// 地震目录列表行双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlEqklist_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = this.gridViewEqklist.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 执行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        if (TabControl.SelectedTabPage.Name != "mapTabPage")
                            TabControl.SelectedTabPage = mapTabPage;
;
                        DataRowView drv = (DataRowView)this.gridViewEqklist.GetRow(hInfo.RowHandle);

                        this.gMapCtrl.Position = new PointLatLng(double.Parse(drv["Latitude"].ToString()), double.Parse(drv["Longtitude"].ToString()));
                        //PointLatLng sitepoint = new PointLatLng(sb.Latitude, sb.Longtitude);
                        //gmapcontrol.Position = sitepoint;
                        this.gMapCtrl.Zoom = 6;
                        //gmapcontrol.Refresh();
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }

        private void dockPanelEqkCatalog_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            IsEqkShow = false;
            this.gridControlEqklist.DataSource = null;
            this.gridControlEqklist.Refresh();
            GMapMarkerKdcSite.ClearAllEqkMarker(gMapCtrl);

        }


        /// <summary>  
        /// 分页事件处理  
        /// </summary>  
        /// <param name="eventString">事件名称</param>  
        /// <param name="button">按钮控件</param>  
        /// <author>PengZhen</author>  
        /// <time>2013-11-5 14:25:59</time>  
        void ShowEvent(string eventString, NavigatorButtonBase button)
        {
            NavigatorCustomButton btn = (NavigatorCustomButton)button;
           
        }

        /// <summary>  
        /// 绑定分页控件和GridControl数据  
        /// </summary>  
        /// <author>PengZhen</author>  
        /// <time>2013-11-5 14:22:22</time>  
        /// <param name="strWhere">查询条件</param>  
        public void BindPageGridList(string strWhere)
        {

            //记录获取开始数  
            int startIndex = (pageIndex - 1) * pagesize;
            //结束数  
            int endIndex = pageIndex * pagesize;

            //总行数  
              
            int row = EqkBll.Instance.GetRecordCount(Regex.Split(strWhere, "ORDER", RegexOptions.IgnoreCase)[0]);

            //获取总页数    
            if (row % pagesize > 0)
            {
                pageCount = row / pagesize + 1;
            }
            else
            {
                pageCount = row / pagesize;
            }

            if (pageIndex == 1)
            {
                dataNavigator.Buttons.First.Enabled = false;
                dataNavigator.Buttons.Prev.Enabled = false;
                dataNavigator.Buttons.Next.Enabled = true;
                dataNavigator.Buttons.Last.Enabled = true;
            }

            //最后页时获取真实记录数  
            if (pageCount == pageIndex)
            {
                endIndex = row;
                dataNavigator.Buttons.First.Enabled = true;
                dataNavigator.Buttons.Prev.Enabled = true;
                dataNavigator.Buttons.Next.Enabled = false;
                dataNavigator.Buttons.Last.Enabled = false;
            }

            List<EqkBean> eqkDataList = EqkBll.Instance.GetListByPage(strWhere, "").ToList();
         
            if (eqkDataList.Count() > 0)
            {
                this.TabControl.SelectedTabPage = this.mapTabPage;
                IsEqkShow = true;
                ChangePanelContainerItemVisible();
                ModelHandler<EqkBean> mh = new ModelHandler<EqkBean>();

                DataTable eqkShowData = mh.FillDataTable(eqkDataList);

                this.gridControlEqklist.DataSource = eqkShowData;
                this.gridControlEqklist.Refresh();
                dataNavigator.DataSource = eqkShowData;
                dataNavigator.TextStringFormat = string.Format("第 {0}页, 共 {1}页", pageIndex, pageCount);

                GMapMarkerKdcSite.ClearAllEqkMarker(gMapCtrl);
                GMapMarkerKdcSite.AnnotationEqkToMap(eqkDataList, gMapCtrl);

            }
            else
            {
                throw new Exception("没有相应震例");
            }
        }

        /// <summary>  
        /// 获取查询条件  
        /// </summary>  
        /// <author>PengZhen</author>  
        /// <time>2013-11-5 15:25:00</time>  
        /// <returns>返回查询条件</returns>  
        private string GetSqlWhere()
        {
            //查询条件  
            string strReturnWhere = " 1=1 ";


            float eqkMlMin = float.NaN;
            float eqkMlMax = float.NaN;
            try
            {
                eqkMlMin = float.Parse(this.beiEqkMinMtd.EditValue.ToString());
                eqkMlMax = float.Parse(this.beiEqkMaxMtd.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("不是有效的震级！", "提示");
                return string.Empty;
            }

            if (eqkMlMin > eqkMlMax)
            {
                XtraMessageBox.Show("最大震级应大于最小震级，重新输入！", "提示");
                this.beiEqkMinMtd.EditValue = "";
                this.beiEqkMaxMtd.EditValue = "";
                return string.Empty;
            }


            float eqkDepthMin = float.NaN;
            float eqkDepthMax = float.NaN;
            try
            {
                eqkDepthMin = float.Parse(this.beiEqkMinDepth.EditValue.ToString());
                eqkDepthMax = float.Parse(this.beiEqkMaxDepth.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("不是有效的震源深度值！", "提示");
                return string.Empty;
            }

            if (eqkDepthMin > eqkDepthMax)
            {
                XtraMessageBox.Show("最大深度应大于最小深度，重新输入！", "提示");
                this.beiEqkMinDepth.EditValue = "";
                this.beiEqkMinDepth.EditValue = "";
                return string.Empty;
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
                return string.Empty;
            }


            if (eqkMlMin == eqkMlMax) strReturnWhere += " and MAGNTD = " + eqkMlMin;
            else
                strReturnWhere += " and MAGNTD >= " + eqkMlMin + " and MAGNTD <=" + eqkMlMax;

            if (eqkDepthMin == eqkDepthMax)
                strReturnWhere += " and DEPTH =" + eqkDepthMin;
            else
                strReturnWhere += " and DEPTH >=" + eqkDepthMin + " and DEPTH <=" + eqkDepthMax;

            if (DateTime.Compare(timeSt, timeEd) == 0)
                strReturnWhere += " and EAKDATE =" + "'" + timeSt.ToString() + "'";
            else
                strReturnWhere += " and EAKDATE between '" + timeSt.ToString() + "' and '" + timeEd.ToString() + "'";

              return strReturnWhere += " ORDER BY t.EQKCODE limit " + pageIndex.ToString() + "," + pagesize.ToString() + "";
        }

        private void dataNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            string type = e.Button.Tag.ToString();
            if (type == "首页")
            {
                pageIndex = 1;
            }

            if (type == "下一页")
            {
                pageIndex++;
            }

            if (type == "末页")
            {
                pageIndex = pageCount;
            }

            if (type == "上一页")
            {
                pageIndex--;
            }

            //绑定分页控件和GridControl数据  
            try
            {
                string sqlwhere = GetSqlWhere();
                if (sqlwhere != string.Empty)
                    BindPageGridList(sqlwhere);
                else
                    throw new Exception("不是有效的查询语句");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("查询失败：" + ex.Message, "错误");
            }

        }

        private void btnXxkAdd_Click(object sender, EventArgs e)
        {
            vGridControlSiteInfo.UpdateFocusedRecord();
            try
            {
                SiteBean AddSiteinfo = new SiteBean();
                string ldordd = GetValue(4);

                if (ldordd == "流动")
                {
                    ldordd = "LD";
                }
                else if (ldordd == "定点")
                {
                    ldordd = "DD";
                }
                else if (ldordd == string.Empty)
                {
                    XtraMessageBox.Show("场地类型不能为空", "提示");
                    return;
                }
                AddSiteinfo.UnitCode = currentClickNodeInfo.ParentFieldName;
                if (this.btnXxkAdd.Text == "更新至数据库")
                {
                    AddSiteinfo.SiteCode = currentClickNodeInfo.KeyFieldName;
                }
                else if (this.btnXxkAdd.Text == "上传至数据库")
                {
                    AddSiteinfo.SiteCode = SiteBll.Instance.CreateNewSiteCode(ldordd);
                }
                AddSiteinfo.SiteName = GetValue(0);
                AddSiteinfo.FaultName = GetValue(1);
                AddSiteinfo.SiteStatus = GetValue(2);
                AddSiteinfo.Historysite = GetValue(3);
                AddSiteinfo.Type = GetValue(5);
                AddSiteinfo.Locations = GetValue(6);
                AddSiteinfo.MarkStoneType = GetValue(7);

                double lat = double.NaN, ln = double.NaN, alt = double.NaN;
                double.TryParse(GetValue(8), out lat);
                double.TryParse(GetValue(9), out ln);
                double.TryParse(GetValue(10), out alt);

                AddSiteinfo.Latitude = lat;
                AddSiteinfo.Longtitude = ln;
                AddSiteinfo.Altitude = alt;

                AddSiteinfo.Place = GetValue(11);
                AddSiteinfo.BuildUnit = GetValue(12);
                AddSiteinfo.ObsUnit = GetValue(13);
                AddSiteinfo.StartDate = GetValue(14);
                AddSiteinfo.Datachg = GetValue(15);
                AddSiteinfo.SiteSituation = GetValue(16);
                AddSiteinfo.GeoSituation = GetValue(17);

                AddSiteinfo.RemoteMap = GetPicStream(18);
                //byte[] blobData = File.ReadAllBytes(filename);
                AddSiteinfo.LayoutMap = GetPicStream(19);
                AddSiteinfo.OtherSituation = GetValue(20);
                AddSiteinfo.Note = GetValue(21);

                if (this.btnXxkAdd.Text == "更新至数据库")
                {
                    SiteBll.Instance.UpdateWhatWhere(
                        new
                        {
                            sitename = AddSiteinfo.SiteName,
                            faultname = AddSiteinfo.FaultName,
                            sitestatus = AddSiteinfo.SiteStatus,
                            historysite = AddSiteinfo.Historysite,
                            unitcode = AddSiteinfo.UnitCode,
                            type = AddSiteinfo.Type,
                            place = AddSiteinfo.Place,
                            markstonetype = AddSiteinfo.MarkStoneType,
                            locations = AddSiteinfo.Locations,
                            altitude = AddSiteinfo.Altitude,
                            buildunit = AddSiteinfo.BuildUnit,
                            obsunit = AddSiteinfo.ObsUnit,
                            startdate = AddSiteinfo.StartDate,
                            datachg = AddSiteinfo.Datachg,
                            sitesituation = AddSiteinfo.SiteSituation,
                            geosituation = AddSiteinfo.GeoSituation,
                            note = AddSiteinfo.Note,
                            othersituation = AddSiteinfo.OtherSituation
                        },
                        new { sitecode = AddSiteinfo.SiteCode }
                        );

                    if (AddSiteinfo.RemoteMap != null)
                        if (AddSiteinfo.RemoteMap.Length != 0)
                        {
                            SiteBll.Instance.UpdateWhatWhere(new { remotemap = AddSiteinfo.RemoteMap }, new { sitecode = AddSiteinfo.SiteCode });
                        }
                    if (AddSiteinfo.LayoutMap != null)
                        if (AddSiteinfo.LayoutMap.Length != 0)
                        {
                            SiteBll.Instance.UpdateWhatWhere(new { layoutmap = AddSiteinfo.LayoutMap }, new { sitecode = AddSiteinfo.SiteCode });
                        }
                }
                else
                {
                    SiteBll.Instance.Add(AddSiteinfo);
                }

                XtraMessageBox.Show("上传成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


                string curentdbfile = string.Empty;
                if (this.dockPanelDb.Text == "远程信息库")
                {
                    curentdbfile = DataFromPath.RemoteDbPath;
                    MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;
                }
                else if (this.dockPanelDb.Text == "本地信息库")
                {
                    curentdbfile = DataFromPath.LocalDbPath;
                    MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["LocalDbConnnect"].ConnectionString;
                }

                //加载树列表
                xtl.bSignDbTree(curentdbfile);
                //加载场地标记
                GMapMarkerKdcSite.LoadSiteMarker(SiteBll.Instance.GetAll(), gMapCtrl);
                //隐藏新增页面
                this.addXxkTabPage.PageVisible = false;
                //重置表格
                for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                {
                    vGridControlSiteInfo.Rows[i].Properties.Value = "";
                }
            }
            catch (Exception excep)
            {
                XtraMessageBox.Show("上传编辑后的值失败，" + excep.Message, "错误提示");
                return;
            }
            btnXxkAdd.Enabled = false;
        }

        /// 设置是VGridControl行列样式
        /// </summary>
        /// 设置是VGridControl行列样式
        /// </summary>
        private void SetBaseinfoVGridControl()
        {
            try
            {
                PublicHelper ph = new PublicHelper();
                int cHeight = vGridControlSiteInfo.Height;

                DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
                memoEdit.LinesCount = 1;

                wxtFileBtnEdit.ButtonClick += wxtFileBtnEdit_ButtonClick;
                bstFileBtnEdit.ButtonClick += bstFileBtnEdit_ButtonClick;

                for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                {
                    vGridControlSiteInfo.Rows[i].Properties.ReadOnly = false;
                    vGridControlSiteInfo.Rows[i].Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;

                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    if (i == 2)//运行状况
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "正常", "停测", "改造中" });
                    }
                    if (i == 4)//场地类型
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "定点", "流动" });
                    }
                    if (i == 5)//观测类型
                    {

                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "基线", "水准", "综合" });
                    }
                    if (i == 7)//标石类型
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "水准标石", "综合观测墩" });
                    }
                    if (i == 12)//建设单位
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "北京局", "天津局","河北局","山西局","内蒙局","辽宁局", "吉林局","黑龙江局","上海局"
                        ,"江苏局","浙江局","安徽局", "福建局","江西局","山东局","河南局","湖南局","湖北局","广东局","广西局","海南局" ,"重庆局","四川局","云南局","西藏局", "陕西局","甘肃局"
                        ,"青海局","宁夏局","新疆局","贵州局","台网中心","搜救中心","震防中心","地壳工程中心","物探中心","一测中心","二测中心","驻深办","服务中心","出版社","防灾学院","地球所"
                        ,"地质所","地壳所","预测所","工力所"});
                    }
                    if (i == 13)//监测单位
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "北京局", "天津局","河北局","山西局","内蒙局","辽宁局", "吉林局","黑龙江局","上海局"
                        ,"江苏局","浙江局","安徽局", "福建局","江西局","山东局","河南局","湖南局","湖北局","广东局","广西局","海南局" ,"重庆局","四川局","云南局","西藏局", "陕西局","甘肃局"
                        ,"青海局","宁夏局","新疆局","贵州局","台网中心","搜救中心","震防中心","地壳工程中心","物探中心","一测中心","二测中心","驻深办","服务中心","出版社","防灾学院","地球所"
                        ,"地质所","地壳所","预测所","工力所"});
                    }
                    if (i == vGridControlSiteInfo.Rows.Count - 1 || i == vGridControlSiteInfo.Rows.Count - 2)
                    {
                        vGridControlSiteInfo.Rows[i].Height = (cHeight) / vGridControlSiteInfo.Rows.Count * 3;
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = memoEdit;
                    }
                    else
                        vGridControlSiteInfo.Rows[i].Height = (cHeight) / vGridControlSiteInfo.Rows.Count;
                }

                vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
                vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 20;
                //vGridControlSiteInfo.Rows[0].Height = vGridControlSiteInfo.Width / 3 * 2 - 10;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        /// <summary>
        /// 设置VGrid行值
        /// </summary>
        /// <param name="sb"></param>
        private void SetSiteValueVGridControl(SiteBean sb, bool NewSite)
        {
            if (NewSite)
            {
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[0], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[1], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[2], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[3], 0,"");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[4], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[5], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[6], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[7], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[8], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[9], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[10], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[11], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[12], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[13], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[14], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[15], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[16], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[17], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[20], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[21], 0, "");
            }
            else
            {
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[0], 0, sb.SiteName);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[1], 0, sb.FaultName);

                string siteStatus = sb.SiteStatus == "0" ? "正常" : (sb.SiteStatus == "1" ? "停测" : "改造中");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[2], 0, siteStatus);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[3], 0, sb.Historysite);

                string siteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[4], 0, siteType);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[5], 0, sb.Type);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[6], 0, sb.Locations);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[7], 0, sb.MarkStoneType);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[8], 0, sb.Latitude);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[9], 0, sb.Longtitude);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[10], 0, sb.Altitude);

                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[11], 0, sb.Place);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[12], 0, sb.BuildUnit);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[13], 0, sb.ObsUnit);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[14], 0, sb.StartDate);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[15], 0, sb.Datachg);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[16], 0, sb.SiteSituation);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[17], 0, sb.GeoSituation);


                //vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[18], 0, sb.RemoteMap);
                //vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[19], 0, sb.LayoutMap);

                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[20], 0, sb.OtherSituation);
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[21], 0, sb.Note);
            }
        }

        void wxtFileBtnEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[16], 0, ofd.FileName);
        }
        void bstFileBtnEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[17], 0, ofd.FileName);
        }

        private string GetValue(int row)
        {
            int iRecordIndex = 0;
            Type type = vGridControlSiteInfo.Rows[row].Properties.RowType;

            object value = vGridControlSiteInfo.GetCellValue(vGridControlSiteInfo.Rows[row], iRecordIndex);

            if (type.FullName == "System.Int32")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.Double")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.DataTime")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.Decimal")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }

            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        private byte[] GetPicStream(int row)
        {
            try
            {
                int iRecordIndex = 0;
                Type type = vGridControlSiteInfo.Rows[row].Properties.RowType;

                object value = vGridControlSiteInfo.GetCellValue(vGridControlSiteInfo.Rows[row], iRecordIndex);

                if (type.FullName == "System.Byte[]")
                {
                    value = (value == DBNull.Value || value == null) ? "null" : value;
                }

                if (value != "null")
                {
                    return (byte[])value;
                }
                else
                {
                    return new byte[0];
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }

        }


        private void recycleControl_RefreshTree(string dbpath)
        {
            xtl.bSignDbTree(dbpath);
            xtl.bSignInitManipdbTree();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //等间隔处理
            //返回值为datatable
            
        }

        private void btnSignup_ItemClick(object sender, ItemClickEventArgs e)
        {
            SignUp sufrm = new SignUp();
            sufrm.ShowDialog(this);
		}
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs EArg = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            string name = EArg.Page.Text;//得到关闭的选项卡的text  
            foreach (XtraTabPage page in TabControl.TabPages)//遍历得到和关闭的选项卡一样的Text  
            {
                if (page.Text == name)
                {
                    //xtraTabControl1.TabPages.Remove(page);
                    //page.Dispose();
                    page.PageVisible = false;
                    return;
                }
            }
        }

        private void cardViewLymp_CustomDrawCardCaption(object sender, DevExpress.XtraGrid.Views.Card.CardCaptionCustomDrawEventArgs e)
        {
            var view = sender as CardView;
            var isFocused = (e.RowHandle == view.FocusedRowHandle);

            //caption的背景
            Brush backBrush;
            if (isFocused)
            {
                backBrush = e.Cache.GetGradientBrush(e.Bounds, Color.FromArgb(0, 120, 215), Color.FromArgb(0, 120, 215), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            }
            else
            {
                backBrush = e.Cache.GetGradientBrush(e.Bounds, Color.LightGray, Color.LightGray, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            }
            //caption字体刷
            var foreBrush = isFocused ? Brushes.White : Brushes.Black;

            var rect = e.Bounds;
            rect.Inflate(1, 1);//边缘加1是因为程序启动后目测边缘少1px
            //画3d效果边缘
            ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.RaisedOuter);
            //边缘画好后填入背景色
            e.Graphics.FillRectangle(backBrush, rect);

            //背景填好后画文字
            e.Appearance.DrawString(e.Cache, view.GetCardCaption(e.RowHandle), rect, foreBrush);
            //句柄为true表明使用当前自定义的capiton
            e.Handled = true;

        }

        private void barFault_EditValueChanged(object sender, EventArgs e)
        {
            List<string> checkedcountys = new List<string>();
            for (int i = 0; i < this.faultChckCbbxEdit.Items.Count; i++)
            {
                if (this.faultChckCbbxEdit.Items[i].CheckState == CheckState.Checked)
                {
                    switch (this.faultChckCbbxEdit.Items[i].Value.ToString())
                    {
                        case "前第四纪活动断裂(隐伏)":
                            {
                                string fp = Application.StartupPath + "//断层数据//前第四纪活动断裂(隐伏).dat";

                            }
                            break;
                    }
                }
                else if (this.faultChckCbbxEdit.Items[i].CheckState == CheckState.Unchecked)
                { } 
            }
        }
    }
}