using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SyncFunction.Options;
using SyncFunction.Workers;
using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Consumers;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;
using SyncFunction.QueryObjects.Mappers;

namespace SyncFunction;

public class SyncOrchestrator
{
    private readonly ILogger<ImportWorker> _logger;
    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderItemMapper _orderItemMapper;
    private readonly IBusControl _busControl;
    private readonly SyncOptions _syncOptions;

    public SyncOrchestrator(
        ILogger<ImportWorker> logger,
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IOrderItemMapper orderItemMapper,
        IBusControl busControl,
        IOptions<SyncOptions> syncOptions)
    {
        _logger = logger;
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _orderItemMapper = orderItemMapper;
        _busControl = busControl;
        _syncOptions = syncOptions.Value;
    }

    [FunctionName("SyncOrchestrator")]
    public async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var outputs = new List<string>();

        // Replace "hello" with the name of your Durable Activity Function.
        outputs.Add(await context.CallActivityAsync<string>("SyncOrchestrator_Hello", "Tokyo"));
        //outputs.Add(await context.CallActivityAsync<string>("SyncOrchestrator_Hello", "Seattle"));
        //outputs.Add(await context.CallActivityAsync<string>("SyncOrchestrator_Hello", "London"));

        // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        return outputs;
    }

    [FunctionName("SyncOrchestrator_Hello")]
    public async Task<string> SayHello([ActivityTrigger] string name, ILogger log)
    {
        _logger.LogInformation($"{nameof(ImportWorker)} start.");

        var sendEndpoint = await _busControl.GetSendEndpoint(_syncOptions.GetEndpointUri());

        var results = await _orderItemRepository.GetOrdersSortedByBrandAndPriceAync(
            _syncOptions.ImportPageSize,
            _completedOrdersQuery);

        while (results.HasNextPage)
        {
            var page = await results.NextPageAsync(_orderItemMapper.Query);
            var messages = page.Select(item => new ImportMessage(item));
            await sendEndpoint.SendBatch<ImportMessage>(
                messages, Pipe.New<SendContext<ImportMessage>>(pipeConfigurator =>
                    pipeConfigurator.UseFilter(new PriorityFilter<ImportMessage>(2))));
        }

        _logger.LogInformation($"{nameof(ImportWorker)} end.");

        //var h = await _busControl.GetSendEndpoint(new Uri("queue:fides"));
        //await h.Send<ImportMessage>(new ImportMessage(null), c => c.SetPriority(2));

        log.LogInformation($"Saying hello to {name}.");
        return $"Hello {name}!";
    }

    [FunctionName("SyncOrchestrator_HttpStart")]
    public async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        // Function input comes from the request content.
        string instanceId = await starter.StartNewAsync("SyncOrchestrator", null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
