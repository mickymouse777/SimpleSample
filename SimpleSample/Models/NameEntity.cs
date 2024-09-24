using Azure;
using Azure.Data.Tables;

namespace SimpleSample.Models
{
    public class NameEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "NamesPartition";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
