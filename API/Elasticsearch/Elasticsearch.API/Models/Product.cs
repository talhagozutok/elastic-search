using Nest;

namespace Elasticsearch.API.Models;

public class Product
{
    [PropertyName("_id")]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProductFeature? Feature { get; set; }
}
