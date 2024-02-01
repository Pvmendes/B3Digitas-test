using Library.Application.Services;
using Library.Core.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Library.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace ConsumerWebSite_ConsoleApp
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var ConnectionString = EnvironmentHelper.GetConfig("DATABASE_CONNECTION") ?? "";

            if (string.IsNullOrEmpty(ConnectionString))
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true)
                    .AddEnvironmentVariables();
                var configurationRoot = builder.Build();

                ConnectionString = configurationRoot.GetConnectionString("DefaultConnectionMongoDB");
            }

            services.AddSingleton<MongoDbContext>(sp => new MongoDbContext(ConnectionString, "B3Digitas"))                
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScoped<ICryptoCurrencyRepository, CryptoCurrencyRepository>()
                .AddScoped<ICryptoCurrencyService, CryptoCurrencyService>()
                .AddSingleton<BitstampWebSocketService>();
        }
    }
}
