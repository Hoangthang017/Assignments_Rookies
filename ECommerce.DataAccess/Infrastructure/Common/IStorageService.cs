namespace ECommerce.DataAccess.Infrastructure.Common
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName, string folder);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string folder);

        Task DeleteFileAsync(string filePath);
    }
}