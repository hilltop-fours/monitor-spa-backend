using Monitor.SPA.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.SPA.Services.Interface
{
    public interface IAgentService
    {
        Task<IEnumerable<AgentViewModel>> GetAgentsAsync(long subscriptionId);
    }
}
