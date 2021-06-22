namespace Bot.Builder.Community.Components.Handoff.ServiceNow.Models
{
    public class ServiceNowResponseMessage
    {
        public string clientSessionId { get; set; }
        public string requestId { get; set; }
        public ServiceNowResponseMessageContent message { get; set; }
        public string userId { get; set; }
        public Body[] body { get; set; }
        public bool completed { get; set; }
        public float score { get; set; }
        public bool takeControl { get; set; }
    }
}
