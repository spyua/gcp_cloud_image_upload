namespace cbk.cloud.serviceProvider.Storage
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(string bucketName, string objectName, string filePath);

        Task<string> UploadFileAsync(string bucketName, string objectName, Stream stream);
    }
}
