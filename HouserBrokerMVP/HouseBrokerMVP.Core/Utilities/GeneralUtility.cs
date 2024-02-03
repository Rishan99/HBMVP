using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Core.Utilities
{

    public class GeneralUtility
    {

        public static string GetLoggedInUsername(IHttpContextAccessor contextAccessor)
        {
            return "superadmin";
            var user = contextAccessor.HttpContext?.User ?? throw new Exception("Cannot Find User");

            if (!user.Claims.Any())
                throw new Exception("No claims data!");

            if (user.Claims.All(x => x.Type != "username"))
                throw new Exception("Username not found in claim!");

            string username = user.Claims.FirstOrDefault(x => x.Type == "username")!.Value;

            return username;
        }

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now.ToLocalTime();
        }

        public static void CreateFolderIfNotExist(string folderPath)
        {
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
        }
        public async static Task<string> SaveImageFile(string folderPath, IFormFile file)
        {
            CreateFolderIfNotExist(folderPath);
            var name = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            name += Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, name);
            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);
            return name;
        }
        public static void DeleteFileFromServer(string imageUploadPath, string fileName)
        {
            if (System.IO.File.Exists($"{imageUploadPath}\\{fileName}"))
            {
                System.IO.File.Delete($"{imageUploadPath}\\{fileName}");
            }
        }

    }
}
