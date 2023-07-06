using cbk.cloud.serviceProvider.Eventarc.EventModel;
using cbk.cloud.serviceProvider.Eventarc.Model;
using System.Text.Json;

namespace cbk.cloud.serviceProvider.Eventarc
{
    // 待修正...使用上不太理解
    public class EventarcParseBodyFactory<T> where T: BaseEvent, new()
    {
        public T CreateEventModel(string kind, string json)
        {
            if (typeof(T) == typeof(StorageEvent) && kind == "storage#object")
            {
                var result = JsonSerializer.Deserialize<StorageEvent>(json) as T;
                return result ?? throw new InvalidOperationException("Deserialization StorageEvent returned null.");
            }

            throw new InvalidOperationException($"Event Type not setting {kind}");
        }
    }
}
