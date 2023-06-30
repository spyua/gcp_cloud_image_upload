namespace cbk.cloudUploadImage.service.login.Model
{
    public class AccountBase
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AccountCreate : AccountBase{}
    public class AccountLogin : AccountBase { }

    public class AccountDto : AccountBase
    {
        public string Token { get; set; } = string.Empty;
    }

}
