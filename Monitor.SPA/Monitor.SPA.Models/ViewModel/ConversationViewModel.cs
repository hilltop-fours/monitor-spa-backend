using Newtonsoft.Json;
using Monitor.SPA.Shared.Events.Models.Enums;
using System;

namespace Monitor.SPA.Models.ViewModel
{
    public class ConversationViewModel
    {
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
        /// The last datetime a message was sent, by either the agent or shopper
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdatedDateTime")]
        public DateTime? LastUpdatedDateTime { get; set; }

        /// <summary>
        /// True is agent, False is shopper
        /// </summary>
        [JsonProperty(PropertyName = "senderWasOwner")]
        public bool SenderWasOwner { get; set; }
    }
}
