using System.Collections.Generic;
using System.Data;

namespace Htsc.Shared.ExcelHelper
{
    public class ExcelExportInputDto
    {
        public DataTable MasterDataTable
        {
            get;
            set;
        }

        public DataTable DataTable
        {
            get;
            set;
        }

        public IEnumerable<ExcelMetaData> MetaDatas
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public bool ShowHeaders
        {
            get;
            set;
        }
        public bool ShowSum { get; internal set; }

        public ExcelExportInputDto()
        {
            ShowHeaders = true;
        }
    }
}
