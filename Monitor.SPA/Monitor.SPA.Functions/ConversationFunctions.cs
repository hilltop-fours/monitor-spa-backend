using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Monitor.SPA.Shared.Events.Models.Dto;
using Monitor.SPA.Services.Interface;
using System.Threading.Tasks;
using Conversation = Monitor.SPA.Shared.Events.Models.Conversation;

namespace Monitor.SPA.Functions
{
    public class ConversationFunctions : BaseFunctions
    {
        private readonly IConversationService _service;

        public ConversationFunctions(IConversationService service) => _service = service;

        [FunctionName("Create_Conversation")]
        public async Task CreateConversationAsync(
            [ServiceBusTrigger("conversationcreated", "monitor", Connection = "ServiceBusConnection")] string message,
            [SignalR(HubName = "monitor")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await ServiceBusTriggerExecuteWithinScope<Conversation>(message, async (data) =>
            {
                var agent = await _service.CreateConversationAsync(data);
                await SignalRSendMessage(signalRMessages, "updateAgent", agent, log);
            }, log);
        }

        [FunctionName("Update_Conversation_State")]
        public async Task UpdateConversationStateAsync(
            [ServiceBusTrigger("conversationstatechanged", "monitor", Connection = "ServiceBusConnection")] string message,
            [SignalR(HubName = "monitor")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await ServiceBusTriggerExecuteWithinScope<ConversationStateChanged>(message, async (data) =>
            {
                var agent = await _service.UpdateConversationStateAsync(data);
                await SignalRSendMessage(signalRMessages, "updateAgent", agent, log);
            }, log);
        }

        [FunctionName("Update_Conversation_Answer_State")]
        public async Task UpdateConversationAnswerStateAsync(
            [ServiceBusTrigger("conversationanswerstatechanged", "monitor", Connection = "ServiceBusConnection")] string message,
            [SignalR(HubName = "monitor")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await ServiceBusTriggerExecuteWithinScope<ConversationAnswerStateChanged>(message, async (data) =>
            {
                var agent = await _service.UpdateConversationAnswerStateAsync(data);
                await SignalRSendMessage(signalRMessages, "updateAgent", agent, log);
            }, log);
        }

        [FunctionName("Update_Conversation_Awaiting_Response")]
        public async Task UpdateConversationAwaitingResponseAsync(
            [ServiceBusTrigger("conversationawaitingresponsechanged", "monitor", Connection = "ServiceBusConnection")] string message,
            [SignalR(HubName = "monitor")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await ServiceBusTriggerExecuteWithinScope<ConversationAwaitingResponseChanged>(message, async (data) =>
            {
                var agent = await _service.UpdateConversationAwaitingResponseAsync(data);
                await SignalRSendMessage(signalRMessages, "updateAgent", agent, log);
            }, log);
        }
    }
}