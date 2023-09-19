using Elasticsearch.API.Models;

namespace Elasticsearch.API.Requests;

public record ProductCreateDto(
    string Name,
    decimal Price,
    int Stock,
    ProductFeatureDto ProductFeature)
{
    public Product ToProduct()
    {
        return new Product
        {
            Name = Name,
            Price = Price,
            Stock = Stock,
            Feature = new ProductFeature()
            {
                Width = ProductFeature.Width,
                Height = ProductFeature.Height,
                Color = ProductFeature.Color
            }
        };
    }
}
