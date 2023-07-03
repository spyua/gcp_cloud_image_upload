using cbk.cloud.serviceProvider.Storage;
using cbk.image.Infrastructure.Database.Entity;
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

        
        public async Task<ImageInformationDto> UploadImage(string userName, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file selected or the file is empty.");

            var objectName = file.FileName;
            UploadResult uploadedObject = null;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    uploadedObject = await _storageService.UploadFileAsync("cbk_mario_test_project_image_bucket", objectName, memoryStream);


                    var newImageInfo = new ImageInformation
                    {
                        AccountName = userName, // set this to the account name
                        OriginalFileName = file.FileName,
                        FileName = objectName, // this is a guess, update as necessary
                        FileLinkPath = uploadedObject.FileLinkPath,
                        Status = true, // assuming the image is successfully uploaded (Exist)
                        CreateTime = DateTime.UtcNow,
                        UpdateTime = DateTime.UtcNow
                    };

                    _imageRepository.Add(newImageInfo);
                    await _imageRepository.SaveChangesAsync();

                    var dto = new ImageInformationDto
                    {
                        // Fill DTO properties here
                        // Assume ImageInformationDto has similar properties to ImageInformation
                        OriginalFileName = newImageInfo.OriginalFileName,
                        FileName = newImageInfo.FileName,
                        FileLinkPath = newImageInfo.FileLinkPath,
                        CreateTime = newImageInfo.CreateTime,
                        UpdateTime = newImageInfo.UpdateTime
                    };
                    return dto;
                }
            }catch(Exception ex)
            {
                if (uploadedObject != null)
                {
                    await _storageService.DeleteFileAsync("cbk_mario_test_project_image_bucket", uploadedObject.Name);
                }

                throw new Exception("Error uploading image", ex);
            }
           
        }
        

    }
}
