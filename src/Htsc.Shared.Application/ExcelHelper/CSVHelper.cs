using Abp.Localization;
using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace Htsc.Shared.ExcelHelper
{
    public static class CSVHelper //:ICSVHelper, ITransientDependency
    {
        public static StringBuilder ConvertDataTableToCsvFile(this DataTable dtData)
        {
            string seperator = ";";
            StringBuilder data = new StringBuilder();
            data.Append('\uFEFF');
            //column names
            for (int column = 0; column < dtData.Columns.Count; column++)
            {
                var dt = string.Format("{0}", LocalizationHelper.GetString(SharedConsts.LocalizationSourceName, dtData.Columns[column].ColumnName.ToString(), new CultureInfo("fa-IR")));
                if (column == dtData.Columns.Count - 1)
                    data.Append(dt.Replace(seperator, ","));
                else
                    data.Append(dt.Replace(seperator, ",") + seperator);
            }

            data.Append(Environment.NewLine);

            for (int row = 0; row < dtData.Rows.Count; row++)
            {
              data.Append('\uFEFF');
                for (int column = 0; column < dtData.Columns.Count; column++)
                {
                    var dataValue = dtData.Rows[row][column].ToString().Replace(seperator, ",");
                    if (string.IsNullOrEmpty(dataValue.Trim())){
                        dataValue = "\uFEFF";
                    }
                    if (column == dtData.Columns.Count - 1)
                        data.Append(dataValue);
                    else
                        data.Append(dataValue + seperator);
                }

                if (row != dtData.Rows.Count - 1)
                    data.Append(Environment.NewLine);
            }
            return data;
        }

    }
}
