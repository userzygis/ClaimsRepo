using Claims.Domain.Dtos;
using Claims.Persistence;
using Claims.Persistence.Interfaces;
using Claims.Persistence.Repositories;
using Claims.Services.Impl;
using Claims.Services.Interfaces;
using Claims.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using Claims.Domain;

namespace Claims.Api.Configurations
{
    internal static class DependencyInjectionExtensions
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services, AppSettingsDto appSettings)
        {
            services.AddDbContext<AuditContext>(options => options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
            InitializeCosmosClient(services, appSettings).Wait();

            #region repository
            services.AddScoped<IAuditerRepository, AuditerRepository>();

            #endregion

            #region mappers
            services.AddScoped<IClaimsMapper, ClaimsMapper>()
                .AddScoped<ICoversMapper, CoversMapper>();
            #endregion

            #region service
            services.AddScoped<IAuditerService, AuditerService>()
                .AddScoped<IClaimsService, ClaimsService>()
                .AddScoped<ICoversService, CoversService>()
                .AddScoped<IPremiumCalcService, PremiumCalcService>();
            #endregion
        }

        private static async Task InitializeCosmosClient(IServiceCollection services, AppSettingsDto appSettings)
        {
            CosmosClient dbClient = new CosmosClient(appSettings.CosmosDb.Account, appSettings.CosmosDb.Key);
            DatabaseResponse database = await dbClient.CreateDatabaseIfNotExistsAsync(appSettings.CosmosDb.DatabaseName);
            await database.Database.CreateContainerIfNotExistsAsync(appSettings.CosmosDb.ClaimContainerName, Constants.PartitionKeyPath);
            await database.Database.CreateContainerIfNotExistsAsync(appSettings.CosmosDb.CoverContainerName, Constants.PartitionKeyPath);

            var claimContainer = dbClient.GetContainer(appSettings.CosmosDb.DatabaseName, appSettings.CosmosDb.ClaimContainerName);
            var coverContainer = dbClient.GetContainer(appSettings.CosmosDb.DatabaseName, appSettings.CosmosDb.CoverContainerName);

            services.AddScoped<IClaimsRepository>(x =>
            {
                return new ClaimsRepository(claimContainer);
            });
            services.AddScoped<ICoversRepository>(x =>
            {
                return new CoversRepository(coverContainer);
            });
        }
    }
}
