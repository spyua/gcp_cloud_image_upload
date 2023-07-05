using cbk.image.Infrastructure.Models;
using cbk.image.service.compress.Dto;
using cbk.image.service.compress.Service;
using Microsoft.AspNetCore.Mvc;

namespace cbk.image.service.compress.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageCompressorController : ControllerBase
    {
        private readonly ILogger<ImageCompressorController> _logger;
        private IImageCompressorService _imageCompressorService;

        public ImageCompressorController(ILogger<ImageCompressorController> logger,
                                         IImageCompressorService imageCompressorService)
        {
            _imageCompressorService = imageCompressorService;
            _logger = logger;
        }

        [HttpGet(nameof(CompressImage))]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> CompressImage([FromQuery] ImageInformationCompress image)
        {
            var compressImageFile = await _imageCompressorService.CompressImageAsync(fileName:image.FileName, fileLinkPath:image.FileLinkPath);

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Compress Image Success",
                Data = compressImageFile
            }); ;
        }

        [HttpGet(nameof(TestRequest))]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> TestRequest([FromQuery] ImageInformationCompress image)
        {

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Test Request Success",
                Data = new ImageInformationDto()
                {
                    FileName = "Comporess Test"
                   ,FileLinkPath = "Compress Test"
                }
            });
               
        }

    }
}
