using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using System.Text.Json;
using Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

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

            
            string connectionString = "use_your_own_storage.student.net";
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
