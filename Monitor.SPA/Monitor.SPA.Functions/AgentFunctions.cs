using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Monitor.SPA.Services.Interface;
using System.Threading.Tasks;

namespace Monitor.SPA.Functions
{
    public class AgentFunctions
    {
        private readonly IAgentService _service;

        public AgentFunctions(IAgentService service) => _service = service;

        private const long SubscriptionIdCheck = 0;

        [FunctionName("GetAgentsBySubscriptionId")]
        public async Task<IActionResult> GetAgentsBySubscriptionId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chat/{subscriptionId}")] HttpRequest req,
            long subscriptionId,
            ILogger log)
        {
            if (SubscriptionIdCheck > subscriptionId) return new NotFoundResult();

            var result = await _service.GetAgentsAsync(subscriptionId);

            return new OkObjectResult(result);
        }
    }
}