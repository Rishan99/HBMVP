using Microsoft.AspNetCore.Http;

namespace HouseBrokerMVP.Business.Services
{
    public interface IFileService
    {
        public void CreateFolderIfNotExist(string folderPath);
        Task<string> SaveFile(string folderPath, IFormFile file);
        void DeleteFile(string imageUploadPath, string fileName);

    }
}
