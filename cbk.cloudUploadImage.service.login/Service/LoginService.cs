using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Model;
using cbk.cloud.gcp.serviceProvider.KMS;
using cbk.cloudUploadImage.Infrastructure.Help.Internet;

namespace cbk.cloudUploadImage.service.login.Service
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly JwtHelpers _jwt;

        public LoginService(IAccountRepository loginRepository, JwtHelpers jwt)
        {
            _jwt = jwt;
            _accountRepository = loginRepository;
        }

        public async Task<AccountDto?> CreateAccount(string userName, string password)
        {
            // Wati Clean
            IKmsService kmsService = new GoogleKmsService("affable-cacao-389805", "asia-east1", "cathy-sample-project", "cathy-sample-project-login-usage", "1");
            var encryptedPassword = kmsService.Encrypt(password);
            
            // 先檢查是否已存在相同名稱的帳號
            var existingAccount = await _accountRepository.GetByName(userName);
            if (existingAccount != null)
            {
                return null;
            }

            // 將密碼雜湊化並建立新的帳戶
            var hashedPassword = Convert.ToBase64String(encryptedPassword);
            var account = new Infrastructure.Entity.Account { Name = userName, Password = hashedPassword, CreateTime = DateTime.UtcNow };

            // 儲存新帳戶
            // _accountRepository.Add(account);
            //await _accountRepository.SaveChangesAsync();

            return new AccountDto() { Token = "" ,UserName = userName, Password = hashedPassword };
        }

        public async Task<AccountDto> LoginAccount(string userName, string password)
        {
            IKmsService kmsService = new GoogleKmsService("affable-cacao-389805", "asia-east1", "cathy-sample-project", "cathy-sample-project-login-usage", "1");
            var encryptedPassword = kmsService.Encrypt(password);

            var existingAccount = await _accountRepository.GetByName(userName);
            if (existingAccount != null)
            {
                return null;
            }

            var hashedPassword = Convert.ToBase64String(encryptedPassword);
            
            // 比對Password
            if(existingAccount?.Password != hashedPassword)
            {
                return null;
            }

            // 產生Token
            var token = _jwt.GenerateToken(userName);
            return new AccountDto() { Token = token, UserName = userName, Password = hashedPassword };
        }

        public string GenerateJwtToken(AccountDto account)
        {
            // 建立 JWT token的邏輯
            //...
            return string.Empty;
        }
    }
}
