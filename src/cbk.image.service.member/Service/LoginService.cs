﻿using cbk.image.Infrastructure.Repository;
using cbk.image.service.member.Model;
using cbk.image.Infrastructure.Security.Jwt;
using cbk.cloud.serviceProvider.KMS;
using cbk.cloud.serviceProvider.SecretManager;

namespace cbk.image.service.member.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IKmsService _kmsService;
        public LoginService( ILogger<LoginService> logger
                          , IAccountRepository loginRepository
                          , IJwtService jwtService
                          , IKmsService kmsService)
        {
            _logger = logger;
            _accountRepository = loginRepository;
            _jwtService = jwtService;
            _kmsService = kmsService;
        }

        public async Task<AccountDto> CreateAccount(string userName, string password)
        {
            _logger.LogInformation("Creating account for user: {UserName}", userName);
            var encryptedPassword = _kmsService.Encrypt(password);

            // 先檢查是否已存在相同名稱的帳號
            var existingAccount = await _accountRepository.GetByName(userName);
            if (existingAccount != null)
            {
                _logger.LogWarning("Attempted to create account for existing user: {UserName}", userName);
                throw new Exception("Account already exists.");
            }

            // 將密碼雜湊化並建立新的帳戶
            var hashedPassword = Convert.ToBase64String(encryptedPassword);
            var account = new Infrastructure.Database.Entity.Account { Name = userName, Password = hashedPassword, CreateTime = DateTime.UtcNow };

            // 儲存新帳戶
            _accountRepository.Add(account);
            await _accountRepository.SaveChangesAsync();

            _logger.LogInformation("Account created successfully for user: {UserName}", userName);
            return new AccountDto() { Token = "" ,UserName = userName, Password = "" };
        }

        public async Task<AccountDto> LoginAccount(string userName, string password)
        {
          
            _logger.LogInformation("Attempting to login for user: {UserName}", userName);
            var encryptedPassword = _kmsService.Encrypt(password);

            var existingAccount = await _accountRepository.GetByName(userName);
            if (existingAccount == null)
            {
                _logger.LogWarning("Attempted to login with non-existing user: {UserName}", userName);
                throw new Exception("Account not found.");
            }

            var hashedPassword = Convert.ToBase64String(encryptedPassword);
            
            // 比對Password
            if(existingAccount?.Password != hashedPassword)
            {
                _logger.LogWarning("Incorrect password for user: {UserName}", userName);
                throw new Exception("Password is not correct.");
            }

            // 產生Token
            // Print token log for debug
            _logger.LogInformation("Token Key Value:"+_jwtService.JwtSettings.TokenSecret);
            var token = _jwtService.GenerateToken(userName);
            _logger.LogInformation("Login successful and token generated for user: {UserName}", userName);
            return new AccountDto() { Token = token, UserName = userName, Password = "" };
        }
    }
}