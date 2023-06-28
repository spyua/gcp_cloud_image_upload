using cbk.cloudUploadImage.Infrastructure.Config.DB;

namespace cbk.cloudUploadImage.Infrastructure.Config
{
    public interface IEnvironmentConfig
    {
        IDbEnvironmentConfig DbConfig { get; }
    }
}
