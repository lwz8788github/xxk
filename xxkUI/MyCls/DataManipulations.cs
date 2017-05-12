using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.Model;
using xxkUI.Tool;

namespace xxkUI.MyCls
{
    /***********************************************************/
    //---模    块： 数据操作类
    //---功能描述：数据导入、导出、保存到工作空间
    //---编码时间：2017-5-12
    //---编码人员：刘文龙
    //---单    位：一测中心
    /***********************************************************/
    public class DataManipulations
    {
        /// <summary>
        /// 保存到工作区
        /// </summary>
        /// <param name="checkedNodes">选中的节点</param>
        public bool SaveToWorkspace(List<LineBean> checkedNodes)
        {
            bool isScud = false;
            try
            {
                foreach (LineBean checkedLb in checkedNodes)
                {
                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue from t_obsrvtntb where OBSLINECODE = '" + checkedLb.OBSLINECODE + "'");

                    NpoiCreator npcreator = new NpoiCreator();
                    string savefile = Application.StartupPath + "/myworkspace";
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
      ///导入观测数据
      /// </summary>
      /// <param name="files">文件路径</param>
      /// <param name="sitecode">场地编码</param>
        public void ImportObslineFromExcel(List<string> files,string sitecode)
        {
            try
            {
                NpoiCreator npcreator = new NpoiCreator();
                ModelHandler<LineObsBean> mhd = new ModelHandler<LineObsBean>();

                foreach (string file in files)
                {
                    /*1、提取测线信息入库*/
                    LineBean lb = new LineBean();
                    lb.SITECODE = sitecode;
                    lb.OBSLINENAME = System.IO.Path.GetFileNameWithoutExtension(file);
                    lb.OBSLINECODE = LineBll.Instance.GenerateLineCode(sitecode);

                    LineBll.Instance.Add(lb);

                    /*2、提取测线观测信息入库*/
                    List<LineObsBean> lineobslist = mhd.FillModel(npcreator.ExcelToDataTable(file, true).DataSet);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 导出测线
        /// </summary>
        public void ExportObslineToExcel()
        {

        }
    }
}
