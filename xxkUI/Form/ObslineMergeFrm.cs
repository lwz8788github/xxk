/***********************************************************/
//---模    块：测项合并对话框
//---功能描述：主要显示测量列表树
//---编码时间：2017-06-16
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/

using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Model;
using xxkUI.MyCls;

namespace xxkUI.Form
{
    public partial class ObslineMergeFrm : DevExpress.XtraEditors.XtraForm
    {
        public string SelectedObsLineCode = "";//选中的测项编码
        public bool MoveToAverage = false;//是否移动到平均值处
        public ObslineMergeFrm()
        {
            InitializeComponent();
            InitTree();
        }



        /// <summary>
        /// 初始化测项树列表
        /// </summary>
        private void InitTree()
        {
            XTreeList xtrlist = new XTreeList();
            this.treeListData.ClearNodes();
            this.treeListData.DataSource = null;
            //树列表显示
            this.treeListData.KeyFieldName = "KeyFieldName";          //这里绑定的ID的值必须是独一无二的
            this.treeListData.ParentFieldName = "ParentFieldName";  //表示使用parentID进行树形绑定
            this.treeListData.DataSource = xtrlist.ObslineMergeTree();  //绑定数据源
            this.treeListData.OptionsView.ShowCheckBoxes = false;
            this.treeListData.OptionsBehavior.AllowRecursiveNodeChecking = true;
            this.treeListData.OptionsBehavior.Editable = false;

            this.treeListData.ExpandAll();

     
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.treeListData.FocusedNode.Level != 2)
            {
                XtraMessageBox.Show("未选中有效测项！", "提示");
                return;
            }

            try
            {

                TreeBean tb = this.treeListData.GetDataRecordByNode(this.treeListData.FocusedNode) as TreeBean;
                SelectedObsLineCode = (tb.Tag as LineBean).OBSLINECODE;
                this.MoveToAverage = (this.radioGroup.SelectedIndex == 0) ? true : false;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
