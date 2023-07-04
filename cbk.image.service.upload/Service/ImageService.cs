using cbk.cloud.serviceProvider.Storage;
using cbk.image.Infrastructure.Config.Storage;
using cbk.image.Infrastructure.Database.Entity;
using cbk.image.Infrastructure.Repository;
using cbk.image.service.upload.Dto;

namespace cbk.image.service.upload.Service
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        private IStorageService _storageService;
        private StorageEnvironmentConfig _storageEnvironmentConfig;
        public ImageService(  IImageRepository imageRepository
                            , IStorageService storageService
                            , StorageEnvironmentConfig storageEnvironmentConfig)
        {
            _imageRepository = imageRepository;
            _storageService = storageService;
            _storageEnvironmentConfig = storageEnvironmentConfig;
        }

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
                    uploadedObject = await _storageService.UploadFileAsync(_storageEnvironmentConfig.OriginalImageBucket, objectName, memoryStream);


                    var newImageInfo = new ImageInformation
                    {
                        AccountName = userName, // set this to the account name
                        OriginalFileName = objectName,
                        FileName = uploadedObject.Name, // this is a guess, update as necessary
                        FileLinkPath = uploadedObject.FileLinkPath,
                        Size = uploadedObject.Size,
                        Status = true, // assuming the image is successfully uploaded (Exist)
                        CreateTime = DateTime.UtcNow,
                        UpdateTime = DateTime.UtcNow
                    };

                    _imageRepository.Create(newImageInfo);
                    await _imageRepository.SaveChangesAsync();

                    var dto = new ImageInformationDto
                    {
                        // Fill DTO properties here
                        // Assume ImageInformationDto has similar properties to ImageInformation
                        FileName = newImageInfo.FileName,
                        FileLinkPath = newImageInfo.FileLinkPath,
                        MediaLink = uploadedObject.MediaLink,
                        Size = uploadedObject.Size,
                        CreateTime = newImageInfo.CreateTime,
                        UpdateTime = newImageInfo.UpdateTime
                    };
                    return dto;
                }
            }catch(Exception ex)
            {
                if (uploadedObject != null)
                {
                    await _storageService.DeleteFileAsync(_storageEnvironmentConfig.OriginalImageBucket, uploadedObject.Name);
                }

                throw new Exception("Error uploading image", ex);
            }
           
        }

        public async Task DeleteImage(string userName, ImageDelete imageDelete)
        {
            var fileName = imageDelete.FileName;
            var imageInformation = await _imageRepository.ReadAsync(userName, fileName);

            if (imageInformation == null)
                throw new Exception("Image not found.");

            await _storageService.DeleteFileAsync(_storageEnvironmentConfig.OriginalImageBucket, imageInformation.FileName);
            _imageRepository.Delete(userName, fileName);
            await _imageRepository.SaveChangesAsync();
        }

        public async Task<List<ImageInformationDto>> ImageInformation(string userName)
        {
            var imageInformation = await _imageRepository.ReadAllAsync(userName);
            var imageInformationDto = new List<ImageInformationDto>();

            foreach (var image in imageInformation)
            {
                var dto = new ImageInformationDto
                {
                    // Fill DTO properties here
                    // Assume ImageInformationDto has similar properties to ImageInformation
                    FileName = image.FileName,
                    FileLinkPath = image.FileLinkPath,
                    MediaLink = image.FileLinkPath,
                    //Size = image.Size,
                    CreateTime = image.CreateTime,
                    UpdateTime = image.UpdateTime
                };
                imageInformationDto.Add(dto);
            }

            return imageInformationDto;
        }
    }
}
