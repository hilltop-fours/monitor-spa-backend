using Moq;
using Monitor.SPA.Shared.Events.Models.Dto;
using Monitor.SPA.Shared.Events.Models.Enums;
using Monitor.SPA.Helpers;
using Monitor.SPA.Models;
using Monitor.SPA.Models.ViewModel;
using Monitor.SPA.Repository.Interface;
using System;
using System.Collections.Generic;
using Xunit;

namespace Monitor.SPA.Services.Tests
{
    public class ConversationServiceTest
    {
        private readonly ConversationService _service;
        private readonly Mock<IGenericRepository<Agent>> _repository;

        public ConversationServiceTest()
        {
            _repository = new Mock<IGenericRepository<Agent>>();
            _service = new ConversationService(_repository.Object);
        }

        [Fact]
        public async void CreateConversationAsync_Should_Call_Repository_ReadAsync_And_UpdateAsync()
        {
            var data = new Shared.Events.Models.Conversation
            {
                InboxChannelType = ChannelType.Instant
            };
            var agent = new Agent { Conversations = new List<Conversation>() };

            _repository
                .Setup(repository => repository.ReadAsync(
                    It.IsAny<string>(),
                    It.IsAny<long>()))
                .ReturnsAsync(agent)
                .Verifiable();

            _repository
                .Setup(repository => repository.UpdateAsync(It.IsAny<Agent>()))
                .ReturnsAsync(agent)
                .Verifiable();

            await _service.CreateConversationAsync(data);

            _repository.Verify();
        }

        [Fact]
        public async void UpdateConversationStateAsync_Should_Call_Repository_ReadAsync_And_UpdateAsync()
        {
            var guid = new Guid("7DFFC949-07C8-46C4-B24F-ABAD4155B675");
            var data = new ConversationStateChanged { ConversationId = guid };
            var agent = new Agent { Conversations = new List<Conversation>() };
            agent.Conversations.Add(new Conversation() { Id = guid.ToString() });

            _repository
                .Setup(repository => repository.ReadAsync(
                    It.IsAny<string>(),
                    It.IsAny<long>()))
                .ReturnsAsync(agent)
                .Verifiable();

            _repository
                .Setup(repository => repository.UpdateAsync(It.IsAny<Agent>()))
                .ReturnsAsync(agent)
                .Verifiable();

            await _service.UpdateConversationStateAsync(data);

            _repository.Verify();
        }

        [Fact]
        public async void UpdateConversationAnswerStateAsync_Should_Call_Repository_ReadAsync_And_UpdateAsync()
        {
            var guid = new Guid("8B810B52-C205-4EB6-81E0-7F3AFBD84A9F");
            var data = new ConversationStateChanged { ConversationId = guid };
            var agent = new Agent { Conversations = new List<Conversation>() };
            agent.Conversations.Add(new Conversation() { Id = guid.ToString() });

            _repository
                .Setup(repository => repository.ReadAsync(
                    It.IsAny<string>(),
                    It.IsAny<long>()))
                .ReturnsAsync(agent)
                .Verifiable();

            _repository
                .Setup(repository => repository.UpdateAsync(It.IsAny<Agent>()))
                .ReturnsAsync(agent)
                .Verifiable();

            await _service.UpdateConversationStateAsync(data);

            _repository.Verify();
        }

        [Fact]
        public async void UpdateConversationAwaitingResponseAsync_Should_Call_Repository_ReadAsync_And_UpdateAsync()
        {
            var guid = new Guid("3CA1211F-8847-4901-BDC0-010C6498B1B4");
            var data = new ConversationAwaitingResponseChanged { ConversationId = guid };
            var agent = new Agent { Conversations = new List<Conversation>() };
            agent.Conversations.Add(new Conversation() { Id = guid.ToString() });

            _repository
                .Setup(repository => repository.ReadAsync(
                    It.IsAny<string>(),
                    It.IsAny<long>()))
                .ReturnsAsync(agent)
                .Verifiable();

            _repository
                .Setup(repository => repository.UpdateAsync(It.IsAny<Agent>()))
                .ReturnsAsync(agent)
                .Verifiable();

            await _service.UpdateConversationAwaitingResponseAsync(data);

            _repository.Verify();
        }

        [Fact]
        public async void CreateConversationAsync_Should_Add_Conversation_To_Agent()
        {
            var agentId = new Guid("92286714-7376-42B3-A615-06232A5BBC26");
            var conversationId = new Guid("9C76CBAC-D02A-40A4-9281-3426789D9B20");
            const long subscriptionId = 17;
            var now = DateTime.Now;

            var data = new Shared.Events.Models.Conversation
            {
                UniqueId = conversationId,
                SubscriptionId = subscriptionId,
                InboxOwnerPersonId = agentId,
                InboxRelationFullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now,
                InboxChannelType = ChannelType.Instant
            };

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };

            var updatedAgent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            updatedAgent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            var expected = new AgentViewModel
            {
                Id = agentId.ToString(),
                FirstName = "first",
                LastName = "last",
                Conversations = new List<ConversationViewModel>()
            };
            expected.Conversations.Add(new ConversationViewModel
            {
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            _repository
                .Setup(repository => repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId))
                .ReturnsAsync(agent);

            _repository
                .Setup(repository => repository.UpdateAsync(agent))
                .ReturnsAsync(updatedAgent);

            var actual = await _service.CreateConversationAsync(data);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void UpdateConversationStateAsync_Should_Update_Conversation_State()
        {
            var agentId = new Guid("6D29582E-30D9-4A9D-A7F0-9373EFDFFAC3");
            var conversationId = new Guid("C721C666-D83A-4B8A-9AA9-573C2CA9EC8C");
            const long subscriptionId = 17;
            var now = DateTime.Now;

            var data = new ConversationStateChanged
            {
                SubscriptionId = subscriptionId,
                InboxOwnerPersonId = agentId,
                ConversationId = conversationId,
                State = ConversationState.Archived
            };

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            agent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            var updatedAgent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            updatedAgent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Archived,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            var expected = new AgentViewModel
            {
                Id = agentId.ToString(),
                FirstName = "first",
                LastName = "last",
                Conversations = new List<ConversationViewModel>()
            };
            expected.Conversations.Add(new ConversationViewModel
            {
                FullName = "full name",
                State = ConversationState.Archived,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            _repository
                .Setup(repository => repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId))
                .ReturnsAsync(agent);

            _repository
                .Setup(repository => repository.UpdateAsync(agent))
                .ReturnsAsync(updatedAgent);

            var actual = await _service.UpdateConversationStateAsync(data);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void UpdateConversationAnswerStateAsync_Should_Update_Conversation_AnswerState()
        {
            var agentId = new Guid("6D29582E-30D9-4A9D-A7F0-9373EFDFFAC3");
            var conversationId = new Guid("C721C666-D83A-4B8A-9AA9-573C2CA9EC8C");
            const long subscriptionId = 17;
            var now = DateTime.Now;

            var data = new ConversationAnswerStateChanged
            {
                SubscriptionId = subscriptionId,
                InboxOwnerPersonId = agentId,
                ConversationId = conversationId,
                AnswerState = AnswerState.OnTime
            };

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            agent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now
            });

            var updatedAgent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            updatedAgent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.OnTime,
                LastUpdatedDateTime = now
            });

            var expected = new AgentViewModel
            {
                Id = agentId.ToString(),
                FirstName = "first",
                LastName = "last",
                Conversations = new List<ConversationViewModel>()
            };
            expected.Conversations.Add(new ConversationViewModel
            {
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.OnTime,
                LastUpdatedDateTime = now
            });

            _repository
                .Setup(repository => repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId))
                .ReturnsAsync(agent);

            _repository
                .Setup(repository => repository.UpdateAsync(agent))
                .ReturnsAsync(updatedAgent);

            var actual = await _service.UpdateConversationAnswerStateAsync(data);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }

        [Fact]
        public async void UpdateConversationAwaitingResponseAsync_Should_Update_Conversation_LastUpdatedDateTime_And_SenderWasOwner()
        {
            var agentId = new Guid("A176B4B7-2E8A-48E2-B359-CE3A7B8D2415");
            var conversationId = new Guid("FD2E4EEB-A28B-4EE6-B3E4-4E89AB330960");
            const long subscriptionId = 17;
            var now = DateTime.Now;

            var data = new ConversationAwaitingResponseChanged
            {
                SubscriptionId = subscriptionId,
                InboxOwnerPersonId = agentId,
                ConversationId = conversationId,
                AnswerDateTime = now.AddMinutes(1),
                SenderWasOwner = false // this message was send by a shopper
            };

            var agent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            agent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.Unknown,
                LastUpdatedDateTime = now,
                SenderWasOwner = true
            });

            var updatedAgent = new Agent
            {
                Id = agentId.ToString(),
                SubscriptionId = subscriptionId,
                FirstName = "first",
                LastName = "last",
                Conversations = new List<Conversation>()
            };
            updatedAgent.Conversations.Add(new Conversation
            {
                Id = conversationId.ToString(),
                SubscriptionId = subscriptionId,
                AgentId = agentId,
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.OnTime,
                LastUpdatedDateTime = now.AddMinutes(1),
                SenderWasOwner = false
            });

            var expected = new AgentViewModel
            {
                Id = agentId.ToString(),
                FirstName = "first",
                LastName = "last",
                Conversations = new List<ConversationViewModel>()
            };
            expected.Conversations.Add(new ConversationViewModel
            {
                FullName = "full name",
                State = ConversationState.Open,
                AnswerState = AnswerState.OnTime,
                LastUpdatedDateTime = now.AddMinutes(1),
                SenderWasOwner = false
            });

            _repository
                .Setup(repository => repository.ReadAsync(data.InboxOwnerPersonId.ToString(), data.SubscriptionId))
                .ReturnsAsync(agent);

            _repository
                .Setup(repository => repository.UpdateAsync(agent))
                .ReturnsAsync(updatedAgent);

            var actual = await _service.UpdateConversationAwaitingResponseAsync(data);

            Assert.Equal(Serializer.Serialize(expected), Serializer.Serialize(actual));
        }
    }
}
