﻿using cbk.image.service.upload.Dto;

namespace cbk.image.service.upload.Service
{
    public interface IImageService
    {
        Task<ImageInformationDto> UploadImage(IFormFile file);
    }
}
