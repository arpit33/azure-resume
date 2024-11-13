using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AzureResume
{
    public class GetResumeCounter
    {
        private readonly CosmosClient _cosmosClient;
        private readonly ILogger<GetResumeCounter> _logger;

        public GetResumeCounter(CosmosClient cosmosClient, ILogger<GetResumeCounter> logger)
        {
            _cosmosClient = cosmosClient;
            _logger = logger;
        }

        [Function("GetResumeCounter")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing request to update resume counter.");

            var container = _cosmosClient.GetContainer("AzureResume", "Counter");
            var response = await container.ReadItemAsync<Counter>("1", new PartitionKey("1"));

            if (response.Resource == null)
            {
                _logger.LogError("Counter document not found in Cosmos DB.");
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.WriteString("Counter document not found.");
                return notFoundResponse;
            }

            var counter = response.Resource;
            counter.Count += 1;

            await container.ReplaceItemAsync(counter, counter.Id, new PartitionKey(counter.PartitionKey));

            var jsonToReturn = JsonConvert.SerializeObject(counter);
            var okResponse = req.CreateResponse(HttpStatusCode.OK);
            okResponse.WriteString(jsonToReturn);
            okResponse.Headers.Add("Content-Type", "application/json");
            return okResponse;
        }
    }

    // Define the Counter class
    public class Counter
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("partitionKey")]
        public required string PartitionKey { get; set; }
    }
}