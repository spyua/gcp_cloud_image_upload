using cbk.cloud.serviceProvider.Storage;
using cbk.image.Infrastructure.Config.Storage;
using cbk.image.Infrastructure.Repository;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace cbk.image.service.compress.Service
{
    public class ImageCompressorService : IImageCompressorService
    {
        private IImageRepository _imageRepository;
        private IStorageService _storageService;
        private StorageEnvironmentConfig _storageEnvironmentConfig;

        public ImageCompressorService(  IImageRepository imageRepository
                                       , IStorageService storageService
                                       , StorageEnvironmentConfig storageEnvironmentConfig)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;
            _storageEnvironmentConfig = storageEnvironmentConfig;
        }

         public async Task CompressImageAsync(string userName, string fileName)
        {
            var originalImage = await _storageService.GetFileAsync(_storageEnvironmentConfig.OriginalImageBucket, fileName);

            if (originalImage == null)
                throw new Exception("Image not found.");

            UploadResult compressedImage = null;
            try
            {
                // Compress the originalImage
                var compressedImageStream = new MemoryStream();
                using (var image = Image.Load(originalImage))
                {
                    var encoder = new JpegEncoder()
                    {
                        Quality = 50 // 壓縮品質設定為 50
                    };

                    image.Save(compressedImageStream, encoder);

                    // Upload the compressed image to the compressed image bucket
                    compressedImage = await _storageService.UploadFileAsync(_storageEnvironmentConfig.ImageBucket, fileName, compressedImageStream);
                    
                    // Get image information from db
                    var imageInfo = await _imageRepository.ReadAsync(userName, fileName);

                    // Update compressedImage data to imageInfo

                    imageInfo.FileName = compressedImage.Name;
                    imageInfo.FileLinkPath = compressedImage.FileLinkPath;
                    imageInfo.Size = compressedImage.Size;
                    imageInfo.Status = true;
                    imageInfo.UpdateTime = DateTime.UtcNow;

                    // Update imageInfo to db
                    _imageRepository.Update(imageInfo);
                    await _imageRepository.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                if (compressedImage != null)
                {
                    await _storageService.DeleteFileAsync(_storageEnvironmentConfig.ImageBucket, compressedImage.Name);
                }
                throw new Exception("Error compress image", ex);
            }
            
        }
    }
}
