using cbk.image.Infrastructure.Database.Entity;
using cbk.image.Infrastructure.Models;
using cbk.image.service.upload.Dto;
using cbk.image.service.upload.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

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
            await _imageService.UploadImage(file);

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Upload Image Success",
                Data = new ImageInformationDto()
            });;
        }
    }
}
