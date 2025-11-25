using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymManagementBLL.Helpers
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public bool Delete(string fileName, string FolderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FolderName))
                    return false;

                var fullPath = Path.Combine(_webHost.WebRootPath, "images", FolderName, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to delete file : {ex}");
                return false;
            }
        }

        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0)
                    return null;

                if (file.Length > maxFileSize)
                    return null;

                var extention = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extention))
                    return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + extention;

                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(fileStream);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to Upload photo in {folderName} because {ex} ");
                return null;
            }
        }
    }
}
