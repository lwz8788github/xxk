using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.BLL;
using xxkUI.Model;
using xxkUI.Form;
using DevExpress.XtraTreeList.Nodes;
using System.IO;

namespace xxkUI.MyCls
{
   public class XTreeList
    {
        private DevExpress.XtraTreeList.TreeList treeListOriData;
         private DevExpress.XtraTreeList.TreeList treeListWorkSpace;

         public XTreeList(DevExpress.XtraTreeList.TreeList _treeListOriData,DevExpress.XtraTreeList.TreeList _treeListWorkSpace)
         {

            _treeListOriData.LookAndFeel.UseDefaultLookAndFeel = false;
            _treeListOriData.LookAndFeel.UseWindowsXPTheme = true;

            _treeListWorkSpace.LookAndFeel.UseDefaultLookAndFeel = false;
            _treeListWorkSpace.LookAndFeel.UseWindowsXPTheme = true;

            treeListOriData = _treeListOriData;
            treeListWorkSpace = _treeListWorkSpace;


         }
        
        /// <summary>
        /// 加载树和地图
        /// </summary>
        /// <param name="userAhtList"></param>
        /// <param name="gmmkks"></param>
        public void InitOriDataTree(List<string> userAhtList, GMapMarkerKdcSite gmmkks) 
        {
            try
            {
                List<TreeBean> treelistOriData = new List<TreeBean>();
                List<TreeBean> treelistWorkSpace = new List<TreeBean>();

                IEnumerable<UnitInfoBean> ubEnumt = UnitInfoBll.Instance.GetAll();

                foreach (UnitInfoBean sb in ubEnumt)
                {
                    TreeBean tb = new TreeBean();
                    if (sb.UnitCode == "152002" || sb.UnitCode == "152003"
                        || sb.UnitCode == "152006" || sb.UnitCode == "152008"
                        || sb.UnitCode == "152009" || sb.UnitCode == "152010"
                        || sb.UnitCode == "152012" || sb.UnitCode == "152015"
                        || sb.UnitCode == "152022" || sb.UnitCode == "152023"
                        || sb.UnitCode == "152026" || sb.UnitCode == "152029"
                        || sb.UnitCode == "152032" || sb.UnitCode == "152034"
                        || sb.UnitCode == "152035" || sb.UnitCode == "152036"
                        || sb.UnitCode == "152039" || sb.UnitCode == "152040"
                        || sb.UnitCode == "152041" || sb.UnitCode == "152042"
                        || sb.UnitCode == "152043" || sb.UnitCode == "152044"
                        || sb.UnitCode == "152045" || sb.UnitCode == "152046"
                        || sb.UnitCode == "152001" || sb.UnitCode == "152047") { continue; }
                    if (userAhtList.Contains(sb.UnitCode))
                    {
                        tb.KeyFieldName = sb.UnitCode;
                        tb.ParentFieldName = "0";
                        tb.Caption = sb.UnitName;
                        tb.SiteType = "";
                        tb.LineStatus = "";
                        tb.Tag = sb;//lwl
                        treelistOriData.Add(tb);
                        treelistWorkSpace.Add(tb);

                    }
                }

                #region 将加载场地标记的的过程移植到此处(lwl)
                string userahths = "(";
                foreach (string str in userAhtList)
                {
                    userahths += "'" + str + "',";
                }
                userahths = userahths.Substring(0, userahths.Length - 1) + ")";
                IEnumerable<SiteBean> sbEnumt = SiteBll.Instance.GetSitesByAuth(userahths);
                gmmkks.LoadSiteMarker(sbEnumt);
                #endregion

                List<string> olSiteCode = new List<string>();
                foreach (SiteBean sb in sbEnumt)
                {
                    olSiteCode.Add(sb.SiteCode);
                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = sb.SiteCode;
                    tb.ParentFieldName = sb.UnitCode;
                    tb.Caption = sb.SiteName;
                    tb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                    tb.Tag = sb;//lwl

                    treelistOriData.Add(tb);
                    treelistWorkSpace.Add(tb);

                }

                //测线列表显示
                IEnumerable<LineBean> olEnumt = LineBll.Instance.GetAll();
                foreach (LineBean ol in olEnumt)
                {
                    if (olSiteCode.Contains(ol.SITECODE))
                    {
                        TreeBean tb = new TreeBean();
                        tb.KeyFieldName = ol.OBSLINECODE;
                        tb.ParentFieldName = ol.SITECODE;
                        tb.Caption = ol.OBSLINENAME;
                        tb.LineStatus = ol.LineStatus == "0" ? "正常" : (ol.LineStatus == "1" ? "停测" : "改造中");
                        tb.Tag = ol;//lwl
                        treelistOriData.Add(tb);
                    }
                }

                //原始数据树列表显示
                this.treeListOriData.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListOriData.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定
                this.treeListOriData.DataSource = treelistOriData;　　//绑定数据源
                this.treeListOriData.OptionsView.ShowCheckBoxes = true;
                this.treeListOriData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListOriData.OptionsBehavior.Editable = false;

                List<String> excelList = new List<string>();
                string excelPath = Application.StartupPath + "/myworkspace";
                excelList = getFile(excelPath);
                foreach (string lineCode in excelList)
                {
                    string subLineCode = lineCode.Substring(0, lineCode.Length - 4);
                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = subLineCode;
                    tb.Caption = LineBll.Instance.GetNameByID("OBSLINENAME", "OBSLINECODE", subLineCode);
                    tb.ParentFieldName = LineBll.Instance.GetNameByID("SITECODE", "OBSLINECODE", subLineCode);
                    treelistWorkSpace.Add(tb);
                }

                //工作区树列表显示
                this.treeListWorkSpace.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListWorkSpace.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定
                this.treeListWorkSpace.DataSource = treelistWorkSpace;　　//绑定数据源

                this.treeListWorkSpace.OptionsView.ShowCheckBoxes = true;
                this.treeListWorkSpace.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListWorkSpace.OptionsBehavior.Editable = false;

                //InitNodeImg(this.treeListOriData.Nodes);
                //SetImageIndex(this.treeListOriData, null, 1, 0);


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        /// <summary> 
        /// 设置TreeList显示的图标 
        /// </summary> 
        /// <param name="tl">TreeList组件</param> 
        /// <param name="node">当前结点，从根结构递归时此值必须=null</param> 
        /// <param name="nodeIndex">根结点图标(无子结点)</param> 
        /// <param name="parentIndex">有子结点的图标</param> 
        private  void SetImageIndex(TreeList tl, TreeListNode node, int nodeIndex, int parentIndex)
        {
            if (node == null)
            {
                foreach (TreeListNode N in tl.Nodes)
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
            }
            else
            {
                if (node.HasChildren || node.ParentNode == null)
                {
                    //node.SelectImageIndex = parentIndex; 
                    node.StateImageIndex = parentIndex;
                    node.ImageIndex = parentIndex;
                }
                else
                {
                    //node.SelectImageIndex = nodeIndex; 
                    node.StateImageIndex = nodeIndex;
                    node.ImageIndex = nodeIndex;
                }

                foreach (TreeListNode N in node.Nodes)
                {
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
                }
            }
        }

        /// <summary>
        /// 太慢了，暂时不用
        /// /// </summary>
        /// <param name="nodes"></param>
        private void InitNodeImg(TreeListNodes nodes)
        {
            foreach (TreeListNode node in nodes)
            {
                if (node.Level == 1)
                {
                    TreeBean tb = node.TreeList.GetDataRecordByNode(node) as TreeBean;
                    if (tb != null)
                    {
                        SiteBean sb = tb.Tag as SiteBean;
                        if (sb.SiteCode.Substring(0, 1) == "L")
                        {
                            node.StateImageIndex = 1;
                            node.ImageIndex = 1;
                        }
                        else
                        {
                            node.StateImageIndex = 0;
                            node.ImageIndex = 0;
                        }
                    }
                }
                else
                {
                    node.StateImageIndex = -1;
                    node.ImageIndex = -1;
                }
                
                if (node.HasChildren)
                {
                    InitNodeImg(node.Nodes);
                }
            }
        }

        /// <summary>
        /// 刷新原始数据树，未完待续
        /// </summary>
        public void RefreshOrigData()
        {

        }

        /// <summary>
        /// 刷新工作区
        /// </summary>
        public void RefreshWorkspace()
        {
            try
            {
                List<TreeBean> treelistOriData = this.treeListWorkSpace.DataSource as List<TreeBean>;

                string excelPath = Application.StartupPath + "/myworkspace";
                List<String> excelList = new List<string>();

                excelList = getFile(excelPath);
                foreach (string lineCode in excelList)
                {
                    string subLineCode = lineCode.Substring(0, lineCode.Length - 4);
                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = subLineCode;
                    tb.Caption = LineBll.Instance.GetNameByID("OBSLINENAME", "OBSLINECODE", subLineCode);
                    tb.ParentFieldName = LineBll.Instance.GetNameByID("SITECODE", "OBSLINECODE", subLineCode);
                    if (treelistOriData.Find(n => n.KeyFieldName == subLineCode) == null)
                        treelistOriData.Add(tb);

                }

                this.treeListWorkSpace.RefreshDataSource();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void GetObsDataByUser(string username)
        {
            //1.根据username查询权限，并放入list中
            List<string> userlist = new List<string>();
            // userlist = ;
            //2.遍历list下载数据



        }

       ///<summary>
       ///遍历获取文件夹下的文件名
       ///<\summary>
       ///
        private List<string> getFile(string SourcePath)
        {
            List<String> list = new List<string>();
            //遍历文件夹
            DirectoryInfo theFolder = new DirectoryInfo(SourcePath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
                //list.Add(NextFile.FullName);
                list.Add(NextFile.Name);
                
            //遍历子文件夹
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                //list.Add(NextFolder.ToString());
                FileInfo[] fileInfo = NextFolder.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo NextFile in fileInfo)  //遍历文件
                    list.Add(NextFile.Name);
                    //list.Add(NextFile.FullName);
            }
            return list;
        }

        //private void treeListOriData_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        //{
        //    if (e.Column == treeListColumn1)
        //    {
        //        if (e.CellValue.ToString()!="")
        //        {
        //            e.Appearance.BackColor = Color.LightGray;
        //            e.Appearance.Options.UseBackColor = true;
        //        }
        //    }
        //}

        //private void treeListWorkSpace_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        //{
        //    if (e.Column == treeListColumn4)
        //    {

        //        if (e.CellValue.ToString() != "")
        //        {
        //            e.Appearance.BackColor = Color.LightGray;
        //            e.Appearance.Options.UseBackColor = true;
        //        }
        //    }
        //}


        /// <summary>
        /// 禁止操作节点CheckBox
        /// 说明
        /// 在BeforeCheckNode事件中使用
        /// </summary>
        /// <param name="tree">TreeListNode</param>
        /// <param name="conditionHanlder">委托</param>
        /// <param name="e">CheckNodeEventArgs</param>
        private void treeListWorkSpace_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = false;
            //if ((bool)sender)
            //{
            //    e.CanCheck = true;
            //}

        }

        private void treeListOriData_BeforeCheckNode_1(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = false;
            //if ((bool) sender)
            //{
            //    e.CanCheck = true;
            //}
        }

        /// <summary>
        /// 获取选中的测线节点tag lwl
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public List<LineBean> GetCheckedLine(string treeType)
        {
            TreeList tree = null;
            if (treeType == "treeListOriData")
                tree = this.treeListOriData;
            else if (treeType == "treeListWorkSpace")
                tree = this.treeListWorkSpace;

            List<LineBean> lblist = new List<LineBean>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                     GetCheckedNode(dn, ref lblist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;
                
        }


        public List<TreeListNode> GetNodesByKey(string treeType,string keyfieldname)
        {
            TreeList tree = null;
            if (treeType == "treeListOriData")
                tree = this.treeListOriData;
            else if (treeType == "treeListWorkSpace")
                tree = this.treeListWorkSpace;

            List<TreeListNode> lblist = new List<TreeListNode>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    GetNodesRec(dn, keyfieldname, ref lblist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;

        }
        /// <summary>
        /// 获取选择状态的数据主键ID集合  lwl
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetCheckedNode(TreeListNode parentNode, ref List<LineBean> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {
               
                    if (node.CheckState == CheckState.Checked)
                    {
                        TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;
                        LineBean tag = nodeInfo.Tag as LineBean;
                        if (tag != null)
                        {
                            lblist.Add(tag);
                        }
                    }
                    GetCheckedNode(node, ref lblist);
              
            }

        }


        private void GetNodesRec(TreeListNode parentNode, string keyfieldname,ref List<TreeListNode> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {
                TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;

                if (nodeInfo.KeyFieldName == keyfieldname)
                    lblist.Add(node);

                GetNodesRec(node, keyfieldname, ref lblist);

            }

        }


        public void ClearTreelistNodes()
        {
            this.treeListOriData.ClearNodes();
            this.treeListWorkSpace.ClearNodes();
        }
    }
}
