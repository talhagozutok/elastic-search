using System.Text.Json.Serialization;

namespace Elasticsearch.Web.Models;

public class Blog
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("created_on")]
    public DateTime CreatedOn { get; set; }
}
