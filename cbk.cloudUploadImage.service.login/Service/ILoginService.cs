using cbk.cloudUploadImage.service.login.Model;

namespace cbk.cloudUploadImage.service.login.Service
{
    public interface ILoginService
    {
        Task<Account> CreateAccount(string name, string password);

        string GenerateJwtToken(Account account);
    }
}
