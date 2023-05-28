//using Abp.Dependency;

//namespace Htsc.Shared.ExcelHelper
//{
//    public interface ITempFileCacheManager : ITransientDependency
//    {
//        void SetFile(string token, byte[] content);

//        byte[] GetFile(string token);

//        void SetFile(string token, TempFileInfo info);

//        TempFileInfo GetFileInfo(string token);
//    }

//    public class TempFileInfo
//    {
//        public string FileName { get; set; }
//        public string FileType { get; set; }
//        public byte[] File { get; set; }

//        public TempFileInfo()
//        {
//        }

//        public TempFileInfo(byte[] file)
//        {
//            File = file;
//        }

//        public TempFileInfo(string fileName, string fileType, byte[] file)
//        {
//            FileName = fileName;
//            FileType = fileType;
//            File = file;
//        }
//    }
//}
