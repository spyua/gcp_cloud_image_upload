using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace cbk.image.Infrastructure.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly DBContext _context;

        public ImageRepository(DBContext dbContext)
        {
            _context = dbContext;
        }

        public void Create(ImageInformation item)
        {
            _context.ImageInformations.Add(item);
        }

        public async Task<ImageInformation> ReadAsync(string userName, string fileName)
        {
            var imageInformation = await _context.ImageInformations.Where(x => x.AccountName == userName && x.FileName == fileName).ToListAsync();
            if(imageInformation == null || imageInformation.Count==0)
                throw new Exception("Image not found.");

            return imageInformation[0];
        }

        public async Task<List<ImageInformation>> ReadAllAsync(string userName)
        {
            var imageInformation = await _context.ImageInformations.Where(x => x.AccountName == userName).ToListAsync();
            return imageInformation;
        }

        public void Delete(string userName, string fileName)
        {
            var imageInformation = _context.ImageInformations.FirstOrDefault(x => x.AccountName == userName && x.FileName == fileName);
            
            if(imageInformation==null)
                throw new Exception("Image not found.");

            _context.ImageInformations.Remove(imageInformation);
        }
        public void Update(ImageInformation item)
        {
            _context.ImageInformations.Update(item);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
