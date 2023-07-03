namespace cbk.cloud.serviceProvider.Storage
{
    
    public interface IStorageService
    {
        Task<string> UploadFileAsync(string bucketName, string objectName, string filePath);

        Task<UploadResultDto> UploadFileAsync(string bucketName, string objectName, Stream stream);

        Task DeleteFileAsync(string bucketName, string objectName);
    }
}
