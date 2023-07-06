using Newtonsoft.Json;

namespace cbk.cloud.serviceProvider.Eventarc.EventModel
{
    public class BaseEvent
    {
        [JsonProperty("kind")]
        public string Kind { get; set; } = string.Empty;
    }
}
