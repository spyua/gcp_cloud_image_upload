using cbk.image.Infrastructure.Models;
using cbk.image.service.upload.Dto;
using cbk.image.service.upload.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cbk.image.service.upload.Controllers
{
    public class ImageController : ControllerBase
    {

        private readonly ILogger<ImageController> _logger;
        private readonly IImageService _imageService;

        public ImageController(ILogger<ImageController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost(nameof(UploadImage))]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> UploadImage(IFormFile file)
        {
            _logger.LogInformation("Start image upload: {fileName}", file.FileName);

            var imageInformation = await _imageService.UploadImage(GetUserName(), file);

            _logger.LogInformation("Image uploaded successfully: {fileName}", file.FileName);

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Upload Image Success",
                Data = imageInformation
            });;
        }

        [HttpDelete(nameof(DeleteImage))]
       [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> DeleteImage(ImageDelete imageDelete)
        {
            _logger.LogInformation("Start image deletion: {fileName}", imageDelete.FileName);

            await _imageService.DeleteImage(GetUserName(), imageDelete);

            _logger.LogInformation("Image deleted successfully: {fileName}", imageDelete.FileName);

            return Ok(new ApiResponse<object>
            {
                Message = "Delete Image Success",
            }); 
        }

        [HttpGet(nameof(GetAllImages))]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ImageInformationDto>>>> GetAllImages()
        {
            _logger.LogInformation("Start getting all images.");

            var images = await _imageService.ImageInformation(GetUserName());

            _logger.LogInformation("Retrieved all images successfully");

            return Ok(new ApiResponse<List<ImageInformationDto>>
            {
                Message = "Get All Images Success",
                Data = images
            }); 
        }

        private string GetUserName()
        {
            //return "Mario";
            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
            if (userNameClaim == null)
            {
                throw new Exception("User claim not found.");
            }
            return userNameClaim.Value;
        }
    }
}
