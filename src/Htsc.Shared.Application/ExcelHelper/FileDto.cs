using System.ComponentModel.DataAnnotations;

namespace Htsc.Shared.ExcelHelper
{
    public class FileDto
    {
        [Required]
        public string FileName { get; set; }

        public string FileType { get; set; }
        public int Count { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public FileDto()
        {

        }

        public FileDto(string fileName, string fileType, byte[] data)
        {
            FileName = fileName;
            FileType = fileType;
            Data = data;
        }
    }
}
