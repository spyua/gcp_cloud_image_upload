using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;
using cbk.image.Infrastructure.CloudRunEnviroment.DB;

namespace cbk.image.Infrastructure.CloudRunEnviroment
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        public IDBEnvironmentConfig DbConfig { get; private set; }

        public IEncryptionEnvironmentConfig EncryptionEnvironmentConfig { get; private set; }

        public EnvironmentConfig(IDBEnvironmentConfig dbConfig
                                , IEncryptionEnvironmentConfig encryptionEnvironmentConfig)
        {
            DbConfig = dbConfig;
            EncryptionEnvironmentConfig = encryptionEnvironmentConfig;
        }
    }
}
