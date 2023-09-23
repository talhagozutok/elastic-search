using Elasticsearch.API.Models;

namespace Elasticsearch.API.Dtos;

public record ProductCreateDto(
    string Name,
    decimal Price,
    int Stock,
    ProductFeatureDto ProductFeature)
{
    public Product ToProduct()
    {
        Color? enumColor = Enum.TryParse<Color>(ProductFeature.Color, out var color) ? color : null;

        return new Product
        {
            Name = Name,
            Price = Price,
            Stock = Stock,
            Feature = new ProductFeature()
            {
                Width = ProductFeature.Width,
                Height = ProductFeature.Height,
                Color = enumColor
            }
        };
    }
}
