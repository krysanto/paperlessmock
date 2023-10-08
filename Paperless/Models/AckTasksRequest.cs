using System.Text.Json.Serialization;

namespace Paperless.Models;

public class AckTasksRequest
{
    [JsonPropertyName("tasks")]
    public IEnumerable<int> Tasks { get; set; }
}