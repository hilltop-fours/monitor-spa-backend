using Microsoft.Azure.Cosmos;
using Moq;
using Monitor.SPA.Helpers;
using Monitor.SPA.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Monitor.SPA.Repository.Tests
{
    public class CosmosRepositoryAgentTest
    {
        private readonly CosmosRepository<Agent> _repository;
        private readonly Mock<Container> _container;

        public CosmosRepositoryAgentTest()
        {
            _container = new Mock<Container>();
            _repository = new CosmosRepository<Agent>(_container.Object);
        }

        [Fact]
        public async void CreateAsync_Should_Call_Cosmos_CreateItemAsync()
        {
            var agent = new Agent();

            _container
                .Setup(container => container.CreateItemAsync(
                    It.IsAny<Agent>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<ItemResponse<Agent>>().Object)
                .Verifiable();

            await _repository.CreateAsync(agent);

            _container.Verify();
        }

        [Fact]
        public async void ReadAsync_Should_Call_Cosmos_ReadItemAsync()
        {
            var agent = new Agent();

            _container
                .Setup(container => container.ReadItemAsync<Agent>(
                    It.IsAny<string>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<ItemResponse<Agent>>().Object)
                .Verifiable();

            await _repository.ReadAsync(agent.Id, agent.SubscriptionId);

            _container.Verify();
        }

        [Fact]
        public async void ReadListAsync_Should_Call_Cosmos_GetItemQueryIterator()
        {
            const long subscriptionId = 17;

            var agents = new List<Agent>
            {
                new Agent(),
                new Agent()
            };

            var feedResponse = new Mock<FeedResponse<Agent>>();
            feedResponse
                .Setup(response => response.GetEnumerator())
                .Returns(agents.GetEnumerator());

            var feedIterator = new Mock<FeedIterator<Agent>>();
            feedIterator.Setup(iterator => iterator.HasMoreResults).Returns(true);
            feedIterator
                .Setup(iterator => iterator.ReadNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(feedResponse.Object)
                .Callback(() => feedIterator
                    .Setup(iterator => iterator.HasMoreResults)
                    .Returns(false));

            _container
                .Setup(container => container.GetItemQueryIterator<Agent>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<QueryRequestOptions>()))
                .Returns(feedIterator.Object)
                .Verifiable();

            await _repository.ReadListAsync(subscriptionId);

            _container.Verify();
        }

        [Fact]
        public async Task UpdateAsync_Should_Call_Cosmos_ReplaceItemAsync()
        {
            var agent = new Agent();

            _container
                .Setup(container => container.ReplaceItemAsync(
                    It.IsAny<Agent>(),
                    It.IsAny<string>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<ItemResponse<Agent>>().Object)
                .Verifiable();

            await _repository.UpdateAsync(agent);

            _container.Verify();
        }

        [Fact]
        public async Task DeleteAsync_Should_Call_Cosmos_DeleteItemAsync()
        {
            var agent = new Agent();

            _container
                .Setup(container => container.DeleteItemAsync<Agent>(
                    It.IsAny<string>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<ItemResponse<Agent>>().Object)
                .Verifiable();

            await _repository.DeleteAsync(agent);

            _container.Verify();
        }

        [Fact]
        public async void CreateAsync_Should_Create_Agent()
        {
            var agentId = new Guid("9F5B2450-95A0-4EB9-AC34-672EBF3D1DA8");
            const long subscriptionId = 17;

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };

            var expected = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };

            var itemResponse = new Mock<ItemResponse<Agent>>();

            itemResponse
                .Setup(response => response.Resource)
                .Returns(agent);

            _container
                .Setup(container => container.CreateItemAsync(
                    agent,
                    new PartitionKey(agent.SubscriptionId),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemResponse.Object));

            var actual = await _repository.CreateAsync(agent);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void ReadAsync_Should_Return_Agent()
        {
            var agentId = new Guid("657DD93F-A650-4A25-B396-C207891F46C5");
            const long subscriptionId = 17;

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };

            var expected = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "new",
                Conversations = new List<Conversation>()
            };

            var itemResponse = new Mock<ItemResponse<Agent>>();

            itemResponse
                .Setup(response => response.Resource)
                .Returns(expected);

            _container
                .Setup(container => container.ReadItemAsync<Agent>(
                    agent.Id,
                    new PartitionKey(agent.SubscriptionId),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemResponse.Object));

            var actual = await _repository.ReadAsync(agentId.ToString(), subscriptionId);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void ReadListAsync_Should_Return_List_Agent()
        {
            const long subscriptionId = 17;

            var agents = new List<Agent>
            {
                new Agent
                {
                    Id = new Guid("F0146158-1559-4864-AD10-37FE238B8505").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Alfa",
                    LastName = "Bravo",
                    Conversations = new List<Conversation>()
                },
                new Agent
                {
                    Id = new Guid("EC332D16-877D-433A-9423-B953A11F27E7").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Charlie",
                    LastName = "Delta",
                    Conversations = new List<Conversation>()
                }
        };

            var expected = new List<Agent>
            {
                new Agent
                {
                    Id = new Guid("F0146158-1559-4864-AD10-37FE238B8505").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Alfa",
                    LastName = "Bravo",
                    Conversations = new List<Conversation>()
                },
                new Agent
                {
                    Id = new Guid("EC332D16-877D-433A-9423-B953A11F27E7").ToString(),
                    SubscriptionId = subscriptionId,
                    FirstName = "Charlie",
                    LastName = "Delta",
                    Conversations = new List<Conversation>()
                }
            };

            var feedResponse = new Mock<FeedResponse<Agent>>();
            feedResponse
                .Setup(response => response.GetEnumerator())
                .Returns(agents.GetEnumerator());

            var feedIterator = new Mock<FeedIterator<Agent>>();
            feedIterator.Setup(iterator => iterator.HasMoreResults).Returns(true);
            feedIterator
                .Setup(iterator => iterator.ReadNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(feedResponse.Object)
                .Callback(() => feedIterator
                    .Setup(iterator => iterator.HasMoreResults)
                    .Returns(false));

            _container
                .Setup(container => container.GetItemQueryIterator<Agent>(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<QueryRequestOptions>()))
                .Returns(feedIterator.Object);

            var actual = await _repository.ReadListAsync(subscriptionId);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void UpdateAsync_Should_Update_Agent()
        {
            var agentId = new Guid("220E369A-8828-495F-AB46-B659CAA76F81");
            const long subscriptionId = 17;

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };

            var expected = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "new",
                Conversations = new List<Conversation>()
            };

            var itemResponse = new Mock<ItemResponse<Agent>>();

            itemResponse
                .Setup(response => response.Resource)
                .Returns(expected);

            _container
                .Setup(container => container.ReplaceItemAsync(
                    expected,
                    agent.Id,
                    new PartitionKey(agent.SubscriptionId),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemResponse.Object));

            var actual = await _repository.UpdateAsync(expected);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }
    }
}
