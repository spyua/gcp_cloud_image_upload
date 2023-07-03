using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.Database.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly DBContext _context;

        public ImageRepository(DBContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(ImageInformation item)
        {
            _context.ImageInformations.Add(item);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
