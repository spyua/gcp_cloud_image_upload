using cbk.cloud.serviceProvider.Eventarc.EventModel;
using System.Text.Json.Serialization;

namespace cbk.cloud.serviceProvider.Eventarc.Model
{
    // 如果JSON名稱沒大小寫問題，可以不用JsonProperty
    public class StorageEvent : BaseEvent
    {
      

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("selfLink")]
        public string SelfLink { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("bucket")]
        public string Bucket { get; set; } = string.Empty;

        [JsonPropertyName("generation")]
        public string Generation { get; set; } = string.Empty;

        [JsonPropertyName("metageneration")]
        public string Metageneration { get; set; } = string.Empty;

        [JsonPropertyName("timeCreated")]
        public string TimeCreated { get; set; } = string.Empty;

        [JsonPropertyName("updated")]
        public string Updated { get; set; } = string.Empty;

        [JsonPropertyName("storageClass")]
        public string StorageClass { get; set; } = string.Empty;

        [JsonPropertyName("timeStorageClassUpdated")]
        public string TimeStorageClassUpdated { get; set; } = string.Empty;

        [JsonPropertyName("size")]
        public string Size { get; set; } = string.Empty;

        [JsonPropertyName("md5Hash")]
        public string Md5Hash { get; set; } = string.Empty;

        [JsonPropertyName("mediaLink")]
        public string MediaLink { get; set; } = string.Empty;

        [JsonPropertyName("crc32c")]
        public string Crc32c { get; set; } = string.Empty;

        [JsonPropertyName("etag")]
        public string Etag { get; set; } = string.Empty;
    }

}
