using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Monitor.SPA.Functions
{
    public class BaseFunctions
    {
        protected static async Task ServiceBusTriggerExecuteWithinScope<T>(string message, Func<T, Task> action, ILogger log)
        {
            try
            {
                log.LogInformation(message);

                var data = JsonSerializer.Deserialize<T>(message);
                await action(data);
            }
            catch (Exception e) { log.LogError(e, "Something went wrong"); }
        }

        protected static async Task SignalRSendMessage<T>(IAsyncCollector<SignalRMessage> signalRMessages, string target, T argument, ILogger log)
        {
            try
            {
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = target,
                    Arguments = new object[] { argument }
                });
            }
            catch (Exception e) { log.LogError(e, "Something went wrong with SignalR"); }
        }
    }
}