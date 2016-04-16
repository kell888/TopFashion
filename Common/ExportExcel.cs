using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using TopFashion;

namespace LGBB_WebApp
{
    public static class ExportExcel
    {
        static string exportSite = Configs.ExportSite;

        //导出工人账号信息Excel
        public static string ExportWorkerExcel(DataTable dt)
        {
            IWorkbook wb = new HSSFWorkbook();
            //创建表  
            ISheet sh = wb.CreateSheet("工人总账记录表");
            //设置单元的宽度  
            sh.SetColumnWidth(0, 15 * 256);
            sh.SetColumnWidth(1, 35 * 256);
            sh.SetColumnWidth(2, 15 * 256);
            sh.SetColumnWidth(3, 10 * 256);

            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell icell0top = row1.CreateCell(0);

            icell0top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell0top.SetCellValue("省市代码");

            ICell icell1top = row1.CreateCell(1);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell1top.SetCellValue("收款方账号");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell2top.SetCellValue("账簿号");

            ICell icell3top = row1.CreateCell(3);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell3top.SetCellValue("货币码");

            ICell icell4top = row1.CreateCell(4);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell4top.SetCellValue("支付金额");

            ICell icell5top = row1.CreateCell(5);
            icell5top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell5top.SetCellValue("是否加急");

            ICell icell6top = row1.CreateCell(6);
            icell6top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell6top.SetCellValue("是否他行");

            ICell icell7top = row1.CreateCell(7);
            icell7top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell7top.SetCellValue("他行行别");

            ICell icell8top = row1.CreateCell(8);
            icell8top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell8top.SetCellValue("收款方户名");

            ICell icell9top = row1.CreateCell(9);
            icell9top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell9top.SetCellValue("开户行行名");

            ICell icell10top = row1.CreateCell(10);
            icell10top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell10top.SetCellValue("支行名称");

            ICell icell11top = row1.CreateCell(11);
            icell11top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell11top.SetCellValue("开户行行号");

            ICell icell12top = row1.CreateCell(12);
            icell12top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell12top.SetCellValue("是否对私");

            ICell icell13top = row1.CreateCell(13);
            icell13top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell13top.SetCellValue("支付用途");

            ICell icell14top = row1.CreateCell(14);
            icell14top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell14top.SetCellValue("附言");

            ICell icell15top = row1.CreateCell(15);
            icell15top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell15top.SetCellValue("是否校验收款方户名");


            #endregion

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    row1 = sh.CreateRow(i + 1);

                    icell0top = row1.CreateCell(0);
                    icell0top.SetCellValue("");

                    icell1top = row1.CreateCell(1);
                    icell1top.SetCellValue(dr["Wb_bankNo"].ToString());

                    icell2top = row1.CreateCell(2);
                    icell2top.SetCellValue("");

                    icell3top = row1.CreateCell(3);
                    icell3top.SetCellValue("01");

                    icell4top = row1.CreateCell(4);
                    icell4top.SetCellValue(dr["Wa_allprofit"].ToString());

                    icell5top = row1.CreateCell(5);
                    icell5top.SetCellValue("否");

                    icell6top = row1.CreateCell(6);
                    icell6top.SetCellValue("");

                    icell7top = row1.CreateCell(7);
                    icell7top.SetCellValue("");

                    icell8top = row1.CreateCell(8);
                    icell8top.SetCellValue(dr["Wb_name"].ToString());

                    icell9top = row1.CreateCell(9);
                    icell9top.SetCellValue(dr["Wb_bankInfo"].ToString());

                    icell10top = row1.CreateCell(10);
                    icell10top.SetCellValue(dr["Wb_aname"].ToString());

                    icell11top = row1.CreateCell(11);
                    icell11top.SetCellValue("");

                    icell12top = row1.CreateCell(12);
                    icell12top.SetCellValue("是");

                    icell13top = row1.CreateCell(13);
                    icell13top.SetCellValue(dr["Wa_thesource"].ToString() + "订单预定工资");

                    icell14top = row1.CreateCell(14);
                    icell14top.SetCellValue("邻工帮帮公司给你支付的工资");

                    icell15top = row1.CreateCell(15);
                    icell15top.SetCellValue("是");
                }
            }

            string exportPath = AppDomain.CurrentDomain.BaseDirectory + "ExportFiles\\";
            string filename = DateTime.Now.ToString("工人总账记录表-yyyy年MM月dd日hh时mm分ss秒");
            string returnStr = string.Empty;

            try
            {
                using (FileStream stm = File.OpenWrite(exportPath + filename + ".xls"))
                {
                    wb.Write(stm);
                    stm.Close();
                    stm.Dispose();
                }
                wb = null;
                returnStr = "{\"success\":true,\"Message\":\"" + exportSite + filename + ".xls\"}";
            }
            catch (Exception ex)
            {
                wb = null;
                returnStr = "{\"success\":false,\"Message\":\"导出文件发生错误！\"}";
            }

            return returnStr;
        }

        //导出进账记录
        public static string ExportOurExcel(DataTable dt)
        {
            IWorkbook wb = new HSSFWorkbook();
            //创建表  
            ISheet sh = wb.CreateSheet("总公司进账记录表");

            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell icell0top = row1.CreateCell(0);

            icell0top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell0top.SetCellValue("订单号");

            ICell icell1top = row1.CreateCell(1);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell1top.SetCellValue("结算时间");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell2top.SetCellValue("结算金额");

            ICell icell3top = row1.CreateCell(3);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell3top.SetCellValue("星级扣款");

            ICell icell4top = row1.CreateCell(4);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell4top.SetCellValue("账号余额");


            #endregion
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    row1 = sh.CreateRow(i + 1);

                    icell0top = row1.CreateCell(0);
                    icell0top.SetCellValue(dr["Aa_oicode"].ToString());

                    icell1top = row1.CreateCell(1);
                    icell1top.SetCellValue(dr["Aa_stime"].ToString());

                    icell2top = row1.CreateCell(2);
                    icell2top.SetCellValue(dr["Aa_serviceCash"].ToString());

                    icell3top = row1.CreateCell(3);
                    icell3top.SetCellValue(dr["Aa_payfor"].ToString());

                    icell4top = row1.CreateCell(4);
                    icell4top.SetCellValue(dr["Aa_recordCash"].ToString());
                }
            }

            string exportPath = AppDomain.CurrentDomain.BaseDirectory + "ExportFiles\\";
            string filename = DateTime.Now.ToString("总公司进账记录表-yyyy年MM月dd日hh时mm分ss秒");
            string returnStr = string.Empty;

            try
            {
                using (FileStream stm = File.OpenWrite(exportPath + filename + ".xls"))
                {
                    wb.Write(stm);
                    stm.Close();
                    stm.Dispose();
                }
                wb = null;
                returnStr = "{\"success\":true,\"Message\":\"" + exportSite + filename + ".xls\"}";
            }
            catch (Exception ex)
            {
                wb = null;
                returnStr = "{\"success\":false,\"Message\":\"导出文件发生错误！\"}";
            }

            return returnStr;
        }

        //导出子公司进账记录
        public static string ExportSubExcel(DataTable dt)
        {
            IWorkbook wb = new HSSFWorkbook();
            //创建表  
            ISheet sh = wb.CreateSheet("子公司进账记录表");

            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell icell0top = row1.CreateCell(0);

            icell0top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell0top.SetCellValue("子公司名称");

            ICell icell1top = row1.CreateCell(1);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell1top.SetCellValue("订单号");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell2top.SetCellValue("利润比例");

            ICell icell3top = row1.CreateCell(3);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell3top.SetCellValue("结算时间");

            ICell icell4top = row1.CreateCell(4);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell4top.SetCellValue("结算金额");

            ICell icell5top = row1.CreateCell(5);
            icell5top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell5top.SetCellValue("账号余额");

            #endregion
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    row1 = sh.CreateRow(i + 1);

                    icell0top = row1.CreateCell(0);
                    icell0top.SetCellValue(dr["Sc_name"].ToString());

                    icell1top = row1.CreateCell(1);
                    icell1top.SetCellValue(dr["Aa_oicode"].ToString());

                    icell2top = row1.CreateCell(2);
                    icell2top.SetCellValue(dr["sc_profitratio"].ToString());

                    icell3top = row1.CreateCell(3);
                    icell3top.SetCellValue(dr["Aa_stime"].ToString());

                    icell4top = row1.CreateCell(4);
                    icell4top.SetCellValue(dr["Aa_serviceCash"].ToString());

                    icell5top = row1.CreateCell(5);
                    icell5top.SetCellValue(dr["Aa_recordCash"].ToString());
                }
            }

            string exportPath = AppDomain.CurrentDomain.BaseDirectory + "ExportFiles\\";
            string filename = DateTime.Now.ToString("子公司进账记录表-yyyy年MM月dd日hh时mm分ss秒");
            string returnStr = string.Empty;

            try
            {
                using (FileStream stm = File.OpenWrite(exportPath + filename + ".xls"))
                {
                    wb.Write(stm);
                    stm.Close();
                    stm.Dispose();
                }
                wb = null;
                returnStr = "{\"success\":true,\"Message\":\"" +exportSite+ filename + ".xls\"}";
            }
            catch (Exception ex)
            {
                wb = null;
                returnStr = "{\"success\":false,\"Message\":\"导出文件发生错误！\"}";
            }

            return returnStr;
        }

        //导出厂家充值记录
        public static string ExportCorpCZExcel(DataTable dt)
        {
            IWorkbook wb = new HSSFWorkbook();
            //创建表  
            ISheet sh = wb.CreateSheet("厂家充值记录表");

            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell icell0top = row1.CreateCell(0);

            icell0top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell0top.SetCellValue("厂家名称");

            ICell icell1top = row1.CreateCell(1);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell1top.SetCellValue("充值金额");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell2top.SetCellValue("充值时间");

            ICell icell3top = row1.CreateCell(3);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell3top.SetCellValue("充值记录人");

            ICell icell4top = row1.CreateCell(4);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell4top.SetCellValue("账户余额");

            ICell icell6top = row1.CreateCell(5);
            icell6top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell6top.SetCellValue("充值说明");

            #endregion

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    row1 = sh.CreateRow(i + 1);

                    icell0top = row1.CreateCell(0);
                    icell0top.SetCellValue(dr["cmname"].ToString());

                    icell1top = row1.CreateCell(1);
                    icell1top.SetCellValue(dr["regmoney"].ToString());

                    icell2top = row1.CreateCell(2);
                    icell2top.SetCellValue(dr["regtime"].ToString());

                    icell3top = row1.CreateCell(3);
                    icell3top.SetCellValue(dr["wbname"].ToString());

                    icell4top = row1.CreateCell(4);
                    icell4top.SetCellValue(dr["remainmoney"].ToString());

                    icell6top = row1.CreateCell(5);
                    icell6top.SetCellValue(dr["remark"].ToString());

                }
            }

            string exportPath = AppDomain.CurrentDomain.BaseDirectory + "ExportFiles\\";
            string filename = DateTime.Now.ToString("厂家充值记录表-yyyy年MM月dd日hh时mm分ss秒");
            string returnStr = string.Empty;

            try
            {
                using (FileStream stm = File.OpenWrite(exportPath + filename + ".xls"))
                {
                    wb.Write(stm);
                    stm.Close();
                    stm.Dispose();
                }
                wb = null;
                returnStr = "{\"success\":true,\"Message\":\"" + exportSite + filename + ".xls\"}";
            }
            catch (Exception ex)
            {
                wb = null;
                returnStr = "{\"success\":false,\"Message\":\"导出文件发生错误！\"}";
            }

            return returnStr;
        }

        //导出厂家订单消费记录
        public static string ExportCorpOrderExcel(DataTable dt)
        {
            IWorkbook wb = new HSSFWorkbook();
            //创建表  
            ISheet sh = wb.CreateSheet("厂家订单消费记录表");

            #region 设置表头
            IRow row1 = sh.CreateRow(0);
            row1.Height = 20 * 20;
            ICell icell0top = row1.CreateCell(0);

            icell0top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell0top.SetCellValue("厂家名称");

            ICell icell1top = row1.CreateCell(1);
            icell1top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell1top.SetCellValue("消费金额");

            ICell icell2top = row1.CreateCell(2);
            icell2top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell2top.SetCellValue("结算时间");

            ICell icell3top = row1.CreateCell(3);
            icell3top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell3top.SetCellValue("订单编号");

            ICell icell4top = row1.CreateCell(4);
            icell4top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell4top.SetCellValue("账户余额");

            ICell icell5top = row1.CreateCell(5);
            icell5top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell5top.SetCellValue("订单产品");

            ICell icell6top = row1.CreateCell(6);
            icell6top.CellStyle = Getcellstyle(wb, stylexls.header);
            icell6top.SetCellValue("服务工人");

            #endregion

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    row1 = sh.CreateRow(i + 1);

                    icell0top = row1.CreateCell(0);
                    icell0top.SetCellValue(dr["cmname"].ToString());

                    icell1top = row1.CreateCell(1);
                    icell1top.SetCellValue(dr["regmoney"].ToString());

                    icell2top = row1.CreateCell(2);
                    icell2top.SetCellValue(dr["regtime"].ToString());

                    icell3top = row1.CreateCell(3);
                    icell3top.SetCellValue(dr["oicode"].ToString());

                    icell4top = row1.CreateCell(4);
                    icell4top.SetCellValue(dr["remainmoney"].ToString());

                    icell5top = row1.CreateCell(5);
                    icell5top.SetCellValue(dr["productname"].ToString());

                    icell6top = row1.CreateCell(6);
                    icell6top.SetCellValue(dr["wbname"].ToString());

                }
            }

            string exportPath = AppDomain.CurrentDomain.BaseDirectory + "ExportFiles\\";
            string filename = DateTime.Now.ToString("厂家充值记录表-yyyy年MM月dd日hh时mm分ss秒");
            string returnStr = string.Empty;

            try
            {
                using (FileStream stm = File.OpenWrite(exportPath + filename + ".xls"))
                {
                    wb.Write(stm);
                    stm.Close();
                    stm.Dispose();
                }
                wb = null;
                returnStr = "{\"success\":true,\"Message\":\"" + exportSite + filename + ".xls\"}";
            }
            catch (Exception ex)
            {
                wb = null;
                returnStr = "{\"success\":false,\"Message\":\"导出文件发生错误！\"}";
            }

            return returnStr;
        }

        ////导出EXCEL
        //public void NpoiExcel(DataTable dt, string title)
        //{
        //    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
        //    NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("Sheet1");

        //    NPOI.SS.UserModel.IRow headerrow = sheet.CreateRow(0);
        //    ICellStyle style = book.CreateCellStyle();
        //    style.Alignment = HorizontalAlignment.Center;
        //    style.VerticalAlignment = VerticalAlignment.Center;

        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ICell cell = headerrow.CreateCell(i);
        //        cell.CellStyle = style;
        //        cell.SetCellValue(dt.Columns[i].ColumnName);

        //    }392863901

        //    MemoryStream ms = new MemoryStream();
        //    book.Write(ms);
        //    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode(title + "_" + DateTime.Now.ToString("yyyy-MM-dd"), System.Text.Encoding.UTF8)));
        //    Response.BinaryWrite(ms.ToArray());
        //    Response.End();
        //    book = null;
        //    ms.Close();
        //    ms.Dispose();
        //}

        #region 定义单元格常用到样式的枚举
        public enum stylexls
        {
            header,
            url,
            time,
            number,
            money,
            percent,
            CHupper,
            Tnumber,
            defaults
        }
        #endregion

        #region 定义单元格常用到样式
        static ICellStyle Getcellstyle(IWorkbook wb, stylexls str)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();
            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
            IFont font12 = wb.CreateFont();
            font12.FontHeightInPoints = 10;
            font12.FontName = "微软雅黑";

            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            //font.Underline = 1;下划线 

            IFont fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.OLIVE_GREEN.BLUE.index;
            fontcolorblue.IsItalic = true;//下划线  
            fontcolorblue.FontName = "微软雅黑";

            //边框  
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.HAIR;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.HAIR;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.DOTTED;
            //边框颜色  
            cellStyle.BottomBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            cellStyle.TopBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
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
                case stylexls.header:
                    // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                    cellStyle.SetFont(font12);
                    break;
                case stylexls.time:
                    IDataFormat datastyle = wb.CreateDataFormat();
                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.number:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.money:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.url:
                    fontcolorblue.Underline = 1;
                    cellStyle.SetFont(fontcolorblue);
                    break;
                case stylexls.percent:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.CHupper:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.Tnumber:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.defaults:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;

        }
        #endregion
    }
}