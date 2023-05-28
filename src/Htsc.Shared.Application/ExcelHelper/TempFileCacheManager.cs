using Abp.Dependency;
using Abp.Runtime.Caching;
using Abp.UI;
using System;
namespace Htsc.Shared.ExcelHelper
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        private const string TempFileCacheName = nameof(TempFileCacheManager);

        private readonly ICache _cache;
        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cache = cacheManager.GetCache(TempFileCacheName);
        }

        public void SetFile(string token, byte[] content)
        {
            _cache.Set(token, new TempFileInfo(content), TimeSpan.FromMinutes(10)); // expire time is 1 min by default
        }

        public byte[] GetFile(string token)
        {
            try
            {
                var cacheObject = _cache.GetOrDefault(token);
                return (byte[])cacheObject.GetType().GetProperty(nameof(TempFileInfo.File)).GetValue(cacheObject);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public (byte[], string) GetFileWithName(string token)
        {
            try
            {
                var cacheObject = _cache.GetOrDefault(token);
                var byteData = (byte[])cacheObject.GetType().GetProperty(nameof(TempFileInfo.File)).GetValue(cacheObject);
                var fileName = (string)cacheObject.GetType().GetProperty(nameof(TempFileInfo.FileName)).GetValue(cacheObject);
                return (byteData, fileName);
            }
            catch (Exception)
            {
                throw new UserFriendlyException("فایل یافت نشد");
            }
        }

        public TempFileInfo GetFileInfo(string token)
        {
            return (TempFileInfo)_cache.GetOrDefault(token);
        }
    }

    public interface ITempFileCacheManager : ITransientDependency
    {
        void SetFile(string token, byte[] content);

        byte[] GetFile(string token);
        (byte[], string) GetFileWithName(string token);
        // void SetFile(string token, TempFileInfo info);

        TempFileInfo GetFileInfo(string token);
    }

    public class TempFileInfo
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }

        public TempFileInfo()
        {
        }

        public TempFileInfo(byte[] file)
        {
            File = file;
        }

        public TempFileInfo(string fileName, string fileType, byte[] file)
        {
            FileName = fileName;
            FileType = fileType;
            File = file;
        }

    }
}