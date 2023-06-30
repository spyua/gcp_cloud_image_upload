using cbk.image.service.member.Model;

namespace cbk.image.service.member.Service
{
    public interface ILoginService
    {
        Task<AccountDto> CreateAccount(string name, string password);
        Task<AccountDto> LoginAccount(string userName, string password);       
    }
}
