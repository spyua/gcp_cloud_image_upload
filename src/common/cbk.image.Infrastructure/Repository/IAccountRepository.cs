using cbk.image.Infrastructure.Database.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public interface IAccountRepository : IBaseRepository
    {
        Task<Account?> GetByName(string name);

        void Add(Account account);
    }
}
