using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Model;
using cbk.cloud.gcp.serviceProvider.KMS;

namespace cbk.cloudUploadImage.service.login.Service
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository _accountRepository;

        public LoginService(IAccountRepository loginRepository)
        {
            _accountRepository = loginRepository;
        }

        public async Task<AccountDto> CreateAccount(string name, string password)
        {
            IKmsService kmsService = new GoogleKmsService("affable-cacao-389805", "asia-east1", "cathy-sample-project", "cathy-sample-project-login-usage", "1");
            var encryptedPassword = kmsService.Encrypt(password);
            // 先檢查是否已存在相同名稱的帳號
            var existingAccount = await _accountRepository.GetByName(name);
            if (existingAccount != null)
            {
                return null;
            }

            // 將密碼雜湊化並建立新的帳戶
            var hashedPassword = Convert.ToBase64String(encryptedPassword);
            var account = new Infrastructure.Entity.Account { Name = name, Password = hashedPassword, CreateTime = DateTime.UtcNow };

            // 儲存新帳戶
           // _accountRepository.Add(account);
            //await _accountRepository.SaveChangesAsync();

            return new AccountDto() { Username = name, Password = hashedPassword };
        }

        public string GenerateJwtToken(AccountDto account)
        {
            // 建立 JWT token的邏輯
            //...
            return string.Empty;
        }
    }
}
