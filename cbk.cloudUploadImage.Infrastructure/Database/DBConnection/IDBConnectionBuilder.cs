using cbk.image.Infrastructure.Database.DBConnection.Model;

namespace cbk.image.Infrastructure.Database.DBConnection
{
    public interface IDBConnectionBuilder
    {
        string BuildConnectionString(IDBConnectionModel connectionSetting, bool sslConnect = false);
    }
}
