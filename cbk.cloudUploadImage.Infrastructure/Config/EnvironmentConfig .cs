using cbk.cloudUploadImage.Infrastructure.Config.DB;

namespace cbk.cloudUploadImage.Infrastructure.Config
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        public IDbEnvironmentConfig DbConfig { get; private set; }
        public EnvironmentConfig(bool useMock = false)
        {
            DbConfig = new DbEnvironmentConfig(useMock);
        }
    }
}
