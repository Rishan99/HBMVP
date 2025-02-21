
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;

namespace HouseBrokerMVP.Business.Services.Interface
{
    public class FileService : IFileService
    {
        public void CreateFolderIfNotExist(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
        }
        public async Task<string> SaveFile(string folderPath, IFormFile file)
        {
            CreateFolderIfNotExist(folderPath);
            var name = GeneralUtility.GetUniqueFileIdentifier();
            name += Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, name);
            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);
            return name;
        }
        public void DeleteFile(string imageUploadPath, string fileName)
        {
            if (System.IO.File.Exists($"{imageUploadPath}\\{fileName}"))
            {
                System.IO.File.Delete($"{imageUploadPath}\\{fileName}");
            }
        }
    }
}
