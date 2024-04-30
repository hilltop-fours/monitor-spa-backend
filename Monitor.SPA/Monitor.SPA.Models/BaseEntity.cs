using Newtonsoft.Json;
using Monitor.SPA.Models.Interface;

namespace Monitor.SPA.Models
{
    public class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// The UniqueId generated set as the Cosmos id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The web store id
        /// </summary>
        [JsonProperty(PropertyName = "subscriptionId")]
        public long SubscriptionId { get; set; }
    }
}
