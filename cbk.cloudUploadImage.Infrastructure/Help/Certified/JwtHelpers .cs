using cbk.cloudUploadImage.Infrastructure.Help.Certified;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cbk.cloudUploadImage.Infrastructure.Help.Internet
{
    public class JwtHelpers
    {
        private readonly JwtSettingsOptions settings;

        public JwtHelpers(IOptions<JwtSettingsOptions> settings)
        {
            this.settings = settings.Value;
        }

        // JwtSecurityTokenHandler與SecurityTokenDescriptor
        public string GenerateToken2(string userName, int expireMinutes = 30)
        {
            var issuer = settings.Issuer;
            var signKey = settings.SignKey;
            var symmetricKey = Encoding.UTF8.GetBytes(signKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                    new Claim(JwtRegisteredClaimNames.Iss, issuer),
                    new Claim(JwtRegisteredClaimNames.Sub, userName), // User.Identity.Name
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(now.AddMinutes(expireMinutes)).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
                    new Claim(ClaimTypes.Name, userName)
            }),
                Expires = now.AddMinutes(expireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        // 使用 JwtBuilder, 可以客製化Claim
        public string GenerateToken(string userName, int expireMinutes = 30)
        {
            var issuer = settings.Issuer;
            var signKey = settings.SignKey;

            var token = JwtBuilder.Create()
                            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                            .WithSecret(signKey)
                            // 在 RFC 7519 規格中(Section#4)，總共定義了 7 個預設的 Claims，我們應該只用的到兩種！
                            .AddClaim("jti", Guid.NewGuid().ToString()) // JWT ID
                            .AddClaim("iss", issuer)
                            // .AddClaim("nameid", userName) // User.Identity.Name
                            .AddClaim("sub", userName) // User.Identity.Name
                            // .AddClaim("aud", "The Audience") // 由於你的 API 受眾通常沒有區分特別對象，因此通常不太需要設定，也不太需要驗證
                            .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds())
                            .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                            // .AddClaim("roles", new string[] { "Admin", "Users" })
                            .AddClaim(ClaimTypes.Name, userName)
                            .Encode();
            return token;
        }

        /// <summary>
        /// GenerateToken from username and secret
        /// </summary>
        public string GenerateToken(string username, string secretKey)
        {
            // Base64字串轉換為字節數組的秘密金鑰。這個金鑰將被用來對 JWT 進行簽名和驗證。
            var symmetricKey = Convert.FromBase64String(secretKey);
            // 處理創建和驗證JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            // 創建一個令牌
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // 令牌的主體，它包含一個聲明，這個聲明說明用戶的名字是什麼
                Subject = new ClaimsIdentity(new[]{new Claim(ClaimTypes.Name, username)}),
                // 令牌的過期時間
                Expires = now.AddMinutes(30),
                // 令牌的簽名 使用的是對稱金鑰和 HmacSha256 簽名算法。
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            // 用前面創建的 tokenHandler 和 tokenDescriptor 來創建一個 JWT 實例
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            // 將 JWT 轉換為字串，並存儲在 token 中
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }
    }
}
