﻿using System.Collections.Immutable;
using Elasticsearch.API.Models;
using Nest;

namespace Elasticsearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _elasticClient;
    private const string ProductIndexName = "products";

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
            x => x.Index(ProductIndexName)
                  .Id(newGuid.ToString()));

        if (!response.IsValid)
        {
            return null;
        }

        product.Id = response.Id;

        return product;
    }

    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var result = await _elasticClient.SearchAsync<Product>(
            s => s.Index(ProductIndexName)
                  .Query(q => q.MatchAll()));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return result.Documents.ToImmutableList();
    }
}
