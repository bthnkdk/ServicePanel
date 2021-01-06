using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Omu.AwesomeMvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Web.UI.Helper
{
    public class GridExcelHelper
    {
        public static HSSFWorkbook Build<T>(GridModel<T> gridModel, string[] columns, string[] headers = null)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            var headerRow = sheet.CreateRow(0);

            var font = workbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            //create header
            for (int i = 0; i < columns.Length; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(headers != null ? headers[i] : columns[i]);
                cell.CellStyle = workbook.CreateCellStyle();
                cell.CellStyle.SetFont(font);
            }

            var currentRow = 0;

            if (gridModel.Data.Groups != null)
            {
                foreach (var groupView in gridModel.Data.Groups)
                {
                    BuildGroup(sheet, columns, ref currentRow, gridModel);
                }
            }
            else if (gridModel.Data.Items != null)
            {
                BuildItems(sheet, columns, ref currentRow, gridModel.Data.Items);
            }

            if (gridModel.Data.Footer != null)
            {
                BuildFooter(sheet, columns, ref currentRow, gridModel.Data.Footer);
            }

            sheet.SetAutoFilter(new CellRangeAddress(0, gridModel.Data.Items.Count, 0, columns.Length - 1));
            return workbook;
        }

        private static void BuildGroup<T>(ISheet sheet, string[] columns, ref int currentRow, GridModel<T> gridModel)
        {

            var groupView = gridModel.Data;
            if (groupView.Header != null)
            {
                currentRow++;
                var row = sheet.CreateRow(currentRow);
                var cell = row.CreateCell(0);
                cell.SetCellValue(groupView.Header.Content);
            }

            if (groupView.Groups != null)
            {
                foreach (var groupViewx in groupView.Groups)
                {
                    BuildGroup(sheet, columns, ref currentRow, gridModel);
                }
            }
            else if (groupView.Items != null)
            {
                BuildItems(sheet, columns, ref currentRow, groupView.Items);
            }

            if (groupView.Footer != null)
            {
                BuildFooter(sheet, columns, ref currentRow, groupView.Footer);
            }
        }

        private static void BuildItems(ISheet sheet, string[] columns, ref int currentRow, IEnumerable<object> items)
        {
            //fill content 

            foreach (var item in items)
            {
                currentRow++;
                var row = sheet.CreateRow(currentRow);

                for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                {
                    var cell = row.CreateCell(columnIndex);
                    CellSetValue(cell, columns[columnIndex], item);
                }
            }
        }

        private static void BuildFooter(ISheet sheet, string[] columns, ref int currentRow, object footer)
        {
            currentRow++;
            var row = sheet.CreateRow(currentRow);
            for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
            {
                var cell = row.CreateCell(columnIndex);
                CellSetValue(cell, columns[columnIndex], footer);
            }
        }

        private static void CellSetValue(ICell cell, string column, object item)
        {
            var prop = item.GetType().GetProperty(column);

            if (prop != null)
            {
                var value = prop.GetValue(item, null);
                if (prop.PropertyType == typeof(DateTime))
                {
                    cell.SetCellValue(((DateTime)value).ToShortDateString());
                }
                else if (prop.PropertyType == typeof(int))
                {
                    cell.SetCellValue(Convert.ToDouble(value));
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        public static List<PropertyInfo> GetProperties(Type type, IEnumerable<string> propertyNames)
        {
            var allProperties = type.GetProperties();
            var properties = new List<PropertyInfo>();
            foreach (var propertyName in propertyNames)
            {
                var property = allProperties.First(p => p.Name == propertyName);
                properties.Add(property);
            }
            return properties;
        }

        public static List<T> ReadData<T>(Stream stream, string fileName, List<PropertyInfo> columns)
        {
            var list = new List<T>();

            if (fileName.EndsWith("xls"))
            {
                var workbook = new HSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);
                if (sheet.PhysicalNumberOfRows <= 1)
                    return list;
                for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var item = Activator.CreateInstance<T>();
                    var row = sheet.GetRow(i);
                    var j = 0;

                    foreach (var column in columns)
                    {
                        if (j == row.Cells.Count)
                            break;
                        var val = row.GetCell(j).StringCellValue;
                        if (string.IsNullOrWhiteSpace(val))
                        {
                            continue;
                        }
                        Type t = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                        column.SetValue(item, Convert.ChangeType(val, t), null);
                        j++;
                    }
                    list.Add(item);
                }
            }
            else if (fileName.EndsWith("xlsx"))
            {
                var workbook = new XSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);
                for (int i = 0; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var item = Activator.CreateInstance<T>();
                    var row = sheet.GetRow(i);
                    var j = 0;
                    foreach (var column in columns)
                    {
                        var val = row.GetCell(j).StringCellValue;
                        if (string.IsNullOrWhiteSpace(val))
                        {
                            continue;
                        }
                        Type t = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                        column.SetValue(item, Convert.ChangeType(val, t), null);
                        j++;
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public static HSSFWorkbook CreateXls<T>(List<T> items, List<PropertyInfo> properties)
        {
            // Create the workbook, sheet, and row
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");

            var i = 0;
            // Header row
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                foreach (var property in properties)
                {
                    row.CreateCell(j).SetCellValue(property.Name);
                    j++;
                }
                i++;
            }

            foreach (var item in items)
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    if (value != null)
                    {
                        row.CreateCell(j).SetCellValue(value.ToString());
                    }
                    j++;
                }
                i++;
            }
            return workbook;
        }

        public static HSSFWorkbook CreateXls(DataTable dt, IEnumerable<string> columnNames = null)
        {
            // Create the workbook, sheet, and row
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            var i = 0;
            // Header row
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                if (columnNames == null)
                {
                    foreach (var columnName in dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList())
                    {
                        row.CreateCell(j).SetCellValue(columnName);
                        j++;
                    }
                    i++;
                }
                else
                {
                    foreach (var columnName in columnNames)
                    {
                        row.CreateCell(j).SetCellValue(columnName);
                        j++;
                    }
                    i++;
                }
            }

            foreach (DataRow dataRow in dt.Rows)
            {
                var row = sheet.CreateRow(i);
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    var value = dataRow[k];
                    if (value != null)
                    {
                        row.CreateCell(k).SetCellValue(value.ToString());
                    }
                    else
                    {
                        row.CreateCell(k).SetCellValue(string.Empty);
                    }
                }
                i++;
            }
            return workbook;
        }

        public static HSSFWorkbook CreateXlsx<T>(List<T> items, List<PropertyInfo> properties, IEnumerable<string> columnNames)
        {
            // Create the workbook, sheet, and row
            //var workbook = new XSSFWorkbook();
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            var i = 0;
            // Header row
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                foreach (var columnName in columnNames)
                {
                    row.CreateCell(j).SetCellValue(columnName);
                    j++;
                }
                i++;
            }

            foreach (var item in items)
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    if (value != null)
                    {
                        row.CreateCell(j).SetCellValue(value.ToString());
                    }
                    else
                    {
                        row.CreateCell(j).SetCellValue(string.Empty);
                    }
                    j++;
                }
                i++;
            }
            return workbook;
        }

        public static XSSFWorkbook CreateXlsx(DataTable dt, IEnumerable<string> columnNames = null)
        {
            // Create the workbook, sheet, and row
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("sheet1");
            var i = 0;
            // Header row
            {
                var j = 0;
                var row = sheet.CreateRow(i);
                if (columnNames == null)
                {
                    foreach (var columnName in dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList())
                    {
                        row.CreateCell(j).SetCellValue(columnName);
                        j++;
                    }
                    i++;
                }
                else
                {
                    foreach (var columnName in columnNames)
                    {
                        row.CreateCell(j).SetCellValue(columnName);
                        j++;
                    }
                    i++;
                }
            }

            foreach (DataRow dataRow in dt.Rows)
            {
                var row = sheet.CreateRow(i);
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    var value = dataRow[k];
                    if (value != null)
                    {
                        row.CreateCell(k).SetCellValue(value.ToString());
                    }
                    else
                    {
                        row.CreateCell(k).SetCellValue(string.Empty);
                    }
                }
                i++;
            }
            return workbook;
        }
    }
}