using Monitor.SPA.Shared.Events.Models.Dto;
using Monitor.SPA.Models.ViewModel;
using System.Threading.Tasks;
using Conversation = Monitor.SPA.Shared.Events.Models.Conversation;

namespace Monitor.SPA.Services.Interface
{
    public interface IConversationService
    {
        Task<AgentViewModel> CreateConversationAsync(Conversation data);
        Task<AgentViewModel> UpdateConversationStateAsync(ConversationStateChanged data);
        Task<AgentViewModel> UpdateConversationAnswerStateAsync(ConversationAnswerStateChanged data);
        Task<AgentViewModel> UpdateConversationAwaitingResponseAsync(ConversationAwaitingResponseChanged data);
    }
}
