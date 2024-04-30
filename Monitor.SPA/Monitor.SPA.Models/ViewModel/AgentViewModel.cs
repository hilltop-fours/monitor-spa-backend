using Newtonsoft.Json;
using System.Collections.Generic;

namespace Monitor.SPA.Models.ViewModel
{
    public class AgentViewModel
    {
        /// <summary>
        /// The UniqueId
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The first name of an <see cref="AgentViewModel"/>
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of an <see cref="AgentViewModel"/>
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The <see cref="ConversationViewModel"/>s that an <see cref="AgentViewModel"/> is responsible for
        /// </summary>
        [JsonProperty(PropertyName = "conversations")]
        public List<ConversationViewModel> Conversations { get; set; }
    }
}