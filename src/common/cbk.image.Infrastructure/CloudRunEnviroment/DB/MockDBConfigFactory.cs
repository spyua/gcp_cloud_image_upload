﻿using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;

namespace cbk.image.Infrastructure.CloudRunEnviroment.DB
{
    public class MockDBConfigFactory : IDBEnvironmentConfigFactory
    {
        public IDBEnvironmentConfig Create()
        {
            var config = new DBEnvironmentConfig();
            config.InstanceName = "";
            config.DatabaseName = "postgres";
            config.UserName = "cbk_testing";
            config.Password = "cbktesting";
            config.SeverCertificatePath = Path.Combine(config.BaseDirectory, config.CertFolder, "server-ca.pem");
            config.ClientCertificatePath = Path.Combine(config.BaseDirectory, config.CertFolder, "client-cert.pem");
            config.ClientCertificateKeyPath = Path.Combine(config.BaseDirectory, config.CertFolder, "client-key.pem");
            return config;
        }

    }

}
