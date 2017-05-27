/***********************************************************/
//---模    块：WordHelper
//---功能描述：Word文档操作类
//---编码时间：2012-05-17
//---编码人员：gaogao
//---单    位：LREIS
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using System.Threading;
using xxkUI.Tool;
using Words = Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;

public class WordHelper
{
    /// <summary>
    /// wordapplication
    /// </summary>
    private Words.Application m_WordApp = null;

    public bool IsRightVesrion(int version)
    {
        RegistryKey rk = Registry.LocalMachine;

        string keyName = @"SOFTWARE\Microsoft\Office\" + version + @".0\Word\InstallRoot\";

        object officePath = null;

        if (SystemHelper.Is32System())
        {
            officePath = RegeditHelper.GetValueWithRegView(Microsoft.Win32.RegistryHive.LocalMachine,
               keyName, "Path", Microsoft.Win32.RegistryView.Registry32);
        }
        else if (SystemHelper.Is64System())
        {
            officePath = RegeditHelper.GetValueWithRegView(Microsoft.Win32.RegistryHive.LocalMachine,
            keyName, "Path", Microsoft.Win32.RegistryView.Registry64);
        }

        if (officePath != null)
        {
            if (File.Exists(officePath + "WINWORD.EXE"))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsInstalltoPdf1()
    {
        RegistryHive rh = Microsoft.Win32.RegistryHive.ClassesRoot;
        RegistryView rv = Microsoft.Win32.RegistryView.Registry32;

        string keyName = @"Installer\Products\000021092B0040800000000000F01FEC\";

        object path = null;

        path = RegeditHelper.GetValueWithRegView(rh,
               keyName, "ProductIcon", rv);

        if (path != null)
        {
            if (File.Exists(path.ToString()))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsInstalltoPdf()
    {
        string sourcePath = Application.StartupPath + "\\" + "converttest.doc";
        string destPath = Application.StartupPath + "\\" + "converttest.pdf";

        try
        {

            DOCConvertToPDF(sourcePath, destPath);

            if (File.Exists(destPath))
            {
                File.SetAttributes(destPath, FileAttributes.Normal);
                File.Delete(destPath);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

        return false;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="isVisible">是否显示界面</param>
    public WordHelper(bool isVisible)
    {
        try
        {
            if (m_WordApp == null)
            {
                m_WordApp = new Words.ApplicationClass();
                m_WordApp.Options.CheckSpellingAsYouType = false;
                m_WordApp.Options.CheckGrammarAsYouType = false;
                m_WordApp.Visible = isVisible;
            }
        }
        catch (Exception excep)
        {
            throw new Exception("创建word应用程序出错,可能原因为：" + excep.Message);
        }
    }
    /// <summary>
    /// 打开word文档
    /// </summary>
    /// <param name="sFileName">word文件名</param>
    /// <returns></returns>
    public Words.Document OpenDocument(string sFileName)
    {
        try
        {
            //检查文件是否存在
            if (!File.Exists(sFileName))
            {
                throw new Exception("word文件不存在");
            }
            else
            {
                FileInfo pFInfo = new FileInfo(sFileName);
                pFInfo.IsReadOnly = false;
            }

            object missing = System.Type.Missing;

            Words.Document wordDoc = null;
            object file = sFileName;//报告文件路径
            object confirmConversions = false;
            object readOnly = false;
            object addToRecentFiles = false;

            object visible = false;

            wordDoc = m_WordApp.Documents.Open(ref file, ref confirmConversions, ref readOnly, ref addToRecentFiles,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref visible,
                    ref missing, ref missing, ref missing, ref missing);

            return wordDoc;

        }
        catch (Exception excep)
        {
            throw new Exception("打开word文档失败,可能原因为:" + excep.Message);
        }

    }
    /// <summary>
    /// 关闭文档
    /// </summary>
    /// <param name="wordDoc">文档</param>
    /// <param name="saveChanges">是否保存修改</param>
    public void CloseDocument(Words.Document wordDoc, bool isSaveChanges)
    {
        try
        {
            object missing = System.Type.Missing;
            if (wordDoc != null && wordDoc.Application != null)
            {

                object saveChanges = isSaveChanges;
                wordDoc.Close(ref saveChanges, ref missing, ref missing);
                wordDoc = null;
            }
        }
        catch (Exception excep)
        {
            throw new Exception("关闭word文档失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 关闭word应用程序
    /// </summary>
    /// <param name="isSaveChanges">是否保存修改</param>
    public void CloseWordApplication(bool isSaveChanges)
    {
        if (m_WordApp == null)
        {
            return;
        }
        try
        {
            object saveChanges = isSaveChanges;
            object missing = System.Type.Missing;
            if (m_WordApp != null)
            {
                if (m_WordApp.Documents.Count > 0)
                {
                    m_WordApp.Documents.Close(ref saveChanges, ref missing, ref missing);
                }

                ((Words._Application)m_WordApp).Quit(ref missing, ref missing, ref missing);
                m_WordApp = null;
            }
        }
        catch (Exception excep)
        {
            throw new Exception("关闭word应用程序失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 关闭word应用程序
    /// </summary>
    public void CloseWordApplication()
    {
        try
        {
            object missing = System.Type.Missing;
            if (m_WordApp != null)
            {
                ((Words._Application)m_WordApp).Quit(ref missing, ref missing, ref missing);
                m_WordApp = null;
            }
        }
        catch (Exception excep)
        {
            throw new Exception("关闭word应用程序失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    /// <summary>
    /// 保存word文档
    /// </summary>
    /// <param name="wordDoc">文档</param>
    public void SaveDocument(Words.Document wordDoc)
    {
        try
        {
            if (wordDoc != null && wordDoc.Application != null)
            {
                wordDoc.Save();//保存修改                    
            }
        }
        catch (Exception excep)
        {
            throw new Exception("保存word文档失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// word文档另存为
    /// </summary>
    /// <param name="wordDoc">文档</param>
    /// <param name="newFileName">新的文件名</param>
    public void SaveDocumentAs(Words.Document wordDoc, string newFileName, bool is2000format)
    {
        try
        {
            object missing = System.Type.Missing;
            if (wordDoc != null && wordDoc.Application != null)
            {
                object fileName = newFileName;
                if (!is2000format)
                {
                    wordDoc.SaveAs(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                }
                else
                {
                    wordDoc.SaveAs2000(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing);
                }
            }
        }
        catch (Exception excep)
        {
            throw new Exception("保存word文档失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 用指定的文本填充指定位置的标签
    /// </summary>
    /// <param name="wordDoc"></param>
    /// <param name="bookMarkName"></param>
    /// <param name="insertText"></param>
    public void FillBookMarkText(ref Words.Document wordDoc, string bookMarkName, string insertText)
    {
        try
        {
            if (wordDoc == null || m_WordApp == null)
            {
                throw new Exception("word文档不存在或没打开");
            }
            if (bookMarkName == null || bookMarkName.Length < 1)
            {
                throw new Exception("标签名未给定");
            }

            if (insertText == null /*|| insertText.Length < 1*/)//ck edit 允许填空字符串。
            {
                return;
            }


            string[] TrimChars = { "\r", "\b", "\f" };
            foreach (string trimc in TrimChars)
            {
                insertText = insertText.Replace(trimc, "");
            }
            //change subject       
            string EndBookMarkName = "end_" + bookMarkName;
            Words.Range range = GetRange(wordDoc, bookMarkName, EndBookMarkName);
            if (range != null)
            {
                int start = range.Start;
                object end = range.End;
                range.Text = insertText;

                //总是把Bokkmark SEnd覆盖了，加上
                end = start + insertText.Length;
                //object end1 = end- 1;
                object SEndBookmark = wordDoc.Range(ref end, ref end);
                range.Bookmarks.Add(EndBookMarkName, ref SEndBookmark);
            }
            else
            {
                throw new Exception("在word文档中未找到相应的标签!");
            }
        }
        catch (Exception excep)
        {
            throw new Exception("在word文档中插入标签文本失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 用指定的文本填充指定位置的标签,自动退格
    /// </summary>
    /// <param name="wordDoc"></param>
    /// <param name="bookMarkName"></param>
    /// <param name="insertText"></param>
    public void FillBookMarkText2(ref Words.Document wordDoc, string bookMarkName, string insertText)
    {
        try
        {
            if (wordDoc == null || m_WordApp == null)
            {
                throw new Exception("word文档不存在或没打开");
            }
            if (bookMarkName == null || bookMarkName.Length < 1)
            {
                throw new Exception("标签名未给定");
            }
            if (insertText == null  /*|| insertText.Length < 1*/)//ck edit 允许填空字符串。
            {
                return;
            }
            //change subject       
            string[] TrimChars = { "\r", "\b", "\f", "\t", "\v" };
            foreach (string trimc in TrimChars)
            {
                insertText = insertText.Replace(trimc, "");
            }
            char[] chars = insertText.ToCharArray();

            //清除文本格式
            // wordDoc.FormattingShowClear = true;

            //change subject       
            string EndBookMarkName = "end_" + bookMarkName;
            Words.Range range = GetRange(wordDoc, bookMarkName, EndBookMarkName);
          
            string sTotalString = "";
            if (range != null)
            {
                int start = range.Start;
                object end = range.End;
                #region 分段
                string[] sections = insertText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string section;
                for (int i = 0; i < sections.Length; i++)
                {
                    section = sections[i].Trim();

                    if (i > 0)
                    {
                        section = "\n" + section;
                    }
                    sTotalString += section;
                }
                range.Text = sTotalString;
                #endregion

                if (!wordDoc.Bookmarks.Exists(bookMarkName))
                {
                    object oStart = (object)start;
                    object SBookmark = wordDoc.Range(ref oStart, ref oStart);
                    range.Bookmarks.Add(bookMarkName, ref SBookmark);
                }

                //总是把Bokkmark SEnd覆盖了，加上               
                end = start + sTotalString.Length;
                object SEndBookmark = wordDoc.Range(ref end, ref end);
                range.Bookmarks.Add(EndBookMarkName, ref SEndBookmark);

                range.Select();
                if (m_WordApp.Selection.ParagraphFormat.OutlineLevel != Microsoft.Office.Interop.Word.WdOutlineLevel.wdOutlineLevelBodyText)
                {
                    m_WordApp.Selection.ParagraphFormat.OutlineLevel = Microsoft.Office.Interop.Word.WdOutlineLevel.wdOutlineLevelBodyText;
                }

            }
            else
            {
                throw new Exception("在word文档中未找到相应的标签!");
            }
        }
        catch (Exception excep)
        {
            throw new Exception("在word文档中插入标签文本失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 用指定的文本填充指定位置的标签
    /// </summary>
    /// <param name="wordDoc"></param>
    /// <param name="bookMarkName"></param>
    /// <param name="insertText"></param>
    public void FillBookMarkText1(ref Words.Document wordDoc, string bookMarkName, string EndBookMarkName,
        string insertText)
    {
        try
        {
            if (wordDoc == null || m_WordApp == null)
            {
                throw new Exception("word文档不存在或没打开");
            }
            if (bookMarkName == null || bookMarkName.Length < 1)
            {
                throw new Exception("标签名未给定");
            }
            if (insertText == null  /*|| insertText.Length < 1*/)//ck edit 允许填空字符串。
            {
                return;
            }
            //change subject       
            Words.Range range = GetRange(wordDoc, bookMarkName, EndBookMarkName);

            string sTotalString = "";
            if (range != null)
            {
                int start = range.Start;
                object end = range.End;


                string[] sections = insertText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string section;

                for (int i = 0; i < sections.Length; i++)
                {
                    section = sections[i].Trim();

                    if (i > 0)
                    {
                        section = "\n" + section;
                    }
                    sTotalString += section;

                }
                range.Text = sTotalString;
                if (!wordDoc.Bookmarks.Exists(bookMarkName))
                {
                    object oStart = (object)start;
                    object SBookmark = wordDoc.Range(ref oStart, ref oStart);
                    range.Bookmarks.Add(bookMarkName, ref SBookmark);
                }

                //总是把Bokkmark SEnd覆盖了，加上
                end = start + insertText.Length;
                object SEndBookmark = wordDoc.Range(ref end, ref end);
                range.Bookmarks.Add(EndBookMarkName, ref SEndBookmark);
            }
            else
            {
                throw new Exception("在word文档中未找到相应的标签!");
                CloseWordApplication(false);
            }
        }
        catch (Exception excep)
        {
            throw new Exception("在word文档中插入标签文本失败,可能原因为:" + excep.Message);
            CloseWordApplication(false);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    /// <summary>
    /// 找到该标签对应的结束标签
    /// </summary>
    /// <param name="pS"></param>
    /// <param name="pDoc">Word的Doc</param>
    /// <returns></returns>
    private Words.Bookmark GetEndBookMark(Words.Document currentDoc, string markName)
    {
        Words.Bookmark pEndBookMark = null;
        foreach (Words.Bookmark pbookmark in currentDoc.Bookmarks)
        {
            if (pbookmark.Name.ToLower() == ("end_" + markName.ToLower()))
            {
                pEndBookMark = pbookmark;
            }
        }
        return pEndBookMark;
    }

    /// <summary>
    /// 根据标签获取一段Range
    /// </summary>
    /// <param name="StartBookMark"></param>
    /// <param name="EndBookMark"></param>
    /// <returns></returns>
    private static Words.Range GetRange(Words.Document CurrentDocument, string StartBookMark, string EndBookMark)
    {
        try
        {

            object oStart = StartBookMark;//word中的书签名   
            Words.Range StartRange = CurrentDocument.Bookmarks.get_Item(ref oStart).Range;//表格插入位置  
            object StartPos = StartRange.Start;
            object oEnd = EndBookMark;
            Words.Range EndRange = CurrentDocument.Bookmarks.get_Item(ref oEnd).Range;//表格插入位置  
            object EndPos = EndRange.Start;
            return CurrentDocument.Range(ref StartPos, ref EndPos);
            //object WhatItem = Words.WdGoToItem.wdGoToBookmark;
            //object Missing = System.Type.Missing;
            //object BookmarkName = StartBookMark;
            //Words.Range StartRange = CurrentDocument.GoTo(ref WhatItem, ref Missing, ref Missing, ref BookmarkName);
            //object StartPos = StartRange.Start;
            //BookmarkName = EndBookMark;
            //Words.Range EndRange = CurrentDocument.GoTo(ref WhatItem, ref Missing, ref Missing, ref BookmarkName);
            //object EndPos = EndRange.Start;
            //return CurrentDocument.Range(ref StartPos, ref EndPos);
        }
        catch
        {
            return null;
        }
    }

    public string GetBookMarkText(ref Words.Document wordDoc, string bookMarkName, string EndBookMarkName)
    {
        try
        {
            if (wordDoc == null || m_WordApp == null)
            {
                throw new Exception("word文档不存在或没打开");
            }
            if (bookMarkName == null || bookMarkName.Length < 1)
            {
                throw new Exception("标签名未给定");
            }

            //change subject       
            Words.Range range = GetRange(wordDoc, bookMarkName, EndBookMarkName);

            string sTotalString = "";
            if (range != null)
            {
                return range.Text;
            }
            else
            {
                throw new Exception("在word文档中未找到相应的标签!");
                CloseWordApplication(false);
            }
        }
        catch (Exception excep)
        {
            throw new Exception("在word文档中插入标签文本失败,可能原因为:" + excep.Message);
            CloseWordApplication(false);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    /// <summary>
    /// 获取指定标签处的文本，该标签必须有以end_开头的结束符
    /// </summary>
    /// <param name="wordDoc"></param>
    /// <param name="bookMarkName"></param>
    /// <returns></returns>
    public string GetBookMarkText(Words.Document wordDoc, string bookMarkName)
    {
        try
        {
            if (wordDoc == null || m_WordApp == null)
            {
                throw new Exception("word文档不存在或没打开");
            }
            if (bookMarkName == null || bookMarkName.Length < 1)
            {
                throw new Exception("标签名未给定");
            }
            //遍历所有的BookMark，获取相应的值

            Words.Bookmark pStartBookMark = null;
            foreach (Words.Bookmark bookmark in wordDoc.Bookmarks)
            {
                pStartBookMark = bookmark;
                if (pStartBookMark.Name.ToLower() == bookMarkName.ToLower())
                {
                    if (pStartBookMark.StoryType.ToString() == "wdMainTextStory")
                    {
                        Words.Bookmark pEndBookMark = null;
                        foreach (Words.Bookmark pbookmark in wordDoc.Bookmarks)
                        {
                            if (pbookmark.Name == ("end_" + bookmark.Name))
                            {
                                pEndBookMark = pbookmark;
                                break;
                            }
                        }

                        if (pEndBookMark == null)
                        {
                            CloseWordApplication(false);
                            throw new Exception("在word文档中获取相应结束标签失败!");
                        }

                        object start = bookmark.Range.Start as object;
                        object end = pEndBookMark.Range.End as object;
                        Words.Range tRange = wordDoc.Range(ref start, ref end);
                        return tRange.Text;
                    }
                }
            }

            if (pStartBookMark == null)
            {
                CloseWordApplication(false);
                throw new Exception("在word文档中获取标签文本失败!");
            }

            return null;
        }
        catch (Exception excep)
        {
            CloseWordApplication(false);
            throw new Exception("在word文档中获取标签文本失败,可能原因为:" + excep.Message);
        }
        finally//释放资源
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    /// <summary>
    /// 显示word窗体
    /// </summary>
    public void ShowWordWindow()
    {
        m_WordApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMaximize;
        m_WordApp.Visible = true;
        m_WordApp.ShowMe();
    }

    /// <summary> 
    /// 从源DOC文档复制内容返回一个Document类 
    /// </summary> 
    /// <param name="sorceDocPath">源DOC文档路径</param> 
    /// <returns>Document</returns> 
    public Words.Document CopyWordDocContent(string sourceDocPath)
    {
        object objDocType = Words.WdDocumentType.wdTypeDocument;
        object type = Words.WdBreakType.wdSectionBreakContinuous;

        //Word文档变量 
        Words.Document newWordDoc;

        object readOnly = false;
        object isVisible = false;

        Object Nothing = System.Reflection.Missing.Value;

        newWordDoc = m_WordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        Words.Document openWord;
        openWord = OpenDocument(sourceDocPath);
        openWord.Select();
        openWord.Sections[1].Range.Copy();

        object start = 0;
        Words.Range newRang = newWordDoc.Range(ref start, ref start);

        //插入换行符    
        //newWordDoc.Sections[1].Range.InsertBreak(ref type); 
        newWordDoc.Sections[1].Range.PasteAndFormat(Words.WdRecoveryType.wdPasteDefault);
        openWord.Close(ref Nothing, ref Nothing, ref Nothing);
        return newWordDoc;
    }

    public void InsertWordContentToWord(Words.Document sourceDoc, Words.Document targetDoc)
    {
        sourceDoc.ActiveWindow.Selection.WholeStory();

        sourceDoc.ActiveWindow.Selection.Copy();

        Words.Selection sel = this.GotoLastLine(targetDoc);

        sel.Paste();
    }

    public void InserTextInWordEnd(Words.Document targetDoc)
    {
        Words.Selection selection = GotoLastLine(targetDoc);

        selection.EndKey();
        selection.TypeParagraph();
        selection.InsertAfter("aaaa");
        //this.m_WordApp.ActiveWindow.Selection.EndKey();
        //this.m_WordApp.ActiveWindow.Selection.TypeParagraph();

        //this.m_WordApp.ActiveWindow.Selection.InsertAfter("aaaa");

    }

    public void GetCursor(Words.Document doc)
    {
        Words.Selection selection = doc.Application.Selection;

        object a = selection.get_Information(Words.WdInformation.wdFirstCharacterLineNumber);

        object b = selection.get_Information(Words.WdInformation.wdFirstCharacterColumnNumber);

        object c = selection.get_Information(Words.WdInformation.wdActiveEndAdjustedPageNumber);

        MessageBox.Show(a.ToString() + "行," + b.ToString() + "列," + c.ToString() + "页");

    }

    /// <summary> 
    /// 替换指定Document的内容，并保存到指定的路径 
    /// </summary> 
    /// <param name="docObject">Document</param> 
    /// <param name="savePath">保存到指定的路径</param> 
    protected void ReplaceWordDocAndSave(Words.Document docObject, object savePath)
    {
        object format = Words.WdSaveFormat.wdFormatDocument;
        object readOnly = false;
        object isVisible = false;

        string strOldText = "{WORD}";
        string strNewText = "{替换后的文本}";

        List<string> IListOldStr = new List<string>();
        IListOldStr.Add("{WORD1}");
        IListOldStr.Add("{WORD2}");

        Object Nothing = System.Reflection.Missing.Value;

        Microsoft.Office.Interop.Word.Document oDoc = docObject;

        object FindText, ReplaceWith, Replace;
        object MissingValue = Type.Missing;

        foreach (string str in IListOldStr)
        {
            oDoc.Content.Find.Text = str;
            //要查找的文本 
            FindText = str;
            //替换文本 
            ReplaceWith = strNewText;

            //wdReplaceAll - 替换找到的所有项。 
            //wdReplaceNone - 不替换找到的任何项。 
            //wdReplaceOne - 替换找到的第一项。 
            Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

            //移除Find的搜索文本和段落格式设置 
            oDoc.Content.Find.ClearFormatting();

            if (oDoc.Content.Find.Execute(ref FindText, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref ReplaceWith, ref Replace, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue))
            {
                //Response.Write("替换成功！");
                //Response.Write("<br>");
            }
            else
            {
                //Response.Write("没有相关要替换的：（" + str + "）字符");
                //Response.Write("<br>");
            }
        }

        oDoc.SaveAs(ref savePath, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        //关闭wordDoc文档对象     
        oDoc.Close(ref Nothing, ref Nothing, ref Nothing);
        //关闭wordApp组件对象     
        //wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
    }

    public void MoveDocEnd(Words.Document doc)
    {
        GotoLastCharacter(doc);
        this.m_WordApp.Selection.EndKey();
        this.m_WordApp.Selection.TypeParagraph();

    }

    //定位到文档最后一行
    public Words.Selection GotoLastLine(Words.Document doc)
    {
        Words.Selection selection = doc.ActiveWindow.Selection;

        object dummy = System.Reflection.Missing.Value;

        object what = Words.WdGoToItem.wdGoToLine;

        object which = Words.WdGoToDirection.wdGoToLast;

        object count = 99999999;

        selection.GoTo(ref what, ref which, ref count, ref dummy);



        return selection;
        //Enter(doc);
        //GetCursor(doc);
    }

    //定位到最后一个字符
    public void GotoLastCharacter(Words.Document doc)
    {
        Words.Selection selection = doc.Application.ActiveWindow.Selection;

        GotoLastLine(doc);

        object dummy = System.Reflection.Missing.Value;

        object count = 99999999;
        object Unit = Words.WdUnits.wdCharacter;

        selection.MoveRight(ref Unit, ref count, ref dummy);




    }

    //回车换行
    public void Enter(Words.Document doc)
    {
        doc.Application.Selection.TypeParagraph();
    }

    public Words.Table GetTable(Words.Document doc, int tableIndex)
    {
        return doc.Tables[tableIndex];

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="tableIndex"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    public string ReadWordTable(Words.Document doc, int tableIndex, int rowIndex, int colIndex)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark
            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    //允许用户填-、－、—代表空。或浮点数（规定小位位数），不限定小数位数。
    public string ReadWordTableNum(Words.Document doc, int tableIndex, int rowIndex, int colIndex)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark
            text = text.Trim();
            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();
            text = text.Replace("。", ".");//半角的句号，改为英文下的点
            text = text.Replace("．", ".");//全角的点，改为英文下的点

            if (text == "")
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列未填入值!");
            }

            if (text == "-" || text == "—" || text == "－")
            {
                return "null";
            }
            else if (!NumberHelper.isDouble(text))
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列不是有效的数值!");
            }

            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    //不允许用户填-－或—代表空。
    public string ReadWordTableNum1(Words.Document doc, int tableIndex, int rowIndex, int colIndex, int pointNum)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark
            text = text.Trim();
            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();
            text = text.Replace("。", ".");//半角的句号，改为英文下的点
            text = text.Replace("．", ".");//全角的点，改为英文下的点

            if (text == "")
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列未填入值!");
            }

            if (!NumberHelper.isDouble(text, pointNum))
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列不是有效的数值或小数位数不为" + pointNum + "!");
            }

            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    //允许用户填-－或—(代表空,返回null,以写数据库)或浮点数（规定小位位数）,pointNum为-1表示不限定小数位数
    public string ReadWordTableNum2(Words.Document doc, int tableIndex, int rowIndex, int colIndex, int pointNum)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark
            text = text.Trim();
            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();
            text = text.Replace("。", ".");//半角的句号，改为英文下的点
            text = text.Replace("．", ".");//全角的点，改为英文下的点

            if (text == "")
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列未填入值!");
            }

            if (text == "-" || text == "—" || text == "－")
            {
                return "null";
            }
            else
            {
                if (!NumberHelper.isDouble(text, pointNum))
                {
                    throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列不是有效的数值或小数位数不为" + pointNum + "!");
                }
            }

            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    public string ReadWordTableText(Words.Document doc, int tableIndex, int rowIndex, int colIndex)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark
            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();


            if (text == "")
            {
                throw new Exception("表格中第" + rowIndex.ToString() + "行，第" + colIndex + "列未填入值!");
            }
            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="tableIndex"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    public string ReadTableText(Words.Table table, int rowIndex, int colIndex)
    {
        try
        {
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark

            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();
            return text;
        }
        catch (Exception ex)
        {
            throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="tableIndex"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    public string ReadTableText1(Words.Table table, int rowIndex, int colIndex, ref bool bError, ref string sMessage)
    {
        try
        {
            string text = table.Cell(rowIndex, colIndex).Range.Text.ToString();
            text = text.Substring(0, text.Length - 2);　　//去除尾部的mark

            text = text.Trim().Trim(new char[] { '\r', '\t' }).Trim();

            if (text == "")
            {
                bError = true;
                sMessage = "该单元格未填入值!";
            }

            return text;
        }
        catch (Exception ex)
        {
            bError = false;
            sMessage = "获取表格内容失败," + ex.Message;
            //throw new Exception("获取表格内容失败！" + ex.Message);
        }

        return "";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="tableIndex"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    public void WriteWordTable(Words.Document doc, int tableIndex, int rowIndex, int colIndex, string text)
    {
        Words.Table table = null;

        try
        {
            table = doc.Tables[tableIndex];
            table.Cell(rowIndex, colIndex).Range.Text = text;
        }
        catch (Exception ex)
        {
            throw new Exception("写表格内容失败！" + ex.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="tableIndex"></param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <returns></returns>
    public void WriteWordTable1(Words.Table table, int rowIndex, int colIndex, string text)
    {
        try
        {

            table.Cell(rowIndex, colIndex).Range.Text = text;
        }
        catch (Exception ex)
        {
            throw new Exception("写表格内容失败！" + ex.Message);
        }
    }
    /// <summary>
    /// 复制某行数据,并添加到最后一行。
    /// </summary>
    /// <param name="table"></param>
    /// <param name="iCopyRow"></param>
    public void AppendRowtoTable(Words.Table table, int iCopyRow)
    {
        //object row = table.Rows[iCopyRow];
        table.Rows.Add();
    }

    public void CopyTextbox(Words.Document sourceDoc, int iIndex)
    {
        Words.StoryRanges sr = sourceDoc.StoryRanges;

        int i = 1;
        foreach (Words.Range r in sr)
        {
            if (r.StoryType == Words.WdStoryType.wdTextFrameStory)//文本框
            {
                if (iIndex == i)
                {
                    r.Select();
                    r.Copy();
                    break;
                }
                i++;
            }
        }
    }
    public void PastetoNewDoc(ref Words.Document doc, string bookMarkName)
    {
        Words.Range range = GetRange(doc, bookMarkName, "end_" + bookMarkName);

        range.Paste();
    }

    public void PastetoDoc(Words.Document doc)
    {
        Words.Selection sel = this.GotoLastLine(doc);
        sel.Range.Paste();
    }

    /// <summary>
    /// 获得指定序号的文本框对象。
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public void CopyTextboxtoDoc(Words.Document sourceDoc, int iIndex, Words.Document targetDoc, string bookMarkName)
    {
        Words.StoryRanges sr = sourceDoc.StoryRanges;

        int i = 1;
        foreach (Words.Range r in sr)
        {
            if (r.StoryType == Words.WdStoryType.wdTextFrameStory)//文本框
            {
                if (iIndex == i)
                {
                    r.Select();
                    //r.Copy();

                    //MessageBox.Show(r.Text);
                    IDataObject data = Clipboard.GetDataObject();

                    //MoveDocEnd(targetDoc);
                    //targetDoc.ActiveWindow.Selection.Paste();
                    sourceDoc.Application.Selection.Copy();


                    Words.Range range = GetRange(targetDoc, bookMarkName, "end_" + bookMarkName);
                    range.Select();

                    range.Paste();
                    break;
                }
                i++;
            }
        }
    }

    /// <summary>
    /// 获得指定序号的文本框对象。
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public Words.Range GetTextbox(Words.Document doc, int iIndex)
    {
        Words.StoryRanges sr = doc.StoryRanges;

        int i = 1;
        foreach (Words.Range r in sr)
        {
            if (r.StoryType == Words.WdStoryType.wdTextFrameStory)//文本框
            {
                if (iIndex == i)
                {
                    return r;
                }
                i++;
            }
        }

        return null;
    }

    /// <summary>
    /// 获得指定序号的文本框对象的文本。
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public string GetTextboxContent(Words.Document doc, int iIndex)
    {
        Words.StoryRanges sr = doc.StoryRanges;

        int i = 1;
        foreach (Words.Range r in sr)
        {
            if (r.StoryType == Words.WdStoryType.wdTextFrameStory)//文本框
            {
                if (iIndex == i)
                {
                    return r.Text;
                }
                i++;
            }
        }

        return "";
    }

    /// <summary>
    /// 获得指定序号的文本框对象的文本。
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public void SetTextboxContent(Words.Document doc, int iIndex, string sText)
    {
        Words.StoryRanges sr = doc.StoryRanges;

        int i = 1;
        foreach (Words.Range r in sr)
        {
            string ss = r.Text;
            if (r.StoryType == Words.WdStoryType.wdTextFrameStory)//文本框
            {
                if (iIndex == i)
                {
                    r.Text = sText;
                    break;
                }
                i++;
            }
        }
    }

    /// <summary>
    /// 获得指定序号的文本框对象的文本。
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="iIndex"></param>
    /// <returns></returns>
    public void SetTextboxContent1(Words.Document doc, string alterMark, string sText)
    {
        for (int i = 1; i <= doc.Shapes.Count; i++)
        {
            object iii = i as object;
            Words.Shape pShape = doc.Shapes[iii];
            if (pShape.AlternativeText == alterMark)
            {
                pShape.TextFrame.TextRange.Text = sText;
                return;
            }
        }

    }

    public void CreateTablesOfContents(Words.Document doc, string locationMark)
    {
        Object oMissing = System.Reflection.Missing.Value;
        Object oTrue = true;
        Object oFalse = false;
        try
        {
            Words.Range myRange = GetRange(doc, locationMark, "end_" + locationMark);
            Object oUpperHeadingLevel = "1";
            Object oLowerHeadingLevel = "4";
            Object oTOCTableID = "TableOfContents";

            doc.TablesOfContents.Add(myRange, ref oTrue, ref oUpperHeadingLevel,
                ref oLowerHeadingLevel, ref oMissing, ref oTOCTableID, ref oTrue,
                ref oTrue, ref oMissing, ref oTrue, ref oTrue, ref oTrue);
        }
        catch (Exception excep)
        {
            throw new Exception("生成目录失败，" + excep.Message);
        }

    }
    /// <summary>
    /// 加图片水印函数，居中显示图片
    /// </summary>
    public void CreateWordFileImageSY(Words.Document WordDoc, string pathImage)
    {
        object Nothing = System.Reflection.Missing.Value;

        m_WordApp.ActiveWindow.Selection.Range.Select();
        m_WordApp.ActiveWindow.ActivePane.View.SeekView = Words.WdSeekView.wdSeekCurrentPageHeader;

        m_WordApp.Selection.HeaderFooter.Shapes.AddPicture(pathImage).Select(ref Nothing);

        //m_WordApp.Selection.ShapeRange.Name = "WordPictureWatermark1";

       
        m_WordApp.Selection.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue;
        m_WordApp.Selection.ShapeRange.Height = 400f;
        m_WordApp.Selection.ShapeRange.Width = 400f;
        m_WordApp.Selection.ShapeRange.Left = (float)Words.WdShapePosition.wdShapeCenter;//居中
        m_WordApp.Selection.ShapeRange.Top = (float)Words.WdShapePosition.wdShapeCenter;//居中
        m_WordApp.Selection.ShapeRange.WrapFormat.AllowOverlap = 0;
        m_WordApp.Selection.ShapeRange.LayoutInCell = 0;
        m_WordApp.Selection.ShapeRange.WrapFormat.Side = Words.WdWrapSideType.wdWrapBoth;
        m_WordApp.Selection.ShapeRange.WrapFormat.Type = Words.WdWrapType.wdWrapNone;// 
        m_WordApp.Selection.ShapeRange.ZOrder(Microsoft.Office.Core.MsoZOrderCmd.msoSendBehindText);//文本底下
        m_WordApp.Selection.ShapeRange.RelativeHorizontalPosition = Words.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionPage;
        m_WordApp.Selection.ShapeRange.RelativeVerticalPosition = Words.WdRelativeVerticalPosition.wdRelativeVerticalPositionPage;
        m_WordApp.ActiveWindow.ActivePane.View.SeekView = Words.WdSeekView.wdSeekMainDocument;
    }

    /// <summary>
    /// 加文字水印函数
    /// </summary>
    public void CreateWordFileWrodSY(Words.Document WordDoc, string words)
    {
        object Nothing = System.Reflection.Missing.Value;

        try
        {
            WordDoc.ActiveWindow.Selection.Range.Select();
            WordDoc.ActiveWindow.ActivePane.View.SeekView = Words.WdSeekView.wdSeekCurrentPageHeader;
            m_WordApp.Selection.HeaderFooter.Shapes.AddTextEffect(Microsoft.Office.Core.MsoPresetTextEffect.msoTextEffect1,
                words, "宋体", (float)20, Microsoft.Office.Core.MsoTriState.msoFalse,
                Microsoft.Office.Core.MsoTriState.msoFalse, 0, 0, ref Nothing).Select(ref Nothing);

            m_WordApp.Selection.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue;
            m_WordApp.Selection.ShapeRange.Height = 100f;
            m_WordApp.Selection.ShapeRange.Width = 75f;
            m_WordApp.Selection.ShapeRange.Left = (float)Words.WdShapePosition.wdShapeCenter;//居中
            m_WordApp.Selection.ShapeRange.Top = (float)Words.WdShapePosition.wdShapeCenter;//居中
            m_WordApp.Selection.ShapeRange.WrapFormat.AllowOverlap = 0;
            //m_WordApp.Selection.ShapeRange.LayoutInCell =0;
            m_WordApp.Selection.ShapeRange.WrapFormat.Side = Words.WdWrapSideType.wdWrapBoth;
            m_WordApp.Selection.ShapeRange.WrapFormat.Type = Words.WdWrapType.wdWrapNone;                   // 
            m_WordApp.Selection.ShapeRange.ZOrder(Microsoft.Office.Core.MsoZOrderCmd.msoSendBehindText);//文本底下
            m_WordApp.Selection.ShapeRange.RelativeHorizontalPosition = Words.WdRelativeHorizontalPosition.wdRelativeHorizontalPositionPage;
            m_WordApp.Selection.ShapeRange.RelativeVerticalPosition = Words.WdRelativeVerticalPosition.wdRelativeVerticalPositionPage;
            WordDoc.ActiveWindow.ActivePane.View.SeekView = Words.WdSeekView.wdSeekMainDocument;
            //WordDoc.SaveAs(ref ofilename1, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);

        }
        catch (Exception ee)
        {
            throw new Exception(ee.Message);
        }


    }

    public void AddWaterMark(Words.Document WordDoc, string text, int fontSize)
    {
        object missing = System.Reflection.Missing.Value;
        //WordDoc.Activate();
        try
        {

            if (WordDoc.ActiveWindow.ActivePane.View.Type == Microsoft.Office.Interop.Word.WdViewType.wdNormalView
                || WordDoc.ActiveWindow.ActivePane.View.Type == Microsoft.Office.Interop.Word.WdViewType.wdOutlineView)
            {
                WordDoc.ActiveWindow.ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;
            }


            for (int i = 1; i <= WordDoc.Sections.Count; i++)
            {
                WordDoc.Sections[i].Range.Select();
                WordDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader; //给文档全部+上水印 

                for (int j = 1; j <= m_WordApp.ActiveWindow.Selection.HeaderFooter.Shapes.Count; j++)
                {
                    if (m_WordApp.ActiveWindow.Selection.HeaderFooter.Shapes[j].Type == MsoShapeType.msoTextEffect)
                    {
                        m_WordApp.ActiveWindow.Selection.HeaderFooter.Shapes[j].Delete();
                    }
                }

                m_WordApp.ActiveWindow.Selection.HeaderFooter.Shapes.AddTextEffect(Microsoft.Office.Core.MsoPresetTextEffect.msoTextEffect1,
                    text, "KaiTi", fontSize, Microsoft.Office.Core.MsoTriState.msoFalse,
                    Microsoft.Office.Core.MsoTriState.msoFalse, 0, 0, ref missing).Select(ref missing); //文档中加入艺术字 

                m_WordApp.ActiveWindow.Selection.ShapeRange.TextEffect.NormalizedHeight = Microsoft.Office.Core.MsoTriState.msoFalse;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Fill.Solid();

                m_WordApp.ActiveWindow.Selection.ShapeRange.Fill.ForeColor.RGB =
                    System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.FromArgb(230, 230, 230));

                m_WordApp.ActiveWindow.Selection.ShapeRange.Rotation = 315;

                m_WordApp.ActiveWindow.Selection.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Height = m_WordApp.CentimetersToPoints(float.Parse("4.13"));

                m_WordApp.ActiveWindow.Selection.ShapeRange.Width = m_WordApp.CentimetersToPoints(float.Parse("16.52"));

                m_WordApp.ActiveWindow.Selection.ShapeRange.WrapFormat.AllowOverlap = -1;

                WordDoc.ActiveWindow.Selection.ShapeRange.WrapFormat.Side =
                    (Microsoft.Office.Interop.Word.WdWrapSideType)Microsoft.Office.Interop.Word.WdWrapType.wdWrapNone;

                m_WordApp.ActiveWindow.Selection.ShapeRange.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapNone;

                m_WordApp.ActiveWindow.Selection.ShapeRange.RelativeHorizontalPosition =
                    (Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition)
                    Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionMargin;

                m_WordApp.ActiveWindow.Selection.ShapeRange.RelativeVerticalPosition =
                    Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionMargin;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Left = (float)Microsoft.Office.Interop.Word.WdShapePosition.wdShapeCenter;

                m_WordApp.ActiveWindow.Selection.ShapeRange.Top = (float)Microsoft.Office.Interop.Word.WdShapePosition.wdShapeCenter;

                WordDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;

            }
        }
        catch (Exception ee)
        {
            throw new Exception(ee.Message);
        }
    }

    public void AddWaterMark1(Words.Document WordDoc, string text, int fontSize)
    {

        object missing = System.Reflection.Missing.Value;
        try
        {
            for (int i = 1; i <= WordDoc.Sections.Count; i++)
            {
                WordDoc.Sections[i].Range.Select();


                WordDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader; //给文档全部+上水印 

                for (int j = 1; j <= m_WordApp.Selection.HeaderFooter.Shapes.Count; j++)
                {
                    if (m_WordApp.Selection.HeaderFooter.Shapes[j].Type == MsoShapeType.msoTextEffect)
                    {
                        m_WordApp.Selection.HeaderFooter.Shapes[j].Delete();
                    }
                }

                m_WordApp.Selection.HeaderFooter.Shapes.AddTextEffect(Microsoft.Office.Core.MsoPresetTextEffect.msoTextEffect1,
                    text, "Arial Black", fontSize, Microsoft.Office.Core.MsoTriState.msoFalse,
                    Microsoft.Office.Core.MsoTriState.msoFalse, 0, 0, ref missing).Select(ref missing); //文档中加入艺术字 

                //m_WordApp.ActiveDocument.Shapes.AddTextEffect(Microsoft.Office.Core.MsoPresetTextEffect.msoTextEffect10, // "请勿带出", "Arial Black", 24, Microsoft.Office.Core.MsoTriState.msoFalse, // Microsoft.Office.Core.MsoTriState.msoFalse, 0, 0, ref missing).Select(ref missing); 

                //m_WordApp.Selection.ShapeRange.Name = "PowerPlusWaterMarkObject1"; 

                m_WordApp.Selection.ShapeRange.TextEffect.NormalizedHeight = Microsoft.Office.Core.MsoTriState.msoFalse;

                m_WordApp.Selection.ShapeRange.Line.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;

                m_WordApp.Selection.ShapeRange.Fill.Visible = Microsoft.Office.Core.MsoTriState.msoTrue;

                m_WordApp.Selection.ShapeRange.Fill.Solid();

                m_WordApp.Selection.ShapeRange.Fill.ForeColor.RGB =
                    System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.FromArgb(230, 230, 230));
                //m_WordApp.Selection.ShapeRange.Fill.ForeColor.RGB = System.Drawing.ColorTranslator.ToWin32(Color.GreenYellow); 

                //m_WordApp.Selection.ShapeRange.Fill.Transparency =float.Parse("0.8"); 

                m_WordApp.Selection.ShapeRange.Rotation = 315;

                m_WordApp.Selection.ShapeRange.LockAspectRatio = Microsoft.Office.Core.MsoTriState.msoTrue;

                m_WordApp.Selection.ShapeRange.Height = m_WordApp.CentimetersToPoints(float.Parse("4.13"));

                m_WordApp.Selection.ShapeRange.Width = m_WordApp.CentimetersToPoints(float.Parse("16.52"));

                m_WordApp.Selection.ShapeRange.WrapFormat.AllowOverlap = -1;

                m_WordApp.Selection.ShapeRange.WrapFormat.Side =
                    (Microsoft.Office.Interop.Word.WdWrapSideType)Microsoft.Office.Interop.Word.WdWrapType.wdWrapNone;

                m_WordApp.Selection.ShapeRange.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapNone;
                m_WordApp.Selection.ShapeRange.RelativeHorizontalPosition =
                    (Microsoft.Office.Interop.Word.WdRelativeHorizontalPosition)
                    Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionMargin;
                m_WordApp.Selection.ShapeRange.RelativeVerticalPosition =
                    Microsoft.Office.Interop.Word.WdRelativeVerticalPosition.wdRelativeVerticalPositionMargin;

                m_WordApp.Selection.ShapeRange.Left = (float)Microsoft.Office.Interop.Word.WdShapePosition.wdShapeCenter;

                m_WordApp.Selection.ShapeRange.Top = (float)Microsoft.Office.Interop.Word.WdShapePosition.wdShapeCenter;

                WordDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;
            }

        }
        catch (Exception ee)
        {
            throw new Exception(ee.Message);
        }
    }

    public void DeleteWatermark(Words.Document WordDoc)
    {
        object missing = System.Reflection.Missing.Value;
        try
        {
            for (int i = 1; i <= WordDoc.Sections.Count; i++)
            {
                WordDoc.Sections[i].Range.Select(); //这个不能少了,少了水印出不来了 

                m_WordApp.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader; //给文档全部+上水印 


                for (int j = 1; j <= m_WordApp.Selection.HeaderFooter.Shapes.Count; j++)
                {
                    if (m_WordApp.Selection.HeaderFooter.Shapes[j].Type == MsoShapeType.msoTextEffect)
                    {
                        m_WordApp.Selection.HeaderFooter.Shapes[j].Delete();
                    }
                }

                WordDoc.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;
            }


        }
        catch (Exception ee)
        {
            throw new Exception(ee.Message);
        }
    }

    /// <summary>
    /// 把Word文件转换成为PDF格式文件，在office2007环境下测试通过。不过是后装微软的office插件。
    /// </summary>
    /// <param name="sourcePath">源文件路径</param>
    /// <param name="targetPath">目标文件路径</param> 
    /// <returns>true=转换成功</returns>
    public void DOCConvertToPDF(string sourcePath, string targetPath)
    {
        bool result = false;
        Microsoft.Office.Interop.Word.WdExportFormat exportFormat = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
        object paramMissing = Type.Missing;

        Microsoft.Office.Interop.Word.Document wordDocument = null;

        try
        {
            object paramSourceDocPath = sourcePath;
            string paramExportFilePath = targetPath;
            Microsoft.Office.Interop.Word.WdExportFormat paramExportFormat = exportFormat;
            bool paramOpenAfterExport = false;
            Microsoft.Office.Interop.Word.WdExportOptimizeFor paramExportOptimizeFor = Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
            Microsoft.Office.Interop.Word.WdExportRange paramExportRange = Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument;
            int paramStartPage = 0;
            int paramEndPage = 0;
            Microsoft.Office.Interop.Word.WdExportItem paramExportItem = Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent;
            bool paramIncludeDocProps = true;
            bool paramKeepIRM = true;
            Microsoft.Office.Interop.Word.WdExportCreateBookmarks paramCreateBookmarks = Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
            bool paramDocStructureTags = true;
            bool paramBitmapMissingFonts = true;
            bool paramUseISO19005_1 = false;

            wordDocument = OpenDocument(sourcePath);

            if (wordDocument != null)
                wordDocument.ExportAsFixedFormat(paramExportFilePath,
                paramExportFormat, paramOpenAfterExport,
                paramExportOptimizeFor, paramExportRange, paramStartPage,
                paramEndPage, paramExportItem, paramIncludeDocProps,
                paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                paramBitmapMissingFonts, paramUseISO19005_1,
                ref paramMissing);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            Thread.Sleep(3000);
            CloseDocument(wordDocument, false);
            //CloseWordApplication();
        }


    }

    public void DOCConvertToPDF(Words.Document wordDocument, string targetPath)
    {
        bool result = false;
        Microsoft.Office.Interop.Word.WdExportFormat exportFormat = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
        object paramMissing = Type.Missing;

        try
        {
            string paramExportFilePath = targetPath;
            Microsoft.Office.Interop.Word.WdExportFormat paramExportFormat = exportFormat;
            bool paramOpenAfterExport = false;
            Microsoft.Office.Interop.Word.WdExportOptimizeFor paramExportOptimizeFor = Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
            Microsoft.Office.Interop.Word.WdExportRange paramExportRange = Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument;
            int paramStartPage = 0;
            int paramEndPage = 0;
            Microsoft.Office.Interop.Word.WdExportItem paramExportItem = Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent;
            bool paramIncludeDocProps = true;
            bool paramKeepIRM = true;
            Microsoft.Office.Interop.Word.WdExportCreateBookmarks paramCreateBookmarks = Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
            bool paramDocStructureTags = true;
            bool paramBitmapMissingFonts = true;
            bool paramUseISO19005_1 = false;

            if (wordDocument != null)
                wordDocument.ExportAsFixedFormat(paramExportFilePath,
                paramExportFormat, paramOpenAfterExport,
                paramExportOptimizeFor, paramExportRange, paramStartPage,
                paramEndPage, paramExportItem, paramIncludeDocProps,
                paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                paramBitmapMissingFonts, paramUseISO19005_1,
                ref paramMissing);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }

    /// <summary>
    /// 在已给定的图片为背景，并在图片上写入文字，生成新的图片。
    /// </summary>
    /// <param name="words"></param>
    /// <param name="backgroundImg"></param>
    /// <param name="path"></param>
    static public void CreateImage_OneBackgroundImg(string words, Image backgroundImg, string path)
    {
        int width = backgroundImg.Width;
        int height = backgroundImg.Height;

        if (words == null || words.Trim() == String.Empty)
        {
            return;
        }

        System.Drawing.Bitmap image = (System.Drawing.Bitmap)backgroundImg;

        Graphics g = Graphics.FromImage(image);

        try
        {
            Font font = new System.Drawing.Font("Arial", 15, (System.Drawing.FontStyle.Bold |
                System.Drawing.FontStyle.Italic));

            Color beginColor = new Color();
            beginColor = Color.FromArgb(213, 213, 213);

            Color endColor = new Color();
            endColor = Color.FromArgb(200, 200, 200);

            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.
                LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                beginColor, endColor, 1.1f, true);

            int beginX, beginY;

            for (int i = 0; i < height; i++)
            {
                if (i % (font.Height + 100) == 0)//100为高的间隔
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j % (words.Length * font.Height + 100) == 0)//100为宽的间隔
                        {
                            beginX = j + 50;
                            beginY = i + 50;

                            Matrix myMatrix = new Matrix();
                            myMatrix.RotateAt(-15f, new PointF(beginX, beginY));
                            g.Transform = myMatrix;

                            g.DrawString(words, font, brush, beginX, beginY);
                        }
                    }
                }
            }


            image.Save(path);

        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }

    }

    /// <summary>
    /// 在指定宽、高的空白图片上均匀的写入图片及文字，并生成新的图片。
    /// </summary>
    /// <param name="words"></param>
    /// <param name="backgroundImg"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="path"></param>
    public void CreateImage_MultiBackgroundImg(string words, Image backgroundImg, int width, int height, string path)
    {
        if (words == null || words.Trim() == String.Empty)
        {
            return;
        }

        System.Drawing.Bitmap image = new System.Drawing.Bitmap(width, height);

        Graphics g = Graphics.FromImage(image);

        try
        {
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);

            Font font = new System.Drawing.Font("Arial", 15, (System.Drawing.FontStyle.Bold |
                System.Drawing.FontStyle.Italic));

            Color beginColor = new Color();
            beginColor = Color.FromArgb(213, 213, 213);

            Color endColor = new Color();
            endColor = Color.FromArgb(200, 200, 200);

            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.
                LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                beginColor, endColor, 1.1f, true);

            int beginX, beginY;

            for (int i = 0; i < height; i++)
            {
                if (i % (font.Height + 100) == 0)//100为高的间隔
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j % (words.Length * font.Height + backgroundImg.Width + 100) == 0)//100为宽的间隔
                        {
                            beginX = j + 50;
                            beginY = i + 50;

                            Matrix myMatrix = new Matrix();
                            myMatrix.RotateAt(-15f, new PointF(beginX, beginY));
                            g.Transform = myMatrix;

                            g.DrawImage(backgroundImg, beginX, beginY);

                            g.DrawString(words, font, brush, beginX + backgroundImg.Width, beginY);
                        }
                    }
                }
            }


            image.Save(path);

        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }

    }

    public bool Find(string sourcePath, string strKey)
    {
        Words.Document WordDoc = null;

        try
        {
            WordDoc = OpenDocument(sourcePath);
            WordDoc.Content.Find.Text = strKey;
            object MissingValue = Type.Missing;

            if (WordDoc.Content.Find.Execute(ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue))
            {
                return true;
            }
            else
            {
                return false;
            }

            CloseDocument(WordDoc, false);
        }
        catch (Exception ex)
        {
            CloseDocument(WordDoc, false);
            return false;
        }
        finally
        {
            CloseWordApplication();
        }

        return false;
    }

    public bool Find1(string sourcePath, string strKey)
    {
        Words.Document WordDoc = null;

        try
        {
            WordDoc = OpenDocument(sourcePath);
            WordDoc.Content.Find.Text = strKey;
            object MissingValue = Type.Missing;

            if (WordDoc.Content.Find.Execute(ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue, ref MissingValue,
            ref MissingValue))
            {
                return true;
            }
            else
            {
                return false;
            }

            CloseDocument(WordDoc, false);
        }
        catch (Exception ex)
        {
            CloseDocument(WordDoc, false);
            return false;
        }
        finally
        {
            CloseWordApplication();
        }

        return false;
    }

    public bool Find(Words.Document WordDoc, string strKey)
    {
        try
        {
            //WordDoc.Content.Find.Text = strKey;

            object key = (object)strKey;
            object MissingValue = Type.Missing;



            Words.Find wfnd = WordDoc.Content.Find;
            wfnd.ClearFormatting();
            wfnd.MatchCase = false;
            wfnd.MatchWholeWord = true;

            if (
                    wfnd.Execute2007(ref key, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue)
                )
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }

        return false;
    }

    public bool Find2(Words.Document WordDoc, string strKey)
    {
        try
        {
            object MissingValue = Type.Missing;
            int i = 0, iCount = 0;
            Words.Find wfnd;
            object key = (object)strKey;
            if (WordDoc.Paragraphs != null && WordDoc.Paragraphs.Count > 0)
            {
                iCount = WordDoc.Paragraphs.Count;
                for (i = 1; i <= iCount; i++)
                {
                    wfnd = WordDoc.Paragraphs[i].Range.Find;
                    wfnd.ClearFormatting();
                    wfnd.MatchCase = false;
                    wfnd.MatchWholeWord = true;

                    if (wfnd.Execute2007(ref key, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue, ref MissingValue,
                    ref MissingValue, ref MissingValue,
                    ref MissingValue))
                    {
                        return true;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            throw ex;
            return false;
        }

        return false;
    }


    public static void CreateFile(string path)
    {
        try
        {
            object Nothing = System.Reflection.Missing.Value;

            object filename = path;  //文件保存路径

            //创建Word文档
            Words.Application WordApp = new Words.ApplicationClass();
            Words.Document WordDoc = WordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            WordDoc.SaveAs(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            WordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
            WordApp.Quit(ref Nothing, ref Nothing, ref Nothing);

        }
        catch (Exception ex)
        {
            throw new Exception("生成word失败，" + ex.Message);
        }

    }


}

