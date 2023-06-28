using Npgsql;

namespace cbk.cloud.gcp.serviceProvider.CloudSQL
{
    public class CloudSQLConnectionStringHelp
    {
        public string GetConnectionString(string instanceName, string databaseName, string userName, string password)
        {
            return $"Server={instanceName};Database={databaseName};Uid={userName};Pwd={password};";
        }

        // Get NpgsqlConnectionStringBuilder
        public string BuildConnectionString(string instanceName, string databaseName, string userName, string password)
        {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Host = instanceName;
            builder.Database = databaseName;
            builder.Username = userName;
            builder.Password = password;
            builder.SslMode = SslMode.VerifyCA;

            builder.RootCertificate = "server-ca.pem";
            builder.SslCertificate = "cert.pem";
            builder.SslPassword = "client-key.pem";

            // Setting certificate pem file path
            builder.TrustServerCertificate = true;
            
            return builder.ConnectionString;
        }
    }
}