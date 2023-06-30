using cbk.cloudUploadImage.service.login.Model;

namespace cbk.cloudUploadImage.service.login.Service
{
    public interface ILoginService
    {
        Task<AccountDto> CreateAccount(string name, string password);
        Task<AccountDto> LoginAccount(string userName, string password);       
    }
}
