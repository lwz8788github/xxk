using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.XSSF.UserModel;
using System.Data;

namespace xxkUI.Tool
{
    public class SheetCell
    {
        private int rowIndex;

        public int RowIndex
        {
            get { return rowIndex; }
            set { rowIndex = value; }
        }
        private int columnIndex;

        public int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }
        private object cellValue;

        public object CellValue
        {
            get { return cellValue; }
            set { cellValue = value; }
        }

        public SheetCell()
        {
        }
        public SheetCell(int rowIndex, int columnIndex, object cellValue)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.cellValue = cellValue;
        }
    }

    public class NpoiCreator
    {
        /// <summary>
        /// 模板文件
        /// </summary>
        private string templateFile;

        public string TemplateFile
        {
            get { return templateFile; }
            set { templateFile = value; }
        }

        /// <summary>
        /// 表名
        /// </summary>
        private string sheetName;

        public string SheetName
        {
            get { return sheetName; }
            set { sheetName = value; }
        }
        /// <summary>
        /// 数据
        /// </summary>
        private List<SheetCell> sheetCells = null;

        public List<SheetCell> SheetCells
        {
            get { return sheetCells; }
            set { sheetCells = value; }
        }

        private HSSFWorkbook hssfWorkbook = null;
        private XSSFWorkbook xssfWorkbook = null;
        private ISheet sheet = null;

        public NpoiCreator()
        {
            this.sheetCells = new List<SheetCell>();
        }


        public void NpoiExcel(System.Data.DataTable dt, string title,string file)
        {
            try
            {
                double sheetCountdbl = double.Parse(dt.Rows.Count.ToString()) / 60000;
                int sheetCount = (int)(Math.Ceiling(sheetCountdbl));


                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

                for (int c = 0; c < sheetCount; c++)
                {
                    string sheetname = "Sheet" + (c + 1).ToString();

                    NPOI.SS.UserModel.ISheet sheet = book.CreateSheet(sheetname);

                    NPOI.SS.UserModel.IRow headerrow = sheet.CreateRow(0);
                    ICellStyle style = book.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Center;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = headerrow.CreateCell(i);
                        cell.CellStyle = style;
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }

                    int js = 0;//计数
                    //表内容
                    for (int i = c * 60000; i <dt.Rows.Count; i++)
                    {
                        if (js > 59999)
                            break;
                        IRow row = sheet.CreateRow(js + 1);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            cell.CellStyle = style;
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }

                        js++;

                    }


                }
               

                FileStream fs = File.Create(file);
                book.Write(fs);
                fs.Flush();
                fs.Close();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="filePath">excel路径</param>  
        /// <param name="isColumnName">第一行是否是列名</param>  
        /// <returns>返回datatable</returns>  
        public DataTable ExcelToDataTable_LineObs(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;

            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                if (OpenWorkbook(filePath))
                {
                    dataTable = new DataTable();
                    if (this.sheet != null)
                    {
                        int rowCount = this.sheet.LastRowNum;//总行数  
                        if (rowCount > 0)
                        {
                            IRow firstRow = this.sheet.GetRow(0);//第一行  
                            int cellCount = firstRow.LastCellNum;//列数  

                            //构建datatable的列  
                            if (isColumnName)
                            {
                                startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                {
                                    cell = firstRow.GetCell(i);
                                    if (cell != null)
                                    {
                                        if (cell.StringCellValue != null)
                                        {
                                            column = new DataColumn(cell.StringCellValue);
                                            dataTable.Columns.Add(column);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                {
                                    column = new DataColumn("column" + (i + 1));
                                    dataTable.Columns.Add(column);
                                }
                            }

                            //填充行  
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                row = this.sheet.GetRow(i);
                                if (row == null) continue;

                                dataRow = dataTable.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; ++j)
                                {
                                    cell = row.GetCell(j);
                                    if (cell == null)
                                    {
                                        dataRow[j] = "";
                                    }
                                    else
                                    {
                                        if (j == 0)
                                        {
                                            dataRow[j] = DateTime.Parse(cell.DateCellValue.ToString());
                                        }
                                        else if (j == 1)
                                            dataRow[j] = cell.ToString();
                                        //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                        //switch (cell.CellType)
                                        //{
                                        //    case CellType.Blank:
                                        //        dataRow[j] = "";
                                        //        break;
                                        //    case CellType.Numeric:
                                        //        short format = cell.CellStyle.DataFormat;
                                        //        //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                        //        if (format == 14 || format == 31 || format == 57 || format == 58)
                                        //            dataRow[j] = DateTime.Parse(cell.DateCellValue.ToShortDateString());
                                        //        else
                                        //            dataRow[j] = cell.NumericCellValue;
                                        //        break;
                                        //    case CellType.String:
                                        //        dataRow[j] = cell.StringCellValue;
                                        //        break;
                                        //}
                                    }
                                }
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }

                }
                return dataTable;
            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }



        //public bool Execute(string savefile)
        //{
        //    if (!OpenWorkbook(templateFile))
        //        return false;
        //    if (!GetSheetCells())
        //        return false;
        //    SaveSheetValues();
        //    return SaveWorkbook(savefile);
        //}

        private bool OpenWorkbook(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                return false;
          
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    // 2007版本  
                    if (fileName.IndexOf(".xlsx") > 0)
                    { 
                        this.xssfWorkbook = new XSSFWorkbook(fs);
                        this.sheet = this.xssfWorkbook.GetSheetAt(0);
                    }
                    // 2003版本  
                    else if (fileName.IndexOf(".xls") > 0)
                    { 
                        this.hssfWorkbook = new HSSFWorkbook(fs);
                        this.sheet = this.hssfWorkbook.GetSheetAt(0);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool SaveWorkbook(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
                this.hssfWorkbook.Write(fs);
                fs.Seek(0, System.IO.SeekOrigin.Begin);

                fs.Close();
                fs.Dispose();


            }
            catch (System.Exception ex)
            {
                return false;
            }

            
           


            //FileResult result = new FileStreamResult(fs, "application/vnd.ms-excel");
            //result.FileDownloadName = fileName + " " + DateTime.Now.ToShortDateString();
            return true;
        }

        private void SaveSheetValues()
        {
            for (int i = 0; i < sheetCells.Count; i++)
            {
                SheetCell sheetCell = sheetCells[i];
                IRow row = sheet.GetRow(sheetCell.RowIndex);
                if (row == null)
                {
                    row = sheet.CreateRow(sheetCell.RowIndex);
                }
                ICell cell = row.GetCell(sheetCell.ColumnIndex);
                if (cell == null)
                {
                    cell = row.CreateCell(sheetCell.ColumnIndex);
                }    
                SetCellValue(cell, sheetCell.CellValue);
            }
        }
        private void SetCellValue(ICell cell, object value)
        {
            if (cell == null)
                return;
            if (value == null)
                return;

            #region 不可空类型

            if (value is Guid)
            {
                cell.SetCellValue(value.ToString());
                return;
            }
            if (value is bool)
            {
                bool bValue = (bool)value;
                if (bValue)
                {
                    cell.SetCellValue("是");
                }
                else
                {
                    cell.SetCellValue("否");
                }
                return;
            }
            if (value is string)
            {
                cell.SetCellValue((string)value);
                return;
            }
            if (value is DateTime)
            {
                cell.SetCellValue((DateTime)value);
                return;
            }
            if (value is Int32)
            {
                cell.SetCellValue((Int32)value);
                return;
            }
            if (value is Int64)
            {
                cell.SetCellValue((Int64)value);
                return;
            }
            if (value is double)
            {
                cell.SetCellValue((double)value);
                return;
            }
            if (value is float)
            {
                cell.SetCellValue((float)value);
                return;
            }
            if (value is decimal)
            {
                decimal d = (decimal)value;
                cell.SetCellValue((double)d);
                return;
            }
            #endregion

            #region 可空类型

            if (value is Guid?)
            {
                Guid? nullableValue = (Guid?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is bool?)
            {
                bool? nullableValue = (bool?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is DateTime?)
            {
                DateTime? nullableValue = (DateTime?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is Int32?)
            {
                Int32? nullableValue = (Int32?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is Int64?)
            {
                Int64? nullableValue = (Int64?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is double?)
            {
                double? nullableValue = (double?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is float?)
            {
                float? nullableValue = (float?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            if (value is decimal?)
            {
                decimal? nullableValue = (decimal?)value;
                if (nullableValue.HasValue)
                    SetCellValue(cell, nullableValue.Value);
                return;
            }
            #endregion
        }



        
    }
}