using Moq;
using Monitor.SPA.Helpers;
using Monitor.SPA.Models;
using Monitor.SPA.Models.ViewModel;
using Monitor.SPA.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Monitor.SPA.Services.Tests
{
    public class AgentServiceTest
    {
        private readonly AgentService _service;
        private readonly Mock<IGenericRepository<Agent>> _repository;

        public AgentServiceTest()
        {
            _repository = new Mock<IGenericRepository<Agent>>();
            _service = new AgentService(_repository.Object);
        }

        [Fact]
        public async void GetAgentsAsync_Should_Call_Repository_ReadListAsync()
        {
            const long subscriptionId = 17;

            _repository
                .Setup(repository => repository.ReadListAsync(It.IsAny<long>()))
                .ReturnsAsync(new Mock<List<Agent>>().Object)
                .Verifiable();

            await _service.GetAgentsAsync(subscriptionId);

            _repository.Verify();
        }

        [Fact]
        public async void GetAgentsAsync_Should_Return_List_AgentViewModel()
        {
            const long subscriptionId = 17;

            var agentList = new List<Agent>
            {
                new Agent
                {
                    Id = new Guid("511262a4-8d61-491a-81a7-11a6bd8775f2").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Alfa",
                    LastName = "Bravo",
                    Conversations = new List<Conversation>()
                },
                new Agent
                {
                    Id = new Guid("9f5d25d9-7096-4a55-b27b-81eb1987bbf9").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Charlie",
                    LastName = "Delta",
                    Conversations = new List<Conversation>()
                }
            };

            var expected = new List<AgentViewModel>
            {
                new AgentViewModel
                {
                    Id = "511262a4-8d61-491a-81a7-11a6bd8775f2",
                    FirstName = "Alfa",
                    LastName = "Bravo",
                    Conversations = new List<ConversationViewModel>()
                },
                new AgentViewModel
                {
                    Id = "9f5d25d9-7096-4a55-b27b-81eb1987bbf9",
                    FirstName = "Charlie",
                    LastName = "Delta",
                    Conversations = new List<ConversationViewModel>()
                }
            };

            _repository
                .Setup(repository => repository.ReadListAsync(subscriptionId))
                .Returns(Task.FromResult<IEnumerable<Agent>>(agentList));

            var actual = await _service.GetAgentsAsync(subscriptionId);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }
    }
}
