using Monitor.SPA.Shared.Events.Models.Dto;
using Monitor.SPA.Shared.Events.Models.Enums;
using Monitor.SPA.Models;
using Monitor.SPA.Models.Exception;
using Monitor.SPA.Models.ViewModel;
using Monitor.SPA.Repository.Interface;
using Monitor.SPA.Services.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.SPA.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IGenericRepository<Agent> _repository;

        public ConversationService(IGenericRepository<Agent> repository) => _repository = repository;

        public async Task<AgentViewModel> CreateConversationAsync(Shared.Events.Models.Conversation data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.InboxChannelType != ChannelType.Instant) return null;

            var agent = await _repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId);

            var conversation = new Conversation
            {
                Id = data.UniqueId.ToString(),
                SubscriptionId = data.SubscriptionId,
                AgentId = data.InboxOwnerPersonId,
                FullName = data.InboxRelationFullName,
                State = data.State,
                AnswerState = data.AnswerState,
                LastUpdatedDateTime = data.LastUpdatedDateTime
            };
            agent.Conversations.Add(conversation);

            return ViewModelService.GetAgentViewModel(await _repository.UpdateAsync(agent));
        }

        public async Task<AgentViewModel> UpdateConversationStateAsync(ConversationStateChanged data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var agent = await _repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId);

            var conversation =
                agent?.Conversations.FirstOrDefault(document => document.Id == data.ConversationId.ToString());
            if (conversation == null) throw new EntityNotFoundException("Conversations not found");
            conversation.State = data.State;

            return ViewModelService.GetAgentViewModel(await _repository.UpdateAsync(agent));
        }

        public async Task<AgentViewModel> UpdateConversationAnswerStateAsync(ConversationAnswerStateChanged data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var agent = await _repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId);

            var conversation =
                agent?.Conversations.FirstOrDefault(document => document.Id == data.ConversationId.ToString());
            if (conversation == null) throw new EntityNotFoundException("Conversations not found");
            conversation.AnswerState = data.AnswerState;

            return ViewModelService.GetAgentViewModel(await _repository.UpdateAsync(agent));
        }

        public async Task<AgentViewModel> UpdateConversationAwaitingResponseAsync(ConversationAwaitingResponseChanged data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var agent = await _repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId);

            var conversation =
                agent?.Conversations.FirstOrDefault(document => document.Id == data.ConversationId.ToString());
            if (conversation == null) throw new EntityNotFoundException("Conversations not found");
            conversation.LastUpdatedDateTime = data.AnswerDateTime;
            conversation.SenderWasOwner = data.SenderWasOwner;

            return ViewModelService.GetAgentViewModel(await _repository.UpdateAsync(agent));
        }
    }
}
