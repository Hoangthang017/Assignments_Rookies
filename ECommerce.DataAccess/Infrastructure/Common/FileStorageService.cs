using ECommerce.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.DataAccess.Infrastructure.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = SystemConstants.ImageSettings.FolderSaveImage;
        private const string HOST_URL = SystemConstants.AppSettings.BackendApiAddress;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _userContentFolder = Path.Combine(webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName, string folder)
        {
            return $"{HOST_URL}/{USER_CONTENT_FOLDER_NAME}/{folder}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string folder)
        {
            var filePath = Path.Combine(_userContentFolder, folder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string filePath)
        {
            var fileName = filePath.Replace($"{HOST_URL}/{USER_CONTENT_FOLDER_NAME}/", "");
            filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}