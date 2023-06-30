namespace cbk.cloudUploadImage.Infrastructure.Security.Jwt
{
    public class TokenDetail
    {
        /// <summary>
        /// JWT Token的資料
        /// </summary>
        /// <value></value>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// JWT唯一識別碼
        /// </summary>
        /// <value></value>
        public string Jti { get; set; } = string.Empty;

        /// <summary>
        /// JWT過期時間
        /// </summary>
        /// <value></value>
        public DateTime Exp { get; set; }
    }

    public interface IJwtService
    {
        string GenerateToken(string username);

        TokenDetail GenerateTokenDetail(string username);
    }
}
