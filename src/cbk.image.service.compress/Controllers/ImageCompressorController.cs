﻿using cbk.image.service.compress.Dto;
using cbk.image.service.compress.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using cbk.cloud.serviceProvider.Eventarc.EventModel;
using cbk.cloud.serviceProvider.Eventarc.Model;
using cbk.cloud.serviceProvider.Eventarc;
using cbk.image.Application.Models;

namespace cbk.image.service.compress.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageCompressorController : ControllerBase
    {
        private readonly ILogger<ImageCompressorController> _logger;
        private readonly IImageCompressorService _imageCompressorService;

        public ImageCompressorController(ILogger<ImageCompressorController> logger,
                                         IImageCompressorService imageCompressorService)
        {
            _imageCompressorService = imageCompressorService;
            _logger = logger;
        }

        [HttpGet(nameof(CompressImage))]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> CompressImage([FromQuery] ImageInformationCompress image)
        {
            var compressImageFile = await _imageCompressorService.CompressImageAsync(fileName:image.FileName);

            return Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Compress Image Success",
                Data = compressImageFile
            });
        }

        [HttpGet(nameof(TestRequest))]
        public async Task<ActionResult<ApiResponse<ImageInformationDto>>> TestRequest([FromQuery] ImageInformationCompress image)
        {
            return await Task.FromResult(Ok(new ApiResponse<ImageInformationDto>
            {
                Message = "Test Request Success",
                Data = new ImageInformationDto()
                {
                    FileName = "Compress Test",
                    FileLinkPath = "Compress Test"
                }
            }));

        }


        // Eventarc receive
        [HttpPost("/")]
        public async Task<IActionResult> ReceiveEvent()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            _logger.LogInformation($"Received message: {body}");

            // 反序列化為基礎事件
            //var eventModel = JsonSerializer.Deserialize<StorageEvent>(body);
            var baseEvent = JsonSerializer.Deserialize<BaseEvent>(body);

            
            if (baseEvent == null)
                throw new Exception("Received message data error, no 'kind' attributes");

            var factory = new EventarcParseBodyFactory<StorageEvent>();
            var eventModel = factory.CreateEventModel(baseEvent.Kind, body);
            
            var compressImageFile = await _imageCompressorService.CompressImageAsync(fileName: eventModel.Name);

            return Ok($"Hello {eventModel.Kind}! Message ID: {eventModel.Id}");
        }
    }
}
