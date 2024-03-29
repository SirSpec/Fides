﻿using MassTransit;
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

namespace SyncFunction.Functions;

public class ImportFunction
{
    public const string Name = $"{nameof(SyncOrchestrator)}_{nameof(ImportFunction)}";

    private readonly ICompletedOrdersQuery _completedOrdersQuery;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderItemMapper _orderItemMapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly RabbitMqOptions _rabbitMqOptions;

    public ImportFunction(
        ICompletedOrdersQuery completedOrdersQuery,
        IOrderItemRepository orderItemRepository,
        IOrderItemMapper orderItemMapper,
        IPublishEndpoint busControl,
        IOptions<RabbitMqOptions> rabbitMqOptions)
    {
        _completedOrdersQuery = completedOrdersQuery;
        _orderItemRepository = orderItemRepository;
        _orderItemMapper = orderItemMapper;
        _publishEndpoint = busControl;
        _rabbitMqOptions = rabbitMqOptions.Value;
    }

    [FunctionName(Name)]
    public async Task Import([ActivityTrigger] object @object, ILogger log)
    {
        var results = await _orderItemRepository.GetOrdersSortedByBrandAndPriceAync(
            _rabbitMqOptions.ImportPageSize,
            _completedOrdersQuery);

        while (results.HasNextPage)
        {
            try
            {
                var page = await results.NextPageAsync(_orderItemMapper.Query);
                var messages = page.Select(item => new ImportMessage(item));

                await _publishEndpoint.PublishBatch(
                    messages,
                    Pipe.New<PublishContext<ImportMessage>>(pipeConfigurator =>
                        pipeConfigurator.UseFilter(new PriorityFilter<ImportMessage>(2))));
            }
            catch (Exception exception)
            {
                log.LogError(exception, $"Unexpected error.");
            }
        }
    }
}
