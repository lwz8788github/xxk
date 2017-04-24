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

namespace xxkUI.MyCls
{
   public class XTreeList
    {
        private DevExpress.XtraTreeList.TreeList treeListOriData;
         private DevExpress.XtraTreeList.TreeList treeListWorkSpace;

         public XTreeList(DevExpress.XtraTreeList.TreeList _treeListOriData,DevExpress.XtraTreeList.TreeList _treeListWorkSpace)
         {
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
                List<TreeBean> treelist = new List<TreeBean>();
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
                        treelist.Add(tb);
                    }
                }

                #region 将加载场地标记的的过程移植到此处(lwl)
                string userahths = "(";
                foreach (string str in userAhtList)
                {
                    userahths +="'"+ str + "',";
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
                    treelist.Add(tb);
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
                        treelist.Add(tb);
                    }
                }

                //原始数据树列表显示

                this.treeListOriData.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListOriData.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定

                this.treeListOriData.DataSource = treelist;　　//绑定数据源
                //this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
                this.treeListOriData.OptionsView.ShowCheckBoxes = true;
                this.treeListOriData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListOriData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListOriData.OptionsBehavior.Editable = false;
                //this.treeListOriData.CustomDrawNodeCell += treeListOriData_CustomDrawNodeCell;
                //this.treeListOriData.BeforeCheckNode += treeListOriData_BeforeCheckNode_1;

                //工作区树列表显示
                this.treeListWorkSpace.KeyFieldName = "KeyFieldName";　　　　      //这里绑定的ID的值必须是独一无二的
                this.treeListWorkSpace.ParentFieldName = "ParentFieldName";　　//表示使用parentID进行树形绑定

                this.treeListWorkSpace.DataSource = treelist;　　//绑定数据源
                //this.treeListOriData.ExpandAll();　　　　　 //默认展开所有节点
                this.treeListWorkSpace.OptionsView.ShowCheckBoxes = true;
                //this.treeListWorkSpace.Enabled = false;
                this.treeListWorkSpace.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListWorkSpace.OptionsBehavior.Editable = false;
                //this.treeListWorkSpace.CustomDrawNodeCell += treeListWorkSpace_CustomDrawNodeCell;
                //this.treeListWorkSpace.BeforeCheckNode += treeListWorkSpace_BeforeCheckNode;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }
        private void GetObsDataByUser(string username)
        {
            //1.根据username查询权限，并放入list中
            List<string> userlist = new List<string>();
            // userlist = ;
            //2.遍历list下载数据



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
    }
}
