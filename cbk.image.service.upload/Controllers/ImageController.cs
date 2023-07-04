using cbk.image.Infrastructure.Models;
using cbk.image.service.upload.Dto;
using cbk.image.service.upload.Service;
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
        //[Authorize]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> UploadImage(IFormFile file)
        {
            var imageInformation = await _imageService.UploadImage("Mario", file);

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Upload Image Success",
                Data = imageInformation
            });;
        }

        [HttpDelete(nameof(DeleteImage))]
        public async Task<ActionResult<ApiResponse<object>>> DeleteImage(ImageDelete imageDelete)
        {
            await _imageService.DeleteImage("Mario", imageDelete);

            return Ok(new ApiResponse<object>
            {
                Message = "Delete Image Success",
            }); 
        }

        [HttpGet(nameof(GetAllImages))]
        public async Task<ActionResult<ApiResponse<List<ImageInformationDto>>>> GetAllImages()
        {
            var images = await _imageService.ImageInformation("Mario");

            return Ok(new ApiResponse<List<ImageInformationDto>>
            {
                Message = "Get All Images Success",
                Data = images
            }); 
        }

        private string GetUserName()
        {
            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid");
            if (userNameClaim == null)
            {
                throw new Exception("User claim not found.");
            }
            return userNameClaim.Value;
        }
    }
}
