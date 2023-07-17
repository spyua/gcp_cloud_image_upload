using cbk.cloud.serviceProvider.Storage;
using cbk.image.Infrastructure.CloudRunEnviroment.IAM;
using cbk.image.Infrastructure.CloudRunEnviroment.Storage;
using cbk.image.Infrastructure.Database.Entity;
using cbk.image.Infrastructure.Repository;
using cbk.image.service.upload.Dto;

namespace cbk.image.service.upload.Service
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger; 
        private IImageRepository _imageRepository;
        private IStorageService _storageService;
        private StorageEnvironmentConfig _storageEnvironmentConfig;
        private AccountServiceCredentialConfig _accountServiceCredentialConfig;
        public ImageService(  ILogger<ImageService> logger
                            , IImageRepository imageRepository
                            , IStorageService storageService
                            , StorageEnvironmentConfig storageEnvironmentConfig
                            , AccountServiceCredentialConfig accountServiceCredentialConfig)
        {
            _logger = logger;
            _imageRepository = imageRepository;
            _storageService = storageService;
            _storageEnvironmentConfig = storageEnvironmentConfig;
            _accountServiceCredentialConfig = accountServiceCredentialConfig;
        }

        public async Task<ImageInformationDto> UploadImage(string userName, IFormFile file)
        {
            _logger.LogInformation("Start image upload for user {userName}: {fileName}", userName, file.FileName);

            if (file == null || file.Length == 0)
                throw new Exception("No file selected or the file is empty.");

            UploadResult uploadedObject = null;
            try
            {

                using (var memoryStream = new MemoryStream())
                {
                    var newFIleName = userName + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    uploadedObject = await _storageService.UploadFileAsync(_storageEnvironmentConfig.OriginalImageBucket, newFIleName, "image/jpeg", memoryStream);
                    var newImageInfo = new ImageInformation
                    {
                        AccountName = userName, // set this to the account name
                        OriginalFileName = file.FileName,
                        FileName = newFIleName,
                        FileLinkPath = uploadedObject.FileLinkPath,
                        Size = uploadedObject.Size,
                        Status = true, // assuming the image is successfully uploaded (Exist)
                        MediaLink = uploadedObject.MediaLink,
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
            _logger.LogInformation("Start image deletion for user {userName}: {fileName}", userName, imageDelete.FileName);

            var fileName = imageDelete.FileName;

            var imageInformation = await _imageRepository.ReadAsync(userName, imageDelete.FileName);

            if (imageInformation == null)
                throw new Exception("Image not found.");

            await _storageService.DeleteFileAsync(_storageEnvironmentConfig.ImageBucket, imageInformation.FileName);
            _imageRepository.Delete(userName, fileName);
            await _imageRepository.SaveChangesAsync();
        }

        public async Task<List<ImageInformationDto>> ImageInformation(string userName)
        {
            _logger.LogInformation("Start getting all images for user {userName}", userName);

            var imageInformation = await _imageRepository.ReadAllAsync(userName);
            var imageInformationDto = new List<ImageInformationDto>();

            foreach (var image in imageInformation)
            {
                var mediaLink = await _storageService.GenerateSignedUrl(_accountServiceCredentialConfig.CredentialFilePath
                                                                         , _storageEnvironmentConfig.ImageBucket
                                                                         , image.FileName);

                var dto = new ImageInformationDto
                {
                    // Fill DTO properties here
                    // Assume ImageInformationDto has similar properties to ImageInformation
                    FileName = image.FileName,
                    OriginalFileName = image.OriginalFileName,
                    FileLinkPath = image.FileLinkPath,
                    MediaLink = mediaLink,
                    Size = image.Size,
                    CreateTime = image.CreateTime,
                    UpdateTime = image.UpdateTime
                };
                imageInformationDto.Add(dto);
            }

            return imageInformationDto;
        }
    }
}
