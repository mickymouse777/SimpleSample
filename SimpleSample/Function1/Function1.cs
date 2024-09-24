using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Azure.Data.Tables;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Azure;
using System;

namespace FunctionApp
{
    public static class AddNameFunction
    {
        [FunctionName("AddNameFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<NameEntity>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                return new BadRequestObjectResult("Invalid input");
            }

            // Azure Table Storage connection settings
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=mickgstorage;AccountKey=133vjGh69KWjSN6f3qEhU2smr2Xtaee4SPrdmrAtsIbclUkeBsIohqbCoz59XgkQD+R2rJA1D7N7+AStj30JKw==;EndpointSuffix=core.windows.net";
            var tableClient = new TableClient(connectionString, "NamesTable");
            await tableClient.CreateIfNotExistsAsync();

            data.PartitionKey = "NamesPartition";
            data.RowKey = Guid.NewGuid().ToString();

            await tableClient.AddEntityAsync(data);

            return new OkObjectResult($"Name '{data.Name}' added successfully.");
        }

        private class NameEntity : ITableEntity
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public string Name { get; set; }
            public ETag ETag { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
        }
    }
}
