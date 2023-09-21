using System.Text.Json.Serialization;
using Elasticsearch.API.Dtos;
using Elasticsearch.API.Requests;

namespace Elasticsearch.API.Models;

public class Product
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProductFeature? Feature { get; set; }

    public ProductDto ToDto()
    {
        if (Feature is null)
        {
            return new ProductDto(
                Id, Name,
                Price, Stock, null);
        }

        var productFeature = new ProductFeatureDto(
            Feature.Width,
            Feature.Height,
            Feature.Color?.ToString("g"));

        return new ProductDto(
            Id, Name,
            Price, Stock, productFeature);
    }
}
