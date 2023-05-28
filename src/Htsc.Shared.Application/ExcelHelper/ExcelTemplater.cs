using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace Htsc.Shared.ExcelHelper
{
    public class ExcelTemplater
    {
        public static List<DataTable> ConvertBinaryExcelToDatatable(byte[] rawData)
        {
                       List<DataTable> list = new List<DataTable>();
            using (MemoryStream memoryStream = new MemoryStream(rawData))
            {
                ExcelPackage val = (ExcelPackage)(object)new ExcelPackage((Stream)memoryStream);
                try
                {
                    foreach (ExcelWorksheet worksheet in val.Workbook.Worksheets)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Clear();
                        if (worksheet.Dimension == null)
                        {
                            continue;
                        }

                        for (int i = worksheet.Dimension.Start.Column; i <= worksheet.Dimension.End.Column; i++)
                        {
                            dataTable.Columns.Add(ExcelCellAddress.GetColumnLetter(i));
                        }

                        for (int j = worksheet.Dimension.Start.Row; j <= worksheet.Dimension.End.Row; j++)
                        {
                            object[] array = new object[worksheet.Dimension.End.Column - worksheet.Dimension.Start.Column + 1];
                            for (int k = worksheet.Dimension.Start.Column; k <= worksheet.Dimension.End.Column; k++)
                            {
                                if (((ExcelRangeBase)worksheet.Cells[j, k]).Value != null)
                                {
                                    array[k - worksheet.Dimension.Start.Column] = ((ExcelRangeBase)worksheet.Cells[j, k]).Value;
                                }
                            }

                            dataTable.Rows.Add(array);
                        }

                        list.Add(dataTable);
                    }
                }
                finally
                {
                    ((IDisposable)val)?.Dispose();
                }
            }

            return list;
        }
    }
}
