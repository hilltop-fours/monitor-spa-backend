using Newtonsoft.Json;
using Monitor.SPA.Shared.Events.Models.Enums;
using System;

namespace Monitor.SPA.Models
{
    public class Conversation : BaseEntity
    {
        /// <summary>
        /// The <see cref="Agent"/> that is handling the <see cref="Conversation"/>.
        /// Originally InboxOwnerPersonId from <see cref="Shared.Events.Models.Conversation"/>
        /// </summary>
        [JsonProperty(PropertyName = "agentId")]
        public Guid AgentId { get; set; }

        /// <summary>
        /// The full name of the shopper
        /// </summary>
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// The <see cref="ConversationState"/> of the <see cref="Conversation"/>
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public ConversationState State { get; set; }

        /// <summary>
        /// The <see cref="Shared.Events.Models.Enums.AnswerState"/> of the <see cref="Conversation"/>
        /// </summary>
        [JsonProperty(PropertyName = "answerState")]
        public AnswerState AnswerState { get; set; }

        /// <summary>
        /// DateTime is in UTC+0
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdatedDateTime")]
        public DateTime? LastUpdatedDateTime { get; set; }

        [JsonProperty(PropertyName = "senderWasOwner")]

        public bool SenderWasOwner { get; set; }
    }
}
