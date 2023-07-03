using cbk.image.Infrastructure.Database.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public interface IImageRepository : IBaseRepository
    {
        void Add(ImageInformation file);
       
    }
}
