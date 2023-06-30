using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;
using cbk.image.Infrastructure.Config.DB;

namespace cbk.image.Infrastructure.Config
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
