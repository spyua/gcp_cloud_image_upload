namespace cbk.image.Domain.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; }
        public DateTime LoginTime { get; set; }

        // 檢查密碼是否包含大小寫字母、數字和特殊字符
        public bool IsValidPassword(string password)
        {
            // Implement your password complexity validation here.
            // This is just a very simple example.
            // Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character
            bool hasLowercase = password.Any(char.IsLower);
            bool hasUppercase = password.Any(char.IsUpper);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.IndexOfAny("!@#$%^&*()".ToCharArray()) != -1;
            bool meetsLengthRequirements = password.Length >= 8;

            return hasLowercase && hasUppercase && hasDigit && hasSpecialChar && meetsLengthRequirements;
        }
        public bool IsActiveAccount()
        {
            TimeSpan span = DateTime.Now - LoginTime;
            return span.TotalDays <= 30;
        }
    }
}