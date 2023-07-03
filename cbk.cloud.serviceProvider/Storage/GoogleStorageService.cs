using Google.Cloud.Storage.V1;

namespace cbk.cloud.serviceProvider.Storage
{
    public class GoogleStorageService : IStorageService
    {
        private readonly StorageClient _storageClient;

        public GoogleStorageService()
        {
            _storageClient = StorageClient.Create();
        }

        public async Task<string> UploadFileAsync(string bucketName, string objectName, string filePath)
        {
            using var f = File.OpenRead(filePath);
            var result = await _storageClient.UploadObjectAsync(bucketName, objectName, null, f);
            return result.MediaLink;
        }

        public async Task<string> UploadFileAsync(string bucketName, string objectName, Stream stream)
        {
            var result = await _storageClient.UploadObjectAsync(bucketName, objectName, null, stream);
            return result.MediaLink;
        }
    }
}
