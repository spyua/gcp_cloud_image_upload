namespace cbk.cloud.serviceProvider.Storage
{
    
    public interface IStorageService
    {
        Task<string> UploadFileAsync(string bucketName, string objectName, string filePath);

        Task<UploadResult> UploadFileAsync(string bucketName, string objectName, Stream stream);

        Task DeleteFileAsync(string bucketName, string objectName);

        Task<Stream> GetFileAsync(string bucketName, string objectName);
    }
}
