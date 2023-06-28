using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Model;
using cbk.cloudUploadImage.Infrastructure;

namespace cbk.cloudUploadImage.service.login.Service
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository _accountRepository;

        public LoginService(IAccountRepository loginRepository)
        {
            _accountRepository = loginRepository;
        }

        public async Task<Account> CreateAccount(string name, string password)
        {
            // 先檢查是否已存在相同名稱的帳號
            var existingAccount = await _accountRepository.GetByName(name);
            if (existingAccount != null)
            {
                return null;
            }

            // 將密碼雜湊化並建立新的帳戶
            //var hashedPassword = _passwordHasher.HashPassword(password);
            var account = new Infrastructure.Entity.Account { Name = name, Password = "", CreateTime = DateTime.UtcNow };

            // 儲存新帳戶
            _accountRepository.Add(account);
            await _accountRepository.SaveChangesAsync();

            return new Account() { Username = name, Password = password};
        }

        public string GenerateJwtToken(Account account)
        {
            // 建立 JWT token的邏輯
            //...
            return string.Empty;
        }
    }
}
