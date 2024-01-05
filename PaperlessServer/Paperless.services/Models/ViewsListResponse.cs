using System.Text.Json.Serialization;

namespace Paperless.services.Models;

public partial class ViewsListResponse : ListResponse<SavedView>
{
    [JsonPropertyName("all")]
    public int[] All { get; set; }
}