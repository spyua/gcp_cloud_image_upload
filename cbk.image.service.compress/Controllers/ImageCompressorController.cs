using cbk.image.Infrastructure.Models;
using cbk.image.service.compress.Dto;
using cbk.image.service.compress.Service;
using Microsoft.AspNetCore.Mvc;
using static cbk.cloud.serviceProvider.Eventarc.PubSubModel;
using System.Text;
using System.Text.Json;

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

        [HttpPost("/")]
        public async Task<IActionResult> ReceiveEvent()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation($"Received message: {body}");

            var pubSubEvent = JsonSerializer.Deserialize<PubSubEvent>(body);
            var pubSubMessage = pubSubEvent.Message;

            if (pubSubMessage == null)
            {
                return BadRequest("Bad request: Invalid Pub/Sub message format");
            }

            var data = pubSubMessage.Data;
            _logger.LogInformation($"Data: {data}");

            // Assuming that the data is base64 encoded.
            var name = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            _logger.LogInformation($"Extracted name: {name}");

            return Ok($"Hello {name}! Message ID: {pubSubMessage.MessageId}");
        }


    }
}
