using cbk.cloudUploadImage.Infrastructure.Database.Entity;

namespace cbk.cloudUploadImage.Infrastructure.Repository
{
    public interface IAccountRepository
    {
        Task<Account?> GetByName(string name);

        void Add(Account account);

        Task SaveChangesAsync();
    }
}
