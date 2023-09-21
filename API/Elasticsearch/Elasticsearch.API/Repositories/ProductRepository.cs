using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elasticsearch.API.Dtos;
using Elasticsearch.API.Models;

namespace Elasticsearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticsearchClient _elasticClient;
    private const string ProductIndexName = "products";

    public ProductRepository(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Product?> SaveAsync(Product product)
    {
        product.CreatedAt = DateTime.Now;

        var newGuid = Guid.NewGuid();
        var response = await _elasticClient.IndexAsync(
            product,
            x => x.Index(ProductIndexName)
                  .Id(newGuid.ToString()));

        if (!response.IsValidResponse)
        {
            return null;
        }

        product.Id = response.Id;

        return product;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var result = await _elasticClient.SearchAsync<Product>(s => s
            .Index(ProductIndexName)
            .Query(q => q.MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        var response = await _elasticClient.GetAsync<Product>(
            id,
            desc => desc.Index(ProductIndexName));

        if (!response.IsValidResponse)
        {
            return null;
        }

        response.Source.Id = response.Id;

        return response.Source;
    }

    public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        var response = await _elasticClient.UpdateAsync<Product, ProductUpdateDto>(
            ProductIndexName,
            productUpdateDto.Id,
            x => x.Doc(productUpdateDto));

        return response.IsValidResponse;
    }

    public async Task<DeleteResponse> DeleteAsync(string id)
    {
        var response = await _elasticClient.DeleteAsync<Product>(
            id,
            s => s.Index(ProductIndexName));

        return response;
    }
}
