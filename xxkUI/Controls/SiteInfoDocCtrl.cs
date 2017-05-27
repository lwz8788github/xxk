using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraRichEdit.API.Native;
using xxkUI.Bll;
using DevExpress.XtraEditors;

namespace xxkUI.Controls
{
    public partial class SiteInfoDocCtrl : UserControl
    {
        private string FileName = string.Empty;
        private WordHelper WordHelperCls;
  
        public SiteInfoDocCtrl()
        {
            InitializeComponent();
            this.richEditControl.DocumentLoaded += RichEditControl_DocumentLoaded;
            WordHelperCls = new WordHelper(false);
        }

        /// <summary>
        /// WORD文档变化后，实现对新文件名称的显示
        /// </summary>
        private void RichEditControl_DocumentLoaded(object sender, EventArgs e)
        {
            //修改默认字体
            DocumentRange range = richEditControl.Document.Range;
            CharacterProperties cp = this.richEditControl.Document.BeginUpdateCharacters(range);
            cp.FontName = "新宋体";
            //cp.FontSize = 12;
            this.richEditControl.Document.EndUpdateCharacters(cp);

                  }

        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="filename">文档路径</param>
        public void LoadDocument(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileName = filename;
                richEditControl.LoadDocument(filename);
            }
        }

        /// <summary>
        /// 填充标签
        /// </summary>
        /// <param name="sb"></param>
        public void FillBookMarkText(SiteBean sb)
        {

            // Microsoft.Office.Interop.Word.Document Dcmt = WordHelperCls.OpenDocument(Application.StartupPath + "/tempDoc/信息库模板.doc");
            if (this.richEditControl.Document == null)
                return;

            try
            {
                this.richEditControl.Document.InsertText(GetPosition("标题"), sb.SiteName);
                this.richEditControl.Document.InsertText(GetPosition("场地名称"), sb.SiteName);
                this.richEditControl.Document.InsertText(GetPosition("所跨断裂断层"), sb.FaultName);
                this.richEditControl.Document.InsertText(GetPosition("运行状况"), sb.SiteStatus == "0" ? "正常" : (sb.SiteStatus == "1" ? "停测" : "改造中"));
                this.richEditControl.Document.InsertText(GetPosition("历史场地"), sb.Historysite);
                this.richEditControl.Document.InsertText(GetPosition("场地类型"), sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点");
                this.richEditControl.Document.InsertText(GetPosition("观测类型"), sb.Type);
                this.richEditControl.Document.InsertText(GetPosition("所在地"), sb.Place);
                this.richEditControl.Document.InsertText(GetPosition("标石类型"), sb.MarkStoneType);
                this.richEditControl.Document.InsertText(GetPosition("场地坐标"), sb.Locations);
                this.richEditControl.Document.InsertText(GetPosition("海拔高程"), sb.Altitude.ToString() + "m");
                this.richEditControl.Document.InsertText(GetPosition("建设单位"), sb.BuildUnit);
                this.richEditControl.Document.InsertText(GetPosition("监测单位"), sb.ObsUnit);
                this.richEditControl.Document.InsertText(GetPosition("起测时间"), sb.StartDate);
                this.richEditControl.Document.InsertText(GetPosition("资料变更"), sb.Datachg);
                this.richEditControl.Document.InsertText(GetPosition("场地概况"), sb.SiteSituation);
                this.richEditControl.Document.InsertText(GetPosition("地质概况"), sb.GeoSituation);
                this.richEditControl.Document.InsertText(GetPosition("备注"), sb.Note);
                this.richEditControl.Document.InsertText(GetPosition("其他情况"), sb.OtherSituation);
                try
                {
                    DocumentImageSource diswxt = DocumentImageSource.FromFile(SiteBll.Instance.DownloadPic("SITECODE", sb.SiteCode, "REMOTEMAP"));
                    if (diswxt != null)
                        this.richEditControl.Document.InsertImage(GetPosition("卫星图"), diswxt);
                }
                catch{ }
                try
                {
                    DocumentImageSource disbst = DocumentImageSource.FromFile(SiteBll.Instance.DownloadPic("SITECODE", sb.SiteCode, "LAYOUTMAP"));
                    if (disbst != null)
                        this.richEditControl.Document.InsertImage(GetPosition("布设图"), disbst);
                }
                catch { }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        private DocumentPosition GetPosition(string BookMark)
        {
            try
            {
                DocumentPosition dpStart = null;
             
                for (int i = 0; i < this.richEditControl.Document.Bookmarks.Count; i++)
                {
                    if (this.richEditControl.Document.Bookmarks[i].Name == BookMark)
                    {
                        DocumentRange StartRange = this.richEditControl.Document.Bookmarks[i].Range;
                        dpStart = StartRange.Start;
                    }
                   
                }
                return dpStart;

            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据标签获取一段Range
        /// </summary>
        /// <param name="StartBookMark"></param>
        /// <param name="EndBookMark"></param>
        /// <returns></returns>
        private  DocumentRange GetRange(string StartBookMark, string EndBookMark)
        {
            try
            {
                DocumentPosition dpStart = null;
                DocumentPosition dpEnd = null;
                for (int i = 0; i < this.richEditControl.Document.Bookmarks.Count; i++)
                {
                    if (this.richEditControl.Document.Bookmarks[i].Name == StartBookMark)
                    {
                        DocumentRange StartRange = this.richEditControl.Document.Bookmarks[i].Range;
                        dpStart = StartRange.Start;
                    }
                    if (this.richEditControl.Document.Bookmarks[i].Name == EndBookMark)
                    {
                        DocumentRange EndRange = this.richEditControl.Document.Bookmarks[i].Range;
                        dpEnd = EndRange.Start;
                    }
                }
                if (dpEnd == null || dpStart == null)
                    return null;
                else
                    return this.richEditControl.Document.CreateRange(dpStart.ToInt(), dpEnd.ToInt());
        
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// WORD文件打印
        /// </summary>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.richEditControl.ShowPrintPreview();
        }


        private void fileSaveItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.richEditControl.SaveDocument() && FileName != string.Empty)
            {
                string sitecode = Path.GetFileName(FileName).Split('.')[0];
                SiteBll.Instance.UpdateBASEINFO(sitecode, FileName);
            }
            else
            {
                throw new Exception("保存失败");
            }
                
        }

        private void fileSaveAsItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                richEditControl.SaveDocumentAs();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }
        }
    }
}
