using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cbk.image.Infrastructure.Security.Jwt
{
    public class JwtService : IJwtService
    {
        public JwtSettings JwtSettings { get; private set; }

        public JwtService(JwtSettings jwtSettings)
        {
            JwtSettings = jwtSettings;
        }

        public virtual string GenerateToken(string username)
        {
            var tokenDescription = GetSecurityToken(username).Item2;

            // 創建Token處理者
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }

        public virtual TokenDetail GenerateTokenDetail(string username)
        {
            var securityToken = GetSecurityToken(username);
            var jti = securityToken.Item1;
            var tokenDescription = securityToken.Item2;

            // 創建Token處理者
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return new TokenDetail
            {
                Value = tokenHandler.WriteToken(token),
                Jti = jti,
                Exp = tokenDescription.Expires!.Value
            };
        }

        private (string, SecurityTokenDescriptor) GetSecurityToken(string username)
        {
            var jti = Guid.NewGuid().ToString();
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtSettings.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(JwtSettings.ExpiredDay),
                SigningCredentials = creds
            };
            return (jti, tokenDescriptor);
        }
    }
}
