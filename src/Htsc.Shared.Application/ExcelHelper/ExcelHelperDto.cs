using System;

namespace Htsc.Shared.ExcelHelper
{
    public class ExcelHelperDto
    {
        public ExcelHelperDto()
        {
            Token = System.Guid.NewGuid();
        }
        public Guid Token { get; set; }
        public string Extension { get; internal set; }
    }
}
