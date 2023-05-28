using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Threading.Tasks;


namespace Htsc.Shared.ExcelHelper
{
    public class ExcelExportBuilder
    {
        public async Task<FileOutputDto> CreateExcelExport(ExcelExportInputDto inputDto)
        {
            return await Task.Run(delegate
            {
               
                ExcelPackage val = (ExcelPackage)(object)new ExcelPackage();
                try
                {
                    ExcelWorksheet val2 = val.Workbook.Worksheets.Add("Sheet 1");
                    string text = "A3";
                    if (inputDto.MasterDataTable != null)
                    {
                        val2.Cells["A1"].LoadFromDataTable(inputDto.MasterDataTable, true);
                    }
                    else
                    {
                        text = "A1";
                    }

                    val2.Cells[text].LoadFromDataTable(inputDto.DataTable, true);
                    int rows = val2.Cells[text].Rows;
                    int row = val2.Cells[text].Start.Row;
                    //val2.DefaultColWidth(30.0);
                    val2.View.RightToLeft = true;
                    val2.View.ShowHeaders = (inputDto.ShowHeaders);
                    foreach (ExcelMetaData metaData in inputDto.MetaDatas)
                    {
                        if (metaData.BackGroundColor.HasValue)
                        {
                            int num = row + metaData.Row;
                            val2.Row(num).Style.Font
                                .Color
                                .SetColor(metaData.FontColor.Value);
                            val2.Row(num).Style.WrapText = (metaData.WrapText);
                            val2.Row(num).Style.ShrinkToFit = (metaData.ShrinkToFit);
                            val2.Row(num).Style.Fill
                                .PatternType = ((ExcelFillStyle)1);
                            val2.Row(num).Style.Fill
                                .BackgroundColor
                                .SetColor(metaData.BackGroundColor.Value);
                            val2.Row(num).Style.Border
                                .Left
                                .Style = ((ExcelBorderStyle)4);
                            val2.Row(num).Style.Border
                                .Right.
                                Style = ((ExcelBorderStyle)4);
                            val2.Row(num).Style.Border
                                .Bottom
                                .Style = ((ExcelBorderStyle)4);
                            val2.Row(num).Style.Border
                                .Top.Style = ((ExcelBorderStyle)4);
                        }
                    }

                    byte[] asByteArray = val.GetAsByteArray();
                    return new FileOutputDto
                    {
                        Data = Convert.ToBase64String(asByteArray),
                        Extension = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Name = (string.IsNullOrWhiteSpace(inputDto.Title) ? "excelOutputResult" : inputDto.Title)
                    };
                }
                finally
                {
                    ((IDisposable)val)?.Dispose();
                }
            });
        }
    }
}
