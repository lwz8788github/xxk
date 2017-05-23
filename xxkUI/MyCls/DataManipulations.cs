/***********************************************************/
//---模    块： 数据操作类
//---功能描述：数据导入、导出、保存到工作空间
//---编码时间：2017-5-12
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.Form;
using xxkUI.Model;
using xxkUI.Tool;

namespace xxkUI.MyCls
{

    public static class DataManipulations
    {

   
        /// <summary>
        /// 保存到工作区
        /// </summary>
        /// <param name="checkedNodes">选中的节点</param>
        public static bool SaveToWorkspace(List<LineBean> checkedNodes)
        {
            bool isScud = false;
            try
            {
                foreach (LineBean checkedLb in checkedNodes)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "'");

                    NpoiCreator npcreator = new NpoiCreator();
                    string savefile = Application.StartupPath + "/数据处理缓存";
                    npcreator.TemplateFile = savefile;
                    npcreator.NpoiExcel(dt, checkedLb.OBSLINECODE + ".xls", savefile + "/" + checkedLb.OBSLINECODE + ".xls");

                    TreeBean tb = new TreeBean();

                    tb.KeyFieldName = checkedLb.OBSLINECODE;
                    tb.ParentFieldName = checkedLb.SITECODE;
                    tb.Caption = checkedLb.OBSLINENAME;
                }
                isScud = true;
            }
            catch (Exception ex)
            {
                isScud = false;
                throw new Exception(ex.Message);
            }

            return isScud;

        }

 
        /// <summary>
        /// 导出测线
        /// </summary>
        public static void ExportObslineToExcel()
        {

        }
    }
}
