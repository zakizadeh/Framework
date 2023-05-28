using Abp.Dependency;
using Abp.Localization;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Htsc.Shared.ExcelHelper
{
    public class ExcelHelper : IExcelHelper, ITransientDependency
    {
        private readonly IIocManager _iocManager;

        public ExcelHelper(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }


        public byte[] GetExcelFile<T>(ExcelHelperDto result)
        {
            return null;
            //var tempFileCacheManager = _iocManager.Resolve<ITempFileCacheManager>();
            //return tempFileCacheManager.GetFile(result.Token.ToString());
        }


        /// <summary>
        /// تبدیل فایل اکسل به کلاس
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<T> ExtractDataFromExcel<T>(byte[] data) where T : new()
        {
            //TODO check empty exception
            DataTable rawData = null;

            try
            {
                var dataTable = ExcelTemplater.ConvertBinaryExcelToDatatable(data);

                rawData = dataTable.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("فایل اکسل نا معتبر می باشد");
            }


            List<T> gMRequestPresenterExcelDtos = new List<T>();

            if (rawData != null)
            {
                var header = rawData.Rows[0].ItemArray;
                for (int i = 1; i < rawData.Rows.Count; i++)
                {
                    var row = rawData.Rows[i];
                    var dto = new T();
                    for (var index = 0; index < header.Length; index++)
                    {
                        SetValue(row[index], dto, header[index].ToString(), index);
                    }
                    gMRequestPresenterExcelDtos.Add(dto);
                }
            }

            return gMRequestPresenterExcelDtos;
        }


        /// <summary>
        /// مقداردهی به کلاس 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dto"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private bool SetValue<T>(object value, T dto, string propertyName, int rowIndex)
        {
            System.Reflection.PropertyInfo propertyInfo = null;
            if (IsEnglish(propertyName))
            {
                propertyInfo = typeof(T).GetProperty(propertyName);
            }
            else
            {
                foreach (var propertyInfoItem in typeof(T).GetProperties())
                {
                    var attr = (DisplayAttribute)Attribute.GetCustomAttribute(propertyInfoItem, typeof(DisplayAttribute));
                    if (attr != null)
                    {
                        if (attr.Name.Equals(propertyName))
                        {
                            propertyInfo = propertyInfoItem;
                            break;
                        }
                    }
                }
            }


            if (propertyInfo == null)
            {
                throw new UserFriendlyException("فایل اکسل معتبر نمی باشد");
            }
            var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
            if (converter == null)
            {
                throw new UserFriendlyException("فایل اکسل معتبر نمی باشد");
            }
            object result = CheckTypeAttribute(value, propertyInfo, propertyName, rowIndex);
            if (result == null)
                result = converter.ConvertFrom(value?.ToString());
            propertyInfo.SetValue(dto, result);
            return true;
        }

        /// <summary>
        /// بررسی فرمت داده ورودی
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyInfo"></param>
        private object CheckTypeAttribute(object value, System.Reflection.PropertyInfo propertyInfo, string propertyName, int rowIndex)
        {
            var maxLengthAttribute = (MaxLengthAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(MaxLengthAttribute));
            try
            {
                if (maxLengthAttribute != null && !maxLengthAttribute.IsValid(value))
                {
                    throw new UserFriendlyException($"مقدار سطر  {rowIndex+1} فیلد {propertyName}  معتبر نمی باشد");
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException($"مقدار سطر  {rowIndex+1} فیلد {propertyName}  معتبر نمی باشد");
            }


            if (!string.IsNullOrEmpty(value?.ToString().Trim()) && Attribute.GetCustomAttribute(propertyInfo, typeof(DataTypeAttribute)) != null)
            {
                var dataType = (DataTypeAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DataTypeAttribute));

                switch (dataType.DataType)
                {
                    case DataType.Time:
                        DateTime.TryParse(value?.ToString(), out DateTime time);
                        string theTime = string.Format("{0:HH:mm}", time);
                        if (!TimeValidation.IsValidTime(theTime))
                        {
                            throw new UserFriendlyException($"مقدار سطر  {rowIndex + 1} فیلد {propertyName}  معتبر نمی باشد");
                        }
                        else
                        {
                            return theTime;
                        }
                        break;
                    case DataType.Date:
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            throw new UserFriendlyException("مقدار نمی تواند خالی باشد");
                        }
                        var val = value.ToString();
                        if (val.Length != 8)
                        {
                            throw new UserFriendlyException($"مقدار {rowIndex + 1}:{propertyName}  معتبر نمی باشد");
                        }
                        int.TryParse(val.Substring(0, 4), out int year);
                        int.TryParse(val.Substring(4, 2), out int month);
                        int.TryParse(val.Substring(6, 2), out int day);
                        try
                        {
                            return new DateTime(year, month, day, new PersianCalendar());
                        }
                        catch (Exception)
                        {
                            throw new UserFriendlyException($"مقدار {rowIndex + 1}:{propertyName}  معتبر نمی باشد");
                        }
                        break;

                    default:
                        break;
                }
            }
            return null;
        }

        bool IsEnglish(string inputstring)
        {
            Regex regex = new Regex(@"[A-Za-z0-9 .,-=+(){}\[\]\\]");
            MatchCollection matches = regex.Matches(inputstring);

            if (matches.Count.Equals(inputstring.Length))
                return true;
            else
                return false;
        }
        public byte[] CompressFiles(FileDto file)
        {
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    var fileBytes = file.Data;
                    var entry = zipStream.CreateEntry(file.FileName);

                    using (var originalFileStream = new MemoryStream(fileBytes))
                    using (var zipEntryStream = entry.Open())
                    {
                        originalFileStream.CopyTo(zipEntryStream);
                        originalFileStream.Close();
                    }
                }
                outputZipFileStream.Close();
                return outputZipFileStream.ToArray();
            }
        }


        public async Task<(string Extension, string Data)> ConvertToExcel(DataTable dataTable)
        {
            List<ExcelMetaData> list = new List<ExcelMetaData>();
            var index = 0;
            list.Add(new ExcelMetaData()
            {
                Row = index,
                BackGroundColor = System.Drawing.Color.LightGray,
                FontColor = System.Drawing.Color.Black,
                ShrinkToFit = true
            });
            index++;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ExcelMetaData excelMetaData = new ExcelMetaData()
                {
                    Row = index,
                    BackGroundColor = System.Drawing.Color.WhiteSmoke,
                    FontColor = System.Drawing.Color.Black,
                    ShrinkToFit = true
                };
                list.Add(excelMetaData);
                index++;
            }
            ExcelExportInputDto inputDto = new ExcelExportInputDto()
            {
                DataTable = dataTable,
                MetaDatas = list,
                ShowHeaders = true
            };
            ExcelExportBuilder excelExportBuilder = new ExcelExportBuilder();
            var excellFile = await excelExportBuilder.CreateExcelExport(inputDto);
            return (excellFile.Extension, excellFile.Data);
        }






        /// <summary>
        /// تبدیل لیست به فایل اکسل و قراردادن در کش جهت استفاده 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns>اطلاعات مربوط به فایل اکسل قرار گرفته در کش برگردانده می شود</returns>
        public async Task<FileOutputDto> ConvertToExcel<T>(List<T> items, string header = null)
        {
            ExcelExportBuilder excelExportBuilder = new ExcelExportBuilder();
            var temp = ConvertExportExcelDto(items, header);
            //temp.ExcelHelperAdditionalDtos = excelHelperAdditionalDtos;
            return await excelExportBuilder.CreateExcelExport(temp);

        }

        private ExcelExportInputDto ConvertExportExcelDto<T>(IEnumerable<T> listData, string header)
        {
            var dataTable = new DataTable();

            List<ExcelMetaData> list = new List<ExcelMetaData>();


            var index = 0;
            var type = typeof(T);
            foreach (var propertyInfo in type.GetProperties())
            {
                var displayAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayAttribute));
                string columnName = propertyInfo.Name;
                if (displayAttribute != null)
                {
                    columnName = displayAttribute.Name;
                }
                Type typex;
                if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                {
                    typex = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                }
                else
                {
                    typex = propertyInfo.PropertyType;
                }
                if (typex == typeof(DateTime))
                {
                    typex = typeof(string);
                }
                dataTable.Columns.Add(string.Format("{0}", columnName), typex);
            }

            list.Add(new ExcelMetaData()
            {
                Row = index,
                BackGroundColor = System.Drawing.Color.LightGray,
                FontColor = System.Drawing.Color.Black,
                ShrinkToFit = true
            });
            index++;
            foreach (var item in listData)
            {
                var array = type
                     .GetProperties().Where(p => p.MemberType == System.Reflection.MemberTypes.Property)
                     .Select(p =>
                     {
                         object value = p.GetValue(item, null);
                         var propertyType = p.PropertyType;

                         if (value == null || (propertyType == typeof(DateTime) && default == (DateTime)value))
                             return null;
                         try
                         {
                             if (propertyType == typeof(DateTime) || Nullable.GetUnderlyingType(propertyType) == typeof(DateTime))
                                 return ((DateTime)value).ToString("yyyy/MM/dd");
                             var converter = TypeDescriptor.GetConverter(propertyType);
                             return converter.ConvertTo(value, propertyType);
                         }
                         catch (Exception)
                         {
                         }
                         return value;
                     })
                     .ToArray();
                dataTable.Rows.Add(array);
                ExcelMetaData excelMetaData = new ExcelMetaData()
                {
                    Row = index,
                    BackGroundColor = System.Drawing.Color.WhiteSmoke,
                    FontColor = System.Drawing.Color.Black,
                    ShrinkToFit = true
                };
                list.Add(excelMetaData);
                index++;
            }

            ExcelExportInputDto inputDto = new ExcelExportInputDto()
            {
                DataTable = dataTable,
                MetaDatas = list,
                ShowHeaders = true,
                Title = header,
                ShowSum = false
            };
            return inputDto;
        }



    }
}
