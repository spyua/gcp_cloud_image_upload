using cbk.cloud.serviceProvider.Storage;
using cbk.image.Infrastructure.Repository;
using cbk.image.service.upload.Dto;

namespace cbk.image.service.upload.Service
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        private IStorageService _storageService;
        public ImageService(IImageRepository imageRepository, IStorageService storageService)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;
        }

        // Create image upload service method

        
        public async Task<ImageInformationDto> UploadImage(IFormFile file)
        {
          if (file == null || file.Length == 0)
            throw new Exception("No file selected or the file is empty.");

            var objectName = file.FileName;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                await _storageService.UploadFileAsync("cbk_mario_test_project_image_bucket", objectName, memoryStream);
            }

            var imageInformation = new ImageInformationDto();
            return imageInformation;
        }
        

    }
}
