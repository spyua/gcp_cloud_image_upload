using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig
{
    public interface IEnvironmentConfig
    {
        IDbEnvironmentConfig DbConfig { get; }
    }
}
