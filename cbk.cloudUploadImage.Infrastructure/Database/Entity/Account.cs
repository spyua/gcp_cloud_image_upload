namespace cbk.cloudUploadImage.Infrastructure.Database.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
