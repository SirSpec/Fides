using SyncFunction.Extensions;
using SyncFunction.Options;
using SyncFunction.QueryObjects.Mappers;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SyncFunction.Startup))]

namespace SyncFunction;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        FunctionsHostBuilderContext context = builder.GetContext();

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddOptions<SyncOptions>()
            .Configure<IConfiguration>((settings, configuration) => configuration.GetSection(nameof(SyncOptions)).Bind(settings));

        var configuration = builder.GetContext().Configuration;
        builder.Services.AddMassTransitEndpoints(configuration);
        builder.Services.AddElasticClient(configuration);

        string connectionString = configuration.GetConnectionString("StoresDbContext");
        builder.Services.AddDbContext<StoresDbContext>(
            options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));


        //builder.Services.AddDbContext<StoresDbContext>(options =>
        //{
        //    //if (builder.HostingEnvironment.IsDevelopment())
        //        options
        //            .EnableDetailedErrors()
        //            .EnableSensitiveDataLogging();

        //    options.UseSqlServer(
        //        configuration.GetConnectionString("StoresDbContext"),
        //        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        //});

        builder.Services.AddScoped<ICompletedOrdersQuery, CompletedOrdersQuery>();
        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IOrderItemMapper, OrderItemMapper>();
    }
}
