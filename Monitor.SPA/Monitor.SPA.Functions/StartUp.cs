using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monitor.SPA.Functions;
using Monitor.SPA.Models;
using Monitor.SPA.Repository;
using Monitor.SPA.Repository.Interface;
using Monitor.SPA.Services;
using Monitor.SPA.Services.Interface;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Monitor.SPA.Functions
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureSettings(builder);

            builder.Services
                .AddSingleton<IGenericRepository<Agent>>(InitializeCosmosClientInstance());

            builder.Services
                .AddScoped<IAgentService, AgentService>()
                .AddScoped<IConversationService, ConversationService>();
        }

        private void ConfigureSettings(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
            _configuration = config;
        }

        private CosmosRepository<Agent> InitializeCosmosClientInstance()
        {
            var databaseName = _configuration["CosmosDatabase"];
            var containerName = _configuration["CosmosContainer"];
            var connectionString = _configuration["CosmosConnection"];
            var client = new CosmosClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);
            return new CosmosRepository<Agent>(container);
        }
    }
}