using cbk.image.Infrastructure.Database.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public interface IImageRepository : IBaseRepository
    {
        void Create(ImageInformation file);
        ImageInformation? Read(string userName, string fileName);
        void Delete(string userName, string fileName);
        Task<List<ImageInformation>> ReadAllAsync(string userName);
    }
}
