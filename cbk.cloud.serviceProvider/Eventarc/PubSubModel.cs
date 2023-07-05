namespace cbk.cloud.serviceProvider.Eventarc
{
    public class PubSubModel
    {
        public class PubSubMessage
        {
            public string Data { get; set; } = string.Empty;
            public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
            public string MessageId { get; set; } = string.Empty;
            public string PublishTime { get; set; } = string.Empty;
        }

        public class PubSubEvent
        {
            public PubSubMessage Message { get; set; } = new PubSubMessage();
            public string Subscription { get; set; } = string.Empty;
        }
    }
}
