using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace Htsc.Shared.ExcelHelper
{
    public interface IExcelHelper
    {
        Task<(string Extension, string Data)> ConvertToExcel(DataTable dataTable);
        Task<FileOutputDto> ConvertToExcel<T>(List<T> items, string header = null);

        byte[] GetExcelFile<T>(ExcelHelperDto result);

        /// <summary>
        /// تبدیل فایل اکسل به کلاس
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<T> ExtractDataFromExcel<T>(byte[] data) where T : new();


        byte[] CompressFiles(FileDto file);
    }
}
