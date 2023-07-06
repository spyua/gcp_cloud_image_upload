using cbk.image.Infrastructure.Database.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public interface IImageRepository : IBaseRepository
    {
        void Create(ImageInformation file);
        Task<ImageInformation> ReadAsync(string userName, string fileName);

        Task<ImageInformation> ReadAsync(string fileName);
        void Update(ImageInformation item);
        void Delete(string userName, string fileName);
        Task<List<ImageInformation>> ReadAllAsync(string userName);
    }
}
