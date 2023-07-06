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

        public async Task<UploadResult> UploadFileAsync(string bucketName, string objectName, string contentType, Stream stream)
        {
            var result = await _storageClient.UploadObjectAsync(bucketName, objectName, contentType, stream);
            var mediaLinkUri = new Uri(result.MediaLink);
            var fileLinkPath = mediaLinkUri.PathAndQuery.Substring(mediaLinkUri.PathAndQuery.LastIndexOf('/') + 1);

            return new UploadResult
            {
                Name = result.Name,
                Bucket = result.Bucket,
                Size = result.Size.GetValueOrDefault(),
                MediaLink = result.MediaLink,
                FileLinkPath = fileLinkPath,
                ContentType = result.ContentType,
                TimeCreated = result.TimeCreated?.ToLocalTime(), // If TimeCreated is DateTimeOffset?
            };
        }
        
        // Delete
        public async Task DeleteFileAsync(string bucketName, string objectName)
        {
            await _storageClient.DeleteObjectAsync(bucketName, objectName);
        }

        // Get file from bucket
        public async Task<Stream> GetFileAsync(string bucketName, string objectName)
        {
            var stream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(bucketName, objectName, stream);
            return stream;
        }

    }
}
