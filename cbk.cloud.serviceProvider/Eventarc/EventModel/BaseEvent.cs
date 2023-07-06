using System.Text.Json.Serialization;

namespace cbk.cloud.serviceProvider.Eventarc.EventModel
{
    public class BaseEvent
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;
    }
}
