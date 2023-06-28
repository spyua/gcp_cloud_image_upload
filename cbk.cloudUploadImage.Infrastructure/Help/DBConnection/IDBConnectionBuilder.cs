using cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model;

namespace cbk.cloudUploadImage.Infrastructure.Help.DBConnection
{
    public interface IDBConnectionBuilder
    {
        string BuildConnectionString(IDBConnectionModel connectionSetting, bool sslConnect = false);
    }
}
