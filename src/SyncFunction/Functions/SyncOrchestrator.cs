using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace SyncFunction.Functions;

public class SyncOrchestrator
{
    [FunctionName(nameof(SyncOrchestrator))]
    public async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        await context.CallActivityAsync(ImportFunction.Name, default);
        await context.CallActivityAsync(DeleteFunction.Name, default);
    }

    [FunctionName($"{nameof(SyncOrchestrator)}_HttpStart")]
    public async Task<HttpResponseMessage> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        [DurableClient] IDurableOrchestrationClient starter,
        ILogger log)
    {
        string instanceId = await starter.StartNewAsync(nameof(SyncOrchestrator), null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}
