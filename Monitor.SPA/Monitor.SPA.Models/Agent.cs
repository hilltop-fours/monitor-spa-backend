using Newtonsoft.Json;
using System.Collections.Generic;


namespace Monitor.SPA.Models
{
    public class Agent : BaseEntity
    {
        /// <summary>
        /// The first name of an <see cref="Agent"/>
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of an <see cref="Agent"/>
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The <see cref="Conversation"/>s that an <see cref="Agent"/> is responsible for
        /// </summary>
        [JsonProperty(PropertyName = "conversations")]
        public List<Conversation> Conversations { get; set; }
    }
}
