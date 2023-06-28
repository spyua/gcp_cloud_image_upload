using cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model;
using Npgsql;

namespace cbk.cloudUploadImage.Infrastructure.Help.DBConnection
{
    public class NpgsqlConnectionBuilder<T> : IDBConnectionBuilder where T : IDBConnectionModel
    {

        public string BuildConnectionString(IDBConnectionModel connectionSetting, bool sslConnect = false)
        {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Host = connectionSetting.InstanceName;
            builder.Database = connectionSetting.DatabaseName;
            builder.Username = connectionSetting.UserName;
            builder.Password = connectionSetting.Password;

            if (sslConnect)
            {
                // Setting certificate pem file path
                builder.TrustServerCertificate = true;
                builder.SslMode = SslMode.VerifyCA;
                builder.RootCertificate = connectionSetting.SeverCertificatePath;
                builder.SslCertificate = connectionSetting.ClientCertificatePath;
                builder.SslPassword = connectionSetting.ClientCertificateKeyPath;
            }

            return builder.ConnectionString;
        }
    }
}
