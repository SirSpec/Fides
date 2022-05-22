using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Consumers;
using SyncFunction.Extensions;
using SyncFunction.Options;
using SyncFunction.QueryObjects.Mappers;
using SyncFunction.QueryObjects.Queries;
using SyncFunction.Repositories;

namespace SyncFunction;

public class ImportFunction
{
    public const string Name = $"{nameof(SyncOrchestrator)}_{nameof(ImportFunction)}";

    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderItemMapper _orderItemMapper;
    private readonly IBusControl _busControl;
    private readonly SyncOptions _syncOptions;

    public ImportFunction(
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IOrderItemMapper orderItemMapper,
        IBusControl busControl,
        IOptions<SyncOptions> syncOptions)
    {
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _orderItemMapper = orderItemMapper;
        _busControl = busControl;
        _syncOptions = syncOptions.Value;
    }

    [FunctionName(Name)]
    public async Task Import([ActivityTrigger] string name, ILogger log)
    {
        log.LogInformation($"{nameof(Import)} start.");

        var sendEndpoint = await _busControl.GetSendEndpoint(_syncOptions.GetEndpointUri());

        var results = await _orderItemRepository.GetOrdersSortedByBrandAndPriceAync(
            _syncOptions.ImportPageSize,
            _completedOrdersQuery);

        while (results.HasNextPage)
        {
            try
            {
                var page = await results.NextPageAsync(_orderItemMapper.Query);
                var messages = page.Select(item => new ImportMessage(item));

                await sendEndpoint.SendBatch(
                    messages,
                    Pipe.New<SendContext<ImportMessage>>(pipeConfigurator =>
                        pipeConfigurator.UseFilter(new PriorityFilter<ImportMessage>(2))));
            }
            catch (Exception)
            {
                log.LogError($"Unexpected error.");
            }
        }

        log.LogInformation($"{nameof(Import)} end.");
    }
}
