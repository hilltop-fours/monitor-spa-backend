using Monitor.SPA.Models;
using Monitor.SPA.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Monitor.SPA.Services
{
    public class ViewModelService
    {
        public static AgentViewModel GetAgentViewModel(Agent agent)
        {
            return new AgentViewModel
            {
                Id = agent.Id,
                FirstName = agent.FirstName,
                LastName = agent.LastName,
                Conversations = agent.Conversations
                    .Aggregate(new List<ConversationViewModel>(), (list, conversation) =>
                    {
                        list.Add(new ConversationViewModel
                        {
                            FullName = conversation.FullName,
                            State = conversation.State,
                            AnswerState = conversation.AnswerState,
                            LastUpdatedDateTime = conversation.LastUpdatedDateTime,
                            SenderWasOwner = conversation.SenderWasOwner
                        });
                        return list;
                    })
            };
        }

        public static IEnumerable<AgentViewModel> GetAgentViewModels(IEnumerable<Agent> agents)
        {
            return agents.Select(GetAgentViewModel);
        }
    }
}
