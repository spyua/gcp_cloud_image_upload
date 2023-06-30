namespace cbk.cloudUploadImage.Infrastructure.Security
{
    public class JwtSettingsOptions
    {
        /// <summary>
        /// JWT 的發行人（issuer）
        /// </summary>
        /// <Remarks>
        /// 在大型或分散式應用中，可能有多個服務可以發行或接受 JWT。在這種情況下，將 JWT 的發行人（issuer）記錄下來，
        /// 並在接收 JWT 的地方進行檢查，可以幫助您確保該令牌是來自您信任的系統或服務。
        /// </Remarks>
        public string Issuer { get; set; } = "";

        /// <summary>
        /// JWT 的接收者（Audience）
        /// </summary>
        public string SignKey { get; set; } = "";
    }
}
