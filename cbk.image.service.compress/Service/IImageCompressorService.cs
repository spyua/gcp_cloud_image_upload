using cbk.image.service.compress.Dto;

namespace cbk.image.service.compress.Service
{
    public interface IImageCompressorService
    {
        Task<ImageInformationDto> CompressImageAsync(string fileName, string fileLinkPath);
    }
}
