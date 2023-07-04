namespace cbk.image.service.compress.Service
{
    public interface IImageCompressorService
    {
        Task CompressImageAsync(string userName, string fileName);
    }
}
