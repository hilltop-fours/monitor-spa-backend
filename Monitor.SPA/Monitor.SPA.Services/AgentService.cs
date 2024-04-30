using Monitor.SPA.Models;
using Monitor.SPA.Models.ViewModel;
using Monitor.SPA.Repository.Interface;
using Monitor.SPA.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.SPA.Services
{
    public class AgentService : IAgentService
    {
        private readonly IGenericRepository<Agent> _repository;

        public AgentService(IGenericRepository<Agent> repository) => _repository = repository;

        public async Task<IEnumerable<AgentViewModel>> GetAgentsAsync(long subscriptionId)
        {
            var agents = await _repository.ReadListAsync(subscriptionId);

            return ViewModelService.GetAgentViewModels(agents);
        }
    }
}
