using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elasticsearch.Web.Models;
using Elasticsearch.Web.ViewModels;

namespace Elasticsearch.Web.Repositories;

public class ECommerceRepository
{
    private readonly ElasticsearchClient _elasticClient;

    private const string IndexName = "kibana_sample_data_ecommerce";

    public ECommerceRepository(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<(List<ECommerce> list, long count)> SearchAsync(
        int page,
        int pageSize,
        ECommerceSearchViewModel searchViewModel)
    {
        List<Action<QueryDescriptor<ECommerce>>> queryDescriptors = new();

        if (!string.IsNullOrEmpty(searchViewModel.Category))
        {
            Action<QueryDescriptor<ECommerce>> query = q => q
                .Match(m => m.Field(f => f.Category)
                    .Query(searchViewModel.Category));

            queryDescriptors.Add(query);
        }

        if (!string.IsNullOrEmpty(searchViewModel.CustomerFullName))
        {
            Action<QueryDescriptor<ECommerce>> query = q => q
                .Match(m => m.Field(f => f.CustomerFullName)
                    .Query(searchViewModel.CustomerFullName));

            queryDescriptors.Add(query);
        }

        if (searchViewModel.OrderDateStart.HasValue)
        {
            Action<QueryDescriptor<ECommerce>> query = q => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.OrderDate)
                        .Gte(searchViewModel.OrderDateStart.Value)));

            queryDescriptors.Add(query);
        }

        if (searchViewModel.OrderDateEnd.HasValue)
        {
            Action<QueryDescriptor<ECommerce>> query = q => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.OrderDate)
                        .Gte(searchViewModel.OrderDateEnd.Value)));

            queryDescriptors.Add(query);
        }

        if (!string.IsNullOrEmpty(searchViewModel.Gender))
        {
            Action<QueryDescriptor<ECommerce>> query = q => q
                .Term(e => e.Gender, searchViewModel.Gender);
        }

        var result = await _elasticClient.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(pageSize).From((page - 1) * pageSize)
            .Query(q => q
                .Bool(b => b
                    .Must(queryDescriptors.ToArray()))));

        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }

        return (list: result.Documents.ToList(),
                count: result.Total);
    }
}
