using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _elasticClient;

    public ProductRepository(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Product?> SaveAsync(Product product)
    {
        product.CreatedAt = DateTime.Now;

        var newGuid = Guid.NewGuid();
        var response = await _elasticClient.IndexAsync(
            product,
            x => x.Index("products")
                  .Id(newGuid.ToString()));

        if (!response.IsValid)
        {
            return null;
        }

        product.Id = response.Id;

        return product;
    }
}
