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
using DevExpress.DXCore.Controls.XtraEditors;
using xxkUI.Bll;

namespace xxkUI.Controls
{
    public partial class SiteInfoDocCtrl : UserControl
    {
        private string FileName = string.Empty;

        public SiteInfoDocCtrl()
        {
            InitializeComponent();
            this.richEditControl.DocumentLoaded += RichEditControl_DocumentLoaded;
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
