using cbk.cloudUploadImage.Infrastructure.Database.DBConnection.Model;

namespace cbk.cloudUploadImage.Infrastructure.Database.DBConnection
{
    public interface IDBConnectionBuilder
    {
        string BuildConnectionString(IDBConnectionModel connectionSetting, bool sslConnect = false);
    }
}
