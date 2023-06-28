using cbk.cloudUploadImage.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace cbk.cloudUploadImage.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DBContext _context;

        public AccountRepository(DBContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(Account item)
        {
            _context.Accounts.Add(item);  
        }

        public async Task<Account?> GetByName(string name)
        {
            return await _context.Accounts.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
