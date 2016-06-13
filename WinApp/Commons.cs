using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Drawing;
using NPOI.XSSF.UserModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace TopFashion
{
    public static class Commons
    {
        public static Attachment UploadAttachment(User user, string localFilename)
        {
            try
            {
                string dir = null;
                if (user != null)
                    dir = user.ID.ToString();
                KellFileTransfer.FILELIST file = KellFileTransfer.Common.GetFILE(localFilename, dir);
                if (KellFileTransfer.FileUploader.SendFile(localFilename, KellFileTransfer.Common.GetUploadIPEndPoint(), user.ID.ToString()))
                {
                    Attachment attch = new Attachment();
                    attch.AttachmentFilename = file.文件路径;
                    attch.Size = file.文件大小;
                    attch.Uploader = user;
                    int r = AttachmentLogic.GetInstance().AddAttachment(attch);
                    string s = "";
                    if (r > 0)
                    {
                        attch.ID = r;
                        return attch;
                    }
                    else
                    {
                        s = "但保存到数据库失败！";
                    }
                    MessageBox.Show("上传成功！" + s);
                }
                else
                {
                    MessageBox.Show("上传失败！");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("上传时出错：" + e.Message);
            }
            return null;
        }

        public static void DownloadAttachment(bool open, User user, int attachId, string attachmentFileName, long size, IPEndPoint ipep = null)
        {
            if (string.IsNullOrEmpty(attachmentFileName))
                return;
            //以下为禁止非原上传用户下载附件的代码，由于现在运行所有用户下载查看附件，所以屏蔽！
            //int index = attachmentFileName.IndexOf("\\");// 322\123.abc
            //if (index > 0 && user != null)
            //{
            //    string userId = attachmentFileName.Substring(0, index);
            //    int R;
            //    if (int.TryParse(userId, out R))
            //    {
            //        if (user.ID != R)
            //        {
            //            return;//非法用户
            //        }
            //    }
            //}
            try
            {
                KellFileTransfer.FileDownloadClient download = new KellFileTransfer.FileDownloadClient();
                if (open)
                    download.DownloadFinishedSingle += new KellFileTransfer.DownloadSingleHandler(download_DownloadFinishedSingle);
                KellFileTransfer.FILELIST attachment = new KellFileTransfer.FILELIST();
                attachment.文件路径 = attachmentFileName;
                attachment.文件大小 = size;
                if (ipep == null)
                    ipep = KellFileTransfer.Common.GetDownloadIPEndPoint();
                if (download.DownloadFileFromServer(attachment, ipep.Address, ipep.Port))
                    AttachmentLogic.GetInstance().AfterDownload(attachId);
                else
                    MessageBox.Show("下载附件失败！很有可能连接到附件服务器的网络已断开。");
            }
            catch (Exception e)
            {
                MessageBox.Show("下载附件时出错：" + e.Message);
            }
        }

        static void download_DownloadFinishedSingle(KellFileTransfer.FileDownloadClient sender, string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Finish), fileFullPath);
            }
            else
            {
                MessageBox.Show("下载附件到本地后，却找不到该文件[" + fileFullPath + "]！");
            }
        }

        private static void Finish(object o)
        {
            string fileFullPath = o.ToString();
            try
            {
                Process.Start(fileFullPath);
            }
            catch (Exception e)
            {
                MessageBox.Show("下载附件成功，但打开附件[" + fileFullPath + "]时出错：" + e.Message);
            }
        }

        /// <summary>
        /// 获取枚举的特性描述
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTopEnumClass">是否改变为返回该类、枚举类型的顶级Description属性，而不是当前的属性或枚举变量（成员）的Description属性</param>
        /// <returns></returns>
        public static string GetDescription(this object obj, bool isTopEnumClass)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTopEnumClass)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }
            return obj.ToString();
        }

        public static SystemType GetSystemType(string type)
        {
            if (type == "System.Object")
                return SystemType.附件;
            else if (type == "System.DateTime")
                return SystemType.时间;
            else if (type == "System.Decimal")
                return SystemType.数字;
            else if (type == "System.String")
                return SystemType.字符;
            else
                return SystemType.字符;
        }

        public static string GetType(SystemType type)
        {
            switch (type)
            {
                case SystemType.附件:
                    return "System.Object";
                case SystemType.时间:
                    return "System.DateTime";
                case SystemType.数字:
                    return "System.Decimal";
                case SystemType.字符:
                default:
                    return "System.String";
            }
        }

        public static List<FinanceDetail> GetDetailsByStr(string ids)
        {
            List<FinanceDetail> details = FinanceDetailLogic.GetInstance().GetFinanceDetailsByIds(ids);
            return details;
        }

        public static string GetDetailsStr(List<FinanceDetail> details)
        {
            StringBuilder sb = new StringBuilder();
            if (details != null)
            {
                foreach (FinanceDetail detail in details)
                {
                    if (sb.Length == 0)
                        sb.Append(detail.ID);
                    else
                        sb.Append("," + detail.ID);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取刚选定的附件（尚未上传），或者已经上传的附件（格式：12|abc.txt）
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static Attachment GetAttachment(string filepath, PermissionForm owner = null)
        {
            Attachment a = null;
            if (!string.IsNullOrEmpty(filepath))
            {
                int id = 0;
                string truePath = filepath;
                int index = filepath.IndexOf("|");
                if (index > -1)//12|abc.txt
                {
                    id = Convert.ToInt32(filepath.Substring(0, index));
                    truePath = filepath.Substring(index + 1);
                }
                a = new Attachment();
                a.ID = id;
                a.AttachmentFilename = truePath;
                if (File.Exists(filepath))
                {
                    FileInfo fi = new FileInfo(filepath);
                    a.Size = fi.Length;
                }
                if (owner != null)
                    a.Uploader = owner.User;
            }
            return a;
        }

        public static Attachment GetAttachmentForDownload(int id)
        {
            return AttachmentLogic.GetInstance().GetAttachment(id);
        }

        public static int GetControlCountInForm<T>(Form owner)
        {
            int count = 0;
            if (owner != null && owner.Controls.Count > 0)
            {
                foreach (Control c in owner.Controls)
                {
                    if (c is T)
                        count++;
                }
            }
            return count;
        }

        public static int GetControlMaxNumInForm<T>(Form owner, string namePrex)
        {
            int count = 0;
            if (owner != null && owner.Controls.Count > 0)
            {
                foreach (Control c in owner.Controls)
                {
                    if (c is T)
                    {
                        string name = c.Name;
                        if (name.StartsWith(namePrex, StringComparison.InvariantCultureIgnoreCase))
                        {
                            string num = name.Substring(namePrex.Length);
                            int R;
                            if (int.TryParse(num, out R))
                            {
                                if (R > count)
                                    count = R;
                            }
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 读取Excel数据(2007及以上的文档暂不支持，因为找不到合适的ICSharpCode.SharpZipLib版本)
        /// </summary>
        /// <param name="excelFile">读取指定Excel文档的第一个Sheet</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="fields">返回列名列表</param>
        /// <returns></returns>
        public static DataTable ReadDataFromExcel(string excelFile, bool isFirstRowColumn, out List<string> fields)
        {
            DataTable data = new DataTable();
            fields = null;
            if (File.Exists(excelFile))
            {
                IWorkbook wb = null;
                try
                {
                    using (FileStream fs = new FileStream(excelFile, FileMode.Open, FileAccess.Read))
                    {
                        wb = WorkbookFactory.Create(fs);
                        //if (excelFile.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)) // 2007版本及其以上的版本
                        //    wb = new XSSFWorkbook(fs);
                        //else if (excelFile.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)) // 2003版本
                        //    wb = new HSSFWorkbook(fs);
                    }
                    if (wb == null)
                        return null;
                    int startRow = 0;
                    ISheet sheet = wb.GetSheetAt(0);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellFirst = firstRow.FirstCellNum;
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                        fields = new List<string>();
                        if (isFirstRowColumn)
                        {
                            for (int i = cellFirst; i < cellCount; ++i)
                            {
                                ICell cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    string cellValue = cell.StringCellValue;
                                    if (cellValue != null)
                                    {
                                        fields.Add(cellValue);
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }
                                }
                            }
                            startRow = sheet.FirstRowNum + 1;
                        }
                        else
                        {
                            for (int i = cellFirst; i < cellCount; ++i)
                            {
                                string cellValue = "Field" + i;
                                fields.Add(cellValue);
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                            startRow = sheet.FirstRowNum;
                        }

                        //最后一行的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    wb = null;
                }
            }
            return data;
        }

        /// <summary>
        /// 保存数据到Excel(2007及以上的文档暂不支持，因为找不到合适的ICSharpCode.SharpZipLib版本)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static Exception SaveAsExcel(DataTable data, string fullname = null)
        {
            Exception e = null;
            string reportName = "报表";
            if (!string.IsNullOrEmpty(data.TableName))
                reportName = data.TableName;
            if (string.IsNullOrEmpty(fullname))
            {
                string exportPath = Directory.GetCurrentDirectory() + "\\Reports\\";
                if (!Directory.Exists(exportPath))
                    Directory.CreateDirectory(exportPath);
                string filename = reportName + "_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                fullname = exportPath + filename + ".xls";
            }

            IWorkbook wb = null;
            try
            {
                if (fullname.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)) // 2007版本及其以上的版本
                    wb = new XSSFWorkbook();
                else if (fullname.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)) // 2003版本
                    wb = new HSSFWorkbook();
                if (wb == null)
                    return new Exception("无法创建Workbook！");

                //创建表  
                ISheet sh = wb.CreateSheet(reportName);
                ICell icell;
                //创建表头
                bool hasSerailCol = false;
                bool showTile = false;
                int colCount = data.Columns.Count;
                int headRowNum = 0;
                if (showTile) headRowNum = 1;
                IRow row0 = sh.CreateRow(headRowNum);
                row0.Height = (short)(20 * 20);//20 * 20;
                //设置表头
                for (int i = 0; i < colCount; i++)
                {
                    if (hasSerailCol && i == 0)
                    {
                        sh.SetColumnWidth(i, 10 * 256);
                    }
                    else
                    {
                        sh.SetColumnWidth(i, 15 * 256);
                    }
                    icell = row0.CreateCell(i);
                    icell.CellStyle = GetCellStyle(wb, XlsStyle.Header);
                    icell.SetCellValue(data.Columns[i].ColumnName);
                }
                if (data.Rows.Count > 0)
                {
                    int begin = 1;
                    if (showTile) begin = 2;
                    ICell cell;
                    int num = 0;
                    for (int j = 0; j < data.Rows.Count; j++)
                    {
                        num++;
                        //创建数据行
                        DataRow dr = data.Rows[j];
                        IRow row1 = sh.CreateRow(j + begin);
                        row1.Height = (short)(20 * 15);//20 * 15;
                        for (int k = 0; k < colCount; k++)
                        {
                            CellType ct = CellType.BLANK;
                            string val = "";
                            decimal d;
                            bool b;
                            DateTime t;
                            if (k > 0)
                            {
                                val = dr[k - 1].ToString();
                                if (bool.TryParse(val, out b))
                                    ct = CellType.BOOLEAN;
                                else if (DateTime.TryParse(val, out t))
                                    ct = CellType.STRING;
                                else if (decimal.TryParse(val, out d))
                                    ct = CellType.NUMERIC;
                            }
                            cell = row1.CreateCell(k, ct);
                            if (k == 0)
                            {
                                cell.CellStyle = GetCellStyle(wb, XlsStyle.Serial);
                                cell.SetCellValue(num);
                                continue;
                            }
                            if (DateTime.TryParse(val, out t))
                                cell.CellStyle = GetCellStyle(wb, XlsStyle.Time);
                            else if (decimal.TryParse(val, out d))
                                cell.CellStyle = GetCellStyle(wb, XlsStyle.Number);
                            else
                                cell.CellStyle = GetCellStyle(wb, XlsStyle.Default);
                            cell.SetCellValue(val);
                        }
                    }
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.Write(ms);//将Excel写入流
                    ms.Flush();
                    ms.Position = 0;
                    FileStream dumpFile = new FileStream(fullname, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                    ms.WriteTo(dumpFile);//将流写入文件
                }
                //using (FileStream stm = File.OpenWrite(fullname))
                //{
                //    wb.Write(stm);
                //}
            }
            catch (Exception ex)
            {
                e = ex;
            }
            finally
            {
                wb = null;
            }

            return e;
        }

        #region 定义单元格常用到样式
        private static ICellStyle GetCellStyle(IWorkbook wb, XlsStyle str, Font headFont = null, Font contentFont = null, Font titleFont = null)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();
            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的

            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 10;

            if (contentFont != null)
            {
                font.FontName = contentFont.Name;
                if (contentFont.Bold) font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                font.FontHeightInPoints = (short)contentFont.SizeInPoints;
                font.IsItalic = contentFont.Italic;
                font.IsStrikeout = contentFont.Strikeout;
                font.Underline = (byte)(contentFont.Underline ? 1 : 0);
            }

            IFont linkAddresFont = wb.CreateFont();
            linkAddresFont.Color = HSSFColor.OLIVE_GREEN.BLUE.index;
            linkAddresFont.IsItalic = true;//下划线  
            linkAddresFont.FontName = "微软雅黑";

            //边框  
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            //边框颜色  
            //cellStyle.BottomBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            //cellStyle.TopBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            //背景图形，我没有用到过。感觉很丑  
            //cellStyle.FillBackgroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            //cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            cellStyle.FillForegroundColor = HSSFColor.WHITE.index;
            // cellStyle.FillPattern = FillPatternType.NO_FILL;  
            cellStyle.FillBackgroundColor = HSSFColor.BLUE.index;
            //水平对齐  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.CENTER;
            //自动换行  
            cellStyle.WrapText = true;
            //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
            cellStyle.Indention = 0;
            //上面基本都是设共公的设置  
            //下面列出了常用的字段类型  
            switch (str)
            {
                case XlsStyle.Title:
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    if (titleFont != null)
                    {
                        font.FontName = titleFont.Name;
                        if (titleFont.Bold) font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font.FontHeightInPoints = (short)titleFont.SizeInPoints;
                        font.IsItalic = titleFont.Italic;
                        font.IsStrikeout = titleFont.Strikeout;
                        font.Underline = (byte)(titleFont.Underline ? 1 : 0);
                    }
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Header:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    if (headFont != null)
                    {
                        font.FontName = headFont.Name;
                        if (headFont.Bold) font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                        font.FontHeightInPoints = (short)headFont.SizeInPoints;
                        font.IsItalic = headFont.Italic;
                        font.IsStrikeout = headFont.Strikeout;
                        font.Underline = (byte)(headFont.Underline ? 1 : 0);
                    }
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.MEDIUM;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.MEDIUM;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.MEDIUM;
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.MEDIUM;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Bottom:
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT;
                    //if (bottomFont != null)
                    //{
                    //    font.FontName = bottomFont.Name;
                    //    if (bottomFont.Bold) font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    //    font.FontHeightInPoints = (short)bottomFont.SizeInPoints;
                    //    font.IsItalic = bottomFont.Italic;
                    //    font.IsStrikeout = bottomFont.Strikeout;
                    //    font.Underline = (byte)(bottomFont.Underline ? 1 : 0);
                    //}
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.NONE;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Serial:
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                    font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Time:
                    IDataFormat dataStyle = wb.CreateDataFormat();
                    cellStyle.DataFormat = dataStyle.GetFormat("yyyy-mm-dd");
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Number:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.GENERAL;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Money:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.GENERAL;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Url:
                    linkAddresFont.Underline = 1;
                    cellStyle.SetFont(linkAddresFont);
                    break;
                case XlsStyle.Percent:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.GENERAL;
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Chupper:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Tnumber:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case XlsStyle.Default:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;

        }
        #endregion

        #region 定义单元格常用到样式的枚举
        public enum XlsStyle
        {
            Title,
            Header,
            Bottom,
            Serial,
            Url,
            Time,
            Number,
            Money,
            Percent,
            Chupper,
            Tnumber,
            Default
        }
        #endregion

        /// <summary>
        /// 获取单元格类型(xls)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLS(HSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.BLANK: //BLANK:  
                    return null;
                case CellType.BOOLEAN: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.NUMERIC: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.STRING: //STRING:  
                    return cell.StringCellValue;
                case CellType.ERROR: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.FORMULA: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }

        /// <summary>  
        /// 获取单元格类型(xlsx)  
        /// </summary>  
        /// <param name="cell"></param>  
        /// <returns></returns>  
        private static object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.BLANK: //BLANK:  
                    return null;
                case CellType.BOOLEAN: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.NUMERIC: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.STRING: //STRING:  
                    return cell.StringCellValue;
                case CellType.ERROR: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.FORMULA: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }

        /// <summary>
        /// 群发短信的接口
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mobiles"></param>
        /// <param name="greets"></param>
        /// <returns></returns>
        public static bool SendSMS(string content, List<string> mobiles, List<string> greets = null)
        {
            return SMSLogic.SendSMS(content, mobiles, greets);
        }

        public static List<string> GetMobilesFromExcel(string excelFile)
        {
            List<string> mobiles = new List<string>();
            if (File.Exists(excelFile))
            {
                IWorkbook wb = null;
                try
                {
                    using (FileStream fs = new FileStream(excelFile, FileMode.Open, FileAccess.Read))
                    {
                        wb = WorkbookFactory.Create(fs);
                        //if (excelFile.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)) // 2007版本及其以上的版本
                        //    wb = new XSSFWorkbook(fs);
                        //else if (excelFile.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)) // 2003版本
                        //    wb = new HSSFWorkbook(fs);
                    }
                    if (wb == null)
                        return null;
                    int startRow = 0;
                    ISheet sheet = wb.GetSheetAt(0);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellFirst = firstRow.FirstCellNum;
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                            startRow = sheet.FirstRowNum;

                        //最后一行的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue; //没有数据的行默认是null
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                {
                                    string mobile = row.GetCell(j).ToString();
                                    if (Regex.IsMatch(mobile, Configs.MobileNumberRegx))
                                    {
                                        mobiles.Add(mobile);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    wb = null;
                }
            }
            return mobiles;
        }

        public static FormObject GetFormModule(FormObject formObject, User user)
        {
            FormObject module = new FormObject();
            module.ID = formObject.ID;
            module.FormName = formObject.FormName;
            module.FormType = formObject.FormType;
            if (user != null)
                module.Owner = user;
            else
                module.Owner = formObject.Owner;
            module.Remark = formObject.Remark;
            module.FormItems = new List<FormItem>();
            foreach (FormItem item in formObject.FormItems)
            {
                FormItem fi = new FormItem();
                fi.ItemName = item.ItemName;
                fi.ItemType = item.ItemType;
                fi.ItemValue = "";
                fi.Flag = item.Flag;
                int id = FormItemLogic.GetInstance().AddFormItem(fi);
                fi.ID = id;
                module.FormItems.Add(fi);
            }
            return module;
        }

        public static bool NewDocument(User user, MainForm owner, FormObject form)
        {
            DocEditForm def = new DocEditForm(user, owner, form);
            return def.ShowDialog() == DialogResult.OK;
        }

        public static List<Staff> GetStaffByIdStr(string ids)
        {
            List<Staff> staffs = new List<Staff>();
            if (!string.IsNullOrEmpty(ids))
            {
                StaffLogic sl = StaffLogic.GetInstance();
                string[] Ids = ids.Split(",".ToCharArray(),   StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in Ids)
                {
                    int ID;
                    if (int.TryParse(id, out ID))
                    {
                        Staff staff = sl.GetStaff(ID);
                        if (staff != null)
                        {
                            staffs.Add(staff);
                        }
                    }
                }
            }
            return staffs;
        }

        public static List<Member> GetMemberByIdStr(string ids)
        {
            List<Member> members = new List<Member>();
            if (!string.IsNullOrEmpty(ids))
            {
                MemberLogic ml = MemberLogic.GetInstance();
                string[] Ids = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string id in Ids)
                {
                    int ID;
                    if (int.TryParse(id, out ID))
                    {
                        Member member = ml.GetMember(ID);
                        if (member != null)
                        {
                            members.Add(member);
                        }
                    }
                }
            }
            return members;
        }

        public static string GetStaffIdStr(List<Staff> list)
        {
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Staff staff in list)
                {
                    if (sb.Length == 0)
                        sb.Append(staff.ID);
                    else
                        sb.Append("," + staff.ID);
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        public static string GetMemberIdStr(List<Member> list)
        {
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Member member in list)
                {
                    if (sb.Length == 0)
                        sb.Append(member.ID);
                    else
                        sb.Append("," + member.ID);
                }
                return sb.ToString();
            }
            return string.Empty;
        }
    }
}
